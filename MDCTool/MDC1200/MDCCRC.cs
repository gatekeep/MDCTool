/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
/*
 * mdc_common.c
 * Author: Matthew Kaufman (matthew@eeph.com)
 *
 * Copyright (c) 2011  Matthew Kaufman  All rights reserved.
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
 */
using System;
 
namespace MDCTool.MDC1200
{
    /// <summary>
    /// This static class provides common CRC utility functions the MDC-1200 library.
    /// </summary>
    public abstract class MDCCRC
    {
        /**
         * Fields
         */
        public const double TWOPI = (2.0 * 3.1415926535);

        /**
         * Methods
         */
        /// <summary>
        /// Helper function to flip the given number of bits in the given CRC.
        /// </summary>
        /// <param name="crc">CRC to flip</param>
        /// <param name="bitnum">Number of bits to flip</param>
        /// <returns>Flipped CRC</returns>
        public ushort Flip(ushort crc, int bitnum)
        {
            ushort crcout, i, j;

            j = 1;
            crcout = 0;

            // iterate through the bits of the CRC flipping them
            for (i = (ushort)(1 << (bitnum - 1)); i != 0; i >>= 1)
            {
                if ((crc & i) != 0)
                    crcout |= j;
                j <<= 1;
            }
            return (crcout);
        }
        
        /// <summary>
        /// Helper function to compute the CRC of the given byte array.
        /// </summary>
        /// <param name="p">Byte array to compute CRC for</param>
        /// <param name="len">Length of array</param>
        /// <returns>CRC of byte array</returns>
        public ushort ComputeCRC(byte[] p, int len)
        {
            int i, j;
            ushort c;
            int bit;
            ushort crc = 0x0000;

            // iterate through the length of the byte array
            for (i = 0; i < len; i++)
            {
                c = (ushort)p[i];
                c = Flip(c, 8);

                for (j = 0x80; j != 0; j >>= 1)
                {
                    bit = crc & 0x8000;
                    crc <<= 1;
                    if ((c & j) != 0)
                        bit ^= 0x8000;
                    if (bit != 0)
                        crc ^= 0x1021;
                }
            }

            crc = Flip(crc, 16);
            crc ^= 0xFFFF;
            crc &= 0xFFFF;

            return (crc);
        }
    } // public abstract class MDCCRC
} // namespace MDCTool.MDCTool
