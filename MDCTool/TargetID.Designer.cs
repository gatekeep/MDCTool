namespace MDCTool
{
    partial class TargetID
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
            this.label1 = new System.Windows.Forms.Label();
            this.targetMDCID = new System.Windows.Forms.TextBox();
            this.transmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Radio ID";
            // 
            // targetMDCID
            // 
            this.targetMDCID.Location = new System.Drawing.Point(117, 12);
            this.targetMDCID.Name = "targetMDCID";
            this.targetMDCID.Size = new System.Drawing.Size(100, 20);
            this.targetMDCID.TabIndex = 1;
            this.targetMDCID.Text = "0001";
            // 
            // transmitButton
            // 
            this.transmitButton.Location = new System.Drawing.Point(241, 10);
            this.transmitButton.Name = "transmitButton";
            this.transmitButton.Size = new System.Drawing.Size(75, 23);
            this.transmitButton.TabIndex = 4;
            this.transmitButton.Text = "Transmit";
            this.transmitButton.UseVisualStyleBackColor = true;
            this.transmitButton.Click += new System.EventHandler(this.transmitButton_Click);
            // 
            // TargetID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 44);
            this.Controls.Add(this.transmitButton);
            this.Controls.Add(this.targetMDCID);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(344, 83);
            this.MinimumSize = new System.Drawing.Size(344, 83);
            this.Name = "TargetID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Target Radio...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox targetMDCID;
        private System.Windows.Forms.Button transmitButton;
    }
}