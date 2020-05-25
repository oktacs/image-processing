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
    public partial class frmBrightness : Form
    {
        public frmBrightness()
        {
            InitializeComponent();
        }

        private void hscBrightness_ValueChanged(object sender, EventArgs e)
        {
            tbBrightness.Text = hscBrightness.Value.ToString();
        }

        private void tbBrightness_TextChanged(object sender, EventArgs e)
        {
            if ((tbBrightness.Text == "") || (tbBrightness.Text == "-"))
            {
                hscBrightness.Value = 0;
                tbBrightness.Text = "0";
            }
            else if ((Convert.ToInt16(tbBrightness.Text) <= 127) &&
            (Convert.ToInt16(tbBrightness.Text) >= -127))
            {
                hscBrightness.Value = Convert.ToInt16(tbBrightness.Text);
            }
            else
            {
                MessageBox.Show("Input nilai Error");
                tbBrightness.Text = "0";
            }
        }

        private void hscContrast_ValueChanged(object sender, EventArgs e)
        {
            tbContrast.Text = hscContrast.Value.ToString();
        }

        private void tbOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBrightness_Load(object sender, EventArgs e)
        {
            tbBrightness.Text = hscBrightness.Value.ToString();
            tbContrast.Text = hscContrast.Value.ToString();
        }
    }
}
