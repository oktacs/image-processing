namespace ImgProc1
{
    partial class LogBrightness
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
            this.sbLogBrightness = new System.Windows.Forms.HScrollBar();
            this.tbLogBrightness = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log Brightness";
            // 
            // sbLogBrightness
            // 
            this.sbLogBrightness.Location = new System.Drawing.Point(104, 26);
            this.sbLogBrightness.Maximum = 114;
            this.sbLogBrightness.Name = "sbLogBrightness";
            this.sbLogBrightness.Size = new System.Drawing.Size(444, 20);
            this.sbLogBrightness.TabIndex = 1;
            this.sbLogBrightness.ValueChanged += new System.EventHandler(this.sbLogBrightness_ValueChanged);
            // 
            // tbLogBrightness
            // 
            this.tbLogBrightness.Location = new System.Drawing.Point(563, 26);
            this.tbLogBrightness.Name = "tbLogBrightness";
            this.tbLogBrightness.Size = new System.Drawing.Size(47, 20);
            this.tbLogBrightness.TabIndex = 2;
            this.tbLogBrightness.TextChanged += new System.EventHandler(this.tbLogBrightness_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(272, 74);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // LogBrightness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 109);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbLogBrightness);
            this.Controls.Add(this.sbLogBrightness);
            this.Controls.Add(this.label1);
            this.Name = "LogBrightness";
            this.Text = "Log Brightness";
            this.Load += new System.EventHandler(this.LogBrightness_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar sbLogBrightness;
        public System.Windows.Forms.TextBox tbLogBrightness;
        private System.Windows.Forms.Button btnOK;
    }
}