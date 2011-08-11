using System.Drawing;
using System.Drawing.Imaging;

namespace mazio {
    //
    // the algorithms below are from: 
    // http://www.codeproject.com/KB/GDI-plus/csharpgraphicfilters11.aspx
    // http://www.codeproject.com/KB/GDI-plus/csharpfilters.aspx
    // http://www.codeproject.com/KB/GDI-plus/displacementfilters.aspx
    //

    public class ImageProcessor {
        public class ConvMatrix {
            public int TopLeft = 0, TopMid = 0, TopRight = 0;
            public int MidLeft = 0, Pixel = 1, MidRight = 0;
            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
            public int Factor = 1;
            public int Offset = 0;

            public void SetAll(int nVal) {
                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =
                                                                BottomLeft = BottomMid = BottomRight = nVal;
            }
        }

        public static bool Conv3x3(Bitmap b, ConvMatrix m) {
            // Avoid divide by zero errors

            if (0 == m.Factor)
                return false;
            Bitmap
                // GDI+ still lies to us - the return format is BGR, NOT RGB. 
                bSrc = (Bitmap) b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite,
                                           PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                                             ImageLockMode.ReadWrite,
                                             PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            int stride2 = stride*2;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe {
                byte* p = (byte*) (void*) Scan0;
                byte* pSrc = (byte*) (void*) SrcScan0;
                int nOffset = stride - b.Width*3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y) {
                    for (int x = 0; x < nWidth; ++x) {
                        nPixel = ((((pSrc[2]*m.TopLeft) +
                                    (pSrc[5]*m.TopMid) +
                                    (pSrc[8]*m.TopRight) +
                                    (pSrc[2 + stride]*m.MidLeft) +
                                    (pSrc[5 + stride]*m.Pixel) +
                                    (pSrc[8 + stride]*m.MidRight) +
                                    (pSrc[2 + stride2]*m.BottomLeft) +
                                    (pSrc[5 + stride2]*m.BottomMid) +
                                    (pSrc[8 + stride2]*m.BottomRight))
                                   /m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[5 + stride] = (byte) nPixel;

                        nPixel = ((((pSrc[1]*m.TopLeft) +
                                    (pSrc[4]*m.TopMid) +
                                    (pSrc[7]*m.TopRight) +
                                    (pSrc[1 + stride]*m.MidLeft) +
                                    (pSrc[4 + stride]*m.Pixel) +
                                    (pSrc[7 + stride]*m.MidRight) +
                                    (pSrc[1 + stride2]*m.BottomLeft) +
                                    (pSrc[4 + stride2]*m.BottomMid) +
                                    (pSrc[7 + stride2]*m.BottomRight))
                                   /m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[4 + stride] = (byte) nPixel;

                        nPixel = ((((pSrc[0]*m.TopLeft) +
                                    (pSrc[3]*m.TopMid) +
                                    (pSrc[6]*m.TopRight) +
                                    (pSrc[0 + stride]*m.MidLeft) +
                                    (pSrc[3 + stride]*m.Pixel) +
                                    (pSrc[6 + stride]*m.MidRight) +
                                    (pSrc[0 + stride2]*m.BottomLeft) +
                                    (pSrc[3 + stride2]*m.BottomMid) +
                                    (pSrc[6 + stride2]*m.BottomRight))
                                   /m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 + stride] = (byte) nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return true;
        }

        public static bool Smooth(Bitmap b, int nWeight /* default to 1 */) {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = nWeight + 8;

            return Conv3x3(b, m);
        }

        public static bool GaussianBlur(Bitmap b) {
            ConvMatrix m = new ConvMatrix();
            m.TopLeft = m.TopRight = m.BottomLeft = m.BottomRight = 1;
            m.Pixel = 4;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;

            m.Factor = 16;

            return Conv3x3(b, m);
        }

        public static bool OffsetFilter(Bitmap b, Point[,] offset) {
            Bitmap bSrc = (Bitmap) b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite,
                                           PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite,
                                             PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe {
                byte* p = (byte*) (void*) Scan0;
                byte* pSrc = (byte*) (void*) SrcScan0;

                int nOffset = bmData.Stride - b.Width*3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y) {
                    for (int x = 0; x < nWidth; ++x) {
                        xOffset = offset[x, y].X;
                        yOffset = offset[x, y].Y;

                        if (y + yOffset >= 0 && y + yOffset < nHeight && x + xOffset >= 0 && x + xOffset < nWidth) {
                            p[0] = pSrc[((y + yOffset)*scanline) + ((x + xOffset)*3)];
                            p[1] = pSrc[((y + yOffset)*scanline) + ((x + xOffset)*3) + 1];
                            p[2] = pSrc[((y + yOffset)*scanline) + ((x + xOffset)*3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public static bool Pixelate(Bitmap b, short pixel, bool bGrid) {
            int nWidth = b.Width;
            int nHeight = b.Height;

            Point[,] pt = new Point[nWidth,nHeight];

            int newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y) {
                    newX = pixel - x%pixel;

                    if (bGrid && newX == pixel)
                        pt[x, y].X = -x;
                    else if (x + newX > 0 && x + newX < nWidth)
                        pt[x, y].X = newX;
                    else
                        pt[x, y].X = 0;

                    newY = pixel - y%pixel;

                    if (bGrid && newY == pixel)
                        pt[x, y].Y = -y;
                    else if (y + newY > 0 && y + newY < nHeight)
                        pt[x, y].Y = newY;
                    else
                        pt[x, y].Y = 0;
                }

            OffsetFilter(b, pt);

            return true;
        }

        public static bool Invert(Bitmap b) {
            // GDI+ still lies to us - the return format is BGR, NOT RGB. 

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe {
                byte* p = (byte*) (void*) Scan0;
                int nOffset = stride - b.Width*3;
                int nWidth = b.Width*3;
                for (int y = 0; y < b.Height; ++y) {
                    for (int x = 0; x < nWidth; ++x) {
                        p[0] = (byte) (255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            return true;
        }
    }
}