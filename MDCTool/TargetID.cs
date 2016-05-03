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
    public partial class TargetID : Form
    {
        /**
         * Fields
         */
        public ushort TargetRadioID;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTargetID"/> class.
        /// </summary>
        public TargetID(ushort targetId)
        {
            InitializeComponent();

            TargetRadioID = targetId;
            targetMDCID.Text = targetId.ToString("X4");

            targetMDCID.TextChanged += targetMDCID_TextChanged;
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateTargetID()
        {
            targetMDCID.ForeColor = Color.Red;
            TargetRadioID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                TargetRadioID = Convert.ToUInt16(targetMDCID.Text, 16);
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
        /// Occurs when the "Transmit" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transmitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } // public partial class TargetID : Form
} // namespace MDCTool
