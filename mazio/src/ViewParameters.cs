using System.Drawing;

namespace mazio
{
    public class ViewParameters
    {
        private readonly int zoomFactor;
        private readonly Point offset;
        private readonly Size canvasSize;
        private readonly Point scrolls;
        private readonly Size viewport;
        private readonly bool forSave;
        
        public ViewParameters(int zoomFactor, Point offset, Size canvasSize, Point scrolls, Size viewport, bool forSave) {
            this.zoomFactor = zoomFactor;
            this.offset = offset;
            this.canvasSize = canvasSize;
            this.scrolls = scrolls;
            this.viewport = viewport;
            this.forSave = forSave;
        }

        public int ZoomFactor { get { return zoomFactor; } }
        public float ZoomRatio { get { return zoomFactor / 100.0f; } }
        public Point Offset { get { return offset; } }
        public Size CanvasSize { get { return canvasSize; } }
        public Point Scrolls { get { return scrolls; } }
        public Size Viewport { get { return viewport; } }
        public bool ForSave { get { return forSave; } }

        public Point toAbsolute(PointF point) {
            return new Point((int)(point.X * ZoomRatio) + Offset.X - Scrolls.X, (int)(point.Y * ZoomRatio) + Offset.Y - Scrolls.Y); 
        }

        public PointF toRelative(Point point) {
            return new PointF((point.X - Offset.X) / ZoomRatio, (point.Y - Offset.Y) / ZoomRatio); 
        }
    }
}
