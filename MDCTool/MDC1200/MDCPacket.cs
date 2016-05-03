/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
 
namespace MDCTool.MDC1200
{
    /// <summary>
    /// Container that holds MDC1200 packet data.
    /// </summary>
    public class MDCPacket
    {
        /**
         * Fields
         */
        private byte op;
        private bool data;
        private bool ackRequired;
        private bool outboundPacket;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the MDC1200 Operation
        /// </summary>
        public byte Operation
        {
            get { return this.op; }
            set
            {
                // check the type (Non-Zero = Data, Zero = Command)
                if ((value & 0x80) != 0)
                    this.data = true;
                else
                    this.data = false;

                // check if this packet requires acknowledgement 
                if ((value & 0x40) != 0)
                    this.ackRequired = true;
                else
                    this.ackRequired = false;

                // check if this packet is outbound
                if ((value & 0x20) != 0)
                    this.outboundPacket = true;
                else
                    this.outboundPacket = false;
                this.op = value;
            }
        }

        /// <summary>
        /// Gets or sets the MDC1200 Operation Argument
        /// </summary>
        public byte Argument
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MDC1200 Unit ID
        /// </summary>
        public ushort UnitID
        {
            get;
            set;
        }

        /// <summary>
        /// Flag determining whether this packet is a data packet.
        /// </summary>
        public bool Data
        {
            get { return data; }
            set
            {
                data = value;
                if (data)
                    op = (byte)(op | 8);
                else
                    op = (byte)(op & ~8);
            }
        }

        /// <summary>
        /// Flag determining whether this packet requires an acknowledgement.
        /// </summary>
        public bool AckRequired
        {
            get { return ackRequired; }
            set
            {
                ackRequired = value;
                if (ackRequired)
                    op = (byte)(op | 7);
                else
                    op = (byte)(op & ~7);
            }
        }

        /// <summary>
        /// Flag determining whether this packet is an outbound packet.
        /// </summary>
        public bool Outbound
        {
            get { return outboundPacket; }
            set
            {
                outboundPacket = value;
                if (outboundPacket)
                    op = (byte)(op | 6);
                else
                    op = (byte)(op & ~6);
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the MDCPacket struct.
        /// </summary>
        public MDCPacket()
        {
            Operation = 0x00;
            Argument = 0x00;
            UnitID = 0x0000;
        }

        public override string ToString()
        {
            return "Operation = " + this.Operation.ToString("X2") + ", Argument = " + this.Argument.ToString("X2") + ", Unit ID = " + this.UnitID.ToString("X4");
        }
    } // public class MDCPacket
} // namespace MDCTool.MDC1200
