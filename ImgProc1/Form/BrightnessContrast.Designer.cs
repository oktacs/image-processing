namespace ImgProc1
{
    partial class frmBrightness
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
            this.label2 = new System.Windows.Forms.Label();
            this.hscBrightness = new System.Windows.Forms.HScrollBar();
            this.hscContrast = new System.Windows.Forms.HScrollBar();
            this.tbBrightness = new System.Windows.Forms.TextBox();
            this.tbContrast = new System.Windows.Forms.TextBox();
            this.tbOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brightness";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Contrast";
            // 
            // hscBrightness
            // 
            this.hscBrightness.Location = new System.Drawing.Point(113, 20);
            this.hscBrightness.Maximum = 127;
            this.hscBrightness.Minimum = -127;
            this.hscBrightness.Name = "hscBrightness";
            this.hscBrightness.Size = new System.Drawing.Size(423, 24);
            this.hscBrightness.TabIndex = 2;
            this.hscBrightness.ValueChanged += new System.EventHandler(this.hscBrightness_ValueChanged);
            // 
            // hscContrast
            // 
            this.hscContrast.Location = new System.Drawing.Point(113, 76);
            this.hscContrast.Maximum = 127;
            this.hscContrast.Minimum = -127;
            this.hscContrast.Name = "hscContrast";
            this.hscContrast.Size = new System.Drawing.Size(423, 24);
            this.hscContrast.TabIndex = 3;
            this.hscContrast.ValueChanged += new System.EventHandler(this.hscContrast_ValueChanged);
            // 
            // tbBrightness
            // 
            this.tbBrightness.Location = new System.Drawing.Point(557, 20);
            this.tbBrightness.Name = "tbBrightness";
            this.tbBrightness.Size = new System.Drawing.Size(44, 20);
            this.tbBrightness.TabIndex = 4;
            this.tbBrightness.Text = "0";
            this.tbBrightness.TextChanged += new System.EventHandler(this.tbBrightness_TextChanged);
            // 
            // tbContrast
            // 
            this.tbContrast.Location = new System.Drawing.Point(557, 80);
            this.tbContrast.Name = "tbContrast";
            this.tbContrast.Size = new System.Drawing.Size(44, 20);
            this.tbContrast.TabIndex = 5;
            this.tbContrast.Text = "0";
            // 
            // tbOK
            // 
            this.tbOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.tbOK.Location = new System.Drawing.Point(287, 119);
            this.tbOK.Name = "tbOK";
            this.tbOK.Size = new System.Drawing.Size(75, 23);
            this.tbOK.TabIndex = 6;
            this.tbOK.Text = "OK";
            this.tbOK.UseVisualStyleBackColor = true;
            this.tbOK.Click += new System.EventHandler(this.tbOK_Click);
            // 
            // frmBrightness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 154);
            this.Controls.Add(this.tbOK);
            this.Controls.Add(this.tbContrast);
            this.Controls.Add(this.tbBrightness);
            this.Controls.Add(this.hscContrast);
            this.Controls.Add(this.hscBrightness);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmBrightness";
            this.Text = "Brightness dan Contrast";
            this.Load += new System.EventHandler(this.frmBrightness_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.HScrollBar hscBrightness;
        private System.Windows.Forms.HScrollBar hscContrast;
        public System.Windows.Forms.TextBox tbBrightness;
        public System.Windows.Forms.TextBox tbContrast;
        private System.Windows.Forms.Button tbOK;
    }
}