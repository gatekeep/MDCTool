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

using NAudio.Wave;

namespace MDCTool
{
    public partial class ConfigureAudioDevice : Form
    {
        /**
         * Fields
         */
        public int WaveInDevice;
        public int WaveOutDevice;

        public int BufferMilliseconds;

        /**
         * Class
         */
        private class ListItem
        {
            /**
             * Fields
             */
            public string Description;
            public int Index;

            /**
             * Methods
             */
            /// <summary>
            /// Initializes a new instance of the <see cref="ListItem"/> class.
            /// </summary>
            public ListItem(int idx, string desc)
            {
                this.Description = desc;
                this.Index = idx;
            }

            public override string ToString()
            {
                return Description;
            }
        } // private class ListItem

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureAudioDevice"/> class.
        /// </summary>
        public ConfigureAudioDevice()
        {
            InitializeComponent();

            this.WaveInDevice = Properties.Settings.Default.InputDevice;
            this.WaveOutDevice = Properties.Settings.Default.OutputDevice;
            this.BufferMilliseconds = Properties.Settings.Default.Buffer;

            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                inputDeviceComboBox.Items.Add(new ListItem(waveInDevice, deviceInfo.ProductName));
            }

            int waveOutDevices = WaveOut.DeviceCount;
            for (int waveOutDevice = 0; waveOutDevice < waveOutDevices; waveOutDevice++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(waveOutDevice);
                outputDeviceComboBox.Items.Add(new ListItem(waveOutDevice, deviceInfo.ProductName));
            }

            inputDeviceComboBox.SelectedIndex = this.WaveInDevice;
            outputDeviceComboBox.SelectedIndex = this.WaveOutDevice;
            bufferTextBox.Text = this.BufferMilliseconds.ToString();

            bufferTextBox.TextChanged += BufferTextBox_TextChanged;
        }

        /// <summary>
        /// Event that occurs when the buffer textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BufferTextBox_TextChanged(object sender, EventArgs e)
        {
            int buffer = Properties.Settings.Default.Buffer;
            if (int.TryParse(bufferTextBox.Text, out buffer))
            {
                bufferTextBox.ForeColor = Color.Black;
                this.BufferMilliseconds = buffer;
            }
            else
            {
                bufferTextBox.ForeColor = Color.Red;
                this.BufferMilliseconds = Properties.Settings.Default.Buffer;
            }
        }

        /// <summary>
        /// Event that occurs when the "Cancel" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Event that occurs when the "Ok" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.WaveInDevice = ((ListItem)inputDeviceComboBox.SelectedItem).Index;
            this.WaveOutDevice = ((ListItem)outputDeviceComboBox.SelectedItem).Index;
            Properties.Settings.Default.InputDevice = this.WaveInDevice;
            Properties.Settings.Default.OutputDevice = this.WaveOutDevice;
            Properties.Settings.Default.Buffer = this.BufferMilliseconds;
            Properties.Settings.Default.Save();

            this.Hide();
        }
    } // public partial class ConfigureAudioDevice : Form
} // namespace MDCTool
