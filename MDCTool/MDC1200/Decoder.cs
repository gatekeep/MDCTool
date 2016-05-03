/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
/*-
 * mdc_decode.c
 *   Decodes a specific format of 1200 BPS MSK data burst
 *   from input audio samples.
 *
 * 4 October 2010 - fixed for 64-bit
 * 5 October 2010 - added four-point method to C version
 * 7 October 2010 - typedefs for easier porting
 * 9 October 2010 - fixed invert case for four-point decoder
 *
 * Author: Matthew Kaufman (matthew@eeph.com)
 *
 * Copyright (c) 2005, 2010  Matthew Kaufman  All rights reserved.
 * 
 *  This file is part of Matthew Kaufman's MDC Encoder/Decoder Library
 *
 *  The MDC Encoder/Decoder Library is free software; you can
 *  redistribute it and/or modify it under the terms of version 2 of
 *  the GNU General Public License as published by the Free Software
 *  Foundation.
 *
 *  If you cannot comply with the terms of this license, contact
 *  the author for alternative license arrangements or do not use
 *  or redistribute this software.
 *
 *  The MDC Encoder/Decoder Library is distributed in the hope
 *  that it will be useful, but WITHOUT ANY WARRANTY; without even the
 *  implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 *  PURPOSE.  See the GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this software; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301
 *  USA.
 *
 *  or see http://www.gnu.org/copyleft/gpl.html
 *
-*/
using System;

using MDCTool;

namespace MDCTool.MDC1200
{
    /**
     * Delegates
     */
    /// <summary>
    /// Delegate used for decoder callback events.
    /// </summary>
    /// <param name="frameCount">Number of frames in decoded MDC packet</param>
    /// <param name="first">First MDC Packet</param>
    /// <param name="second">Second MDC Packet (Double Packet)</param>
    public delegate void MDCDecoderCallback(int frameCount, MDCPacket first, MDCPacket second);
    /// <summary>
    /// Delegate used when the decoder fails to decode a packet.
    /// </summary>
    /// <param name="frameCount">Number of frames in decoded MDC packet</param>
    public delegate void MDCDecoderFailedCallback(int frameCount);

    /// <summary>
    /// Implements a decoder for MDC-1200 packets in a audio stream.
    /// </summary>
    public class Decoder : MDCCRC
    {
        /**
         * Constants
         */
        /// <summary>
        /// Number of MDC decoders
        /// </summary>
        public const int MDC_ND = 5;

        /// <summary>
        /// Threshold for the number of "good bits"
        /// </summary>
        public const int MDC_GDTHRESH = 5;

        /// <summary>
        /// Maximum number of bits in a single MDC1200 codeword.
        /// </summary>
        public const int MAX_MDC1200_BITS = 112;

        /**
         * Fields
         */
        private double incr;                    //
        private int frameCount;                 // number of frames in MDC packet

        private double[] th;                    //

        private int[] shstate;                  //
        private int[] shcount;                  //

        private int[] nlstep;                   //
        private double[,] nlevel;               //

        private int[,] bits;                    // bits in data stream

        private bool[] xorBit;                  // XOR bit flag array
        private bool[] invertBit;               // Invert bit flag array

        private uint[] syncLow;                 //
        private uint[] syncHigh;                //

        private MDCPacket first;                // First packet (used as the only or first in a doubled MDC1200 stream)
        private MDCPacket second;               // Second packet

        /**
         * Events
         */
        /// <summary>
        /// Occurs when a MDC packet is successfully decoded.
        /// </summary>
        public event MDCDecoderCallback DecoderCallback;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the Decoder class.
        /// </summary>
        /// <param name="sampleRate">Audio Sample Rate</param>
        public Decoder(int sampleRate)
        {
            this.incr = (1200.0 * TWOPI) / sampleRate;
            Clear();
        }

        /// <summary>
        /// Process samples input byte array for an MDC-1200 packet.
        /// </summary>
        /// <param name="samples">Byte array containing samples</param>
        /// <returns>Number of MDC1200 frames processed</returns>
        public int ProcessSamples(byte[] samples)
        {
            // clear existing data
            Clear();

            // iterate through the samples
            for (int i = 0; i < samples.Length; i++)
            {
                byte sample = samples[i];
                double value = (((double)sample) - 128.0) / 256;

                // decode through all available decoders
                for (int j = 0; j < MDC_ND; j++)
                {
                    this.th[j] += (5.0 * this.incr);
                    if (this.th[j] >= TWOPI)
                    {
                        this.nlstep[j]++;
                        if (this.nlstep[j] > 9)
                            this.nlstep[j] = 0;
                        this.nlevel[j, this.nlstep[j]] = value;

                        nlProcess(j);

                        this.th[j] -= TWOPI;
                    }
                }
            }

            // if we have a frame count return the number of MDC-1200 frames processed
            if (this.frameCount > 0) /* not sure what boolean value is supposed to be checked, assuming non-zero */
                return this.frameCount;
            return 0;
        }

