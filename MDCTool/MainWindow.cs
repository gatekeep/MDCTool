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
using System.IO;
using System.Threading;

using NAudio.Wave;

using MDCTool.MDC1200;
using MDCDecoder = MDCTool.MDC1200.Decoder;
using MDCEncoder = MDCTool.MDC1200.Encoder;

using MDCTool.Xml;

namespace MDCTool
{
    public partial class MainWindow : Form
    {
        /**
         * Constants
         */
        /// <summary>
        /// Audio Sample Rate
        /// </summary>
        public const int SAMPLE_RATE = 44100;
        /// <summary>
        /// Bits Per Sample (Bit Rate)
        /// </summary>
        public const int BITS_PER_SAMPLE = 8;

        public const int TAIL_TIME = 150;
        public const int MAX_PREAMBLES_ALLOWED = 10;

        public const string XML_FILE = "user_settings.xml";

        /**
         * Fields
         */
        private ushort myID;                            // MDCTool's PTT ID
        private ushort targetID;                        // Target Radio ID

        private ConfigureAudioDevice audioDeviceModal;

        private WaveFormat waveFormat;                  //
        private BufferedWaveProvider waveProvider;      //

        private WaveOut waveOut;
        private WaveIn waveIn;

        private delegate void __UpdateListBox(string str);
        private delegate void __MDCDelegate(MDCPacket packet);
        private delegate void __VoidDelegate();

        public XmlResource rsrc;

        /**
         * Properties
         */
        /// <summary>
        /// MDC1200 Decoder
        /// </summary>
        public MDCDecoder Decoder
        {
            get;
            private set;
        }

        /// <summary>
        /// MDC1200 Encoder
        /// </summary>
        public MDCEncoder Encoder
        {
            get;
            private set;
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Decoder = new MDCDecoder(SAMPLE_RATE);
            Encoder = new MDCEncoder(SAMPLE_RATE);

            this.myID = 0x0001;
            this.targetID = 0x0001;

            this.Encoder.NumberOfPreambles = 3;

            this.emergAck.Enabled = false;

            this.audioDeviceModal = new ConfigureAudioDevice();

            if (!File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE))
            {
                this.myID = 0x0001;
                this.Encoder.NumberOfPreambles = 3;

                SaveXml();
            }
            else
                LoadXml();

            toolMDCID.Text = this.myID.ToString("X4");
            numOfPreamblesTextBox.Text = this.Encoder.NumberOfPreambles.ToString();

            this.FormClosed += new FormClosedEventHandler(MainWindow_FormClosed);

            this.waveFormat = new WaveFormat(SAMPLE_RATE, BITS_PER_SAMPLE, 1);
            this.waveProvider = new BufferedWaveProvider(waveFormat);
            this.waveProvider.DiscardOnBufferOverflow = true;

            InitializeInputAudio();
            InitializeOutputAudio();

            // hook decoder callback to display any decoded packets
            Decoder.DecoderCallback += new MDCDecoderCallback(Decoder_DecoderCallback);
        }

        /// <summary>
        /// Load the user settings to XML.
        /// </summary>
        private void LoadXml()
        {
            rsrc = new XmlResource(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE);

            this.myID = (ushort)rsrc["Settings"]["MyID"];
            this.Encoder.NumberOfPreambles = (int)rsrc["Settings"]["MDCPreambles"];

            this.audioDeviceModal.WaveInDevice = (int)rsrc["Settings"]["InputDevice"];
            this.audioDeviceModal.WaveOutDevice = (int)rsrc["Settings"]["OutputDevice"];
            this.audioDeviceModal.BufferMilliseconds = (int)rsrc["Settings"]["Buffer"];

            this.decodeEncodedPacketsToolStripMenuItem.Checked = (bool)rsrc["Settings"]["DecodeEncoded"];
        }

