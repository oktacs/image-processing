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
    public partial class Brightness : Form
    {
        public int r, g, b;

        public Brightness()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tbR.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            tbG.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            tbB.Text = trackBar3.Value.ToString();
        }

        private void tbR_TextChanged(object sender, EventArgs e)
        {

        }

        private void Brightness_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            r = Convert.ToInt32(tbR.Text);
            g = Convert.ToInt32(tbG.Text);
            b = Convert.ToInt32(tbB.Text);

            this.Close();
        }
    }
}
