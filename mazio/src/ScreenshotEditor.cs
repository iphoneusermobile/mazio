using System.Drawing;
using System.Windows.Forms;

namespace mazio
{
    public interface ScreenshotEditor
    {
        bool HaveScreenshot { get; }
        Bitmap Screenshot { get; }
        ViewParameters CurrentViewParameters { get; }
        bool CanAddNewShape { get;  set; }
        //Size ScaledScreenshotSize { get; }
        //int ZoomFactor { get; }
        //Size CanvasSize { get; }
        //Size CanvasHolderSize { get; }
        //Point CanvasOffset { get; }
        Graphics CanvasGraphics { get; }
        Color BackgroundColor { get; }
        Bitmap getAnnotatedScreenshot(Shape upTo);
        void addControl(Control c);
        void removeControl(Control c);
        void addSettingsChangeListener(SettingsChangeListener l);
        void removeSettingsChangeListener(SettingsChangeListener l);
        void notifyUndoRedoState(bool undoEmpty, bool redoEmpty);
        void addCommandToUndo(Command c);
        void invalidateView();
        void invalidateAllViews();
        void setCursor(Cursor c);
    }

    public interface SettingsChangeListener
    {
        void colorChanged(Color color);
        void thicknessChanged(int thickness);
        void screenShotGrabbed(Bitmap screenshot);
        void screenshotZoomed(int zoomFactor);
        void canvasResized(Size newSize);
    }
}
