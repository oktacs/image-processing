using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgProc1
{
    public partial class Form1 : Form
    {
        int threshold;
        Bitmap otsu_bitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void bukaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog bukaFile = new OpenFileDialog();
            bukaFile.Filter = "Image File (*.bmp, *.jpg, *.jpeg, .*png)|*.bmp;*.jpg;*.jpeg;*.png";
            if (DialogResult.OK == bukaFile.ShowDialog())
            {
                this.pbInput.Image = new Bitmap(bukaFile.FileName);
                status1.Text = Path.GetFullPath(bukaFile.FileName);

                int h = pbInput.Image.Height;
                int w = pbInput.Image.Width;
                status2.Text = " | Res. Citra: " + h.ToString() + " x " + w.ToString();
            }
        }

        private void simpanSebagaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbOutput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan disimpan");
            }
            else
            {
                SaveFileDialog simpanFile = new SaveFileDialog();
                simpanFile.Title = "Simpan File Citra";
                simpanFile.Filter = "Image File (*.bmp, *.jpg, *.jpeg)|*.bmp;*.jpg;*.jpeg";
                if (DialogResult.OK == simpanFile.ShowDialog())
                    this.pbOutput.Image.Save(simpanFile.FileName);
            }
        }

        private void keluarAplikasiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        
        private void brightnessContrastToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                frmBrightness frm2 = new frmBrightness();
                if (frm2.ShowDialog() == DialogResult.OK)
                {
                    Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                    int nilaiBrightness = Convert.ToInt16(frm2.tbBrightness.Text);
                    int nilaiContrast = Convert.ToInt16(frm2.tbContrast.Text);
                    progressBar1.Visible = true;
                    for (int i = 0; i < b.Width; i++)
                    {
                        for (int j = 0; j < b.Height; j++)
                        {
                            Color c1 = b.GetPixel(i, j);
                            int r1 = c1.R + nilaiBrightness;
                            int g1 = c1.G + nilaiBrightness;
                            int b1 = c1.G + nilaiBrightness;

                            int red = truncate(r1);
                            int green = truncate(g1);
                            int blue = truncate(b1);
                            b.SetPixel(i, j, Color.FromArgb(red, green, blue));
                            
                            Color c2 = b.GetPixel(i, j);
                            double F = (259 * (nilaiContrast + 255)) / (255 * (259 - nilaiContrast));
                            double Rred = (F * (c2.R - 128) + 128);
                            double Rgreen = (F * (c2.G - 128) + 128);
                            double Rblue = (F * (c2.B - 128) + 128);

                            double red1 = truncateContrast(Rred);
                            double green1 = truncateContrast(Rgreen);
                            double blue1 = truncateContrast(Rblue);

                            int red2 = Convert.ToInt16(red1);
                            int green2 = Convert.ToInt16(green1);
                            int blue2 = Convert.ToInt16(blue1);

                            b.SetPixel(i, j, Color.FromArgb(red2, green2, blue2));
                        }
                        progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                    }
                    progressBar1.Visible = false;
                    this.pbOutput.Image = b;
                }
            }
            
        }
        
        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brightness brightness = new Brightness();
            brightness.ShowDialog();

            Bitmap copy = new Bitmap((Bitmap)this.pbInput.Image);
            olahCitra.keRGB(copy, brightness.r, brightness.g, brightness.b);
            this.pbOutput.Image = copy;
        }

        private static int truncate(int x)
        {
            if (x > 255) x = 255;
            else if (x < 0) x = 0;
            return x;
        }

        private static double truncateContrast(double x)
        {
            if (x > 255) x = 255;
            else if (x < 0) x = 0;
            return x;
        }
        
        private void inverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {

                        Color c1 = b.GetPixel(i, j);
                        int r1 = 255 - c1.R;
                        int g1 = 255 - c1.G;
                        int b1 = 255 - c1.B;
                        b.SetPixel(i, j, Color.FromArgb(r1, g1, b1));

                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Image = b;
            }

        }
        
        private void logBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                LogBrightness frm3 = new LogBrightness();
                if (frm3.ShowDialog() == DialogResult.OK)
                {
                    Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                    double nilaiBrightness = Convert.ToDouble(frm3.tbLogBrightness.Text);
                    progressBar1.Visible = true;
                    for (int i = 0; i < b.Width; i++)
                    {
                        for (int j = 0; j < b.Height; j++)
                        {
                            Color c1 = b.GetPixel(i, j);
                            double r1 = nilaiBrightness * Math.Log10(1 + Math.Abs(c1.R));
                            double g1 = nilaiBrightness * Math.Log10(1 + Math.Abs(c1.G));
                            double b1 = nilaiBrightness * Math.Log10(1 + Math.Abs(c1.B));

                            int red = Convert.ToInt16(r1);
                            int green = Convert.ToInt16(g1);
                            int blue = Convert.ToInt16(b1);

                            b.SetPixel(i, j, Color.FromArgb(red, green, blue));
                        }
                        progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                    }
                    progressBar1.Visible = false;
                    this.pbOutput.Image = b;
                }
            }
        }
        
        private void gammaTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                frmGamma frm4 = new frmGamma();
                if (frm4.ShowDialog() == DialogResult.OK)
                {
                    Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                    double nilaiGamma = Convert.ToDouble(frm4.tbGamma.Text);
                    progressBar1.Visible = true;
                    double r1, g1, b1;
                    bool gray = false;
                    double merah, hijau, biru;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            merah = b.GetPixel(i, j).R;
                            hijau = b.GetPixel(i, j).G;
                            biru = b.GetPixel(i, j).B;
                            if (merah.Equals(hijau).Equals(biru))
                            {
                                gray = true;
                            }
                            else
                            {
                                r1 = 255 * Math.Pow(merah / 255, 1 / nilaiGamma);
                                g1 = 255 * Math.Pow(hijau / 255, 1 / nilaiGamma);
                                b1 = 255 * Math.Pow(biru / 255, 1 / nilaiGamma);
                                b.SetPixel(i, j, Color.FromArgb(Convert.ToInt16(r1), Convert.ToInt16(g1), Convert.ToInt16(b1)));
                            }
                        }
                        progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                    }

                    for (int i = 0; i < b.Width; i++)
                    {
                        for (int j = 0; j < b.Height; j++)
                        {
                            merah = b.GetPixel(i, j).R;
                            hijau = b.GetPixel(i, j).G;
                            biru = b.GetPixel(i, j).B;
                            if (gray == false)
                            {
                                r1 = 255 * Math.Pow(merah / 255, 1 / nilaiGamma);
                                g1 = 255 * Math.Pow(hijau / 255, 1 / nilaiGamma);
                                b1 = 255 * Math.Pow(biru / 255, 1 / nilaiGamma);
                                b.SetPixel(i, j, Color.FromArgb(Convert.ToInt16(r1), Convert.ToInt16(g1), Convert.ToInt16(b1)));
                            }
                            else
                            {
                                r1 = 255 * Math.Pow(merah / 255, 1 / nilaiGamma);
                                b.SetPixel(i, j, Color.FromArgb(Convert.ToInt16(r1), Convert.ToInt16(r1), Convert.ToInt16(r1)));
                            }
                        }
                        progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                    }
                    progressBar1.Visible = false;
                    this.pbOutput.Image = b;
                }
            }
        }
        
        private void averageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        int grayAvg = (c1.R + c1.G + c1.B) / 3;
                        b.SetPixel(i, j, Color.FromArgb(grayAvg, grayAvg, grayAvg));
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Image = b;
            }
        }
        
        private void lightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        int nilaiMax = Math.Max(c1.R, Math.Max(c1.G, c1.B));
                        int nilaiMin = Math.Min(c1.R, Math.Min(c1.G, c1.B));
                        int gLight = (nilaiMax + nilaiMin) / 2;
                        b.SetPixel(i, j, Color.FromArgb(gLight, gLight, gLight));
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Image = b;
            }
        }
        
        private void luminanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan diolah");
            }
            Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
            progressBar1.Visible = true;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color c1 = b.GetPixel(i, j);
                    double gLum = 0.21 * c1.R + 0.72 * c1.G + 0.07 * c1.B;
                    b.SetPixel(i, j, Color.FromArgb(Convert.ToInt16(gLum), Convert.ToInt16(gLum), Convert.ToInt16(gLum)));
                }
                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
            }
            progressBar1.Visible = false;
            this.pbOutput.Image = b;
        }
        
        private void bitDepth(int bit)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak Ada citra yang akan diolah");
            }
            else
            {

                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                double level = 255 / (Math.Pow(2, bit) - 1);
                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        int R = Convert.ToInt16(Math.Round(c1.R / level) * level);
                        int G = Convert.ToInt16(Math.Round(c1.G / level) * level);
                        int B = Convert.ToInt16(Math.Round(c1.B / level) * level);
                        b.SetPixel(i, j, Color.FromArgb(R, G, B));
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Image = b;
            }
        }

        private void bitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitDepth(1);
        }

        private void bitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bitDepth(2);
        }

        private void bitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            bitDepth(3);
        }

        private void bitToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            bitDepth(4);
        }

        private void bitToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bitDepth(5);
        }

        private void bitToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            bitDepth(6);
        }

        private void bitToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            bitDepth(7);
        }
        
        private void averageDenoisingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
            List<Image> pictureArray = new List<Image>();
            foreach (string item in
            Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories))
            {
                Image _image = Image.FromFile(item);
                pictureArray.Add(_image);
            }
            pbInput.Image = pictureArray[0];
            Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
            Bitmap c = new Bitmap((Bitmap)this.pbInput.Image);
            status2.Text = "Res. Citra: " + pbInput.Image.Width + " x " +
            pbInput.Image.Height;
            progressBar1.Visible = true;
            int R, G, B, newR, newG, newB;
            int jumGambar = 50;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    for (int k = 0; k < jumGambar - 1; k++)
                    {
                        b = (Bitmap)pictureArray[k];
                        Color c1 = b.GetPixel(i, j);
                        R += c1.R;
                        G += c1.G;
                        B += c1.B;
                    }
                    newR = R / jumGambar;
                    newG = G / jumGambar;
                    newB = B / jumGambar;
                    c.SetPixel(i, j, Color.FromArgb(Convert.ToInt16(newR), Convert.ToInt16(newG), Convert.ToInt16(newB)));
                }
                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / c.Width);
            }
            progressBar1.Visible = false;
            this.pbOutput.Image = c;
        }

        private static double warnaTerdekat(int pValueR, int pValueG, int pValueB)
        {
            double minDistance = 255 * 255 + 255 * 255 + 255 * 255;
            int palColor, rDiff, gDiff, bDiff;
            double pValueR1 = 0;
            double distance;
            int[,] palletteColor = new int[,] { { 0, 0, 0 }, {255, 0, 0}, {0, 255, 0}, {255, 255, 0},
                {0, 0, 255}, {0, 255, 255}, {255, 0, 255}, {255, 255, 255} };
            for (palColor = 0; palColor <= palletteColor.GetLength(0) - 1; palColor++)
            {
                rDiff = pValueR - palletteColor[palColor, 0];
                gDiff = pValueG - palletteColor[palColor, 1];
                bDiff = pValueB - palletteColor[palColor, 2];

                distance = rDiff * rDiff + gDiff * gDiff + bDiff * bDiff;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    pValueR1 = palColor;
                }
            }
            return pValueR1;
        }
        
        private void nearest8ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                double baru;
                int[,] palletteColor = new int[,] { { 0, 0, 0 }, {255, 0, 0}, {0, 255, 0}, {255, 255, 0},
                {0, 0, 255}, {0, 255, 255}, {255, 0, 255}, {255, 255, 255} };
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                this.pbOutput.Image = b;
                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        baru = warnaTerdekat(c1.R, c1.G, c1.B);
                        b.SetPixel(i, j, Color.FromArgb(palletteColor[Convert.ToInt16(baru), 0], palletteColor[Convert.ToInt16(baru), 1], palletteColor[Convert.ToInt16(baru), 2]));
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Image = b;
            }
        }
        
        private void floydAndSteinbergErrorDiffusionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                int[,] paletteColor = {{ 0, 0, 0 }, {255, 0, 0}, {0, 255, 0}, {255, 255, 0},
                {0, 0, 255}, {0, 255, 255}, {255, 0, 255}, {255, 255, 255}};
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                this.pbOutput.Image = b;
                int merah, hijau, biru;
                double baru, errorR, errorG, errorB;
                progressBar1.Visible = true;
                for (int i = 0; i <= b.Width - 2; i++)
                {
                    for (int j = 0; j <= b.Height - 2; j++)
                    {
                        merah = b.GetPixel(i, j).R;
                        hijau = b.GetPixel(i, j).G;
                        biru = b.GetPixel(i, j).B;
                        baru = warnaTerdekat(merah, hijau, biru);
                        errorR = merah - paletteColor[Convert.ToInt16(baru), 0];
                        errorG = hijau - paletteColor[Convert.ToInt16(baru), 1];
                        errorB = biru - paletteColor[Convert.ToInt16(baru), 2];

                        if (i == 0)
                        {
                            b.SetPixel(i + 1, j, Color.FromArgb(truncate(b.GetPixel(i + 1, j).R + 7 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i + 1, j).G + 7 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i + 1, j).B + 7 / 16 * Convert.ToInt32(errorB))));
                            b.SetPixel(i, j + 1, Color.FromArgb(truncate(b.GetPixel(i, j + 1).R + 5 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i, j + 1).G + 5 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i, j + 1).B + 5 / 16 * Convert.ToInt32(errorB))));
                            b.SetPixel(i + 1, j + 1, Color.FromArgb(truncate(b.GetPixel(i + 1, j + 1).R + 1 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i + 1, j + 1).G + 1 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i + 1, j + 1).B + 1 / 16 * Convert.ToInt32(errorB))));
                        }
                        else
                        {
                            b.SetPixel(i + 1, j, Color.FromArgb(truncate(b.GetPixel(i + 1, j).R + 7 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i + 1, j).G + 7 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i + 1, j).B + 7 / 16 * Convert.ToInt32(errorB))));
                            b.SetPixel(i - 1, j + 1, Color.FromArgb(truncate(b.GetPixel(i - 1, j + 1).R + 3 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i - 1, j + 1).G + 3 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i - 1, j + 1).B + 3 / 16 * Convert.ToInt32(errorB))));
                            b.SetPixel(i, j + 1, Color.FromArgb(truncate(b.GetPixel(i, j + 1).R + 5 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i, j + 1).G + 5 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i, j + 1).B + 5 / 16 * Convert.ToInt32(errorB))));
                            b.SetPixel(i + 1, j + 1, Color.FromArgb(truncate(b.GetPixel(i + 1, j + 1).R + 1 / 16 * Convert.ToInt32(errorR)), truncate(b.GetPixel(i + 1, j + 1).G + 1 / 16 * Convert.ToInt32(errorG)), truncate(b.GetPixel(i + 1, j + 1).B + 1 / 16 * Convert.ToInt32(errorB))));
                        }

                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                this.pbOutput.Refresh();
            }
        }

        private void inputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                Dictionary<Byte, Double> histoR = new Dictionary<Byte, Double>();
                Dictionary<Byte, Double> histoG = new Dictionary<Byte, Double>();
                Dictionary<Byte, Double> histoB = new Dictionary<Byte, Double>();
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                GrayscaleHistogram frm5 = new GrayscaleHistogram();
                RGBHistogram frm6 = new RGBHistogram();
                for (int counter = 0; counter <= 255; counter++)
                {
                    histoR[(Byte)counter] = 0.0;
                    histoG[(Byte)counter] = 0.0;
                    histoB[(Byte)counter] = 0.0;
                }
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        if (histoR.ContainsKey(c1.R))
                        {
                            histoR[c1.R] += 1.0;
                        }
                        if (histoG.ContainsKey(c1.G))
                        {
                            histoG[c1.G] += 1.0;
                        }
                        if (histoB.ContainsKey(c1.R))
                        {
                            histoB[c1.R] += 1.0;
                        }
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                if (histoR.Count == histoG.Count && !histoR.Except(histoG).Any())
                {
                    List<Byte> kunci1 = new List<Byte>(histoR.Keys.ToList());
                    frm5.Show();
                    frm5.chart1.Series["Series1"].Color = Color.Gray;
                    frm5.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                    frm5.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                    foreach (Byte key in kunci1)
                    {
                        histoR[key] = histoR[key] / (b.Width * b.Height);
                        frm5.chart1.Series["Series1"].Points.AddXY(key, histoR[key]);
                    }
                }
                else
                {
                    List<Byte> kunci1 = new List<Byte>(histoR.Keys.ToList());
                    List<Byte> kunci2 = new List<Byte>(histoG.Keys.ToList());
                    List<Byte> kunci3 = new List<Byte>(histoB.Keys.ToList());
                    kunci1.Sort();
                    kunci2.Sort();
                    kunci3.Sort();
                    frm6.Show();
                    foreach (Byte key in kunci1)
                    {
                        histoR[key] = histoR[key] / (b.Width * b.Height);
                        frm6.chart1.Series["Series1"].Points.AddXY(key, histoR[key]);
                    }
                    foreach (Byte key in kunci2)
                    {
                        histoG[key] = histoG[key] / (b.Width * b.Height);
                        frm6.chart2.Series["Series1"].Points.AddXY(key, histoG[key]);
                    }
                    foreach (Byte key in kunci3)
                    {
                        histoB[key] = histoB[key] / (b.Width * b.Height);
                        frm6.chart3.Series["Series1"].Points.AddXY(key, histoB[key]);
                    }
                }
            }
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbOutput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                Dictionary<int, double> HistoR = new Dictionary<int, double>();
                Dictionary<int, double> HistoG = new Dictionary<int, double>();
                Dictionary<int, double> HistoB = new Dictionary<int, double>();

                Bitmap b = new Bitmap((Bitmap)this.pbOutput.Image);
                GrayscaleHistogram histogramGrayscale = new GrayscaleHistogram();
                RGBHistogram histogramColor = new RGBHistogram();

                for (int h = 0; h <= 255; h++)
                {
                    HistoR.Add(h, 0);
                    HistoG.Add(h, 0);
                    HistoB.Add(h, 0);
                }
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j);

                        for (int k = 0; k <= 255; k++)
                        {
                            if (c1.G == k)
                            {
                                HistoG[k] = HistoG[k] + 1;
                            }
                            if (c1.R == k)
                            {
                                HistoR[k] = HistoR[k] + 1;
                            }
                            if (c1.B == k)
                            {
                                HistoB[k] = HistoB[k] + 1;
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                histogramGrayscale.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramGrayscale.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                histogramColor.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                histogramColor.chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                histogramColor.chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart3.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                if (HistoR.Count == HistoG.Count && !HistoR.Except(HistoG).Any())
                {
                    histogramGrayscale.chart1.Series["Series1"].Color = Color.Gray;
                    histogramGrayscale.chart1.Series[0].Points.DataBindXY(HistoR.Keys, HistoR.Values);
                    histogramGrayscale.ShowDialog();
                }
                else
                {
                    histogramColor.chart1.Series["Series1"].Color = Color.Red;
                    histogramColor.chart2.Series["Series1"].Color = Color.Green;
                    histogramColor.chart3.Series["Series1"].Color = Color.Blue;

                    histogramColor.chart1.Series[0].Points.DataBindXY(HistoR.Keys, HistoR.Values);
                    histogramColor.chart2.Series[0].Points.DataBindXY(HistoG.Keys, HistoG.Values);
                    histogramColor.chart3.Series[0].Points.DataBindXY(HistoB.Keys, HistoB.Values);
                    histogramColor.ShowDialog();
                }
            }
        }

        private void input()
        {
            if (pbInput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                Dictionary<Byte, Double> histoR = new Dictionary<Byte, Double>();
                Dictionary<Byte, Double> histoG = new Dictionary<Byte, Double>();
                Dictionary<Byte, Double> histoB = new Dictionary<Byte, Double>();
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                GrayscaleHistogram frm5 = new GrayscaleHistogram();
                RGBHistogram frm6 = new RGBHistogram();
                for (int counter = 0; counter <= 255; counter++)
                {
                    histoR[(Byte)counter] = 0.0;
                    histoG[(Byte)counter] = 0.0;
                    histoB[(Byte)counter] = 0.0;
                }
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        Color c1 = b.GetPixel(i, j);
                        if (histoR.ContainsKey(c1.R))
                        {
                            histoR[c1.R] += 1.0;
                        }
                        if (histoG.ContainsKey(c1.G))
                        {
                            histoG[c1.G] += 1.0;
                        }
                        if (histoB.ContainsKey(c1.R))
                        {
                            histoB[c1.R] += 1.0;
                        }
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                if (histoR.Count == histoG.Count && !histoR.Except(histoG).Any())
                {
                    List<Byte> kunci1 = new List<Byte>(histoR.Keys.ToList());
                    frm5.Show();
                    frm5.chart1.Series["Series1"].Color = Color.Gray;
                    frm5.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                    frm5.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                    foreach (Byte key in kunci1)
                    {
                        histoR[key] = histoR[key] / (b.Width * b.Height);
                        frm5.chart1.Series["Series1"].Points.AddXY(key, histoR[key]);
                    }
                }
                else
                {
                    List<Byte> kunci1 = new List<Byte>(histoR.Keys.ToList());
                    List<Byte> kunci2 = new List<Byte>(histoG.Keys.ToList());
                    List<Byte> kunci3 = new List<Byte>(histoB.Keys.ToList());
                    kunci1.Sort();
                    kunci2.Sort();
                    kunci3.Sort();
                    frm6.Show();
                    foreach (Byte key in kunci1)
                    {
                        histoR[key] = histoR[key] / (b.Width * b.Height);
                        frm6.chart1.Series["Series1"].Points.AddXY(key, histoR[key]);
                    }
                    foreach (Byte key in kunci2)
                    {
                        histoG[key] = histoG[key] / (b.Width * b.Height);
                        frm6.chart2.Series["Series1"].Points.AddXY(key, histoG[key]);
                    }
                    foreach (Byte key in kunci3)
                    {
                        histoB[key] = histoB[key] / (b.Width * b.Height);
                        frm6.chart3.Series["Series1"].Points.AddXY(key, histoB[key]);
                    }
                }
            }
        }

        private void output()
        {
            if (pbOutput.Image == null)
                MessageBox.Show("Tidak ada citra yang akan diolah");
            else
            {
                Dictionary<int, double> HistoR = new Dictionary<int, double>();
                Dictionary<int, double> HistoG = new Dictionary<int, double>();
                Dictionary<int, double> HistoB = new Dictionary<int, double>();

                Bitmap b = new Bitmap((Bitmap)this.pbOutput.Image);
                GrayscaleHistogram histogramGrayscale = new GrayscaleHistogram();
                RGBHistogram histogramColor = new RGBHistogram();

                for (int h = 0; h <= 255; h++)
                {
                    HistoR.Add(h, 0);
                    HistoG.Add(h, 0);
                    HistoB.Add(h, 0);
                }
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c1 = b.GetPixel(i, j); 

                        for (int k = 0; k <= 255; k++)
                        {
                            if (c1.G == k)
                            {
                                HistoG[k] = HistoG[k] + 1;
                            }
                            if (c1.R == k)
                            {
                                HistoR[k] = HistoR[k] + 1;
                            }
                            if (c1.B == k)
                            {
                                HistoB[k] = HistoB[k] + 1;
                            }
                        }
                    }
                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }
                progressBar1.Visible = false;
                histogramGrayscale.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramGrayscale.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                histogramColor.chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                histogramColor.chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
                histogramColor.chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
                histogramColor.chart3.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

                if (HistoR.Count == HistoG.Count && !HistoR.Except(HistoG).Any())
                {
                    histogramGrayscale.chart1.Series["Series1"].Color = Color.Gray;
                    histogramGrayscale.chart1.Series[0].Points.DataBindXY(HistoR.Keys, HistoR.Values);
                    histogramGrayscale.ShowDialog();
                }
                else
                {
                    histogramColor.chart1.Series["Series1"].Color = Color.Red;
                    histogramColor.chart2.Series["Series1"].Color = Color.Green;
                    histogramColor.chart3.Series["Series1"].Color = Color.Blue;

                    histogramColor.chart1.Series[0].Points.DataBindXY(HistoR.Keys, HistoR.Values);
                    histogramColor.chart2.Series[0].Points.DataBindXY(HistoG.Keys, HistoG.Values);
                    histogramColor.chart3.Series[0].Points.DataBindXY(HistoB.Keys, HistoB.Values);
                    histogramColor.ShowDialog();
                }
            }
        }

        private void inputOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            input();
            output();
        }

        private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang diolah");
            }
            else
            {
                Dictionary<byte, double> histoR = new Dictionary<byte, double>();
                Dictionary<byte, double> histoG = new Dictionary<byte, double>();
                Dictionary<byte, double> histoB = new Dictionary<byte, double>();

                Bitmap image1 = new Bitmap(pbInput.Image);
                pbOutput.Image = image1;
                int baris, kolom;

                RGBHistogram frm3 = new RGBHistogram();
                GrayscaleHistogram frm2 = new GrayscaleHistogram();

                byte c1, c2, c3;
                double[] s1 = new double[256];
                double[] s2 = new double[256];
                double[] s3 = new double[256];
                double jum = 0;

                for (int counter = 0; counter <= 255; counter++)
                {
                    histoR[(Byte)counter] = 0.0;
                    histoG[(Byte)counter] = 0.0;
                    histoB[(Byte)counter] = 0.0;
                }

                for (baris = 0; baris < image1.Width; baris++)
                {
                    for (kolom = 0; kolom < image1.Height; kolom++)
                    {
                        c1 = image1.GetPixel(baris, kolom).R;
                        c2 = image1.GetPixel(baris, kolom).G;
                        c3 = image1.GetPixel(baris, kolom).B;

                        if (histoR.ContainsKey(c1))
                        {
                            histoR[c1] += 1;
                        }
                        if (histoG.ContainsKey(c1))
                        {
                            histoG[c1] += 1;
                        }
                        if (histoB.ContainsKey(c1))
                        {
                            histoB[c1] += 1;
                        }
                    }
                }
                
                List<byte> kunci1 = histoR.Keys.ToList();
                List<byte> kunci2 = histoG.Keys.ToList();
                List<byte> kunci3 = histoB.Keys.ToList();
                foreach (byte key in kunci1)
                {
                    histoR[key] = histoR[key] / (image1.Width * image1.Height);
                    jum += 255 * histoR[key];
                    s1[key] = jum;
                }
                jum = 0;
                foreach (byte key in kunci2)
                {
                    histoG[key] = histoG[key] / (image1.Width * image1.Height);
                    jum += 255 * histoG[key];
                    s2[key] = jum;
                }
                jum = 0;
                foreach (byte key in kunci3)
                {
                    histoB[key] = histoB[key] / (image1.Width * image1.Height);
                    jum += 255 * histoB[key];
                    s3[key] = jum;
                }
                progressBar1.Visible = true;
                for (baris = 0; baris < image1.Width; baris++)
                {
                    for (kolom = 0; kolom < image1.Height; kolom++)
                    {
                        c1 = image1.GetPixel(baris, kolom).R;
                        c2 = image1.GetPixel(baris, kolom).G;
                        c3 = image1.GetPixel(baris, kolom).B;
                        int s = Convert.ToInt16(s1[c1]);
                        int ss = Convert.ToInt16(s2[c2]);
                        int sss = Convert.ToInt16(s3[c3]);
                        image1.SetPixel(baris, kolom, Color.FromArgb(s, ss, sss));
                    }
                    progressBar1.Value = Convert.ToInt32(Math.Floor((double)(100 * (baris + 1) /
                    image1.Width)));
                }
                progressBar1.Visible = false;
                this.pbOutput.Refresh();
            }
        }

        private void fuzzyHEGrayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Dictionary<byte, double> histoR = new Dictionary<byte, double>();
                Dictionary<byte, double> histoG = new Dictionary<byte, double>();
                Dictionary<byte, double> histoB = new Dictionary<byte, double>();

                int cAbu;

                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                this.pbOutput.Image = b;

                GrayscaleHistogram histoGray = new GrayscaleHistogram();

                progressBar1.Visible = true;

                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {

                        Color c1 = b.GetPixel(i, j);
                        cAbu = (c1.R + c1.G + c1.B) / 3;

                        double woaAbu = fuzzy(cAbu);

                        int woaAbuFix = Convert.ToInt16(Math.Round(woaAbu, 0));
                        b.SetPixel(i, j, Color.FromArgb(woaAbuFix, woaAbuFix, woaAbuFix));
                    }

                    progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
                }

                progressBar1.Visible = false;
                this.pbOutput.Refresh();
            }
        }

        private void fuzzyHERGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {  
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                this.pbOutput.Image = b;

                int cRed, cGreen, cBlue;
                Color c;

                Dictionary<byte, double> histoR = new Dictionary<byte, double>();
                Dictionary<byte, double> histoG = new Dictionary<byte, double>();
                Dictionary<byte, double> histoB = new Dictionary<byte, double>();

                RGBHistogram histoRGB = new RGBHistogram();

                progressBar1.Visible = true;
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        c = b.GetPixel(i, j);
                        cRed = c.R;
                        cGreen = c.G;
                        cBlue = c.B;

                        double woaRed = fuzzy(cRed);
                        double woaGreen = fuzzy(cGreen);
                        double woaBlue = fuzzy(cBlue);

                        int woaRedFix = Convert.ToInt16(Math.Round(woaRed, 0));
                        int woaGreenFix = Convert.ToInt16(Math.Round(woaGreen, 0));
                        int woaBlueFix = Convert.ToInt16(Math.Round(woaBlue, 0));

                        b.SetPixel(i, j, Color.FromArgb(woaRedFix, woaGreenFix, woaBlueFix));
                    }
                    progressBar1.Value = Convert.ToInt32(Math.Floor((double)(100 * (i + 1) / b.Width)));
                }
                progressBar1.Visible = false;
                this.pbOutput.Refresh();
            }
        }

        private int fuzzy(int color)
        {
            double batasA, batasB, woa, wob, hasilFuzzy1, hasilFuzzy2, woaColor;
            batasA = batasB = woa = wob = hasilFuzzy1 = hasilFuzzy2 = woaColor = 0;
            
            if (color >= 0 && color <= 63)
            {
                batasA = 0;
                batasB = 63;
                woa = 0;
                wob = 0;
            }
            else if (color >= 64 && color <= 126)
            {
                batasA = 63;
                batasB = 127;
                woa = 0;
                wob = 127;
            }
            else if (color >= 128 && color <= 190)
            {
                batasA = 127;
                batasB = 191;
                woa = 127;
                wob = 255;
            }
            else if (color >= 191 && color <= 255)
            {
                batasA = 191;
                batasB = 255;
                woa = 255;
                wob = 255;
            }
            
            hasilFuzzy1 = (color - batasA) / (batasB - batasA);
            hasilFuzzy2 = -(color - batasB) / (batasB - batasA);
            
            if (color >= 0 && color <= 63)
            {
                woaColor = 0;
            }
            else if (color >= 64 && color <= 126)
            {
                woaColor = ((hasilFuzzy1 * wob) + (hasilFuzzy2 * woa)) / (hasilFuzzy1 + hasilFuzzy2);
            }
            else if (color == 127)
            {
                woaColor = 127;
            }
            else if (color >= 128 && color <= 190)
            {
                woaColor = ((hasilFuzzy1 * wob) + (hasilFuzzy2 * woa)) / (hasilFuzzy1 + hasilFuzzy2);
            }
            else if (color >= 191 && color <= 255)
            {
                woaColor = 255;
            }

            return Convert.ToInt16(Math.Round(woaColor));
        }
        
        private Bitmap konvolusiFilter3x3(Bitmap bit, int[] kernel)
        {
            Bitmap b = new Bitmap(bit);

            bool b1, b2, b3, b4, b5, b6, b7, b8, b9;
            Color c1, c2, c3, c4, c5, c6, c7, c8, c9;
            float sumR, sumG, sumB;
            int red, green, blue;
            int w = bit.Width - 1;
            int h = bit.Height - 1;

            int blok = 0;

            progressBar1.Visible = true;
            for (int i = 0; i <= w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = false;
                    sumR = sumG = sumB = 0;
                    blok = 0;
                    
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = true;
                            b8 = b9 = true;
                        }
                        
                        else if (0 < i && i < w)
                        {
                            b4 = b5 = b6 = true;
                            b7 = b8 = b9 = true;
                        }
                        
                        else if (i == w)
                        {
                            b4 = b5 = true;
                            b7 = b8 = true;
                        }
                    }
                    
                    else if (0 < j && j < h)
                    {
                        if (i == 0)
                        {
                            b2 = b3 = true;
                            b5 = b6 = true;
                            b8 = b9 = true;
                        }

                        else if (0 < i && i < w)
                        {
                            b1 = b2 = b3 = true;
                            b4 = b5 = b6 = true;
                            b7 = b8 = b9 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = true;
                            b4 = b5 = true;
                            b7 = b8 = true;
                        }
                    }
                    
                    else if (j == h)
                    {
                        if (i == 0)
                        {
                            b2 = b3 = true;
                            b5 = b6 = true;
                        }
                        
                        else if (0 < i && i < w)
                        {
                            b1 = b2 = b3 = true;
                            b4 = b5 = b6 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = true;
                            b4 = b5 = true;
                        }
                    }

                    if (b1)
                    {
                        c1 = bit.GetPixel(i - 1, j - 1);
                        sumR += (c1.R * kernel[0]);
                        sumG += (c1.G * kernel[0]);
                        sumB += (c1.B * kernel[0]);
                        blok += kernel[0];
                    }

                    if (b2)
                    {
                        c2 = bit.GetPixel(i, j - 1);
                        sumR += (c2.R * kernel[1]);
                        sumG += (c2.G * kernel[1]);
                        sumB += (c2.B * kernel[1]);
                        blok += kernel[1];
                    }

                    if (b3)
                    {
                        c3 = bit.GetPixel(i + 1, j - 1);
                        sumR += (c3.R * kernel[2]);
                        sumG += (c3.G * kernel[2]);
                        sumB += (c3.B * kernel[2]);
                        blok += kernel[2];
                    }

                    if (b4)
                    {
                        c4 = bit.GetPixel(i - 1, j);
                        sumR += (c4.R * kernel[3]);
                        sumG += (c4.G * kernel[3]);
                        sumB += (c4.B * kernel[3]);
                        blok += kernel[3];
                    }

                    if (b5)
                    {
                        c5 = bit.GetPixel(i, j);
                        sumR += (c5.R * kernel[4]);
                        sumG += (c5.G * kernel[4]);
                        sumB += (c5.B * kernel[4]);
                        blok += kernel[4];
                    }

                    if (b6)
                    {
                        c6 = bit.GetPixel(i + 1, j);
                        sumR += (c6.R * kernel[5]);
                        sumG += (c6.G * kernel[5]);
                        sumB += (c6.B * kernel[5]);
                        blok += kernel[5];
                    }

                    if (b7)
                    {
                        c7 = bit.GetPixel(i - 1, j + 1);
                        sumR += (c7.R * kernel[6]);
                        sumG += (c7.G * kernel[6]);
                        sumB += (c7.B * kernel[6]);
                        blok += kernel[6];
                    }

                    if (b8)
                    {
                        c8 = bit.GetPixel(i, j + 1);
                        sumR += (c8.R * kernel[7]);
                        sumG += (c8.G * kernel[7]);
                        sumB += (c8.B * kernel[7]);
                        blok += kernel[7];
                    }

                    if (b9)
                    {
                        c9 = bit.GetPixel(i + 1, j + 1);
                        sumR += (c9.R * kernel[8]);
                        sumG += (c9.G * kernel[8]);
                        sumB += (c9.B * kernel[8]);
                        blok += kernel[8];
                    }

                    red = blok != 0 ? (int)sumR / blok : (int)sumR;
                    green = blok != 0 ? (int)sumG / blok : (int)sumG;
                    blue = blok != 0 ? (int)sumB / blok : (int)sumB;

                    red = truncate(red);
                    green = truncate(green);
                    blue = truncate(blue);

                    b.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / bit.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        
        private Bitmap konvolusiFilter5x5(Bitmap bit, int[] kernel)
        {
            Bitmap b = new Bitmap(bit);

            bool b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20, b21, b22, b23, b24, b25;
            Color c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18, c19, c20, c21, c22, c23, c24, c25;
            float sumR, sumG, sumB;
            int red, green, blue;
            int w = b.Width - 1;
            int h = b.Height - 1;
            int blok = 0;

            progressBar1.Visible = true;
            for (int i = 0; i <= w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    b1 = b2 = b3 = b4 = b5 = false;
                    b6 = b7 = b8 = b9 = b10 = false;
                    b11 = b12 = b13 = b14 = b15 = false;
                    b16 = b17 = b18 = b19 = b20 = false;
                    b21 = b22 = b23 = b24 = b25 = false;

                    sumR = sumG = sumB = 0;
                    blok = 0;
                    
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (j == 1)
                    {
                        if (i == 0)
                        {
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (1 < j && j < h - 1)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }

                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (j == h - 1)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                        }
                    }
                    
                    else if (j == h)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                        }

                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                        }
                    }

                    if (b1)
                    {
                        c1 = b.GetPixel(i - 2, j - 2);
                        sumR += (c1.R * kernel[0]);
                        sumG += (c1.G * kernel[0]);
                        sumB += (c1.B * kernel[0]);
                        blok += kernel[0];
                    }

                    if (b2)
                    {
                        c2 = b.GetPixel(i - 1, j - 2);
                        sumR += (c2.R * kernel[1]);
                        sumG += (c2.G * kernel[1]);
                        sumB += (c2.B * kernel[1]);
                        blok += kernel[1];
                    }

                    if (b3)
                    {
                        c3 = b.GetPixel(i, j - 2);
                        sumR += (c3.R * kernel[2]);
                        sumG += (c3.G * kernel[2]);
                        sumB += (c3.B * kernel[2]);
                        blok += kernel[2];
                    }

                    if (b4)
                    {
                        c4 = b.GetPixel(i + 1, j - 2);
                        sumR += (c4.R * kernel[3]);
                        sumG += (c4.G * kernel[3]);
                        sumB += (c4.B * kernel[3]);
                        blok += kernel[3];
                    }

                    if (b5)
                    {
                        c5 = b.GetPixel(i + 2, j - 2);
                        sumR += (c5.R * kernel[4]);
                        sumG += (c5.G * kernel[4]);
                        sumB += (c5.B * kernel[4]);
                        blok += kernel[4];
                    }

                    if (b6)
                    {
                        c6 = b.GetPixel(i - 2, j - 1);
                        sumR += (c6.R * kernel[5]);
                        sumG += (c6.G * kernel[5]);
                        sumB += (c6.B * kernel[5]);
                        blok += kernel[5];
                    }

                    if (b7)
                    {
                        c7 = b.GetPixel(i - 1, j - 1);
                        sumR += (c7.R * kernel[6]);
                        sumG += (c7.G * kernel[6]);
                        sumB += (c7.B * kernel[6]);
                        blok += kernel[6];
                    }

                    if (b8)
                    {
                        c8 = b.GetPixel(i, j - 1);
                        sumR += (c8.R * kernel[7]);
                        sumG += (c8.G * kernel[7]);
                        sumB += (c8.B * kernel[7]);
                        blok += kernel[7];
                    }

                    if (b9)
                    {
                        c9 = b.GetPixel(i + 1, j - 1);
                        sumR += (c9.R * kernel[8]);
                        sumG += (c9.G * kernel[8]);
                        sumB += (c9.B * kernel[8]);
                        blok += kernel[8];
                    }

                    if (b10)
                    {
                        c10 = b.GetPixel(i + 2, j - 1);
                        sumR += (c10.R * kernel[9]);
                        sumG += (c10.G * kernel[9]);
                        sumB += (c10.B * kernel[9]);
                        blok += kernel[9];
                    }

                    if (b11)
                    {
                        c11 = b.GetPixel(i - 2, j);
                        sumR += (c11.R * kernel[10]);
                        sumG += (c11.G * kernel[10]);
                        sumB += (c11.B * kernel[10]);
                        blok += kernel[10];
                    }

                    if (b12)
                    {
                        c12 = b.GetPixel(i - 1, j);
                        sumR += (c12.R * kernel[11]);
                        sumG += (c12.G * kernel[11]);
                        sumB += (c12.B * kernel[11]);
                        blok += kernel[11];
                    }

                    if (b13)
                    {
                        c13 = b.GetPixel(i, j);
                        sumR += (c13.R * kernel[12]);
                        sumG += (c13.G * kernel[12]);
                        sumB += (c13.B * kernel[12]);
                        blok += kernel[12];
                    }

                    if (b14)
                    {
                        c14 = b.GetPixel(i + 1, j);
                        sumR += (c14.R * kernel[13]);
                        sumG += (c14.G * kernel[13]);
                        sumB += (c14.B * kernel[13]);
                        blok += kernel[13];
                    }

                    if (b15)
                    {
                        c15 = b.GetPixel(i + 2, j);
                        sumR += (c15.R * kernel[14]);
                        sumG += (c15.G * kernel[14]);
                        sumB += (c15.B * kernel[14]);
                        blok += kernel[14];
                    }

                    if (b16)
                    {
                        c16 = b.GetPixel(i - 2, j + 1);
                        sumR += (c16.R * kernel[15]);
                        sumG += (c16.G * kernel[15]);
                        sumB += (c16.B * kernel[15]);
                        blok += kernel[15];
                    }

                    if (b17)
                    {
                        c17 = b.GetPixel(i - 1, j + 1);
                        sumR += (c17.R * kernel[16]);
                        sumG += (c17.G * kernel[16]);
                        sumB += (c17.B * kernel[16]);
                        blok += kernel[16];
                    }

                    if (b18)
                    {
                        c18 = b.GetPixel(i, j + 1);
                        sumR += (c18.R * kernel[17]);
                        sumG += (c18.G * kernel[17]);
                        sumB += (c18.B * kernel[17]);
                        blok += kernel[17];
                    }

                    if (b19)
                    {
                        c19 = b.GetPixel(i + 1, j + 1);
                        sumR += (c19.R * kernel[18]);
                        sumG += (c19.G * kernel[18]);
                        sumB += (c19.B * kernel[18]);
                        blok += kernel[18];
                    }

                    if (b20)
                    {
                        c20 = b.GetPixel(i + 2, j + 1);
                        sumR += (c20.R * kernel[19]);
                        sumG += (c20.G * kernel[19]);
                        sumB += (c20.B * kernel[19]);
                        blok += kernel[19];
                    }

                    if (b21)
                    {
                        c21 = b.GetPixel(i - 2, j + 2);
                        sumR += (c21.R * kernel[20]);
                        sumG += (c21.G * kernel[20]);
                        sumB += (c21.B * kernel[20]);
                        blok += kernel[20];
                    }

                    if (b22)
                    {
                        c22 = b.GetPixel(i - 1, j + 2);
                        sumR += (c22.R * kernel[21]);
                        sumG += (c22.G * kernel[21]);
                        sumB += (c22.B * kernel[21]);
                        blok += kernel[21];
                    }

                    if (b23)
                    {
                        c23 = b.GetPixel(i, j + 2);
                        sumR += (c23.R * kernel[22]);
                        sumG += (c23.G * kernel[22]);
                        sumB += (c23.B * kernel[22]);
                        blok += kernel[22];
                    }

                    if (b24)
                    {
                        c24 = b.GetPixel(i + 1, j + 2);
                        sumR += (c24.R * kernel[23]);
                        sumG += (c24.G * kernel[23]);
                        sumB += (c24.B * kernel[23]);
                        blok += kernel[23];
                    }

                    if (b25)
                    {
                        c25 = b.GetPixel(i + 2, j + 2);
                        sumR += (c25.R * kernel[24]);
                        sumG += (c25.G * kernel[24]);
                        sumB += (c25.B * kernel[24]);
                        blok += kernel[24];
                    }

                    red = blok != 0 ? (int)sumR / blok : (int)sumR;
                    green = blok != 0 ? (int)sumG / blok : (int)sumG;
                    blue = blok != 0 ? (int)sumB / blok : (int)sumB;

                    red = truncate(red);
                    green = truncate(green);
                    blue = truncate(blue);

                    b.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
            }
            progressBar1.Visible = false;

            return b;
        }
        
        private void highPassFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    -1, 0, 1,
                    -1, 0, 3,
                    -3, 0, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void identityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    0, 0, 0,
                    0, 1, 0,
                    0, 0, 0
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void edgeDetection1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    1, 0, -1,
                    0, 0, 0,
                    -1, 0, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void edgeDetection2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    0, 1, 0,
                    1, -4, 1,
                    0, 1, 0
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void edgeDetection3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    -1, -1, -1,
                    -1, 8, -1,
                    -1, -1, -1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    0, -1, 0,
                    -1, 5, -1,
                    0, -1, 0
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void gaussianBlur3x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    1, 2, 1,
                    2, 4, 2,
                    1, 2, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void gaussianBlur5x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[25] {
                    1, 4, 6, 4, 1,
                    4, 16, 24,16, 4,
                    6, 24, 36, 24, 6,
                    4, 16, 24,16, 4,
                    1, 4, 6, 4, 1
                };

                this.pbOutput.Image = konvolusiFilter5x5(b, kernel);
            }
        }

        private void unsharpMaskingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[25] {
                    1, 4, 6, 4, 1,
                    4, 16, 24,16, 4,
                    6, 24, -476, 24, 6,
                    4, 16, 24,16, 4,
                    1, 4, 6, 4, 1
                };

                this.pbOutput.Image = konvolusiFilter5x5(b, kernel);
            }
        }

        private Bitmap dimensi3x3(Bitmap bit, int[] se, string str = "")
        {
            Bitmap b = new Bitmap(bit);
            bool b1, b2, b3, b4, b5, b6, b7, b8, b9;
            Color c;
            float sumR, sumG, sumB;
            List<int> listR = new List<int>();
            List<int> listG = new List<int>();
            List<int> listB = new List<int>();
            int merah = 0, hijau = 0, biru = 0;

            int w = bit.Width - 1;
            int h = bit.Height - 1;

            int blok = 0;

            progressBar1.Visible = true;
            for (int i = 0; i <= w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    b1 = b2 = b3 = false;
                    b4 = b5 = b6 = false;
                    b7 = b8 = b9 = false;
                    sumR = sumG = sumB = 0;
                    blok = 0;

                    listR.Clear();
                    listG.Clear();
                    listB.Clear();
                    
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = true;
                            b8 = b9 = true;
                        }
                        
                        else if (0 < i && i < w)
                        {
                            b4 = b5 = b6 = true;
                            b7 = b8 = b9 = true;
                        }
                        
                        else if (i == w)
                        {
                            b4 = b5 = true;
                            b7 = b8 = true;
                        }
                    }
                    
                    else if (0 < j && j < h)
                    {
                        if (i == 0)
                        {
                            b2 = b3 = true;
                            b5 = b6 = true;
                            b8 = b9 = true;
                        }
                        
                        else if (0 < i && i < w)
                        {
                            b1 = b2 = b3 = true;
                            b4 = b5 = b6 = true;
                            b7 = b8 = b9 = true;
                        }

                        
                        else if (i == w)
                        {
                            b1 = b2 = true;
                            b4 = b5 = true;
                            b7 = b8 = true;
                        }
                    }
                    
                    else if (j == h)
                    {
                        if (i == 0)
                        {
                            b2 = b3 = true;
                            b5 = b6 = true;
                        }
                        
                        else if (0 < i && i < w)
                        {
                            b1 = b2 = b3 = true;
                            b4 = b5 = b6 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = true;
                            b4 = b5 = true;
                        }
                    }

                    if (b1)
                    {
                        c = bit.GetPixel(i - 1, j - 1);
                        listR.Add(c.R * se[0]);
                        listG.Add(c.G * se[0]);
                        listB.Add(c.B * se[0]);
                        sumR += (c.R * se[0]);
                        sumG += (c.G * se[0]);
                        sumB += (c.B * se[0]);
                        blok += se[0];
                    }

                    if (b2)
                    {
                        c = bit.GetPixel(i, j - 1);
                        listR.Add(c.R * se[1]);
                        listG.Add(c.G * se[1]);
                        listB.Add(c.B * se[1]);
                        sumR += (c.R * se[1]);
                        sumG += (c.G * se[1]);
                        sumB += (c.B * se[1]);
                        blok += se[1];
                    }

                    if (b3)
                    {
                        c = bit.GetPixel(i + 1, j - 1);
                        listR.Add(c.R * se[2]);
                        listG.Add(c.G * se[2]);
                        listB.Add(c.B * se[2]);
                        sumR += (c.R * se[2]);
                        sumG += (c.G * se[2]);
                        sumB += (c.B * se[2]);
                        blok += se[2];
                    }
                    
                    if (b4)
                    {
                        c = bit.GetPixel(i - 1, j);
                        listR.Add(c.R * se[3]);
                        listG.Add(c.G * se[3]);
                        listB.Add(c.B * se[3]);
                        sumR += (c.R * se[3]);
                        sumG += (c.G * se[3]);
                        sumB += (c.B * se[3]);
                        blok += se[3];
                    }

                    if (b5)
                    {
                        c = bit.GetPixel(i, j);
                        listR.Add(c.R * se[4]);
                        listG.Add(c.G * se[4]);
                        listB.Add(c.B * se[4]);
                        sumR += (c.R * se[4]);
                        sumG += (c.G * se[4]);
                        sumB += (c.B * se[4]);
                        blok += se[4];
                    }

                    if (b6)
                    {
                        c = bit.GetPixel(i + 1, j);
                        listR.Add(c.R * se[5]);
                        listG.Add(c.G * se[5]);
                        listB.Add(c.B * se[5]);
                        sumR += (c.R * se[5]);
                        sumG += (c.G * se[5]);
                        sumB += (c.B * se[5]);
                        blok += se[5];
                    }
                    
                    if (b7)
                    {
                        c = bit.GetPixel(i - 1, j + 1);
                        listR.Add(c.R * se[6]);
                        listG.Add(c.G * se[6]);
                        listB.Add(c.B * se[6]);
                        sumR += (c.R * se[6]);
                        sumG += (c.G * se[6]);
                        sumB += (c.B * se[6]);
                        blok += se[6];
                    }

                    if (b8)
                    {
                        c = bit.GetPixel(i, j + 1);
                        listR.Add(c.R * se[7]);
                        listG.Add(c.G * se[7]);
                        listB.Add(c.B * se[7]);
                        sumR += (c.R * se[7]);
                        sumG += (c.G * se[7]);
                        sumB += (c.B * se[7]);
                        blok += se[7];
                    }

                    if (b9)
                    {
                        c = bit.GetPixel(i + 1, j + 1);
                        sumR += (c.R * se[8]);
                        sumG += (c.G * se[8]);
                        sumB += (c.B * se[8]);
                        blok += se[8];
                    }

                    if (str.Equals(""))
                    {
                        merah = blok != 0 ? (int)sumR / blok : (int)sumR;
                        hijau = blok != 0 ? (int)sumG / blok : (int)sumG;
                        biru = blok != 0 ? (int)sumB / blok : (int)sumB;
                    }
                    else
                    {
                        if (str.Equals("erosi"))
                        {
                            merah = erosi((int)sumR, blok);
                            hijau = erosi((int)sumG, blok);
                            biru = erosi((int)sumB, blok);
                        }

                        if (str.Equals("dilasi"))
                        {
                            merah = dilasi((int)sumR, blok);
                            hijau = dilasi((int)sumG, blok);
                            biru = dilasi((int)sumB, blok);
                        }

                        else if (str.Equals("grayscale_dilasi"))
                        {
                            merah = listR.Max();
                            hijau = listG.Max();
                            biru = listB.Max();
                        }
                        else if (str.Equals("grayscale_erosi"))
                        {
                            merah = listR.Min();
                            hijau = listG.Min();
                            biru = listB.Min();
                        }
                    }

                    merah = truncate(merah);
                    hijau = truncate(hijau);
                    biru = truncate(biru);

                    b.SetPixel(i, j, Color.FromArgb(merah, hijau, biru));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / bit.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        private int erosi(int sumColor, int blok)
        {
            return sumColor == (255 * blok) ? 255 : 0;
        }

        private int dilasi(int sumColor, int blok)
        {
            return sumColor == 0 ? 0 : 255;
        }

        private void erosionSquare3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                this.pbOutput.Image = dimensi3x3(b, se, "erosi");
            }
        }

        private Bitmap dimensi5x5(Bitmap bit, int[] se, string str = "")
        {

            Bitmap b = new Bitmap(bit);
            Color c;

            bool b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20, b21, b22, b23, b24, b25;
            int w = bit.Width - 1;
            int h = bit.Height - 1;

            float sumR, sumG, sumB;
            List<int> listR = new List<int>();
            List<int> listG = new List<int>();
            List<int> listB = new List<int>();
            int merah = 0, hijau = 0, biru = 0;

            int blok = 0;

            progressBar1.Visible = true;
            for (int i = 0; i <= w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    b1 = b2 = b3 = b4 = b5 = false;
                    b6 = b7 = b8 = b9 = b10 = false;
                    b11 = b12 = b13 = b14 = b15 = false;
                    b16 = b17 = b18 = b19 = b20 = false;
                    b21 = b22 = b23 = b24 = b25 = false;

                    sumR = sumG = sumB = 0;
                    blok = 0;

                    listR.Clear();
                    listG.Clear();
                    listB.Clear();
                    
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (j == 1)
                    {
                        if (i == 0)
                        {
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (1 < j && j < h - 1)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                            b23 = b24 = b25 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                            b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                            b21 = b22 = b23 = b24 = b25 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                            b21 = b22 = b23 = b24 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                            b21 = b22 = b23 = true;
                        }
                    }
                    
                    else if (j == h - 1)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                            b18 = b19 = b20 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                            b17 = b18 = b19 = b20 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                            b16 = b17 = b18 = b19 = b20 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                            b16 = b17 = b18 = b19 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                            b16 = b17 = b18 = true;
                        }
                    }
                    
                    else if (j == h)
                    {
                        if (i == 0)
                        {
                            b3 = b4 = b5 = true;
                            b8 = b9 = b10 = true;
                            b13 = b14 = b15 = true;
                        }

                      
                        else if (i == 1)
                        {
                            b2 = b3 = b4 = b5 = true;
                            b7 = b8 = b9 = b10 = true;
                            b12 = b13 = b14 = b15 = true;
                        }
                        
                        else if (1 < i && i < w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b6 = b7 = b8 = b9 = b10 = true;
                            b11 = b12 = b13 = b14 = b15 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = true;
                            b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = true;
                            b6 = b7 = b8 = true;
                            b11 = b12 = b13 = true;
                        }
                    }

                    if (b1)
                    {
                        c = bit.GetPixel(i - 2, j - 2);
                        listR.Add(c.R * se[0]);
                        listG.Add(c.G * se[0]);
                        listB.Add(c.B * se[0]);
                        sumR += (c.R * se[0]);
                        sumG += (c.G * se[0]);
                        sumB += (c.B * se[0]);
                        blok += se[0];
                    }

                    if (b2)
                    {
                        c = bit.GetPixel(i - 1, j - 2);
                        listR.Add(c.R * se[1]);
                        listG.Add(c.G * se[1]);
                        listB.Add(c.B * se[1]);
                        sumR += (c.R * se[1]);
                        sumG += (c.G * se[1]);
                        sumB += (c.B * se[1]);
                        blok += se[1];
                    }

                    if (b3)
                    {
                        c = bit.GetPixel(i, j - 2);
                        listR.Add(c.R * se[2]);
                        listG.Add(c.G * se[2]);
                        listB.Add(c.B * se[2]);
                        sumR += (c.R * se[2]);
                        sumG += (c.G * se[2]);
                        sumB += (c.B * se[2]);
                        blok += se[2];
                    }

                    if (b4)
                    {
                        c = bit.GetPixel(i + 1, j - 2);
                        listR.Add(c.R * se[3]);
                        listG.Add(c.G * se[3]);
                        listB.Add(c.B * se[3]);
                        sumR += (c.R * se[3]);
                        sumG += (c.G * se[3]);
                        sumB += (c.B * se[3]);
                        blok += se[3];
                    }

                    if (b5)
                    {
                        c = bit.GetPixel(i + 2, j - 2);
                        listR.Add(c.R * se[4]);
                        listG.Add(c.G * se[4]);
                        listB.Add(c.B * se[4]);
                        sumR += (c.R * se[4]);
                        sumG += (c.G * se[4]);
                        sumB += (c.B * se[4]);
                        blok += se[4];
                    }
                    
                    if (b6)
                    {
                        c = bit.GetPixel(i - 2, j - 1);
                        listR.Add(c.R * se[5]);
                        listG.Add(c.G * se[5]);
                        listB.Add(c.B * se[5]);
                        sumR += (c.R * se[5]);
                        sumG += (c.G * se[5]);
                        sumB += (c.B * se[5]);
                        blok += se[5];
                    }

                    if (b7)
                    {
                        c = bit.GetPixel(i - 1, j - 1);
                        listR.Add(c.R * se[6]);
                        listG.Add(c.G * se[6]);
                        listB.Add(c.B * se[6]);
                        sumR += (c.R * se[6]);
                        sumG += (c.G * se[6]);
                        sumB += (c.B * se[6]);
                        blok += se[6];
                    }

                    if (b8)
                    {
                        c = bit.GetPixel(i, j - 1);
                        listR.Add(c.R * se[7]);
                        listG.Add(c.G * se[7]);
                        listB.Add(c.B * se[7]);
                        sumR += (c.R * se[7]);
                        sumG += (c.G * se[7]);
                        sumB += (c.B * se[7]);
                        blok += se[7];
                    }

                    if (b9)
                    {
                        c = bit.GetPixel(i + 1, j - 1);
                        listR.Add(c.R * se[8]);
                        listG.Add(c.G * se[8]);
                        listB.Add(c.B * se[8]);
                        sumR += (c.R * se[8]);
                        sumG += (c.G * se[8]);
                        sumB += (c.B * se[8]);
                        blok += se[8];
                    }

                    if (b10)
                    {
                        c = bit.GetPixel(i + 2, j - 1);
                        listR.Add(c.R * se[9]);
                        listG.Add(c.G * se[9]);
                        listB.Add(c.B * se[9]);
                        sumR += (c.R * se[9]);
                        sumG += (c.G * se[9]);
                        sumB += (c.B * se[9]);
                        blok += se[9];
                    }
                    
                    if (b11)
                    {
                        c = bit.GetPixel(i - 2, j);
                        listR.Add(c.R * se[10]);
                        listG.Add(c.G * se[10]);
                        listB.Add(c.B * se[10]);
                        sumR += (c.R * se[10]);
                        sumG += (c.G * se[10]);
                        sumB += (c.B * se[10]);
                        blok += se[10];
                    }

                    if (b12)
                    {
                        c = bit.GetPixel(i - 1, j);
                        listR.Add(c.R * se[11]);
                        listG.Add(c.G * se[11]);
                        listB.Add(c.B * se[11]);
                        sumR += (c.R * se[11]);
                        sumG += (c.G * se[11]);
                        sumB += (c.B * se[11]);
                        blok += se[11];
                    }

                    if (b13)
                    {
                        c = bit.GetPixel(i, j);
                        listR.Add(c.R * se[12]);
                        listG.Add(c.G * se[12]);
                        listB.Add(c.B * se[12]);
                        sumR += (c.R * se[12]);
                        sumG += (c.G * se[12]);
                        sumB += (c.B * se[12]);
                        blok += se[12];
                    }

                    if (b14)
                    {
                        c = bit.GetPixel(i + 1, j);
                        listR.Add(c.R * se[13]);
                        listG.Add(c.G * se[13]);
                        listB.Add(c.B * se[13]);
                        sumR += (c.R * se[13]);
                        sumG += (c.G * se[13]);
                        sumB += (c.B * se[13]);
                        blok += se[13];
                    }

                    if (b15)
                    {
                        c = bit.GetPixel(i + 2, j);
                        listR.Add(c.R * se[14]);
                        listG.Add(c.G * se[14]);
                        listB.Add(c.B * se[14]);
                        sumR += (c.R * se[14]);
                        sumG += (c.G * se[14]);
                        sumB += (c.B * se[14]);
                        blok += se[14];
                    }

                    if (b16)
                    {
                        c = bit.GetPixel(i - 2, j + 1);
                        listR.Add(c.R * se[15]);
                        listG.Add(c.G * se[15]);
                        listB.Add(c.B * se[15]);
                        sumR += (c.R * se[15]);
                        sumG += (c.G * se[15]);
                        sumB += (c.B * se[15]);
                        blok += se[15];
                    }

                    if (b17)
                    {
                        c = bit.GetPixel(i - 1, j + 1);
                        listR.Add(c.R * se[16]);
                        listG.Add(c.G * se[16]);
                        listB.Add(c.B * se[16]);
                        sumR += (c.R * se[16]);
                        sumG += (c.G * se[16]);
                        sumB += (c.B * se[16]);
                        blok += se[16];
                    }

                    if (b18)
                    {
                        c = bit.GetPixel(i, j + 1);
                        listR.Add(c.R * se[17]);
                        listG.Add(c.G * se[17]);
                        listB.Add(c.B * se[17]);
                        sumR += (c.R * se[17]);
                        sumG += (c.G * se[17]);
                        sumB += (c.B * se[17]);
                        blok += se[17];
                    }

                    if (b19)
                    {
                        c = bit.GetPixel(i + 1, j + 1);
                        listR.Add(c.R * se[18]);
                        listG.Add(c.G * se[18]);
                        listB.Add(c.B * se[18]);
                        sumR += (c.R * se[18]);
                        sumG += (c.G * se[18]);
                        sumB += (c.B * se[18]);
                        blok += se[18];
                    }

                    if (b20)
                    {
                        c = bit.GetPixel(i + 2, j + 1);
                        listR.Add(c.R * se[19]);
                        listG.Add(c.G * se[19]);
                        listB.Add(c.B * se[19]);
                        sumR += (c.R * se[19]);
                        sumG += (c.G * se[19]);
                        sumB += (c.B * se[19]);
                        blok += se[19];
                    }

                    if (b21)
                    {
                        c = bit.GetPixel(i - 2, j + 2);
                        listR.Add(c.R * se[20]);
                        listG.Add(c.G * se[20]);
                        listB.Add(c.B * se[20]);
                        sumR += (c.R * se[20]);
                        sumG += (c.G * se[20]);
                        sumB += (c.B * se[20]);
                        blok += se[20];
                    }

                    if (b22)
                    {
                        c = bit.GetPixel(i - 1, j + 2);
                        listR.Add(c.R * se[21]);
                        listG.Add(c.G * se[21]);
                        listB.Add(c.B * se[21]);
                        sumR += (c.R * se[21]);
                        sumG += (c.G * se[21]);
                        sumB += (c.B * se[21]);
                        blok += se[21];
                    }

                    if (b23)
                    {
                        c = bit.GetPixel(i, j + 2);
                        listR.Add(c.R * se[22]);
                        listG.Add(c.G * se[22]);
                        listB.Add(c.B * se[22]);
                        sumR += (c.R * se[22]);
                        sumG += (c.G * se[22]);
                        sumB += (c.B * se[22]);
                        blok += se[22];
                    }

                    if (b24)
                    {
                        c = bit.GetPixel(i + 1, j + 2);
                        listR.Add(c.R * se[23]);
                        listG.Add(c.G * se[23]);
                        listB.Add(c.B * se[23]);
                        sumR += (c.R * se[23]);
                        sumG += (c.G * se[23]);
                        sumB += (c.B * se[23]);
                        blok += se[23];
                    }

                    if (b25)
                    {
                        c = bit.GetPixel(i + 2, j + 2);
                        listR.Add(c.R * se[24]);
                        listG.Add(c.G * se[24]);
                        listB.Add(c.B * se[24]);
                        sumR += (c.R * se[24]);
                        sumG += (c.G * se[24]);
                        sumB += (c.B * se[24]);
                        blok += se[24];
                    }

                    if (str.Equals(""))
                    {
                        merah = blok != 0 ? (int)sumR / blok : (int)sumR;
                        hijau = blok != 0 ? (int)sumG / blok : (int)sumG;
                        biru = blok != 0 ? (int)sumB / blok : (int)sumB;
                    }
                    else
                    {
                        if (str.Equals("erosi"))
                        {
                            merah = erosi((int)sumR, blok);
                            hijau = erosi((int)sumG, blok);
                            biru = erosi((int)sumB, blok);
                        }

                        if (str.Equals("dilasi"))
                        {
                            merah = dilasi((int)sumR, blok);
                            hijau = dilasi((int)sumG, blok);
                            biru = dilasi((int)sumB, blok);
                        }

                        else if (str.Equals("grayscale_dilasi"))
                        {
                            merah = listR.Max();
                            hijau = listG.Max();
                            biru = listB.Max();
                        }
                        else if (str.Equals("grayscale_erosi"))
                        {
                            merah = listR.Min();
                            hijau = listG.Min();
                            biru = listB.Min();
                        }
                    }

                    merah = truncate(merah);
                    hijau = truncate(hijau);
                    biru = truncate(biru);

                   b.SetPixel(i, j, Color.FromArgb(merah, hijau, biru));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / bit.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        private void erosionSquare5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1
                };

                this.pbOutput.Image = dimensi5x5(b, se, "erosi");
            }
        }

        private void erosionCross3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 0,
                    1, 1, 1,
                    0, 1, 0
                };

                this.pbOutput.Image = dimensi3x3(b, se, "erosi");
            }
        }

        private void dilationSquare3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                this.pbOutput.Image = dimensi3x3(b, se, "dilasi");
            }
        }

        private void dilationSquare5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1
                };

                this.pbOutput.Image = dimensi5x5(b, se, "dilasi");
            }
        }

        private void dilationCross3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 0,
                    1, 1, 1,
                    0, 1, 0
                };

                this.pbOutput.Image = dimensi3x3(b, se, "dilasi");
            }
        }

        private Bitmap dimensi9x9(Bitmap bit, int[] se, string str = "")
        {
            Bitmap b = new Bitmap(bit);

            int w = bit.Width - 1;
            int h = bit.Height - 1;
            bool b1, b2, b3, b4, b5, b6, b7, b8, b9;
            bool b10, b11, b12, b13, b14, b15, b16, b17, b18;
            bool b19, b20, b21, b22, b23, b24, b25, b26, b27;
            bool b28, b29, b30, b31, b32, b33, b34, b35, b36;
            bool b37, b38, b39, b40, b41, b42, b43, b44, b45;
            bool b46, b47, b48, b49, b50, b51, b52, b53, b54;
            bool b55, b56, b57, b58, b59, b60, b61, b62, b63;
            bool b64, b65, b66, b67, b68, b69, b70, b71, b72;
            bool b73, b74, b75, b76, b77, b78, b79, b80, b81;

            Color c;
            int merah = 0, hijau = 0, biru = 0;

            progressBar1.Visible = true;
            for (int i = 0; i <= w; i++)
            {
                for (int j = 0; j <= h; j++)
                {
                    b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = false;
                    b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = false;
                    b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = false;
                    b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = false;
                    b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = false;
                    b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = false;
                    b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = false;
                    b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = false;
                    b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = false;

                    float sumR = 0, sumG = 0, sumB = 0;

                    int blok = 0;
                    
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                            b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = true;
                        }
                        
                        else if (i == w)
                        {
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                            b73 = b74 = b75 = b76 = b77 = true;
                        }
                    }
                    
                    else if (j == 1)
                    {
                        if (i == 0)
                        {
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                            b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = true;
                        }
                        
                        else if (i == w)
                        {
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                            b73 = b74 = b75 = b76 = b77 = true;
                        }
                    }
                    
                    else if (j == 2)
                    {
                        if (i == 0)
                        {
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                            b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = true;
                        }
                        
                        else if (i == w)
                        {
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                            b73 = b74 = b75 = b76 = b77 = true;
                        }
                    }
                    
                    else if (j == 3)
                    {
                        if (i == 0)
                        {
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                            b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }

                        else if (i == 2)
                        {
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = true;
                        }
                        
                        else if (i == w)
                        {
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                            b73 = b74 = b75 = b76 = b77 = true;
                        }
                    }
                    
                    else if (3 < j && j < h - 3)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = b7 = b8 = b9 = true;
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                            b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = b81 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = b80 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = b79 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                            b73 = b74 = b75 = b76 = b77 = b78 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                            b73 = b74 = b75 = b76 = b77 = true;
                        }
                    }
                    
                    else if (j == h - 3)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = b7 = b8 = b9 = true;
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                            b68 = b69 = b70 = b71 = b72 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b67 = b68 = b69 = b70 = b71 = b72 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = b72 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = b71 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = b70 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                            b64 = b65 = b66 = b67 = b68 = b69 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                            b64 = b65 = b66 = b67 = b68 = true;
                        }
                    }
                    
                    else if (j == h - 2)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = b7 = b8 = b9 = true;
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                            b59 = b60 = b61 = b62 = b63 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b58 = b59 = b60 = b61 = b62 = b63 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = b63 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = b62 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = b61 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                            b55 = b56 = b57 = b58 = b59 = b60 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                            b55 = b56 = b57 = b58 = b59 = true;
                        }
                    }
                    
                    else if (j == h - 1)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = b7 = b8 = b9 = true;
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                            b50 = b51 = b52 = b53 = b54 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b49 = b50 = b51 = b52 = b53 = b54 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = b54 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = b53 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = b52 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                            b46 = b47 = b48 = b49 = b50 = b51 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                            b46 = b47 = b48 = b49 = b50 = true;
                        }
                    }
                    
                    else if (j == h)
                    {
                        if (i == 0)
                        {
                            b5 = b6 = b7 = b8 = b9 = true;
                            b14 = b15 = b16 = b17 = b18 = true;
                            b23 = b24 = b25 = b26 = b27 = true;
                            b32 = b33 = b34 = b35 = b36 = true;
                            b41 = b42 = b43 = b44 = b45 = true;
                        }
                        
                        else if (i == 1)
                        {
                            b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b40 = b41 = b42 = b43 = b44 = b45 = true;
                        }
                        
                        else if (i == 2)
                        {
                            b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                        }
                        
                        else if (i == 3)
                        {
                            b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                        }
                        
                        else if (3 < i && i < w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = b9 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = b18 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = b27 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = b36 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = b45 = true;
                        }
                        
                        else if (i == w - 3)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = b8 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = b17 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = b26 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = b35 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = b44 = true;
                        }
                        
                        else if (i == w - 2)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = b7 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = b16 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = b25 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = b34 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = b43 = true;
                        }
                        
                        else if (i == w - 1)
                        {
                            b1 = b2 = b3 = b4 = b5 = b6 = true;
                            b10 = b11 = b12 = b13 = b14 = b15 = true;
                            b19 = b20 = b21 = b22 = b23 = b24 = true;
                            b28 = b29 = b30 = b31 = b32 = b33 = true;
                            b37 = b38 = b39 = b40 = b41 = b42 = true;
                        }
                        
                        else if (i == w)
                        {
                            b1 = b2 = b3 = b4 = b5 = true;
                            b10 = b11 = b12 = b13 = b14 = true;
                            b19 = b20 = b21 = b22 = b23 = true;
                            b28 = b29 = b30 = b31 = b32 = true;
                            b37 = b38 = b39 = b40 = b41 = true;
                        }
                    }

                    if (b1)
                    {
                        c = bit.GetPixel(i - 4, j - 4);
                        sumR += (c.R * se[0]);
                        sumG += (c.G * se[0]);
                        sumB += (c.B * se[0]);
                        blok += se[0];
                    }

                    if (b2)
                    {
                        c = bit.GetPixel(i - 3, j - 4);
                        sumR += (c.R * se[1]);
                        sumG += (c.G * se[1]);
                        sumB += (c.B * se[1]);
                        blok += se[1];
                    }

                    if (b3)
                    {
                        c = bit.GetPixel(i - 2, j - 4);
                        sumR += (c.R * se[2]);
                        sumG += (c.G * se[2]);
                        sumB += (c.B * se[2]);
                        blok += se[2];
                    }

                    if (b4)
                    {
                        c = bit.GetPixel(i - 1, j - 4);
                        sumR += (c.R * se[3]);
                        sumG += (c.G * se[3]);
                        sumB += (c.B * se[3]);
                        blok += se[3];
                    }

                    if (b5)
                    {
                        c = bit.GetPixel(i, j - 4);
                        sumR += (c.R * se[4]);
                        sumG += (c.G * se[4]);
                        sumB += (c.B * se[4]);
                        blok += se[4];
                    }

                    if (b6)
                    {
                        c = bit.GetPixel(i + 1, j - 4);
                        sumR += (c.R * se[5]);
                        sumG += (c.G * se[5]);
                        sumB += (c.B * se[5]);
                        blok += se[5];
                    }

                    if (b7)
                    {
                        c = bit.GetPixel(i + 2, j - 4);
                        sumR += (c.R * se[6]);
                        sumG += (c.G * se[6]);
                        sumB += (c.B * se[6]);
                        blok += se[6];
                    }

                    if (b8)
                    {
                        c = bit.GetPixel(i + 3, j - 4);
                        sumR += (c.R * se[7]);
                        sumG += (c.G * se[7]);
                        sumB += (c.B * se[7]);
                        blok += se[7];
                    }

                    if (b9)
                    {
                        c = bit.GetPixel(i + 4, j - 4);
                        sumR += (c.R * se[8]);
                        sumG += (c.G * se[8]);
                        sumB += (c.B * se[8]);
                        blok += se[8];
                    }

                    if (b10)
                    {
                        c = bit.GetPixel(i - 4, j - 3);
                        sumR += (c.R * se[9]);
                        sumG += (c.G * se[9]);
                        sumB += (c.B * se[9]);
                        blok += se[9];
                    }

                    if (b11)
                    {
                        c = bit.GetPixel(i - 3, j - 3);
                        sumR += (c.R * se[10]);
                        sumG += (c.G * se[10]);
                        sumB += (c.B * se[10]);
                        blok += se[10];
                    }

                    if (b12)
                    {
                        c = bit.GetPixel(i - 2, j - 3);
                        sumR += (c.R * se[11]);
                        sumG += (c.G * se[11]);
                        sumB += (c.B * se[11]);
                        blok += se[11];
                    }

                    if (b13)
                    {
                        c = bit.GetPixel(i - 1, j - 3);
                        sumR += (c.R * se[12]);
                        sumG += (c.G * se[12]);
                        sumB += (c.B * se[12]);
                        blok += se[12];
                    }

                    if (b14)
                    {
                        c = bit.GetPixel(i, j - 3);
                        sumR += (c.R * se[13]);
                        sumG += (c.G * se[13]);
                        sumB += (c.B * se[13]);
                        blok += se[13];
                    }

                    if (b15)
                    {
                        c = bit.GetPixel(i + 1, j - 3);
                        sumR += (c.R * se[14]);
                        sumG += (c.G * se[14]);
                        sumB += (c.B * se[14]);
                        blok += se[14];
                    }

                    if (b16)
                    {
                        c = bit.GetPixel(i + 2, j - 3);
                        sumR += (c.R * se[15]);
                        sumG += (c.G * se[15]);
                        sumB += (c.B * se[15]);
                        blok += se[15];
                    }

                    if (b17)
                    {
                        c = bit.GetPixel(i + 3, j - 3);
                        sumR += (c.R * se[16]);
                        sumG += (c.G * se[16]);
                        sumB += (c.B * se[16]);
                        blok += se[16];
                    }

                    if (b18)
                    {
                        c = bit.GetPixel(i + 4, j - 3);
                        sumR += (c.R * se[17]);
                        sumG += (c.G * se[17]);
                        sumB += (c.B * se[17]);
                        blok += se[17];
                    }
                    
                    if (b19)
                    {
                        c = bit.GetPixel(i - 4, j - 2);
                        sumR += (c.R * se[18]);
                        sumG += (c.G * se[18]);
                        sumB += (c.B * se[18]);
                        blok += se[18];
                    }

                    if (b20)
                    {
                        c = bit.GetPixel(i - 3, j - 2);
                        sumR += (c.R * se[19]);
                        sumG += (c.G * se[19]);
                        sumB += (c.B * se[19]);
                        blok += se[19];
                    }

                    if (b21)
                    {
                        c = bit.GetPixel(i - 2, j - 2);
                        sumR += (c.R * se[20]);
                        sumG += (c.G * se[20]);
                        sumB += (c.B * se[20]);
                        blok += se[20];
                    }

                    if (b22)
                    {
                        c = bit.GetPixel(i - 1, j - 2);
                        sumR += (c.R * se[21]);
                        sumG += (c.G * se[21]);
                        sumB += (c.B * se[21]);
                        blok += se[21];
                    }

                    if (b23)
                    {
                        c = bit.GetPixel(i, j - 2);
                        sumR += (c.R * se[22]);
                        sumG += (c.G * se[22]);
                        sumB += (c.B * se[22]);
                        blok += se[22];
                    }

                    if (b24)
                    {
                        c = bit.GetPixel(i + 1, j - 2);
                        sumR += (c.R * se[23]);
                        sumG += (c.G * se[23]);
                        sumB += (c.B * se[23]);
                        blok += se[23];
                    }

                    if (b25)
                    {
                        c = bit.GetPixel(i + 2, j - 2);
                        sumR += (c.R * se[24]);
                        sumG += (c.G * se[24]);
                        sumB += (c.B * se[24]);
                        blok += se[24];
                    }

                    if (b26)
                    {
                        c = bit.GetPixel(i + 3, j - 2);
                        sumR += (c.R * se[25]);
                        sumG += (c.G * se[25]);
                        sumB += (c.B * se[25]);
                        blok += se[25];
                    }

                    if (b27)
                    {
                        c = bit.GetPixel(i + 4, j - 2);
                        sumR += (c.R * se[26]);
                        sumG += (c.G * se[26]);
                        sumB += (c.B * se[26]);
                        blok += se[26];
                    }
                    
                    if (b28)
                    {
                        c = bit.GetPixel(i - 4, j - 1);
                        sumR += (c.R * se[27]);
                        sumG += (c.G * se[27]);
                        sumB += (c.B * se[27]);
                        blok += se[27];
                    }

                    if (b29)
                    {
                        c = bit.GetPixel(i - 3, j - 1);
                        sumR += (c.R * se[28]);
                        sumG += (c.G * se[28]);
                        sumB += (c.B * se[28]);
                        blok += se[28];
                    }

                    if (b30)
                    {
                        c = bit.GetPixel(i - 2, j - 1);
                        sumR += (c.R * se[29]);
                        sumG += (c.G * se[29]);
                        sumB += (c.B * se[29]);
                        blok += se[29];
                    }

                    if (b31)
                    {
                        c = bit.GetPixel(i - 1, j - 1);
                        sumR += (c.R * se[30]);
                        sumG += (c.G * se[30]);
                        sumB += (c.B * se[30]);
                        blok += se[30];
                    }

                    if (b32)
                    {
                        c = bit.GetPixel(i, j - 1);
                        sumR += (c.R * se[31]);
                        sumG += (c.G * se[31]);
                        sumB += (c.B * se[31]);
                        blok += se[31];
                    }

                    if (b33)
                    {
                        c = bit.GetPixel(i + 1, j - 1);
                        sumR += (c.R * se[32]);
                        sumG += (c.G * se[32]);
                        sumB += (c.B * se[32]);
                        blok += se[32];
                    }

                    if (b34)
                    {
                        c = bit.GetPixel(i + 2, j - 1);
                        sumR += (c.R * se[33]);
                        sumG += (c.G * se[33]);
                        sumB += (c.B * se[33]);
                        blok += se[33];
                    }

                    if (b35)
                    {
                        c = bit.GetPixel(i + 3, j - 1);
                        sumR += (c.R * se[34]);
                        sumG += (c.G * se[34]);
                        sumB += (c.B * se[34]);
                        blok += se[34];
                    }

                    if (b36)
                    {
                        c = bit.GetPixel(i + 4, j - 1);
                        sumR += (c.R * se[35]);
                        sumG += (c.G * se[35]);
                        sumB += (c.B * se[35]);
                        blok += se[35];
                    }
                    
                    if (b37)
                    {
                        c = bit.GetPixel(i - 4, j);
                        sumR += (c.R * se[36]);
                        sumG += (c.G * se[36]);
                        sumB += (c.B * se[36]);
                        blok += se[36];
                    }

                    if (b38)
                    {
                        c = bit.GetPixel(i - 3, j);
                        sumR += (c.R * se[37]);
                        sumG += (c.G * se[37]);
                        sumB += (c.B * se[37]);
                        blok += se[37];
                    }

                    if (b39)
                    {
                        c = bit.GetPixel(i - 2, j);
                        sumR += (c.R * se[38]);
                        sumG += (c.G * se[38]);
                        sumB += (c.B * se[38]);
                        blok += se[38];
                    }

                    if (b40)
                    {
                        c = bit.GetPixel(i - 1, j);
                        sumR += (c.R * se[39]);
                        sumG += (c.G * se[39]);
                        sumB += (c.B * se[39]);
                        blok += se[39];
                    }

                    if (b41)
                    {
                        c = bit.GetPixel(i, j);
                        sumR += (c.R * se[40]);
                        sumG += (c.G * se[40]);
                        sumB += (c.B * se[40]);
                        blok += se[40];
                    }

                    if (b42)
                    {
                        c = bit.GetPixel(i + 1, j);
                        sumR += (c.R * se[41]);
                        sumG += (c.G * se[41]);
                        sumB += (c.B * se[41]);
                        blok += se[41];
                    }

                    if (b43)
                    {
                        c = bit.GetPixel(i + 2, j);
                        sumR += (c.R * se[42]);
                        sumG += (c.G * se[42]);
                        sumB += (c.B * se[42]);
                        blok += se[42];
                    }

                    if (b44)
                    {
                        c = bit.GetPixel(i + 3, j);
                        sumR += (c.R * se[43]);
                        sumG += (c.G * se[43]);
                        sumB += (c.B * se[43]);
                        blok += se[43];
                    }

                    if (b45)
                    {
                        c = bit.GetPixel(i + 4, j);
                        sumR += (c.R * se[44]);
                        sumG += (c.G * se[44]);
                        sumB += (c.B * se[44]);
                        blok += se[44];
                    }
                    
                    if (b46)
                    {
                        c = bit.GetPixel(i - 4, j + 1);
                        sumR += (c.R * se[45]);
                        sumG += (c.G * se[45]);
                        sumB += (c.B * se[45]);
                        blok += se[45];
                    }

                    if (b47)
                    {
                        c = bit.GetPixel(i - 3, j + 1);
                        sumR += (c.R * se[46]);
                        sumG += (c.G * se[46]);
                        sumB += (c.B * se[46]);
                        blok += se[46];
                    }

                    if (b48)
                    {
                        c = bit.GetPixel(i - 2, j + 1);
                        sumR += (c.R * se[47]);
                        sumG += (c.G * se[47]);
                        sumB += (c.B * se[47]);
                        blok += se[47];
                    }

                    if (b49)
                    {
                        c = bit.GetPixel(i - 1, j + 1);
                        sumR += (c.R * se[48]);
                        sumG += (c.G * se[48]);
                        sumB += (c.B * se[48]);
                        blok += se[48];
                    }

                    if (b50)
                    {
                        c = bit.GetPixel(i, j + 1);
                        sumR += (c.R * se[49]);
                        sumG += (c.G * se[49]);
                        sumB += (c.B * se[49]);
                        blok += se[49];
                    }

                    if (b51)
                    {
                        c = bit.GetPixel(i + 1, j + 1);
                        sumR += (c.R * se[50]);
                        sumG += (c.G * se[50]);
                        sumB += (c.B * se[50]);
                        blok += se[50];
                    }

                    if (b52)
                    {
                        c = bit.GetPixel(i + 2, j + 1);
                        sumR += (c.R * se[51]);
                        sumG += (c.G * se[51]);
                        sumB += (c.B * se[51]);
                        blok += se[51];
                    }

                    if (b53)
                    {
                        c = bit.GetPixel(i + 3, j + 1);
                        sumR += (c.R * se[52]);
                        sumG += (c.G * se[52]);
                        sumB += (c.B * se[52]);
                        blok += se[52];
                    }

                    if (b54)
                    {
                        c = bit.GetPixel(i + 4, j + 1);
                        sumR += (c.R * se[53]);
                        sumG += (c.G * se[53]);
                        sumB += (c.B * se[53]);
                        blok += se[53];
                    }
                   
                    if (b55)
                    {
                        c = bit.GetPixel(i - 4, j + 2);
                        sumR += (c.R * se[54]);
                        sumG += (c.G * se[54]);
                        sumB += (c.B * se[54]);
                        blok += se[54];
                    }

                    if (b56)
                    {
                        c = bit.GetPixel(i - 3, j + 2);
                        sumR += (c.R * se[55]);
                        sumG += (c.G * se[55]);
                        sumB += (c.B * se[55]);
                        blok += se[55];
                    }

                    if (b57)
                    {
                        c = bit.GetPixel(i - 2, j + 2);
                        sumR += (c.R * se[56]);
                        sumG += (c.G * se[56]);
                        sumB += (c.B * se[56]);
                        blok += se[56];
                    }

                    if (b58)
                    {
                        c = bit.GetPixel(i - 1, j + 2);
                        sumR += (c.R * se[57]);
                        sumG += (c.G * se[57]);
                        sumB += (c.B * se[57]);
                        blok += se[57];
                    }

                    if (b59)
                    {
                        c = bit.GetPixel(i, j + 2);
                        sumR += (c.R * se[58]);
                        sumG += (c.G * se[58]);
                        sumB += (c.B * se[58]);
                        blok += se[58];
                    }

                    if (b60)
                    {
                        c = bit.GetPixel(i + 1, j + 2);
                        sumR += (c.R * se[59]);
                        sumG += (c.G * se[59]);
                        sumB += (c.B * se[59]);
                        blok += se[59];
                    }

                    if (b61)
                    {
                        c = bit.GetPixel(i + 2, j + 2);
                        sumR += (c.R * se[60]);
                        sumG += (c.G * se[60]);
                        sumB += (c.B * se[60]);
                        blok += se[60];
                    }

                    if (b62)
                    {
                        c = bit.GetPixel(i + 3, j + 2);
                        sumR += (c.R * se[61]);
                        sumG += (c.G * se[61]);
                        sumB += (c.B * se[61]);
                        blok += se[61];
                    }

                    if (b63)
                    {
                        c = bit.GetPixel(i + 4, j + 2);
                        sumR += (c.R * se[62]);
                        sumG += (c.G * se[62]);
                        sumB += (c.B * se[62]);
                        blok += se[62];
                    }
                    
                    if (b64)
                    {
                        c = bit.GetPixel(i - 4, j + 3);
                        sumR += (c.R * se[63]);
                        sumG += (c.G * se[63]);
                        sumB += (c.B * se[63]);
                        blok += se[63];
                    }

                    if (b65)
                    {
                        c = bit.GetPixel(i - 3, j + 3);
                        sumR += (c.R * se[64]);
                        sumG += (c.G * se[64]);
                        sumB += (c.B * se[64]);
                        blok += se[64];
                    }

                    if (b66)
                    {
                        c = bit.GetPixel(i - 2, j + 3);
                        sumR += (c.R * se[65]);
                        sumG += (c.G * se[65]);
                        sumB += (c.B * se[65]);
                        blok += se[65];
                    }

                    if (b67)
                    {
                        c = bit.GetPixel(i - 1, j + 3);
                        sumR += (c.R * se[66]);
                        sumG += (c.G * se[66]);
                        sumB += (c.B * se[66]);
                        blok += se[66];
                    }

                    if (b68)
                    {
                        c = bit.GetPixel(i, j + 3);
                        sumR += (c.R * se[67]);
                        sumG += (c.G * se[67]);
                        sumB += (c.B * se[67]);
                        blok += se[67];
                    }

                    if (b69)
                    {
                        c = bit.GetPixel(i + 1, j + 3);
                        sumR += (c.R * se[68]);
                        sumG += (c.G * se[68]);
                        sumB += (c.B * se[68]);
                        blok += se[68];
                    }

                    if (b70)
                    {
                        c = bit.GetPixel(i + 2, j + 3);
                        sumR += (c.R * se[69]);
                        sumG += (c.G * se[69]);
                        sumB += (c.B * se[69]);
                        blok += se[69];
                    }

                    if (b71)
                    {
                        c = bit.GetPixel(i + 3, j + 3);
                        sumR += (c.R * se[70]);
                        sumG += (c.G * se[70]);
                        sumB += (c.B * se[70]);
                        blok += se[70];
                    }

                    if (b72)
                    {
                        c = bit.GetPixel(i + 4, j + 3);
                        sumR += (c.R * se[71]);
                        sumG += (c.G * se[71]);
                        sumB += (c.B * se[71]);
                        blok += se[71];
                    }
                    
                    if (b73)
                    {
                        c = bit.GetPixel(i - 4, j + 4);
                        sumR += (c.R * se[72]);
                        sumG += (c.G * se[72]);
                        sumB += (c.B * se[72]);
                        blok += se[72];
                    }

                    if (b74)
                    {
                        c = bit.GetPixel(i - 3, j + 4);
                        sumR += (c.R * se[73]);
                        sumG += (c.G * se[73]);
                        sumB += (c.B * se[73]);
                        blok += se[73];
                    }

                    if (b75)
                    {
                        c = bit.GetPixel(i - 2, j + 4);
                        sumR += (c.R * se[74]);
                        sumG += (c.G * se[74]);
                        sumB += (c.B * se[74]);
                        blok += se[74];
                    }

                    if (b76)
                    {
                        c = bit.GetPixel(i - 1, j + 4);
                        sumR += (c.R * se[75]);
                        sumG += (c.G * se[75]);
                        sumB += (c.B * se[75]);
                        blok += se[75];
                    }

                    if (b77)
                    {
                        c = bit.GetPixel(i, j + 4);
                        sumR += (c.R * se[76]);
                        sumG += (c.G * se[76]);
                        sumB += (c.B * se[76]);
                        blok += se[76];
                    }

                    if (b78)
                    {
                        c = bit.GetPixel(i + 1, j + 4);
                        sumR += (c.R * se[77]);
                        sumG += (c.G * se[77]);
                        sumB += (c.B * se[77]);
                        blok += se[77];
                    }

                    if (b79)
                    {
                        c = bit.GetPixel(i + 2, j + 4);
                        sumR += (c.R * se[78]);
                        sumG += (c.G * se[78]);
                        sumB += (c.B * se[78]);
                        blok += se[78];
                    }

                    if (b80)
                    {
                        c = bit.GetPixel(i + 3, j + 4);
                        sumR += (c.R * se[79]);
                        sumG += (c.G * se[79]);
                        sumB += (c.B * se[79]);
                        blok += se[79];
                    }

                    if (b81)
                    {
                        c = bit.GetPixel(i + 4, j + 4);
                        sumR += (c.R * se[80]);
                        sumG += (c.G * se[80]);
                        sumB += (c.B * se[80]);
                        blok += se[80];
                    }

                    if (str.Equals(""))
                    {
                        merah = blok != 0 ? (int)sumR / blok : (int)sumR;
                        hijau = blok != 0 ? (int)sumG / blok : (int)sumG;
                        biru = blok != 0 ? (int)sumB / blok : (int)sumB;
                    }
                    else
                    {
                        if (str.Equals("erosi"))
                        {
                            merah = erosi((int)sumR, blok);
                            hijau = erosi((int)sumG, blok);
                            biru = erosi((int)sumB, blok);
                        }

                        if (str.Equals("dilasi"))
                        {
                            merah = dilasi((int)sumR, blok);
                            hijau = dilasi((int)sumG, blok);
                            biru = dilasi((int)sumB, blok);
                        }
                    }

                    merah = truncate(merah);
                    hijau = truncate(hijau);
                    biru = truncate(biru);

                    b.SetPixel(i, j, Color.FromArgb(merah, hijau, biru));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / bit.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        private void openingSquare9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[81] {
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                };

                b = dimensi9x9(b, se, "erosi");
                this.pbOutput.Image = dimensi9x9(b, se, "dilasi");
            }
        }

        private void closingSquare9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[81] {
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                };

                b = dimensi9x9(b, se, "dilasi");
                this.pbOutput.Image = dimensi9x9(b, se, "erosi");
            }
        }

        public Bitmap pengurangan(Bitmap bit1, Bitmap bit2)
        {
            Bitmap b = new Bitmap(bit1);
            Color c1, c2;
            int merah = 0, hijau = 0, biru = 0;

            progressBar1.Visible = true;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    c1 = bit1.GetPixel(i, j);
                    c2 = bit2.GetPixel(i, j);

                    merah = c1.R - c2.R;
                    hijau = c1.G - c2.G;
                    biru = c1.B - c2.B;

                    merah = truncate(merah);
                    hijau = truncate(hijau);
                    biru = truncate(biru);

                    b.SetPixel(i, j, Color.FromArgb(merah, hijau, biru));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        private void tophatTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[81] {
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                };

                b2 = dimensi9x9(b, se, "erosi");
                b2 = dimensi9x9(b2, se, "dilasi");
                this.pbOutput.Image = pengurangan(b, b2);
            }
        }

        private void bottomhatTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[81] {
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1, 1, 1, 1, 1,
                };

                b2 = dimensi9x9(b, se, "dilasi");
                b2 = dimensi9x9(b2, se, "erosi");
                this.pbOutput.Image = pengurangan(b2, b);
            }
        }
        
        private void openingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }
        
        private void closingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                b = dimensi3x3(b, se, "grayscale_dilasi");
                this.pbOutput.Image = dimensi3x3(b, se, "grayscale_erosi");
            }
        }
        
        private void grayscaleTophatTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                b2 = dimensi3x3(b, se, "grayscale_erosi");
                b2 = dimensi3x3(b2, se, "grayscale_dilasi");
                this.pbOutput.Image = pengurangan(b, b2);
            }
        }

        private void grayscaleBottomhatTransformToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }

        public Bitmap penambahan(Bitmap bit1, Bitmap bit2)
        {
            Bitmap b = new Bitmap(bit1);
            Color c1, c2;
            int merah = 0, hijau = 0, biru = 0;

            progressBar1.Visible = true;
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    c1 = bit1.GetPixel(i, j);
                    c2 = bit2.GetPixel(i, j);

                    merah = c1.R + c2.R;
                    hijau = c1.G + c2.G;
                    biru = c1.B + c2.B;

                    merah = truncate(merah);
                    hijau = truncate(hijau);
                    biru = truncate(biru);

                    b.SetPixel(i, j, Color.FromArgb(merah, hijau, biru));
                }

                progressBar1.Value = Convert.ToInt16(100 * (i + 1) / b.Width);
            }
            progressBar1.Visible = false;

            return b;
        }

        private void edgeSobelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] sePlus45, seMinus45, seHorizontal, seVertical;

                sePlus45 = new int[9] {
                    0, 1, 2,
                    -1, 0, 1,
                    -2, -1, 0
                };

                seMinus45 = new int[9] {
                    -2, -1, 0,
                    -1, 0, 1,
                    0, 1, 2
                };

                seHorizontal = new int[9] {
                    -1, -2, -1,
                    0, 0, 0,
                    1, 2, 1
                };

                seVertical = new int[9] {
                    -1, 0, 1,
                    -2, 0, 2,
                    -1, 0, 1
                };

                b2 = konvolusiFilter3x3(b, seHorizontal);
                b3 = konvolusiFilter3x3(b, sePlus45);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seVertical);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seMinus45);
                b3 = penambahan(b2, b3);

                this.pbOutput.Image = b3;
            }
        }

        private void edgeSobelPlus45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 2,
                    -1, 0, 1,
                    -2, -1, 0
                };
                
                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgeSobelMinus45ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -2, -1, 0,
                    -1, 0, 1,
                    0, 1, 2
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgeSobelHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, -2, -1,
                    0, 0, 0,
                    1, 2, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void egdeSobelVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, 0, 1,
                    -2, 0, 2,
                    -1, 0, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgePrewittToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] sePlus45, seMinus45, seHorizontal, seVertical;

                sePlus45 = new int[9] {
                     0, 1, 1,
                    -1, 0, 1,
                    -1, -1, 0
                };

                seMinus45 = new int[9] {
                    -1, -1, 0,
                    -1, 0, 1,
                    0, 1, 1
                };

                seHorizontal = new int[9] {
                    -1, -1, -1,
                    0, 0, 0,
                    1, 1, 1
                };

                seVertical = new int[9] {
                    -1, 0, 1,
                    -1, 0, 1,
                    -1, 0, 1
                };

                b2 = konvolusiFilter3x3(b, seHorizontal);
                b3 = konvolusiFilter3x3(b, sePlus45);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seVertical);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seMinus45);
                b3 = penambahan(b2, b3);

                this.pbOutput.Image = b3;
            }
        }

        private void edgePrewittPlus45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 1,
                    -1, 0, 1,
                    -1, -1, 0
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgePrewittMinus45ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                   -1, -1, 0,
                    -1, 0, 1,
                    0, 1, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgePrewittHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, -1, -1,
                    0, 0, 0,
                    1, 1, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void edgePrewittVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, 0, 1,
                    -1, 0, 1,
                    -1, 0, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void pointDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, -8, 1,
                    1, 1, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void lineDetectionAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] sePlus45, seMinus45, seHorizontal, seVertical;

                sePlus45 = new int[9] {
                    2, -1, -1,
                    -1, 2, -1,
                    -1, -1, 2
                };

                seMinus45 = new int[9] {
                    -1, -1, 2,
                    -1, 2, -1,
                    2, -1, -1
                };

                seHorizontal = new int[9] {
                    -1, -1, -1,
                    2, 2, 2,
                    -1, -1, -1
                };

                seVertical = new int[9] {
                    -1, 2, -1,
                    -1, 2, -1,
                    -1, 2, -1
                };

                b2 = konvolusiFilter3x3(b, seHorizontal);
                b3 = konvolusiFilter3x3(b, sePlus45);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seVertical);
                b3 = penambahan(b2, b3);

                b2 = konvolusiFilter3x3(b, seMinus45);
                b3 = penambahan(b2, b3);

                this.pbOutput.Image = b3;
            }
        }

        private void lineDetectionPlus45toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    2, -1, -1,
                    -1, 2, -1,
                    -1, -1, 2
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void lineDetectionMinus45toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, -1, 2,
                    -1, 2, -1,
                    2, -1, -1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void lineDetectionHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, -1, -1,
                    2, 2, 2,
                    -1, -1, -1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void lineDetectionVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    -1, 2, -1,
                    -1, 2, -1,
                    -1, 2, -1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, se);
            }
        }

        private void square3X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                b2 = dimensi3x3(b, se, "grayscale_dilasi");
                b3 = dimensi3x3(b, se, "graysacale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void cross3X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 0,
                    1, 1, 1,
                    0, 1, 0
                };

                b2 = dimensi3x3(b, se, "grayscale_dilasi");
                b3 = dimensi3x3(b, se, "graysacale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void square3X3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                b2 = dimensi3x3(b, se, "grayscale_dilasi");
                b3 = dimensi3x3(b, se, "grayscale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void cross3X3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[9] {
                    0, 1, 0,
                    1, 1, 1,
                    0, 1, 0
                };

                b2 = dimensi3x3(b, se, "grayscale_dilasi");
                b2 = dimensi3x3(b2, se, "grayscale_erosi");
                this.pbOutput.Image = pengurangan(b2, b);
            }
        }

        private void square5X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1

                };

                b = dimensi5x5(b, se, "grayscale_erosi");
                this.pbOutput.Image = dimensi5x5(b, se, "grayscale_dilasi");
            }
        }

        private void circle5X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 1, 1, 1
                };

                b = dimensi5x5(b, se, "grayscale_erosi");
                this.pbOutput.Image = dimensi5x5(b, se, "grayscale_dilasi");
            }
        }

        private void morphologyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pbOutput_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void square5X5ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1
                };

                b2 = dimensi5x5(b, se, "grayscale_dilasi");
                b3 = dimensi5x5(b, se, "graysacale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void circle5X5ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 1, 1, 1
                };

                b2 = dimensi5x5(b, se, "grayscale_dilasi");
                b3 = dimensi5x5(b, se, "graysacale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void square5X5ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1
                };

                b2 = dimensi5x5(b, se, "grayscale_dilasi");
                b3 = dimensi5x5(b, se, "grayscale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void circle5X5ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b2 = new Bitmap((Bitmap)this.pbInput.Image);
                Bitmap b3 = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 1, 1, 1
                };

                b2 = dimensi5x5(b, se, "grayscale_dilasi");
                b3 = dimensi5x5(b, se, "grayscale_erosi");
                this.pbOutput.Image = pengurangan(b2, b3);
            }
        }

        private void edgeDetetctionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void x3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    1, 1, 1,
                    1, 1, 1,
                    1, 1, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] kernel;
                kernel = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                    1, 1, 1, 1, 1,
                };
                this.pbOutput.Image = konvolusiFilter5x5(b, kernel);
            }
        }

        private void lowPassFilterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    1, 1, 1,
                    1, 4, 1,
                    1, 1, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void highPassFilterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap(pbInput.Image);

                int[] kernel;
                kernel = new int[9] {
                    -1, 0, 1,
                    -1, 0, 3,
                    -3, 0, 1
                };

                this.pbOutput.Image = konvolusiFilter3x3(b, kernel);
            }
        }

        private void otsuThresholdingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bitmap bitmapotsu = (Bitmap)pbInput.Image.Clone();
            otsu_bitmap = OtsuThresholding.Otsu_Threshold(bitmapotsu, true, ref threshold);
            pbOutput.Image = otsu_bitmap;
            textBox1.Text = threshold.ToString();
        }

        private void erosionCircle5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 1, 1, 1
                };
                this.pbOutput.Image = dimensi5x5(b, se, "erosi");
            }
        }

        private void dilationCircle5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pbInput.Image == null)
            {
                MessageBox.Show("Tidak ada citra yang akan diolah");
            }
            else
            {
                Bitmap b = new Bitmap((Bitmap)this.pbInput.Image);

                int[] se;
                se = new int[25] {
                    1, 1, 1, 1, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 0, 0, 0, 1,
                    1, 1, 1, 1, 1
                };
                this.pbOutput.Image = dimensi5x5(b, se, "dilasi");
            }
        }
    }
}