        /// <summary>
        /// Noise level processing
        /// </summary>
        /// <param name="idx">Decoder Index</param>
        private void nlProcess(int idx)
        {
            double vnow, vpast;

            switch (this.nlstep[idx])
            {
                case 3:
                    vnow = ((-0.60 * this.nlevel[idx, 3]) + (.97 * this.nlevel[idx, 1]));
                    vpast = ((-0.60 * this.nlevel[idx, 7]) + (.97 * this.nlevel[idx, 9]));
                    break;
                case 8:
                    vnow = ((-0.60 * this.nlevel[idx, 8]) + (.97 * this.nlevel[idx, 6]));
                    vpast = ((-0.60 * this.nlevel[idx, 2]) + (.97 * this.nlevel[idx, 4]));
                    break;
                default:
                    return;
            }

            this.xorBit[idx] = (vnow > vpast) ? true : false;
            if (this.invertBit[idx])
                this.xorBit[idx] = !(this.xorBit[idx]);

            ShiftIn(idx);
        }

        /// <summary>
        /// Internal function to bit shift the MDC1200 data accounting for sync data.
        /// </summary>
        /// <param name="idx">Decoder Index</param>
        private void ShiftIn(int idx)
        {
            bool bit = this.xorBit[idx];
            int gcount;

            switch (this.shstate[idx])
            {
                case -1:
                case 0:
                    {
                        if (this.shstate[idx] == -1)
                        {
                            this.syncHigh[idx] = 0;
                            this.syncLow[idx] = 0;
                            this.shstate[idx] = 0;
                        }

                        this.syncHigh[idx] <<= 1;
                        if ((this.syncLow[idx] & 0x80000000) != 0)
                            this.syncHigh[idx] |= 1;
                        this.syncLow[idx] <<= 1;
                        if (bit)
                            this.syncLow[idx] |= 1;

                        gcount = OneBits(0x000000FF & (0x00000007 ^ this.syncHigh[idx]));
                        gcount += OneBits(0x092A446F ^ this.syncLow[idx]);

                        // check if the "good" bits is less then the threshold
                        if (gcount <= MDC_GDTHRESH)
                        {
                            Messages.Trace("sync " + gcount + " H:" + this.syncHigh[idx].ToString("X") + " L:" + this.syncLow[idx].ToString("X"));

                            this.shstate[idx] = 1;
                            this.shcount[idx] = 0;
                            ClearBits(idx);
                        }
                        else if (gcount >= (40 - MDC_GDTHRESH))
                        {
                            Messages.Trace("isync " + gcount);

                            this.shstate[idx] = 1;
                            this.shcount[idx] = 0;
                            this.xorBit[idx] = !(this.xorBit[idx]);
                            this.invertBit[idx] = !(this.invertBit[idx]);
                            ClearBits(idx);
                        }
                    }
                    return;

                case 1:
                case 2:
                    {
                        this.bits[idx, this.shcount[idx]] = (byte)((bit) ? 1 : 0);
                        this.shcount[idx]++;

                        if (this.shcount[idx] > 111)
                            ProcessBits(idx);
                    }
                    return;

                default:
                    return;
            }
        }

