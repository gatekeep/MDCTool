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
    /// Structure containing constants for MDC1200 operation codes
    /// </summary>
    public struct OpType
    {
        /**
         * Single Packets
         */
        /// <summary>
        /// Emergency
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     PTT_PRE (0x80)
        ///     EMERG_UNKNW (0x81)
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte EMERGENCY = 0x00;
        /// <summary>
        /// Emergency Acknowledge
        /// </summary>
        /// <remarks>
        /// Has no arguments, should always have NO_ARG.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte EMERGENCY_ACK = 0x20;

        /// <summary>
        /// PTT ID
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     NO_ARG (0x00) value indicates Post- ID
        ///     PTT_PRE (0x80) value indicated Pre- ID
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte PTT_ID = 0x01;

        /// <summary>
        /// Radio Check
        /// </summary>
        /// <remarks>
        /// Always takes the argument RADIO_CHECK (0x85).
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte RADIO_CHECK = 0x63;
        /// <summary>
        /// Radio Check Acknowledge
        /// </summary>
        /// <remarks>
        /// Has no arguments, should always have NO_ARG.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte RADIO_CHECK_ACK = 0x03;

        /// <summary>
        /// Message
        /// </summary>
        /// <remarks>
        /// Argument contains the ID of the given message. This opcode
        /// optionally supports setting of bit 7 for required acknowledgement
        /// of receipt.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack (or) Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte MESSAGE = 0x07;
        /// <summary>
        /// Message Acknowledge
        /// </summary>
        /// <remarks>
        /// Always takes the argument NO_ARG.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte MESSAGE_ACK = 0x23;

        /// <summary>
        /// Status Request
        /// </summary>
        /// <remarks>
        /// Always takes the argument STATUS_REQ (0x06).
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte STATUS_REQUEST = 0x22;

        /// <summary>
        /// Status Response
        /// </summary>
        /// <remarks>
        /// Argument contains the ID of the given status.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte STATUS_RESPONSE = 0x06;

        /// <summary>
        /// Remote Monitor
        /// </summary>
        /// <remarks>
        /// Always takes the argument REMOTE_MONITOR (0x8A).
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte REMOTE_MONITOR = 0x11;

        /// <summary>
        /// Selective Radio Inhibit
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     NO_ARG (0x00) value indicates Unit ID to inhibit
        ///     CANCEL_INHIBIT (0x0C) value indicates Unit ID to uninhibit
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte RADIO_INHIBIT = 0x2B;

        /// <summary>
        /// Selective Radio Inhibit Acknowledge
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     NO_ARG (0x00) value indicates the Unit ID acknowledges the inhibit
        ///     CANCEL_INHIBIT (0x0C) value indicates the Unit ID acknowledges is uninhibited
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte RADIO_INHIBIT_ACK = 0x0B;

        /// <summary>
        /// Repeater Access
        /// </summary>
        /// <remarks>
        /// Always takes the argument RTT (0x01).
        /// 
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte RAC = 0x30;

        /// <summary>
        /// Request to Talk
        /// </summary>
        /// <remarks>
        /// This operand is doubled to 0x41?
        /// 
        /// Always takes the argument RTT (0x01).
        /// 
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte RTT_1 = 0x40;
        public const byte RTT_2 = 0x41;

        /// <summary>
        /// Request to Talk Acknowledge
        /// </summary>
        /// <remarks>
        /// Has no arguments, should always have NO_ARG.
        /// 
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte RTT_ACK = 0x23;

        /**
         * Double Packets
         */
        /// <summary>
        /// Double Packet Operation (0x35)
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     DOUBLE_PACKET_FROM (0x89) value indicates who transmitted the double packet
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte DOUBLE_PACKET_TYPE1 = 0x35;
        /// <summary>
        /// Double Packet Operation (0x55)
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Command
        ///     7 = Ack/No Ack Required     = Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </summary>
        public const byte DOUBLE_PACKET_TYPE2 = 0x55;

        /// <summary>
        /// Call Alert/Page
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     CALL_ALERT_TO (0x0D) value indicates the intended target of the call
        ///     
        /// The DOUBLE_PACKET_FROM (0x89) of the DOUBLE_PACKET_TYPE1 frame will contain the unit ID that transmitted
        /// the call alert. This opcode expects an ack, regardless of the bit 7 setting.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Data
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte CALL_ALERT_ACK_EXPECTED = 0x83;
        /// <summary>
        /// Call Alert/Page
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     CALL_ALERT_TO (0x0D) value indicates the intended target of the call
        ///     
        /// The DOUBLE_PACKET_FROM (0x89) of the DOUBLE_PACKET_TYPE1 frame will contain the unit ID that transmitted
        /// the call alert. This opcode does not expect an ack, regardless of the bit 7 setting.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Data
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte CALL_ALERT_NOACK_EXPECTED = 0x81;

        /// <summary>
        /// Call Alert/Page Acknowledge
        /// </summary>
        /// <remarks>
        /// Supports the following arguments:
        ///     NO_ARG (0x00) value indicates the unit ID which initiated the call
        ///     
        /// The DOUBLE_PACKET_FROM (0x89) of the DOUBLE_PACKET_TYPE1 frame will contain the unit ID that transmitted
        /// the acknowledge.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Data
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Outbound
        /// </remarks>
        public const byte CALL_ALERT_ACK = 0xA0;

        /// <summary>
        /// Voice Selective Call
        /// </summary>
        /// <remarks>
        /// This opcode is doubled to operand 0x82 as well as 0x80?
        /// 
        /// Supports the following arguments:
        ///     SELECTIVE_CALL_TO (0x15) value indicates the intended target of the call
        ///
        /// The DOUBLE_PACKET_FROM (0x89) of the DOUBLE_PACKET_TYPE1 frame will contain the unit ID that transmitted
        /// the call alert.
        ///
        /// Opcode Bits:
        ///     8 = Command/Data Packet     = Data
        ///     7 = Ack/No Ack Required     = No Ack
        ///     6 = Inbound/Outbound Packet = Inbound
        /// </remarks>
        public const byte SELECTIVE_CALL_1 = 0x80;
        public const byte SELECTIVE_CALL_2 = 0x82;
    } // public struct OpType

    /// <summary>
    /// Structure containing constants for MDC1200 operation arguments
    /// </summary>
    public struct ArgType
    {
        /// <summary>
        /// No Argument
        /// </summary>
        public const byte NO_ARG = 0x00;

        /**
         * Single Packets
         */
        /// <summary>
        /// Emergency Argument (unknown use)
        /// </summary>
        public const byte EMERG_UNKNW = 0x81;

        /// <summary>
        /// PTT ID Pre-
        /// </summary>
        public const byte PTT_PRE = 0x80;

        /// <summary>
        /// Radio Check
        /// </summary>
        public const byte RADIO_CHECK = 0x85;

        /// <summary>
        /// Status Request
        /// </summary>
        public const byte STATUS_REQ = 0x06;

        /// <summary>
        /// Remote Monitor
        /// </summary>
        public const byte REMOTE_MONITOR = 0x8A;

        /// <summary>
        /// Cancel Selective Radio Inhibit
        /// </summary>
        public const byte CANCEL_INHIBIT = 0x0C;

        /// <summary>
        /// Request to Talk
        /// </summary>
        public const byte RTT = 0x01;

        /**
         * Double Packets
         */
        /// <summary>
        /// Double To Argument
        /// </summary>
        /// <remarks>Unit ID represents what radio ID the call is targeting</remarks>
        public const byte DOUBLE_PACKET_TO = 0x89;

        /// <summary>
        /// Call Alert Argument
        /// </summary>
        /// <remarks>Unit ID represents what radio ID the call originated from</remarks>
        public const byte CALL_ALERT = 0x0D;
    } // public struct ArgType
} // namespace MDCTool.MDC1200
