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
    public partial class Brightness2 : Form
    {
        public int brightness;

        public Brightness2()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tbBrightness.Text = trackBar1.Value.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            brightness = Convert.ToInt32(tbBrightness.Text);
            this.Close();
        }
    }
}
