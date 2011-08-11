using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace mazio.util {
    internal static class GraphicsUtils {

        public static void drawText(Graphics g, Font f, Brush fg, Brush bg, string text, float x, float y) {

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;

            GraphicsPath gp = new GraphicsPath();
            gp.AddString(text, f.FontFamily, (int)f.Style, f.Size, new PointF(x, y), StringFormat.GenericTypographic);
            Pen p = new Pen(bg, 4) {
                LineJoin = LineJoin.Round,
                EndCap = LineCap.Round,
                StartCap = LineCap.Round
            };

            g.DrawPath(p, gp);
            p.Dispose();
            gp.Dispose();
            g.DrawString(text, f, fg, x, y, StringFormat.GenericTypographic);
        }

    }
}
