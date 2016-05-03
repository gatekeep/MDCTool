namespace MDCTool
{
    partial class RawMDCEncoder
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
            this.secondGroupBox = new System.Windows.Forms.GroupBox();
            this.secondUnitIDTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.secondArgTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.secondOpTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mdcDebugDouble = new System.Windows.Forms.Button();
            this.firstGroupBox = new System.Windows.Forms.GroupBox();
            this.firstUnitIDTextBox = new System.Windows.Forms.TextBox();
            this.unitLabel = new System.Windows.Forms.Label();
            this.firstArgTextBox = new System.Windows.Forms.TextBox();
            this.argLabel = new System.Windows.Forms.Label();
            this.firstOpTextBox = new System.Windows.Forms.TextBox();
            this.opLabel = new System.Windows.Forms.Label();
            this.mdcDebugSingle = new System.Windows.Forms.Button();
            this.secondGroupBox.SuspendLayout();
            this.firstGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // secondGroupBox
            // 
            this.secondGroupBox.Controls.Add(this.secondUnitIDTextBox);
            this.secondGroupBox.Controls.Add(this.label1);
            this.secondGroupBox.Controls.Add(this.secondArgTextBox);
            this.secondGroupBox.Controls.Add(this.label2);
            this.secondGroupBox.Controls.Add(this.secondOpTextBox);
            this.secondGroupBox.Controls.Add(this.label3);
            this.secondGroupBox.Controls.Add(this.mdcDebugDouble);
            this.secondGroupBox.Location = new System.Drawing.Point(320, 12);
            this.secondGroupBox.Name = "secondGroupBox";
            this.secondGroupBox.Size = new System.Drawing.Size(265, 79);
            this.secondGroupBox.TabIndex = 12;
            this.secondGroupBox.TabStop = false;
            this.secondGroupBox.Text = "MDC-1200 Second Packet";
            // 
            // secondUnitIDTextBox
            // 
            this.secondUnitIDTextBox.Location = new System.Drawing.Point(160, 16);
            this.secondUnitIDTextBox.MaxLength = 4;
            this.secondUnitIDTextBox.Name = "secondUnitIDTextBox";
            this.secondUnitIDTextBox.Size = new System.Drawing.Size(99, 20);
            this.secondUnitIDTextBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Unit ID:";
            // 
            // secondArgTextBox
            // 
            this.secondArgTextBox.Location = new System.Drawing.Point(36, 45);
            this.secondArgTextBox.MaxLength = 2;
            this.secondArgTextBox.Name = "secondArgTextBox";
            this.secondArgTextBox.Size = new System.Drawing.Size(69, 20);
            this.secondArgTextBox.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Arg:";
            // 
            // secondOpTextBox
            // 
            this.secondOpTextBox.Location = new System.Drawing.Point(36, 16);
            this.secondOpTextBox.MaxLength = 2;
            this.secondOpTextBox.Name = "secondOpTextBox";
            this.secondOpTextBox.Size = new System.Drawing.Size(69, 20);
            this.secondOpTextBox.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Op:";
            // 
            // mdcDebugDouble
            // 
            this.mdcDebugDouble.Location = new System.Drawing.Point(114, 42);
            this.mdcDebugDouble.Name = "mdcDebugDouble";
            this.mdcDebugDouble.Size = new System.Drawing.Size(145, 23);
            this.mdcDebugDouble.TabIndex = 20;
            this.mdcDebugDouble.Text = "Encode Double Packet";
            this.mdcDebugDouble.UseVisualStyleBackColor = true;
            // 
            // firstGroupBox
            // 
            this.firstGroupBox.Controls.Add(this.firstUnitIDTextBox);
            this.firstGroupBox.Controls.Add(this.unitLabel);
            this.firstGroupBox.Controls.Add(this.firstArgTextBox);
            this.firstGroupBox.Controls.Add(this.argLabel);
            this.firstGroupBox.Controls.Add(this.firstOpTextBox);
            this.firstGroupBox.Controls.Add(this.opLabel);
            this.firstGroupBox.Controls.Add(this.mdcDebugSingle);
            this.firstGroupBox.Location = new System.Drawing.Point(12, 12);
            this.firstGroupBox.Name = "firstGroupBox";
            this.firstGroupBox.Size = new System.Drawing.Size(302, 79);
            this.firstGroupBox.TabIndex = 11;
            this.firstGroupBox.TabStop = false;
            this.firstGroupBox.Text = "MDC-1200 First Packet";
            // 
            // firstUnitIDTextBox
            // 
            this.firstUnitIDTextBox.Location = new System.Drawing.Point(191, 16);
            this.firstUnitIDTextBox.MaxLength = 4;
            this.firstUnitIDTextBox.Name = "firstUnitIDTextBox";
            this.firstUnitIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.firstUnitIDTextBox.TabIndex = 15;
            // 
            // unitLabel
            // 
            this.unitLabel.AutoSize = true;
            this.unitLabel.Location = new System.Drawing.Point(142, 19);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(43, 13);
            this.unitLabel.TabIndex = 5;
            this.unitLabel.Text = "Unit ID:";
            // 
            // firstArgTextBox
            // 
            this.firstArgTextBox.Location = new System.Drawing.Point(36, 45);
            this.firstArgTextBox.MaxLength = 2;
            this.firstArgTextBox.Name = "firstArgTextBox";
            this.firstArgTextBox.Size = new System.Drawing.Size(100, 20);
            this.firstArgTextBox.TabIndex = 14;
            // 
            // argLabel
            // 
            this.argLabel.AutoSize = true;
            this.argLabel.Location = new System.Drawing.Point(6, 48);
            this.argLabel.Name = "argLabel";
            this.argLabel.Size = new System.Drawing.Size(26, 13);
            this.argLabel.TabIndex = 3;
            this.argLabel.Text = "Arg:";
            // 
            // firstOpTextBox
            // 
            this.firstOpTextBox.Location = new System.Drawing.Point(36, 16);
            this.firstOpTextBox.MaxLength = 2;
            this.firstOpTextBox.Name = "firstOpTextBox";
            this.firstOpTextBox.Size = new System.Drawing.Size(100, 20);
            this.firstOpTextBox.TabIndex = 13;
            // 
            // opLabel
            // 
            this.opLabel.AutoSize = true;
            this.opLabel.Location = new System.Drawing.Point(6, 19);
            this.opLabel.Name = "opLabel";
            this.opLabel.Size = new System.Drawing.Size(24, 13);
            this.opLabel.TabIndex = 1;
            this.opLabel.Text = "Op:";
            // 
            // mdcDebugSingle
            // 
            this.mdcDebugSingle.Location = new System.Drawing.Point(145, 42);
            this.mdcDebugSingle.Name = "mdcDebugSingle";
            this.mdcDebugSingle.Size = new System.Drawing.Size(146, 23);
            this.mdcDebugSingle.TabIndex = 16;
            this.mdcDebugSingle.Text = "Encode Single Packet";
            this.mdcDebugSingle.UseVisualStyleBackColor = true;
            // 
            // RawMDCEncoder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 102);
            this.Controls.Add(this.secondGroupBox);
            this.Controls.Add(this.firstGroupBox);
            this.MaximumSize = new System.Drawing.Size(613, 141);
            this.MinimumSize = new System.Drawing.Size(613, 141);
            this.Name = "RawMDCEncoder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Raw MDC Encoder";
            this.secondGroupBox.ResumeLayout(false);
            this.secondGroupBox.PerformLayout();
            this.firstGroupBox.ResumeLayout(false);
            this.firstGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox secondGroupBox;
        private System.Windows.Forms.TextBox secondUnitIDTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox secondArgTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox secondOpTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button mdcDebugDouble;
        private System.Windows.Forms.GroupBox firstGroupBox;
        private System.Windows.Forms.TextBox firstUnitIDTextBox;
        private System.Windows.Forms.Label unitLabel;
        private System.Windows.Forms.TextBox firstArgTextBox;
        private System.Windows.Forms.Label argLabel;
        private System.Windows.Forms.TextBox firstOpTextBox;
        private System.Windows.Forms.Label opLabel;
        private System.Windows.Forms.Button mdcDebugSingle;
    }
}