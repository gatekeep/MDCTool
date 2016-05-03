/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDCTool
{
    public partial class MessageTargetID : Form
    {
        /**
         * Fields
         */
        public ushort TargetID;
        public ushort MessageID;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTargetID"/> class.
        /// </summary>
        public MessageTargetID()
        {
            InitializeComponent();

            TargetID = 0x0001;
            MessageID = 0x0001;

            targetMDCID.TextChanged += targetMDCID_TextChanged;
            messageMDCID.TextChanged += messageMDCID_TextChanged;
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateTargetID()
        {
            targetMDCID.ForeColor = Color.Red;
            TargetID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                TargetID = Convert.ToUInt16(targetMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateTargetID();
            }
        }

        /// <summary>
        /// Occurs when the text in the Target ID box changes.
        /// </summary>
        private void targetMDCID_TextChanged(object sender, EventArgs e)
        {
            if (targetMDCID.Text.Length < 4)
                InvalidateTargetID();
            else
                ValidateTargetID();
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateMessageID()
        {
            messageMDCID.ForeColor = Color.Red;
            MessageID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateMessageID()
        {
            try
            {
                messageMDCID.ForeColor = Color.Black;
                MessageID = Convert.ToUInt16(messageMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateMessageID();
            }
        }

        /// <summary>
        /// Occurs when the text in the Target ID box changes.
        /// </summary>
        private void messageMDCID_TextChanged(object sender, EventArgs e)
        {
            if (messageMDCID.Text.Length < 4)
                InvalidateMessageID();
            else
                ValidateMessageID();
        }

        /// <summary>
        /// Occurs when the "Transmit" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transmitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } // public partial class MessageTargetID : Form
} // namespace MDCTool
