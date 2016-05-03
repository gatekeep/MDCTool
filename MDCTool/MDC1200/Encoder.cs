/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
/*-
 * mdc_encode.c
 *  Encodes a specific format from 1200 BPS MSK data burst
 *  to output audio samples.
 *
 * 9 October 2010 - typedefs for easier porting
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
    /// <summary>
    /// Implements a encoder for MDC-1200 packets in a audio stream.
    /// </summary>
    public class Encoder : MDCCRC
    {
        /**
         * Constants
         */
        public static byte[] sinTable = {
            127, 130, 133, 136, 139, 142, 145, 148, 151, 154, 157, 160, 163, 166, 169, 172,
            175, 178, 180, 183, 186, 189, 191, 194, 196, 199, 201, 204, 206, 209, 211, 213,
            215, 218, 220, 222, 224, 226, 227, 229, 231, 233, 234, 236, 237, 239, 240, 241,
            242, 244, 245, 246, 247, 247, 248, 249, 250, 250, 251, 251, 251, 252, 252, 252,
            252, 252, 252, 252, 251, 251, 251, 250, 250, 249, 248, 247, 247, 246, 245, 244,
            242, 241, 240, 239, 237, 236, 234, 233, 231, 229, 227, 226, 224, 222, 220, 218,
            215, 213, 211, 209, 206, 204, 201, 199, 196, 194, 191, 189, 186, 183, 180, 178,
            175, 172, 169, 166, 163, 160, 157, 154, 151, 148, 145, 142, 139, 136, 133, 130,
            127, 124, 121, 118, 115, 112, 109, 106, 103, 100,  97,  94,  91,  88,  85,  82,
             79,  76,  74,  71,  68,  65,  63,  60,  58,  55,  53,  50,  48,  45,  43,  41,
             39,  36,  34,  32,  30,  28,  27,  25,  23,  21,  20,  18,  17,  15,  14,  13,
             12,  10,   9,   8,   7,   7,   6,   5,   4,   4,   3,   3,   3,   2,   2,   2,
              2,   2,   2,   2,   3,   3,   3,   4,   4,   5,   6,   7,   7,   8,   9,  10,
             12,  13,  14,  15,  17,  18,  20,  21,  23,  25,  27,  28,  30,  32,  34,  36,
             39,  41,  43,  45, 48,  50,  53,  55,   58,  60,  63,  65,  68,  71,  74,  76,
             79,  82,  85,  88,  91,  94,  97, 100, 103, 106, 109, 112, 115, 118, 121, 124
        };

        /// <summary>
        /// MDC1200 Codewords are 14 bytes each
        /// </summary>
        public const int CODEWORD_LENGTH = 14;

        /// <summary>
        /// MDC1200 Bit Sync is 5 bytes
        /// </summary>
        public const int BITSYNC_LENGTH = 5;

        /// <summary>
        /// Preamble is 8 bytes
        /// </summary>
        public const int PREAMBLE_LENGTH = 7;

        /// <summary>
        /// Default number of preambles transmitted with all packets
        /// </summary>
        public const int DEFAULT_PREAMBLES = 2;

        /**
         * Fields
         */
        private double incr;

        private double th;
        private double tth;

        private int bpos;
        private int ipos;

        private bool state;
        private int lb;

        private int xorBit;

        /// <summary>
        /// MDC1200 Data Array
        /// </summary>
        /// <remarks>The MDC1200 data array, consists of a 7 byte pad leader, followed by a 5 byte sync block,
        /// followed by 2 consecutive blocks of 14 bytes of packet data. This makes a full MDC1200 double packet array
        /// a full 40 bytes.</remarks>
        private byte[] dataArray;
        private int dataLoaded;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the number of preambles to send in the MDC1200 data stream.
        /// </summary>
        public int NumberOfPreambles
        {
            get;
            set;
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the Decoder class.
        /// </summary>
        /// <param name="sampleRate">Audio Sample Rate</param>
        public Encoder(int sampleRate)
        {
            this.incr = (1200.0 * TWOPI) / sampleRate;
            this.NumberOfPreambles = DEFAULT_PREAMBLES;
        }

        /// <summary>
        /// Internal function to generate and add a preamble.
        /// </summary>
        /// <param name="array">Array to load into</param>
        /// <param name="offset">Offset in array to add bit sync</param>
        /// <returns>Number of bytes loaded into array</returns>
        private int InsertPreamble(ref byte[] array, int offset)
        {
            // standard Mot 00-00-00-AA-AA-AA-AA
            array[offset] = 0x00;
            array[offset + 1] = 0x00;
            array[offset + 2] = 0x00;
            array[offset + 3] = 0xAA;
            array[offset + 4] = 0xAA;
            array[offset + 5] = 0xAA;
            array[offset + 6] = 0xAA;
            return 7;
        }

        /// <summary>
        /// Internal function to generate the MDC1200 header (preambles + 40 bit sync codeword)
        /// </summary>
        /// <param name="array">Array to load header into</param>
        /// <param name="bytesLoaded">Number of bytes loaded</param>
        /// <param name="preambles">Number of 24-bit preambles to insert</param>
        /// <param name="singlePacket">Flag that determines whether or not this is a single or double packet MDC1200 stream</param>
        private void GenerateHeader(ref byte[] array, ref int bytesLoaded, int preambles = 1, bool singlePacket = false)
        {
            if (preambles <= 0)
                throw new InvalidOperationException();

            // determine codeword length
            int codewordLength = 0;
            if (singlePacket)
                codewordLength = (CODEWORD_LENGTH);
            else
                codewordLength = (CODEWORD_LENGTH * 2);

            // determine data stream size
            int dataSize = (codewordLength) + (BITSYNC_LENGTH) + (PREAMBLE_LENGTH); // 112 bits or 40 bytes
            if (preambles > 1)
                dataSize += (PREAMBLE_LENGTH * preambles);

            // initialize a new data array
            this.dataArray = new byte[dataSize];
            this.dataLoaded = 0;
            this.state = false;

            // insert preambles
            for (int i = 0; i < preambles; i++)
                bytesLoaded += InsertPreamble(ref array, bytesLoaded);

            // insert bit sync preamble
            bytesLoaded += InsertPreamble(ref array, bytesLoaded);

            // add MDC1200 sync data (5 bytes) [40 bit]
            array[bytesLoaded] = 0x07;
            array[bytesLoaded + 1] = 0x09;
            array[bytesLoaded + 2] = 0x2A;
            array[bytesLoaded + 3] = 0x44;
            array[bytesLoaded + 4] = 0x6F;
            bytesLoaded += 5;
        }

        /// <summary>
        /// Internal function to pack 32-bit data codewords.
        /// </summary>
        /// <param name="data">Data to pack</param>
        private void PackData(byte[] data)
        {
            ushort ccrc;
            int[] csr = new int[7];
            int[] lbits = new int[Decoder.MAX_MDC1200_BITS];

            // generate CRC and add data at position 4 and 5
            ccrc = ComputeCRC(data, 4);
            data[4] = (byte)(ccrc & 0x00ff);
            data[5] = (byte)((ccrc >> 8) & 0x00ff);

            // insert data pad at position 6
            data[6] = 0x00;

            // convolutional encoding for error detection - deep magic
            for (int i = 0; i < 7; i++)
                csr[i] = 0;

            for (int i = 0; i < 7; i++)
            {
                data[i + 7] = 0;
                for (int j = 0; j <= 7; j++)
                {
                    for (int k = 6; k > 0; k--)
                        csr[k] = csr[k - 1];

                    csr[0] = (data[i] >> j) & 0x01;
                    int b = csr[0] + csr[2] + csr[5] + csr[6];

                    data[i + 7] |= (byte)((b & 0x01) << j);
                }
            }

            // dump output data for debug purposes
            Messages.TraceHex("Encoded Data Dump CCRC " + ccrc.ToString("X4"), data);

            // do deep magic bit manipulation
            int l = 0, m = 0;
            for (int i = 0; i < 14; i++)
            {
                // loop through 8 bits (for each byte)
                for (int j = 0; j <= 7; j++)
                {
                    // get if the bit is set
                    lbits[l] = 0x01 & (data[i] >> j);
                    l += 16;
                    if (l > 111)
                        l = ++m;
                }
            }

            l = 0;
            for (int i = 0; i < 14; i++)
            {
                data[i] = 0;
                
                // loop through 8 bits (for each byte)
                for (int j = 7; j >= 0; j--)
                {
                    // is this bit set?
                    if (lbits[l] != 0)
                        data[i] |= (byte)(1 << j);
                    ++l;
                }
            }

            Messages.TraceHex("Post-Bit Packing", data);
        }

        /// <summary>
        /// Create an array of MDC1200 preambles.
        /// </summary>
        /// <param name="numOfPreambles">Number of preambles</param>
        public void CreatePreambles(int numOfPreambles)
        {
            // initialize MDC1200 header
            GenerateHeader(ref dataArray, ref dataLoaded, numOfPreambles, true);
        }

        /// <summary>
        /// Create a single packet MDC1200 data array.
        /// </summary>
        /// <param name="packet">MDC packet to create</param>
        public void CreateSingle(MDCPacket packet)
        {
            Messages.Trace("Creating single-length MDC1200 packet " + packet.ToString());

            // initialize MDC1200 header
            GenerateHeader(ref dataArray, ref dataLoaded, NumberOfPreambles, true);

            // create a temporary array DATA_SIZE - 12 (stripping the size used by
            // the header)
            byte[] dp = new byte[CODEWORD_LENGTH];

            // fill the first bytes with the proper opcode and argument data
            dp[0] = packet.Operation;
            dp[1] = packet.Argument;
            dp[2] = (byte)((packet.UnitID >> 8) & 0x00FF);
            dp[3] = (byte)(packet.UnitID & 0x00FF);

            // pack data
            PackData(dp);

            // block copy new data into MDC1200 data array
            Buffer.BlockCopy(dp, 0, this.dataArray, dataLoaded, dp.Length);
            this.dataLoaded += 14;

            // dump output data for debug purposes
            Messages.TraceHex("Raw Data Dump", dataArray);
        }

        /// <summary>
        /// Create a double packet MDC1200 data array.
        /// </summary>
        /// <param name="first">First MDC packet to create</param>
        /// <param name="second">First MDC packet to create</param>
        public void CreateDouble(MDCPacket first, MDCPacket second)
        {
            Messages.Trace("Creating double-length MDC1200 packet First (" + first.ToString() + "), Second (" + second.ToString() + ")");

            // initialize MDC1200 header
            GenerateHeader(ref dataArray, ref dataLoaded, NumberOfPreambles);

            // create a temporary array DATA_SIZE - 12 (stripping the size used by
            // the header)
            byte[] dp = new byte[CODEWORD_LENGTH];

            // fill the first bytes with the proper opcode and argument data
            dp[0] = first.Operation;
            dp[1] = first.Argument;
            dp[2] = (byte)((first.UnitID >> 8) & 0x00FF);
            dp[3] = (byte)(first.UnitID & 0x00FF);

            // pack data
            PackData(dp);

            // block copy new data into MDC1200 data array
            Buffer.BlockCopy(dp, 0, this.dataArray, dataLoaded, dp.Length);
            this.dataLoaded += 14;

            // clear temporary buffer
            dp = new byte[CODEWORD_LENGTH];

            // fill the first bytes with the proper opcode and argument data
            dp[0] = second.Operation;
            dp[1] = second.Argument;
            dp[2] = (byte)((second.UnitID >> 8) & 0x00FF);
            dp[3] = (byte)(second.UnitID & 0x00FF);

            // pack data
            PackData(dp);

            // block copy new data into MDC1200 data array
            Buffer.BlockCopy(dp, 0, this.dataArray, dataLoaded, dp.Length);
            this.dataLoaded += 14;

            // dump output data for debug purposes
            Messages.TraceHex("Raw Data Dump", dataArray);
        }

        /// <summary>
        /// Internal function to get a single audio sample.
        /// </summary>
        /// <returns>Single audio sample</returns>
        private byte GetSample()
        {
            int b, ofs;

            this.th += this.incr;
            if (this.th >= TWOPI)
            {
                this.th -= TWOPI;
                this.ipos++;
                if (this.ipos > 7)
                {
                    this.ipos = 0;
                    this.bpos++;
                    if (this.bpos >= this.dataLoaded)
                    {
                        this.state = false;
                        return 127;
                    }
                }

                b = 0x01 & (this.dataArray[this.bpos] >> (7 - (this.ipos)));
                if (b != this.lb)
                {
                    this.xorBit = 1;
                    this.lb = b;
                }
                else
                    this.xorBit = 0;
            }

            // are we XORing the bits?
            if (this.xorBit != 0)
                this.tth += 1.5f * this.incr;
            else
                this.tth += 1.0f * this.incr;

            if (this.tth >= TWOPI)
                this.tth -= TWOPI;

            ofs = (int)(this.tth * (256.0 / TWOPI));
            return sinTable[ofs];
        }

        /// <summary>
        /// Get raw audio samples from the encoder.
        /// </summary>
        /// <param name="sampleBuffer">Buffer for sample data</param>
        /// <returns>Number of samples retrieved</returns>
        public int GetSamples(ref byte[] sampleBuffer)
        {
            // make sure we have loaded data
            if (this.dataLoaded <= 12)
                return 0;

            // check the current state
            if (!this.state)
            {
                this.th = 0.0f;
                this.tth = 0.0f;
                this.bpos = 0;
                this.ipos = 0;
                this.state = true;
                this.xorBit = 1;
                this.lb = 0;
            }

            int i = 0;
            while ((i < sampleBuffer.Length) && this.state)
                sampleBuffer[i++] = GetSample();

            if (this.state == false)
                this.dataLoaded = 0;

            return i;
        }
    } // public class Encoder
} // namespace MDCTool.MDC1200
