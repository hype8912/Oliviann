namespace Oliviann.Windows.Forms
{
    partial class FrmAdvancedMessageBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be
        /// disposed; otherwise, false.</param>
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
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatuStripText = new System.Windows.Forms.ToolStripStatusLabel();
            this.Btn1 = new System.Windows.Forms.Button();
            this.Btn2 = new System.Windows.Forms.Button();
            this.Btn3 = new System.Windows.Forms.Button();
            this.PnlDockButtons = new System.Windows.Forms.Panel();
            this.PnlButtons = new System.Windows.Forms.Panel();
            this.LblText = new System.Windows.Forms.Label();
            this.PicBoxIcon = new System.Windows.Forms.PictureBox();
            this.Worker1 = new System.ComponentModel.BackgroundWorker();
            this.PnlLeft = new System.Windows.Forms.Panel();
            this.PnlCenter = new System.Windows.Forms.Panel();
            this.StatusStrip1.SuspendLayout();
            this.PnlDockButtons.SuspendLayout();
            this.PnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxIcon)).BeginInit();
            this.PnlLeft.SuspendLayout();
            this.PnlCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel,
            this.StatuStripText});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 190);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(390, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 0;
            this.StatusStrip1.Text = "statusStrip1";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.BackColor = System.Drawing.SystemColors.Control;
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(94, 17);
            this.StatusStripLabel.Text = "Time remaining:";
            // 
            // StatuStripText
            // 
            this.StatuStripText.BackColor = System.Drawing.SystemColors.Control;
            this.StatuStripText.Name = "StatuStripText";
            this.StatuStripText.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.StatuStripText.Size = new System.Drawing.Size(4, 17);
            // 
            // Btn1
            // 
            this.Btn1.Location = new System.Drawing.Point(292, 13);
            this.Btn1.Name = "Btn1";
            this.Btn1.Size = new System.Drawing.Size(85, 25);
            this.Btn1.TabIndex = 3;
            this.Btn1.Text = "OK";
            this.Btn1.UseVisualStyleBackColor = true;
            this.Btn1.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Btn2
            // 
            this.Btn2.Location = new System.Drawing.Point(194, 13);
            this.Btn2.Name = "Btn2";
            this.Btn2.Size = new System.Drawing.Size(85, 25);
            this.Btn2.TabIndex = 4;
            this.Btn2.Text = "Button2";
            this.Btn2.UseVisualStyleBackColor = true;
            this.Btn2.Visible = false;
            this.Btn2.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Btn3
            // 
            this.Btn3.Location = new System.Drawing.Point(96, 13);
            this.Btn3.Name = "Btn3";
            this.Btn3.Size = new System.Drawing.Size(85, 25);
            this.Btn3.TabIndex = 5;
            this.Btn3.Text = "Button3";
            this.Btn3.UseVisualStyleBackColor = true;
            this.Btn3.Visible = false;
            this.Btn3.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // PnlDockButtons
            // 
            this.PnlDockButtons.BackColor = System.Drawing.SystemColors.Control;
            this.PnlDockButtons.Controls.Add(this.PnlButtons);
            this.PnlDockButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlDockButtons.Location = new System.Drawing.Point(0, 140);
            this.PnlDockButtons.Name = "PnlDockButtons";
            this.PnlDockButtons.Size = new System.Drawing.Size(390, 50);
            this.PnlDockButtons.TabIndex = 6;
            // 
            // PnlButtons
            // 
            this.PnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlButtons.Controls.Add(this.Btn3);
            this.PnlButtons.Controls.Add(this.Btn2);
            this.PnlButtons.Controls.Add(this.Btn1);
            this.PnlButtons.Location = new System.Drawing.Point(0, 0);
            this.PnlButtons.Name = "PnlButtons";
            this.PnlButtons.Size = new System.Drawing.Size(390, 50);
            this.PnlButtons.TabIndex = 6;
            // 
            // LblText
            // 
            this.LblText.AutoSize = true;
            this.LblText.Location = new System.Drawing.Point(0, 0);
            this.LblText.Margin = new System.Windows.Forms.Padding(6, 15, 6, 0);
            this.LblText.MaximumSize = new System.Drawing.Size(640, 0);
            this.LblText.Name = "LblText";
            this.LblText.Padding = new System.Windows.Forms.Padding(0, 15, 0, 15);
            this.LblText.Size = new System.Drawing.Size(0, 43);
            this.LblText.TabIndex = 7;
            // 
            // PicBoxIcon
            // 
            this.PicBoxIcon.Location = new System.Drawing.Point(20, 20);
            this.PicBoxIcon.Name = "PicBoxIcon";
            this.PicBoxIcon.Size = new System.Drawing.Size(48, 48);
            this.PicBoxIcon.TabIndex = 8;
            this.PicBoxIcon.TabStop = false;
            // 
            // Worker1
            // 
            this.Worker1.WorkerSupportsCancellation = true;
            this.Worker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker1_DoWork);
            // 
            // PnlLeft
            // 
            this.PnlLeft.Controls.Add(this.PicBoxIcon);
            this.PnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlLeft.Location = new System.Drawing.Point(0, 0);
            this.PnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.PnlLeft.Name = "PnlLeft";
            this.PnlLeft.Size = new System.Drawing.Size(91, 140);
            this.PnlLeft.TabIndex = 9;
            // 
            // PnlCenter
            // 
            this.PnlCenter.AutoSize = true;
            this.PnlCenter.Controls.Add(this.LblText);
            this.PnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlCenter.Location = new System.Drawing.Point(91, 0);
            this.PnlCenter.Margin = new System.Windows.Forms.Padding(0);
            this.PnlCenter.Name = "PnlCenter";
            this.PnlCenter.Size = new System.Drawing.Size(299, 140);
            this.PnlCenter.TabIndex = 10;
            // 
            // FrmAdvancedMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(390, 212);
            this.Controls.Add(this.PnlCenter);
            this.Controls.Add(this.PnlLeft);
            this.Controls.Add(this.PnlDockButtons);
            this.Controls.Add(this.StatusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 800);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(396, 240);
            this.Name = "FrmAdvancedMessageBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmAdvancedMessageBox_Load);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.PnlDockButtons.ResumeLayout(false);
            this.PnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxIcon)).EndInit();
            this.PnlLeft.ResumeLayout(false);
            this.PnlCenter.ResumeLayout(false);
            this.PnlCenter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.PictureBox PicBoxIcon;
        private System.Windows.Forms.Button Btn1;
        private System.Windows.Forms.Button Btn2;
        private System.Windows.Forms.Button Btn3;
        private System.Windows.Forms.Panel PnlDockButtons;
        private System.Windows.Forms.Label LblText;
        private System.Windows.Forms.ToolStripStatusLabel StatuStripText;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.Panel PnlButtons;
        private System.ComponentModel.BackgroundWorker Worker1;
        private System.Windows.Forms.Panel PnlLeft;
        private System.Windows.Forms.Panel PnlCenter;
    }
}