        /// <summary>
        /// Internal function to process the bits of a MDC1200 data stream.
        /// </summary>
        /// <param name="idx">Decoder Index</param>
        /// <returns>True, if CRC was matched and decoding succeeded, otherwise false</returns>
        private bool ProcessBits(int idx)
        {
            int[] lbits = new int[MAX_MDC1200_BITS];
            int lbc = 0;
            byte[] data = new byte[14];
            ushort ccrc, rcrc;

            // do deep magic bit manipulation
            for (int i = 0; i < 16; i++)
            {
                // loop through 8 bits
                for (int j = 0; j < 7; j++)
                {
                    int k = (j * 16) + i;
                    lbits[lbc] = this.bits[idx, k];
                    ++lbc;
                }
            }

            for (int i = 0; i < 14; i++)
            {
                data[i] = 0;

                // loop through 8 bits (get each byte)
                for (int j = 0; j < 8; j++)
                {
                    int k = (i * 8) + j;
                    
                    if (lbits[k] != 0)
                        data[i] |= (byte)(1 << j);
                }
            }

            // compute CRC
            ccrc = ComputeCRC(data, 4);
            rcrc = (ushort)(data[5] << 8 | data[4]);

            // dump output data for debug purposes
            Messages.TraceHex("Decoded Data Dump CCRC " + ccrc.ToString("X4") + " RCRC " + rcrc.ToString("X4"), data);

            // compare the computed CRC to the recieved CRC
            if (ccrc == rcrc)
            {
                byte crc;

                if (this.shstate[idx] == 2)
                {
                    // copy second packet data
                    second.Operation = data[0];
                    second.Argument = data[1];
                    second.UnitID = (ushort)((data[2] << 8) | data[3]);

                    // reset the states for all decoders
                    for(int k = 0; k < MDC_ND; k++)
                        this.shstate[k] = 0;

                    this.frameCount = 2;
                }
                else
                {
                    this.frameCount = 1;

                    // copy first packet data
                    first.Operation = data[0];
                    first.Argument = data[1];
                    first.UnitID = (ushort)((data[2] << 8) | data[3]);
                    crc = (byte)((data[4] << 8) | data[5]);
    
                    // reset the states for all decoders
                    for(int k = 0; k < MDC_ND; k++)
                        this.shstate[k] = 0;

                    // check if the operation code is for a "double" packet
                    switch(data[0])
                    {
                    case OpType.DOUBLE_PACKET_TYPE1:
                    case OpType.DOUBLE_PACKET_TYPE2:
                        {
                            // we have a double packet reset the frame count to 0
                            // and set the state to reflect a double packet
                            this.frameCount = 0;
                            this.shstate[idx] = 2;
                            this.shcount[idx] = 0;

                            ClearBits(idx);
                        }
                        break;

                    default:
                        break;
                    }
                }

                // if our frame count is non-zero execute the decoded callback
                if (this.frameCount > 0)
                {
                    Messages.Trace("Frame Count: " + frameCount);
                    Messages.Trace("MDC Frame 1 = " + ToString(first));

                    // if we have a frame count of > 1 then display second packet data
                    if (this.frameCount > 1)
                        Messages.Trace("MDC Frame 2 = " + ToString(second));

                    Messages.Trace("MDC1200 packet First (" + first.ToString() + "), Second (" + second.ToString() + ")");

                    // fire event
                    if (DecoderCallback != null)
                        DecoderCallback(this.frameCount, first, second);

                    // reset frame count
                    this.frameCount = 0;
                }

                return true;
            }
            else
            {
                Messages.Trace("CRC Mismatch! Bad MDC Frame " + this.frameCount);

                // since the CRC is bad reset the frame count
                this.frameCount = 0;
                this.shstate[idx] = -1;
            }
            return false;
        }

        /// <summary>
        /// Internal function to reset any internal data fields before doing any operations.
        /// </summary>
        private void Clear()
        {
            this.frameCount = 0;

            // clear old packet data
            first = new MDCPacket();
            second = new MDCPacket();

            // wipe all arrays
            this.th = new double[MDC_ND];

            this.shstate = new int[MDC_ND];
            this.shcount = new int[MDC_ND];

            this.nlstep = new int[MDC_ND];
            this.nlevel = new double[MDC_ND, 14];

            this.bits = new int[MDC_ND, MAX_MDC1200_BITS];

            this.xorBit = new bool[MDC_ND];
            this.invertBit = new bool[MDC_ND];

            this.syncLow = new uint[MDC_ND];
            this.syncHigh = new uint[MDC_ND];

            // iterate through decoders setting up fields
            for (int i = 0; i < MDC_ND; i++)
            {
                this.th[i] = 0.0 + (i * (TWOPI / MDC_ND));

                this.xorBit[i] = false;
                this.invertBit[i] = false;

                this.shstate[i] = 0;
                this.shcount[i] = 0;

                this.nlstep[i] = i;

                // ensure all bit arrays are cleared to zero
                ClearBits(i);
            }
        }

        /// <summary>
        /// Internal function to clear the bits array for the given decoder ID.
        /// </summary>
        /// <param name="decoderID">Decoder ID</param>
        private void ClearBits(int decoderID)
        {
            // iterate through all the bits clearing them (set to zero)
            for (int i = 0; i < MAX_MDC1200_BITS; i++)
                this.bits[decoderID, i] = 0;
        }

