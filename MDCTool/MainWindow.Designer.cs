// ----------------------------------------------------------------------
// <copyright file="MainWindow.Designer.cs" company="Bryan Biedenkapp">
//     Copyright (c) 2012 Bryan Biedenkapp., All Rights Reserved.
// </copyright>
// ----------------------------------------------------------------------
/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 * 
 * $created guid: f39f7ca1-2851-404f-83f1-4107feba6cc5 2012/1/11$
 */
namespace MDCTool
{
    /// <summary>
    /// This class serves as the main interface for the MDCTool application.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureAudioDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decodeEncodedPacketsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recievedPackets = new System.Windows.Forms.ListBox();
            this.rxPacketsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.captureLogLabel = new System.Windows.Forms.Label();
            this.myIDLabel = new System.Windows.Forms.Label();
            this.toolMDCID = new System.Windows.Forms.TextBox();
            this.targetMDCID = new System.Windows.Forms.TextBox();
            this.targetIDLabel = new System.Windows.Forms.Label();
            this.generateGroupBox = new System.Windows.Forms.GroupBox();
            this.emergAck = new System.Windows.Forms.Button();
            this.messageButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            this.selectiveCallButton = new System.Windows.Forms.Button();
            this.callAlertButton = new System.Windows.Forms.Button();
            this.radioCheckButton = new System.Windows.Forms.Button();
            this.reviveButton = new System.Windows.Forms.Button();
            this.stunButton = new System.Windows.Forms.Button();
            this.txToolPTT = new System.Windows.Forms.Button();
            this.preamblesLabel = new System.Windows.Forms.Label();
            this.numOfPreamblesTextBox = new System.Windows.Forms.TextBox();
            this.mainMenuStrip.SuspendLayout();
            this.rxPacketsContextMenu.SuspendLayout();
            this.generateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(686, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureAudioDeviceToolStripMenuItem,
            this.decodeEncodedPacketsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // configureAudioDeviceToolStripMenuItem
            // 
            this.configureAudioDeviceToolStripMenuItem.Name = "configureAudioDeviceToolStripMenuItem";
            this.configureAudioDeviceToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.configureAudioDeviceToolStripMenuItem.Text = "Configure Audio Device...";
            this.configureAudioDeviceToolStripMenuItem.Click += new System.EventHandler(this.configureAudioDeviceToolStripMenuItem_Click);
            // 
            // decodeEncodedPacketsToolStripMenuItem
            // 
            this.decodeEncodedPacketsToolStripMenuItem.Checked = true;
            this.decodeEncodedPacketsToolStripMenuItem.CheckOnClick = true;
            this.decodeEncodedPacketsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.decodeEncodedPacketsToolStripMenuItem.Name = "decodeEncodedPacketsToolStripMenuItem";
            this.decodeEncodedPacketsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.decodeEncodedPacketsToolStripMenuItem.Text = "Decode Encoded Packets";
            // 
            // recievedPackets
            // 
            this.recievedPackets.ContextMenuStrip = this.rxPacketsContextMenu;
            this.recievedPackets.FormattingEnabled = true;
            this.recievedPackets.Location = new System.Drawing.Point(15, 66);
            this.recievedPackets.Name = "recievedPackets";
            this.recievedPackets.Size = new System.Drawing.Size(469, 225);
            this.recievedPackets.TabIndex = 21;
            // 
            // rxPacketsContextMenu
            // 
            this.rxPacketsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.rxPacketsContextMenu.Name = "rxPacketsContextMenu";
            this.rxPacketsContextMenu.Size = new System.Drawing.Size(111, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.clearToolStripMenuItem.Text = "Clear...";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // captureLogLabel
            // 
            this.captureLogLabel.AutoSize = true;
            this.captureLogLabel.Location = new System.Drawing.Point(12, 50);
            this.captureLogLabel.Name = "captureLogLabel";
            this.captureLogLabel.Size = new System.Drawing.Size(119, 13);
            this.captureLogLabel.TabIndex = 2;
            this.captureLogLabel.Text = "MDC1200 Capture Log:";
            // 
            // myIDLabel
            // 
            this.myIDLabel.AutoSize = true;
            this.myIDLabel.Location = new System.Drawing.Point(12, 30);
            this.myIDLabel.Name = "myIDLabel";
            this.myIDLabel.Size = new System.Drawing.Size(38, 13);
            this.myIDLabel.TabIndex = 4;
            this.myIDLabel.Text = "My ID:";
            // 
            // toolMDCID
            // 
            this.toolMDCID.Location = new System.Drawing.Point(56, 27);
            this.toolMDCID.MaxLength = 4;
            this.toolMDCID.Name = "toolMDCID";
            this.toolMDCID.Size = new System.Drawing.Size(100, 20);
            this.toolMDCID.TabIndex = 1;
            this.toolMDCID.Text = "0001";
            this.toolMDCID.TextChanged += new System.EventHandler(this.toolMDCID_TextChanged);
            // 
            // targetMDCID
            // 
            this.targetMDCID.Location = new System.Drawing.Point(223, 27);
            this.targetMDCID.MaxLength = 4;
            this.targetMDCID.Name = "targetMDCID";
            this.targetMDCID.Size = new System.Drawing.Size(106, 20);
            this.targetMDCID.TabIndex = 2;
            this.targetMDCID.Text = "0001";
            this.targetMDCID.TextChanged += new System.EventHandler(this.targetMDCID_TextChanged);
            // 
            // targetIDLabel
            // 
            this.targetIDLabel.AutoSize = true;
            this.targetIDLabel.Location = new System.Drawing.Point(162, 30);
            this.targetIDLabel.Name = "targetIDLabel";
            this.targetIDLabel.Size = new System.Drawing.Size(55, 13);
            this.targetIDLabel.TabIndex = 7;
            this.targetIDLabel.Text = "Target ID:";
            // 
            // generateGroupBox
            // 
            this.generateGroupBox.Controls.Add(this.emergAck);
            this.generateGroupBox.Controls.Add(this.messageButton);
            this.generateGroupBox.Controls.Add(this.statusButton);
            this.generateGroupBox.Controls.Add(this.selectiveCallButton);
            this.generateGroupBox.Controls.Add(this.callAlertButton);
            this.generateGroupBox.Controls.Add(this.radioCheckButton);
            this.generateGroupBox.Controls.Add(this.reviveButton);
            this.generateGroupBox.Controls.Add(this.stunButton);
            this.generateGroupBox.Controls.Add(this.txToolPTT);
            this.generateGroupBox.Location = new System.Drawing.Point(490, 66);
            this.generateGroupBox.Name = "generateGroupBox";
            this.generateGroupBox.Size = new System.Drawing.Size(184, 225);
            this.generateGroupBox.TabIndex = 8;
            this.generateGroupBox.TabStop = false;
            this.generateGroupBox.Text = "MDC1200 Encoder";
            // 
            // emergAck
            // 
            this.emergAck.ForeColor = System.Drawing.Color.Teal;
            this.emergAck.Location = new System.Drawing.Point(6, 136);
            this.emergAck.Name = "emergAck";
            this.emergAck.Size = new System.Drawing.Size(168, 23);
            this.emergAck.TabIndex = 10;
            this.emergAck.Text = "Emergency Acknowledge";
            this.emergAck.UseVisualStyleBackColor = true;
            this.emergAck.Click += new System.EventHandler(this.emergAck_Click);
            // 
            // messageButton
            // 
            this.messageButton.Location = new System.Drawing.Point(107, 107);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(67, 23);
            this.messageButton.TabIndex = 9;
            this.messageButton.Text = "Message";
            this.messageButton.UseVisualStyleBackColor = true;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // statusButton
            // 
            this.statusButton.Location = new System.Drawing.Point(6, 107);
            this.statusButton.Name = "statusButton";
            this.statusButton.Size = new System.Drawing.Size(95, 23);
            this.statusButton.TabIndex = 8;
            this.statusButton.Text = "Status";
            this.statusButton.UseVisualStyleBackColor = true;
            this.statusButton.Click += new System.EventHandler(this.statusButton_Click);
            // 
            // selectiveCallButton
            // 
            this.selectiveCallButton.Location = new System.Drawing.Point(107, 78);
            this.selectiveCallButton.Name = "selectiveCallButton";
            this.selectiveCallButton.Size = new System.Drawing.Size(67, 23);
            this.selectiveCallButton.TabIndex = 7;
            this.selectiveCallButton.Text = "SelCall";
            this.selectiveCallButton.UseVisualStyleBackColor = true;
            this.selectiveCallButton.Click += new System.EventHandler(this.selectiveCallButton_Click);
            // 
            // callAlertButton
            // 
            this.callAlertButton.Location = new System.Drawing.Point(6, 78);
            this.callAlertButton.Name = "callAlertButton";
            this.callAlertButton.Size = new System.Drawing.Size(95, 23);
            this.callAlertButton.TabIndex = 6;
            this.callAlertButton.Text = "Call Alert/Page";
            this.callAlertButton.UseVisualStyleBackColor = true;
            this.callAlertButton.Click += new System.EventHandler(this.callAlertButton_Click);
            // 
            // radioCheckButton
            // 
            this.radioCheckButton.Location = new System.Drawing.Point(6, 49);
            this.radioCheckButton.Name = "radioCheckButton";
            this.radioCheckButton.Size = new System.Drawing.Size(168, 23);
            this.radioCheckButton.TabIndex = 5;
            this.radioCheckButton.Text = "Radio Check";
            this.radioCheckButton.UseVisualStyleBackColor = true;
            this.radioCheckButton.Click += new System.EventHandler(this.radioCheckButton_Click);
            // 
            // reviveButton
            // 
            this.reviveButton.ForeColor = System.Drawing.Color.ForestGreen;
            this.reviveButton.Location = new System.Drawing.Point(93, 196);
            this.reviveButton.Name = "reviveButton";
            this.reviveButton.Size = new System.Drawing.Size(85, 23);
            this.reviveButton.TabIndex = 12;
            this.reviveButton.Text = "Revive Target";
            this.reviveButton.UseVisualStyleBackColor = true;
            this.reviveButton.Click += new System.EventHandler(this.reviveButton_Click);
            // 
            // stunButton
            // 
            this.stunButton.ForeColor = System.Drawing.Color.Firebrick;
            this.stunButton.Location = new System.Drawing.Point(6, 196);
            this.stunButton.Name = "stunButton";
            this.stunButton.Size = new System.Drawing.Size(81, 23);
            this.stunButton.TabIndex = 11;
            this.stunButton.Text = "Stun Target";
            this.stunButton.UseVisualStyleBackColor = true;
            this.stunButton.Click += new System.EventHandler(this.stunButton_Click);
            // 
            // txToolPTT
            // 
            this.txToolPTT.Location = new System.Drawing.Point(29, 20);
            this.txToolPTT.Name = "txToolPTT";
            this.txToolPTT.Size = new System.Drawing.Size(123, 23);
            this.txToolPTT.TabIndex = 4;
            this.txToolPTT.Text = "Console PTT ID";
            this.txToolPTT.UseVisualStyleBackColor = true;
            this.txToolPTT.Click += new System.EventHandler(this.txToolPTT_Click);
            // 
            // preamblesLabel
            // 
            this.preamblesLabel.AutoSize = true;
            this.preamblesLabel.Location = new System.Drawing.Point(335, 30);
            this.preamblesLabel.Name = "preamblesLabel";
            this.preamblesLabel.Size = new System.Drawing.Size(59, 13);
            this.preamblesLabel.TabIndex = 12;
            this.preamblesLabel.Text = "Preambles:";
            // 
            // numOfPreamblesTextBox
            // 
            this.numOfPreamblesTextBox.Location = new System.Drawing.Point(396, 27);
            this.numOfPreamblesTextBox.MaxLength = 4;
            this.numOfPreamblesTextBox.Name = "numOfPreamblesTextBox";
            this.numOfPreamblesTextBox.Size = new System.Drawing.Size(106, 20);
            this.numOfPreamblesTextBox.TabIndex = 3;
            this.numOfPreamblesTextBox.Text = "3";
            this.numOfPreamblesTextBox.TextChanged += new System.EventHandler(this.numOfPreamblesTextBox_TextChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 309);
            this.Controls.Add(this.preamblesLabel);
            this.Controls.Add(this.numOfPreamblesTextBox);
            this.Controls.Add(this.generateGroupBox);
            this.Controls.Add(this.targetIDLabel);
            this.Controls.Add(this.toolMDCID);
            this.Controls.Add(this.myIDLabel);
            this.Controls.Add(this.targetMDCID);
            this.Controls.Add(this.captureLogLabel);
            this.Controls.Add(this.recievedPackets);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(702, 348);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(702, 348);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDCTool Console";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.rxPacketsContextMenu.ResumeLayout(false);
            this.generateGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListBox recievedPackets;
        private System.Windows.Forms.Label captureLogLabel;
        private System.Windows.Forms.ContextMenuStrip rxPacketsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Label myIDLabel;
        private System.Windows.Forms.TextBox toolMDCID;
        private System.Windows.Forms.TextBox targetMDCID;
        private System.Windows.Forms.Label targetIDLabel;
        private System.Windows.Forms.GroupBox generateGroupBox;
        private System.Windows.Forms.Button reviveButton;
        private System.Windows.Forms.Button stunButton;
        private System.Windows.Forms.Button txToolPTT;
        private System.Windows.Forms.Button radioCheckButton;
        private System.Windows.Forms.Label preamblesLabel;
        private System.Windows.Forms.TextBox numOfPreamblesTextBox;
        private System.Windows.Forms.Button selectiveCallButton;
        private System.Windows.Forms.Button callAlertButton;
        private System.Windows.Forms.ToolStripMenuItem configureAudioDeviceToolStripMenuItem;
        private System.Windows.Forms.Button messageButton;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.ToolStripMenuItem decodeEncodedPacketsToolStripMenuItem;
        private System.Windows.Forms.Button emergAck;
    } // public partial class MainWindow
} // namespace MDCTool