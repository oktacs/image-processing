using System.Drawing;
using System.Drawing.Imaging;

namespace ImgProc1
{
    internal class OtsuThresholding
    {
        public static unsafe Bitmap Otsu_Threshold(Bitmap bitmap, bool isGray, ref int threshold)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte* p = (byte*)data.Scan0;
            int offset = data.Stride - bitmap.Width * 3;
            if (isGray == false)
            {
                for (int i = 0; i < bitmap.Height; i++)
                {
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        byte t = (byte)(p[0] * 0.07f + p[1] * 0.72f + p[2] * 0.21);
                        p[0] = p[1] = p[2] = t;
                        p += 3;
                    }
                    p += offset;
                }
                p = (byte*)data.Scan0;
            }
            int[] count = new int[256];
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    count[p[0]]++;
                    p += 3;
                }
                p += offset;
            }
            bitmap.UnlockBits(data);
            int total = bitmap.Width * bitmap.Height;
            float sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += i * count[i];
            }
            float sumB = 0;
            int wB = 0;
            int wF = 0;
            float varMax = 0;
            threshold = 0;
            for (int t = 0; t < 256; t++)
            {
                wB += count[t];
                if (wB == 0)
                    continue;
                wF = total - wB;
                if (wF == 0)
                    break;

                sumB += (float)(t * count[t]);
                float mB = sumB / wB;
                float mF = (sum - sumB) / wF;
                float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);
                if (varMax < varBetween)
                {
                    varMax = varBetween;
                    threshold = t;
                }

            }
            Bitmap otsu = bitmap;
            data = otsu.LockBits(new Rectangle(0, 0, otsu.Width, otsu.Height), ImageLockMode.ReadWrite, otsu.PixelFormat);
            p = (byte*)data.Scan0;
            offset = data.Stride - otsu.Width * 3;
            for (int i = 0; i < otsu.Height; i++)
            {
                for (int j = 0; j < otsu.Width; j++)
                {
                    if (p[0] > threshold)
                    {
                        p[0] = 255;
                        p[1] = 255;
                        p[2] = 255;
                    }
                    else
                    {
                        p[0] = 0;
                        p[1] = 0;
                        p[2] = 0;
                    }
                    p += 3;
                }
                p += offset;
            }
            otsu.UnlockBits(data);
            return otsu;
        }
    }
}