        /// <summary>
        /// Internal function to return the number of bits set to true (1) in a unsigned integer.
        /// </summary>
        /// <param name="n">Unsigned integer to check</param>
        /// <returns>Number of bits set to true (1)</returns>
        private int OneBits(uint n)
        {
            int i = 0;
            while (n > 0) /* not sure what boolean value is supposed to be checked, assuming non-zero */
            {
                ++i;
                n &= (n - 1);
            }
            return i;
        }

        /// <summary>
        /// Translates a MDC1200 operation to a string
        /// </summary>
        /// <param name="packet">MDC Packet to decode</param>
        /// <returns>String containing parsed MDC1200 operation</returns>
        public string ToString(MDCPacket packet)
        {
            string ret = string.Empty;
            string unit = packet.UnitID.ToString("X4");

            // check operation
            switch (packet.Operation)
            {
                /**
                 * Single Packet Operations
                 */
                case OpType.PTT_ID:
                    {
                        // check argument
                        switch (packet.Argument)
                        {
                            case ArgType.NO_ARG:
                                ret += "PTT ID: " + unit + " [Post- ID]";
                                break;

                            case ArgType.PTT_PRE:
                                ret += "PTT ID: " + unit + " [ Pre- ID]";
                                break;

                            default:
                                ret += "PTT ID: " + unit + " [Unkw- ID]";
                                break;
                        }
                    }
                    break;

                case OpType.EMERGENCY:
                    ret += "!!! EMERGENCY " + unit;
                    break;

                case OpType.EMERGENCY_ACK:
                    ret += "!!! EMERGENCY Acknowledge: " + unit;
                    break;

                case OpType.RADIO_CHECK:
                    ret += "Radio Check Unit: " + unit;
                    break;

                case OpType.RADIO_CHECK_ACK:
                    ret += "Radio Check Acknowledge: " + unit;
                    break;

                case OpType.REMOTE_MONITOR:
                    ret += "Remote Monitor Unit: " + unit;
                    break;

                case OpType.RAC:
                    ret += "Repeater Access Repeater ID: " + unit;
                    break;

                case OpType.RTT_1:
                case OpType.RTT_2:
                    ret += "RTT From: " + unit;
                    break;

                case OpType.RTT_ACK: // also OpType.MESSAGE_ACK
                    ret += "RTT/Message Acknowledge: " + unit;
                    break;

                case OpType.STATUS_REQUEST:
                    ret += "Status Request To: " + unit;
                    break;

                case OpType.STATUS_RESPONSE:
                    ret += "Status Response From: " + unit + " Status: " + packet.Argument.ToString("X2");
                    break;

                case OpType.MESSAGE:
                    ret += "Message Request To: " + unit + " Message: " + packet.Argument.ToString("X2");
                    break;

                case OpType.RADIO_INHIBIT:
                    {
                        // check argument
                        switch (packet.Argument)
                        {
                            case ArgType.NO_ARG:
                                ret += "Stun/Inhibit Target: " + unit;
                                break;

                            case ArgType.CANCEL_INHIBIT:
                                ret += "Revive/Uninhibit Target: " + unit;
                                break;

                            default:
                                ret += "UNK Inhibit Target: " + unit;
                                break;
                        }
                    }
                    break;

                case OpType.RADIO_INHIBIT_ACK:
                    {
                        // check argument
                        switch (packet.Argument)
                        {
                            case ArgType.NO_ARG:
                                ret += "Stun/Inhibit: Acknowlege: " + unit;
                                break;

                            case ArgType.CANCEL_INHIBIT:
                                ret += "Revive/Uninhibit Acknowledge: " + unit;
                                break;

                            default:
                                ret += "UNK Inhibit Acknowledge: " + unit;
                                break;
                        }
                    }
                    break;

                /**
                 * Double Packet Operations
                 */
                case OpType.DOUBLE_PACKET_TYPE1:
                case OpType.DOUBLE_PACKET_TYPE2:
                    ret += "Request To: " + unit;
                    break;

                case OpType.CALL_ALERT_ACK_EXPECTED:
                case OpType.CALL_ALERT_NOACK_EXPECTED:
                    ret += "Call Alert/Page From: " + unit;
                    break;

                case OpType.CALL_ALERT_ACK:
                    ret += "Call Alert/Page Acknowledge: " + unit;
                    break;

                case OpType.SELECTIVE_CALL_1:
                case OpType.SELECTIVE_CALL_2:
                    ret += "Sel-Call: From: " + unit;
                    break;

                default:
                    ret += "UNK Op " + packet.Operation.ToString("X2") + " Arg " + packet.Argument.ToString("X2") + " Unit ID " + unit;
                    break;
            }

            return ret;
        }
    } // public class Decoder
} // namespace MDCTool.MDC1200
