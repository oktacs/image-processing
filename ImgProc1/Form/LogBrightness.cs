using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgProc1
{
    public partial class LogBrightness : Form
    {
        public LogBrightness()
        {
            InitializeComponent();
        }

        private void sbLogBrightness_ValueChanged(object sender, EventArgs e)
        {
            tbLogBrightness.Text = sbLogBrightness.Value.ToString();
        }

        private void tbLogBrightness_TextChanged(object sender, EventArgs e)
        {
            if ((tbLogBrightness.Text == "") || (tbLogBrightness.Text == "-"))
            {
                sbLogBrightness.Value = 0;
                tbLogBrightness.Text = "0";
            }
            else if ((Convert.ToInt16(tbLogBrightness.Text) <= 105) &&
            (Convert.ToInt16(tbLogBrightness.Text) >= 0))
            {
                sbLogBrightness.Value = Convert.ToInt16(tbLogBrightness.Text);
            }
            else
            {
                MessageBox.Show("Input nilai Error");
                tbLogBrightness.Text = "0";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogBrightness_Load(object sender, EventArgs e)
        {
            tbLogBrightness.Text = sbLogBrightness.Value.ToString();
        }
    }
}