        /// <summary>
        /// Save the user settings to XML.
        /// </summary>
        private void SaveXml()
        {
            rsrc = new XmlResource();
            rsrc.CreateTable("Settings");
            rsrc.Submit("Settings", "MyID", typeof(ushort));
            rsrc.Submit("Settings", "MDCPreambles", typeof(int));

            rsrc.Submit("Settings", "InputDevice", typeof(int));
            rsrc.Submit("Settings", "OutputDevice", typeof(int));
            rsrc.Submit("Settings", "Buffer", typeof(int));

            rsrc.Submit("Settings", "DecodeEncoded", typeof(bool));

            // add data
            var data = rsrc.NewData("Settings");
            {
                data["MyID"] = this.myID;
                data["MDCPreambles"] = this.Encoder.NumberOfPreambles;

                data["InputDevice"] = this.audioDeviceModal.WaveInDevice;
                data["OutputDevice"] = this.audioDeviceModal.WaveOutDevice;
                data["Buffer"] = this.audioDeviceModal.BufferMilliseconds;

                data["DecodeEncoded"] = this.decodeEncodedPacketsToolStripMenuItem.Checked;
            }
            rsrc.Submit(data);
            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE);
        }

        /// <summary>
        /// Shuts down the audio resources.
        /// </summary>
        private void ShutdownAudio()
        {
            if (this.waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                    waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (this.waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
        }

        /// <summary>
        /// Initializes the output audio wave device.
        /// </summary>
        private void InitializeOutputAudio()
        {
            if (this.waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                    waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            waveOut = new WaveOut();
            waveOut.DeviceNumber = audioDeviceModal.WaveOutDevice;
            waveOut.Init(waveProvider);
        }

        /// <summary>
        /// Initializes the input audio wave device.
        /// </summary>
        private void InitializeInputAudio()
        {
            if (this.waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }

            waveIn = new WaveIn();
            waveIn.WaveFormat = this.waveFormat;
            waveIn.DeviceNumber = audioDeviceModal.WaveInDevice;
            waveIn.BufferMilliseconds = audioDeviceModal.BufferMilliseconds;

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();
        }

        /// <summary>
        /// Occurs when there is wave data available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            Decoder.ProcessSamples(e.Buffer);
        }

        /// <summary>
        /// Occurs when the MDC-1200 decoder, decodes a packet.
        /// </summary>
        /// <param name="frameCount"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void Decoder_DecoderCallback(int frameCount, MDCPacket first, MDCPacket second)
        {
            string timeString = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            if (recievedPackets.Items.Count > 128)
                recievedPackets.Invoke(new __VoidDelegate(delegate ()
                {
                    recievedPackets.Items.Insert(0, timeString + "\tList auto-cleared, more than 128 entries.");
                    recievedPackets.Items.Clear();
                }), new object[] { });

            recievedPackets.Invoke(new __UpdateListBox(delegate(string str)
                {
                    recievedPackets.Items.Insert(0, timeString + "\t" + str);
                }), new object[] { Decoder.ToString(first) });

            if (frameCount == 2)
                recievedPackets.Invoke(new __UpdateListBox(delegate(string str)
                    {
                        recievedPackets.Items.Insert(0, timeString + "\t" + str);
                    }), new object[] { Decoder.ToString(second) });

            if (first.Operation == OpType.PTT_ID)
                this.targetMDCID.Invoke(new __MDCDelegate(delegate(MDCPacket packet)
                {
                    this.targetID = packet.UnitID;
                    this.targetMDCID.Text = this.targetID.ToString("X4");
                }), new object[] { first });
            if (first.Operation == OpType.EMERGENCY)
                this.emergAck.Enabled = true;
        }

        /// <summary>
        /// Occurs when the "About..." menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        /// <summary>
        /// Occurs when the main program dialog is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // forward to the exit tool strip event handler...
            exitToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Exit" menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShutdownAudio();
            SaveXml();
            Environment.Exit(0);
        }

        /// <summary>
        /// Event that occurs when the "Configure Audio Device..." toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configureAudioDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            audioDeviceModal.ShowDialog();
            SaveXml();

            InitializeInputAudio();
            InitializeOutputAudio();
        }

        /// <summary>
        /// Occurs when the "Clear..." menu item is clicked in the Rx packets context menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recievedPackets.Items.Clear();
        }

        /// <summary>
        /// Internal function to invalidate the PTT ID input box.
        /// </summary>
        private void InvalidateMyID()
        {
            toolMDCID.ForeColor = Color.Red;
            myID = (ushort)rsrc["Settings"]["MyID"]; // reverts the previous setting
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateTargetID()
        {
            targetMDCID.ForeColor = Color.Red;
            targetID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the PTT ID input box.
        /// </summary>
        private void ValidateMyID()
        {
            try
            {
                toolMDCID.ForeColor = Color.Black;
                myID = Convert.ToUInt16(toolMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateMyID();
            }
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                targetID = Convert.ToUInt16(targetMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateTargetID();
            }
        }

        /// <summary>
        /// Occurs when the text in the PTT ID box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolMDCID_TextChanged(object sender, EventArgs e)
        {
            if (toolMDCID.Text.Length < 4)
                InvalidateMyID();
            else
            {
                char[] checkValues = toolMDCID.Text.ToCharArray();
                // no value in the ID field should contain 'F'
                if ((checkValues[0] == 'F') || (checkValues[1] == 'F') || (checkValues[2] == 'F') || (checkValues[3] == 'F'))
                    InvalidateMyID();
                else
                    ValidateMyID();
            }
        }

        /// <summary>
        /// Occurs when the text in the Target ID box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void targetMDCID_TextChanged(object sender, EventArgs e)
        {
            if (targetMDCID.Text.Length < 4)
                InvalidateTargetID();
            else
                ValidateTargetID();
        }

        /// <summary>
        /// Occurs when the text in the preambles text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numOfPreamblesTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                numOfPreamblesTextBox.ForeColor = Color.Black;
                int numOfPreambles = Convert.ToInt32(numOfPreamblesTextBox.Text);

                // limit the maximum number of preambles
                if (numOfPreambles > MAX_PREAMBLES_ALLOWED)
                {
                    numOfPreamblesTextBox.Text = string.Empty + MAX_PREAMBLES_ALLOWED;
                    Encoder.NumberOfPreambles = 10;
                }
                else
                    Encoder.NumberOfPreambles = numOfPreambles;
            }
            catch (FormatException)
            {
                numOfPreamblesTextBox.ForeColor = Color.Red;
                this.Encoder.NumberOfPreambles = (int)rsrc["Settings"]["MDCPreambles"];
            }
        }

        /// <summary>
        /// Internal function to play the encoded MDC1200 generated.
        /// </summary>
        private void PlayEncodedPackets()
        {
            byte[] tmpBuffer = new byte[2048 * 16];
            int numOfSamples = 0;

            // copy samples into a temporary buffer
            numOfSamples += Encoder.GetSamples(ref tmpBuffer);

            try
            {
                byte[] audioBuffer = new byte[numOfSamples];
                for (int i = 0; i < numOfSamples; i++)
                    audioBuffer[i] = tmpBuffer[i];

                waveProvider.AddSamples(audioBuffer, 0, audioBuffer.Length);
                new Thread(() =>
                {
                    waveOut.Play();
                    Thread.Sleep((int)waveProvider.BufferDuration.TotalMilliseconds + TAIL_TIME);

                    waveOut.Stop();
                    waveProvider.ClearBuffer();
                }).Start();
            }
            catch (Exception e)
            {
                Messages.Write("There was an error while trying to play audio!", e);
            }

            if (decodeEncodedPacketsToolStripMenuItem.Checked)
            {
                if (numOfSamples > 0)
                {
                    short[] decBuffer = new short[numOfSamples];
                    for (int i = 0; i < numOfSamples; i++)
                        decBuffer[i] = tmpBuffer[i];

                    Decoder.ProcessSamples(tmpBuffer);
                }
            }
        }

        /** CONSOLE BUTTON OPERATIONS */
        /// <summary>
        /// Occurs when the "Tool PTT ID" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txToolPTT_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.PTT_ID,
                Argument = ArgType.NO_ARG,
                UnitID = myID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Stun Target" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stunButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(this.targetID);
            tgtId.ShowDialog();

            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.RADIO_INHIBIT,
                Argument = ArgType.NO_ARG,
                UnitID = tgtId.TargetRadioID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Revive Target" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reviveButton_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.RADIO_INHIBIT,
                Argument = ArgType.CANCEL_INHIBIT,
                UnitID = targetID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Radio Check" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioCheckButton_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.RADIO_CHECK,
                Argument = ArgType.RADIO_CHECK,
                UnitID = targetID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Call Alert/Page" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callAlertButton_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.DOUBLE_PACKET_TYPE1,
                Argument = ArgType.DOUBLE_PACKET_TO,
                UnitID = targetID
            };
            MDCPacket pckt2 = new MDCPacket()
            {
                Operation = OpType.CALL_ALERT_ACK_EXPECTED,
                Argument = ArgType.CALL_ALERT,
                UnitID = myID
            };
            Encoder.CreateDouble(pckt, pckt2);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "SelCall" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectiveCallButton_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.DOUBLE_PACKET_TYPE1,
                Argument = ArgType.DOUBLE_PACKET_TO,
                UnitID = targetID
            };
            MDCPacket pckt2 = new MDCPacket()
            {
                Operation = OpType.SELECTIVE_CALL_1,
                Argument = ArgType.CALL_ALERT,
                UnitID = myID
            };
            Encoder.CreateDouble(pckt, pckt2);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Status" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusButton_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.STATUS_REQUEST,
                Argument = ArgType.STATUS_REQ,
                UnitID = targetID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Message" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageButton_Click(object sender, EventArgs e)
        {
            MessageTargetID msgTgtId = new MessageTargetID();
            msgTgtId.ShowDialog();

            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.MESSAGE,
                Argument = (byte)msgTgtId.MessageID,
                UnitID = msgTgtId.TargetID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();
        }

        /// <summary>
        /// Occurs when the "Emergency Acknowledge" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emergAck_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.EMERGENCY_ACK,
                Argument = ArgType.NO_ARG,
                UnitID = targetID
            };
            Encoder.CreateSingle(pckt);
            PlayEncodedPackets();

            // emergency was acknowledged disable the button now
            emergAck.Enabled = false;
        }
    } // public partial class MainWindow : Form
} // namespace MDCTool
