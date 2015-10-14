using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32;
using mazio.shapes;
using mazio.uploaders;
using Encoder = System.Drawing.Imaging.Encoder;

namespace mazio {
    public partial class Mazio : Form, ScreenshotEditor {
        private Bitmap backBuffer;

        private const int BORDER_SIZE = 74;

        private int bmpWidth = 200;
        private int bmpHeight = 200;

        private Grabber grabber;

        private System.Windows.Forms.Timer delayer;
        private System.Windows.Forms.Timer delayerTo;

        private System.Windows.Forms.Timer pasteTimer;

        private string imageFileName;

        private Color currentColor;

        private LinkedList<Shape> shapes = new LinkedList<Shape>();

        private readonly UndoRedo undoRedo;

        private readonly List<SettingsChangeListener> settigsListeners = new List<SettingsChangeListener>();

        private RectangleF cropChangeStart, cropChangeEnd;

        private PointF moveFrom, moveTo;

        private class ShapeButtonInfo {
            public ShapeButtonInfo(int idx, CreateShape create, CreateSettingsPanel panelCreate, string hint) {
                this.idx = idx;
                this.create = create;
                this.panelCreate = panelCreate;
                this.hint = hint;
            }

            public readonly int idx;
            public readonly string hint;

            public delegate Shape CreateShape(PointF p, ShapeSettingsPanel panel);

            public delegate ShapeSettingsPanel CreateSettingsPanel();

            public readonly CreateShape create;
            public readonly CreateSettingsPanel panelCreate;
        }

        private readonly Dictionary<CheckBox, ShapeButtonInfo> shapeButtons =
            new Dictionary<CheckBox, ShapeButtonInfo>();

        private CropBox cropBox;

        private bool cropHit;
        private Shape shapeHit;
        private Shape addingShape;
        private Shape editingShape;
        private string editingHandle;

        private Button grabButton;
        private Button startDrawingButton;

        private const string REG_SELECTED_UPLOADER = "UploadTo";
        private const string REG_FILE_TYPE = "FileType";
        private const string REG_COLOR_R = "ColorR";
        private const string REG_COLOR_G = "ColorG";
        private const string REG_COLOR_B = "ColorB";
        private const string REG_LINE_WIDTH = "LineWidth";
        private const string REG_OPACITY = "Opacity";
        private const string REG_SELECTED_TOOL = "SelectedTool";

        private class CustomPanel : Panel {
            public CustomPanel() {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer, true);
            }
        }

        private CustomPanel canvasPanel;

        private ScreenShot screenshot;

        public bool HaveScreenshot {
            get { return screenshot != null; }
        }

        private bool canAddNewShape = true;

        public bool CanAddNewShape {
            get { return canAddNewShape; }
            set { canAddNewShape = value; }
        }

        private ViewParameters viewParams;

        private readonly string runtimeArgs;

        private enum ProcessDPIAwareness {
            ProcessDPIUnaware = 0,
            ProcessSystemDPIAware = 1,
            ProcessPerMonitorDPIAware = 2
        }

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(ProcessDPIAwareness value);

        private static void SetDpiAwareness(ProcessDPIAwareness awareness) {
            try {
                if (Environment.OSVersion.Version.Major >= 6) {
                    SetProcessDpiAwareness(awareness);
                }
            }
            catch (EntryPointNotFoundException)//this exception occures if OS does not implement this API, just ignore it.
            {
            }
        }

        public Mazio() {
            SetDpiAwareness(ProcessDPIAwareness.ProcessPerMonitorDPIAware);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                runtimeArgs = args[1];

            if (Array.IndexOf(args, "-sg") > 0 || Array.IndexOf(args, "--startgrabbing") > 0) {
                Shown += startGrabbingOnStartup;
            }

            InitializeComponent();

//            getQueryStringParameters();

            Thread thr = new Thread(checkNewVersion);
            thr.Start();

            undoRedo = new UndoRedo(this);

//            Icon = Properties.Resources.trayIcon_Icon;
//            trayIcon.Icon = Properties.Resources.trayIcon_Icon;

            trayIcon.Visible = false;

            boxFileName.Text = "mazio";

            ConfigDialog.ShowUsageHintsChanged += showUsageHintsChanged;
            ConfigDialog.MagnifierZoomFactorChanged += magnifierZoomFactorChanged;

            setupCanvasPanel();

            canvasPanel.MouseDown += onMouseDown;
            canvasPanel.MouseUp += onMouseUp;
            canvasPanel.MouseMove += onMouseMove;
            canvasPanel.MouseDoubleClick += onMouseDoubleClick;
            setupShapeButtons();

            registerMazioProtocol(args[0]);

            setupInvalidateHandler();

            setupToolTips();

            setupFileTypes();

            setupUploaders();

            setupWidthTrackBar();

            setupOpacityTrackBar();

            setupColor();

            setupStartingButtons();

            setSelectedTool();

            setDragLabelEnableState(false);

            Size = new Size(500, 500);
            setSizes();

            setViewParams();

            setPasteTimer();

            ShapeSettings.Instance.Owner = this;

            BackgroundTransformator.Instance.SettingsChanged += (s, e) => {
                                                                    setViewParams();
                                                                    invalidateAllViews();
                                                                };
        }

        private void startGrabbingOnStartup(object sender, EventArgs e) {
            Shown -= startGrabbingOnStartup;
            startGrabber();
        }

        private void magnifierZoomFactorChanged(object sender, ConfigDialog.ValueEventArgs<int> e) {
            invalidateView();
        }

        private void showUsageHintsChanged(object sender, ConfigDialog.ValueEventArgs<bool> e) {
            invalidateView();
        }

        private static void registerMazioProtocol(string self) {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\mazio");
            if (key == null) {
                return;
            }
            key.SetValue("", "URL:mazio Protocol");
            key.SetValue("URL Protocol", "");
            key = key.CreateSubKey("shell");
            if (key == null) {
                return;
            }
            key = key.CreateSubKey("open");
            if (key == null) {
                return;
            }
            key = key.CreateSubKey("command");
            if (key != null) {
                key.SetValue("", "\"" + self + "\"" + " \"%1\"");
            }
        }

/*
        private static NameValueCollection getQueryStringParameters()
        {
            NameValueCollection col = new NameValueCollection();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (ApplicationDeployment.CurrentDeployment.ActivationUri != null)
                {
                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                    col = HttpUtility.ParseQueryString(queryString);
                }
            }

            return col;
        }  
*/

        public Bitmap Screenshot {
            get { return screenshot != null ? screenshot.Shot : null; }
        }

        public int ZoomFactor {
            get { return trackbarZoom.Value; }
        }

        public ViewParameters CurrentViewParameters {
            get { return viewParams; }
        }

        private readonly Dictionary<Shape, Bitmap> annotatedScreenshots = new Dictionary<Shape, Bitmap>();

        private void invalidateAnnotatedScreenshot(object sender, EventArgs e) {
            foreach (Bitmap b in annotatedScreenshots.Values)
                b.Dispose();

            annotatedScreenshots.Clear();
        }

        public void setCursor(Cursor c) {
            Cursor = c;
        }

        public Bitmap getAnnotatedScreenshot(Shape upTo) {
            if (!annotatedScreenshots.ContainsKey(upTo)) {
// ReSharper disable PossibleLossOfFraction
                int w = (int) (canvasPanel.Size.Width*100/ZoomFactor + 0.5);
// ReSharper restore PossibleLossOfFraction
                int h = canvasPanel.Size.Height*100/ZoomFactor;

                if (w < 1) w = 1;
                if (h < 1) h = 1;

                Size sz = new Size(w, h);
                Bitmap b = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(b);

                g.Clear(HaveScreenshot ? SystemColors.Desktop : Color.White);

                ViewParameters p = new ViewParameters(100, new Point(w/2, h/2), sz, new Point(0, 0), sz, false);

                if (screenshot != null)
                    screenshot.getView().paint(g, false, p);

                if (upTo != null) {
                    foreach (Shape s in shapes) {
                        if (s == upTo)
                            break;
                        s.getView().paint(g, false, p);
                    }
                }

                if (upTo != null) {
                    annotatedScreenshots[upTo] = b;
                }
            }

            return upTo != null ? annotatedScreenshots[upTo] : null;
        }

        public void invalidateAllViews() {
            invalidateAnnotatedScreenshot(null, null);
            invalidateView();
        }

        public void invalidateView() {
            int x = canvasHolderPanel.HorizontalScroll.Value;
            int y = canvasHolderPanel.VerticalScroll.Value;
            int w = canvasHolderPanel.ClientSize.Width;
            int h = canvasHolderPanel.ClientSize.Height;
            Rectangle r = new Rectangle(x, y, w, h);

            canvasPanel.Invalidate(r);
        }

        public Size CanvasSize {
            get {
                int w, h;
                if (cropBox == null) {
                    w = bmpWidth + 2 * ConfigDialog.Instance.GrabMargin * ZoomFactor / 100;
                    h = bmpHeight + 2 * ConfigDialog.Instance.GrabMargin * ZoomFactor / 100;
                } else {
                    PointF maxOffset = cropBox.MaxOffset;
                    int x = Math.Max((int) (maxOffset.X * ZoomFactor / 100), bmpWidth / 2);
                    int y = Math.Max((int) (maxOffset.Y * ZoomFactor / 100), bmpHeight / 2);
                    w = 2 * (x + ConfigDialog.Instance.GrabMargin * ZoomFactor / 100);
                    h = 2 * (y + ConfigDialog.Instance.GrabMargin * ZoomFactor / 100);
                }

                return new Size(w, h);
            }
        }

        public Point CanvasOffset {
            get {
                Size s = CanvasSize;
                int w = s.Width;
                int h = s.Height;

                if (canvasHolderPanel.Width > w)
                    w = canvasHolderPanel.Width;
                if (canvasHolderPanel.Height > h)
                    h = canvasHolderPanel.Height;

                return new Point(w/2, h/2);
            }
        }

        public Graphics CanvasGraphics {
            get { return canvasPanel.CreateGraphics(); }
        }

        public Color BackgroundColor {
            get { return canvasPanel.BackColor; }
        }

        public void addControl(Control c) {
            canvasPanel.Controls.Add(c);
        }

        public void removeControl(Control c) {
            // need to hide the control first, otherwise canvas get scrolled to (0, 0)
            c.Visible = false;
            canvasPanel.Focus();
            canvasPanel.Controls.Remove(c);
        }

        public void addCommandToUndo(Command c) {
            undoRedo.addCommand(c);
        }

        public void addSettingsChangeListener(SettingsChangeListener l) {
            settigsListeners.Add(l);
        }

        public void removeSettingsChangeListener(SettingsChangeListener l) {
            if (settigsListeners.Contains(l))
                settigsListeners.Remove(l);
        }

        private void setSelectedTool() {
            int tool = 0;
            try {
                tool = Math.Max(0,
                                Math.Min(SettingsWrapper.getSettingValue(REG_SELECTED_TOOL, 0),
                                         shapeButtons.Keys.Count - 1));
            } catch (NullReferenceException) {}
            foreach (CheckBox check in shapeButtons.Keys)
                if (shapeButtons[check].idx == tool) {
                    check.Checked = true;
                    ShapeButtonInfo.CreateSettingsPanel create = shapeButtons[check].panelCreate;
                    if (create != null)
                        ShapeSettings.Instance.Contents = create();
                    break;
                }

            // unselect crop if selected
            if (btnCrop.Checked) {
                uncheckShapeButtons();
                btnArrow.Checked = true;
                ShapeSettings.Instance.Contents = shapeButtons[btnArrow].panelCreate();
            }
        }

        private void setupWidthTrackBar() {
            trackbarWidth.Minimum = 2;
            trackbarWidth.TickFrequency = 2;
            trackbarWidth.Maximum = 20;

            int l = 10;
            try {
                l = Math.Max(2, Math.Min(20, SettingsWrapper.getSettingValue(REG_LINE_WIDTH, 0)));
            } catch (NullReferenceException) {}
            trackbarWidth.Value = l;
            labelSize.Text = trackbarWidth.Value.ToString();
        }

        private void setupOpacityTrackBar() {
            trackbarOpacity.Minimum = 10;
            trackbarOpacity.TickFrequency = 25;
            trackbarOpacity.Maximum = 255;

            int l = 255;
            try {
                l = Math.Max(10, Math.Min(255, SettingsWrapper.getSettingValue(REG_OPACITY, 255)));
            } catch (NullReferenceException) {}
            trackbarOpacity.Value = l;
            setOpacity();
        }

        private void setOpacity() {
            int tr = 100*trackbarOpacity.Value/255;
            labelOpacity.Text = "" + tr + "%";
            btnColor.BackColor = CurrentColor;
        }

        private void setupColor() {
            try {
                int r = Math.Min(0xff, SettingsWrapper.getSettingValue(REG_COLOR_R, 0xff));
                int g = Math.Min(0xff, SettingsWrapper.getSettingValue(REG_COLOR_G, 0));
                int b = Math.Min(0xff, SettingsWrapper.getSettingValue(REG_COLOR_B, 0));
                currentColor = Color.FromArgb(trackbarOpacity.Value, r, g, b);
                btnColor.BackColor = currentColor;
            } catch (NullReferenceException) {}
        }

        private void setupFileTypes() {
            comboFileType.Items.Add(new MazioFormat());
            comboFileType.Items.Add(new PngFormat());
            comboFileType.Items.Add(new JpgFormat());

            int idx = 0;
            try {
                idx = SettingsWrapper.getSettingValue(REG_FILE_TYPE, 0);
            } catch (NullReferenceException) {}

            comboFileType.SelectedIndex = Math.Min(idx, comboFileType.Items.Count - 1);
        }

        private void setupInvalidateHandler() {
            btnUndo.Click += invalidateAnnotatedScreenshot;
            btnRedo.Click += invalidateAnnotatedScreenshot;
            btnClear.Click += invalidateAnnotatedScreenshot;
            btnGrab.Click += invalidateAnnotatedScreenshot;
            btnUp.Click += invalidateAnnotatedScreenshot;
            btnTop.Click += invalidateAnnotatedScreenshot;
            btnDown.Click += invalidateAnnotatedScreenshot;
            btnBottom.Click += invalidateAnnotatedScreenshot;
        }

        private void setupShapeButtons() {
            int i = 0;

            shapeButtons[btnArrow] = new ShapeButtonInfo(i++,
                                                         (p, panel) => new Arrow(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                         Arrow.createSettingsPanel, 
                                                         Arrow.HintText);
            shapeButtons[btnLine] = new ShapeButtonInfo(i++,
                                                        (p, panel) => new Line(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                        Line.createSettingsPanel, 
                                                        Line.HintText);
            shapeButtons[btnCircle] = new ShapeButtonInfo(i++,
                                                          (p, panel) => new Oval(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                          Oval.createSettingsPanel, 
                                                          Oval.HintText);
            shapeButtons[btnRectangle] = new ShapeButtonInfo(i++,
                                                             (p, panel) => new RectangleShape(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                             RectangleShape.createSettingsPanel, 
                                                             RectangleShape.HintText);
            shapeButtons[btnText] = new ShapeButtonInfo(i++,
                                                        (p, panel) => new TextShape(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                        TextShape.createSettingsPanel, 
                                                        TextShape.HintText);
            shapeButtons[btnMagnifier] = new ShapeButtonInfo(i++,
                                                             (p, panel) => new MagnifyingGlass(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                             MagnifyingGlass.createSettingsPanel, 
                                                             MagnifyingGlass.HintText);
            shapeButtons[btnPonyVille] = new ShapeButtonInfo(i++,
                                                             (p, panel) => new PonyVille(this, CurrentColor, trackbarWidth.Value, p, panel), 
                                                             PonyVille.createSettingsPanel, 
                                                             PonyVille.HintText);
            shapeButtons[checkImage] = new ShapeButtonInfo(i++,
                                                             (p, panel) => new Picture(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                             Picture.createSettingsPanel,
                                                             Picture.HintText);
            shapeButtons[btnCensor] = new ShapeButtonInfo(i++,
                                                          (p, panel) => new Censor(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                          Censor.createSettingsPanel, 
                                                          Censor.HintText);
            shapeButtons[btnPencil] = new ShapeButtonInfo(i++,
                                                          (p, panel) => new Pencil(this, CurrentColor, trackbarWidth.Value, p, panel),
                                                          Pencil.createSettingsPanel, 
                                                          Pencil.HintText);

            shapeButtons[btnCrop] = new ShapeButtonInfo(i, delegate { return null; }, CropBox.createSettingsPanel, CropBox.HintText);
        }

        private void setupUploaders() {
            UploadType t = CommandLineParser.Instance.getUploader(runtimeArgs);
            if (t == null) {
                comboUploadSite.Items.Add(new FlickrUploadBuilder());
                comboUploadSite.Items.Add(new JiraUploadBuilder());
                comboUploadSite.Items.Add(new PicasawebUploadBuilder());
                comboUploadSite.Items.Add(new SkitchUploadBuilder());

                int idx = 0;
                try {
                    idx = SettingsWrapper.getSettingValue(REG_SELECTED_UPLOADER, 0);
                } catch (NullReferenceException) {}

                comboUploadSite.SelectedIndex = Math.Min(idx, comboUploadSite.Items.Count - 1);
            } else {
                comboUploadSite.Items.Add(t);
                comboUploadSite.SelectedIndex = 0;
                comboUploadSite.Visible = false;
                btnUpload.Text = "Upload to " + t.getName();
                btnUpload.Size = new Size(btnUpload.Size.Width + 100, btnUpload.Size.Height);
            }
        }

        private void setupCanvasPanel() {
            canvasPanel = new CustomPanel();
            canvasHolderPanel.BackColor = SystemColors.Desktop;
            canvasPanel.BackColor = SystemColors.Desktop;
            canvasPanel.Location = new Point(0, 0);
            canvasPanel.Size = new Size(200, 100);
            canvasPanel.TabStop = false;
            canvasPanel.Paint += canvasPanel_Paint;
            canvasPanel.AutoScroll = false;
            canvasHolderPanel.Controls.Add(canvasPanel);
            canvasHolderPanel.Scroll += onScroll;
        }

        private void onScroll(object sender, ScrollEventArgs e) {
            //Debug.WriteLine("onScroll: " + sender + " e: " + e);
            setViewParams();
            invalidateView();
        }

        private void labelDrag_MouseDown(object sender, MouseEventArgs e) {
            if (screenshot == null && shapes.Count == 0)
                return;

            string name = saveBitmapToTemp();

            DataObject d = new DataObject();
            StringCollection col = new StringCollection {name};

            d.SetFileDropList(col);
            DoDragDrop(d, DragDropEffects.Copy);
        }

        private Bitmap getImageToSave() {
            int factor = checkSaveAtFullSize.Checked ? 100 : ZoomFactor;

            int w, h;

            if (checkSaveAtFullSize.Checked) {
// ReSharper disable PossibleLossOfFraction
                w = (int) (canvasPanel.Width*100/ZoomFactor + 0.5);
                h = (int) (canvasPanel.Height*100/ZoomFactor + 0.5);
// ReSharper restore PossibleLossOfFraction
            } else {
                w = canvasPanel.Width;
                h = canvasPanel.Height;
            }

            if (w < 1) w = 1;
            if (h < 1) h = 1;

            Size sz = new Size(w, h);

            Bitmap bitmap = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(bitmap);

            ViewParameters p = new ViewParameters(factor, new Point(sz.Width/2, sz.Height/2), sz, new Point(0, 0), sz, true);

            Shape.UseHilights = false;
            paintPicture(g, false, p);
            Shape.UseHilights = true;

            Bitmap resultBitmap = bitmap;
            if (cropBox != null) {
                Rectangle r = Shape.ShapeView.getShapeRectangle(cropBox.Handles["start"], cropBox.Handles["end"], p);
                resultBitmap = new Bitmap(r.Width, r.Height);
                Graphics gr = Graphics.FromImage(resultBitmap);
                gr.DrawImage(bitmap, 0, 0, r, GraphicsUnit.Pixel);
                bitmap.Dispose();
            }

            return resultBitmap;
        }

        private string saveBitmapToTemp(string name = null, bool raw = false, Format format = null) {
            string createdName = Path.GetTempPath() + (name ?? boxFileName.Text) + comboFileType.SelectedItem;
            saveBitmap(createdName, raw, format);
            return createdName;
        }

        private void saveBitmap(string fileName, bool raw = false, Format format = null) {
            Bitmap resultBitmap = raw ? Screenshot : getImageToSave();
            if (resultBitmap == null) {
                return;
            }

            Format f = format ?? (Format) comboFileType.SelectedItem;

            if (f is MazioFormat) {
                saveInMazioFormat(fileName);
            } else {
                resultBitmap.Save(fileName, f.getCodecInfo(), f.getParameters());
                if (!raw) {
                    resultBitmap.Dispose();
                }
            }
        }

        private void saveInMazioFormat(string fileName) {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8)) {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("mazio");
                writer.WriteStartElement("screenshot");
                if (Screenshot != null) {
                    string tmpFile = saveBitmapToTemp("tempSaveForMazioFormat", true, new PngFormat());
                    byte[] bytes = File.ReadAllBytes(tmpFile);
                    writer.WriteBase64(bytes, 0, bytes.Length);
                }
                writer.WriteEndElement();
                writer.WriteStartElement("shapes");
                foreach (Shape shape in shapes) {
                    shape.toXml(writer);
                }
                if (cropBox != null) {
                    cropBox.toXml(writer);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void loadBitmap(Stream stream, string name) {
            if (name.ToLower().EndsWith(".maz")) {
                loadMazioFile(stream);
            } else {
                loadBitmap(new Bitmap(stream));
            }

            int slashIdx = name.LastIndexOf('\\') + 1;
            int dotIdx = name.LastIndexOf('.');
            boxFileName.Text = name.Substring(slashIdx, dotIdx - slashIdx);
            string ext = name.Substring(dotIdx);
            foreach (Object o in comboFileType.Items) {
                if (!ext.Equals(o.ToString())) {
                    continue;
                }
                comboFileType.SelectedItem = o;
                break;
            }
        }

        private void loadBitmap(Bitmap bmp) {
            screenshot = new ScreenShot(this, bmp);

            scaleToFit(canvasHolderPanel.Width, canvasHolderPanel.Height);

            setSizes();

            updateUiItems();

            ClientSize = new Size(bmpWidth + ConfigDialog.Instance.GrabMargin + BORDER_SIZE,
                                  bmpHeight + ConfigDialog.Instance.GrabMargin + BORDER_SIZE);

            invalidateView();

            cropBox = new CropBox(this, btnCrop.Checked, CropBox.createSettingsPanel());
            cropBox.cropTight();
        }

        private void loadMazioFile(Stream stream) {
            try {
                XPathDocument doc = new XPathDocument(stream);
                XPathNavigator nav = doc.CreateNavigator();
                XPathExpression exp = nav.Compile("/mazio/screenshot");
                XPathNodeIterator it = nav.Select(exp);
                Bitmap bmp = null;
                if (it.MoveNext()) {
                    String b64 = it.Current.Value;
                    byte[] bytes = Convert.FromBase64String(b64);
                    MemoryStream ms = new MemoryStream(bytes);
                    bmp = new Bitmap(ms);
                }
                exp = nav.Compile("/mazio/shapes/shape");
                it = nav.Select(exp);
                List<Shape> l = new List<Shape>();
                while (it.MoveNext()) {
                    l.Add(ShapeFactory.createShape(this, btnCrop.Checked, it.Current.Clone()));
                }
                if (bmp != null) {
                    loadBitmap(bmp);
                } else {
                    screenshot = null;
                }
                shapes.Clear();
                foreach (Shape shape in l) {
                    if (shape != null) {
                        if (shape is CropBox) {
                            cropBox = shape as CropBox;
                        } else {
                            shapes.AddLast(shape);
                        }
                    }
                }
                foreach (Shape s in shapes) {
                    s.finished();
                }

                invalidateAllViews();
            } catch (Exception e) {
                MessageBox.Show("Error: Unable to load Mazio file: " + e.Message);
            }
        }

        public void buttonGrab_Click(object sender, EventArgs e) {
            startGrabber();
        }

        private void startGrabber() {
            Opacity = 0.99;
            grabber = new Grabber(ConfigDialog.Instance.LimitGrabByActiveScreen
                                      ? Screen.FromPoint(Cursor.Position)
                                      : null);
            ShapeSettings.Instance.Hide();
            BackgroundTransformator.Instance.resetAll();
            Hide();
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e) {
            Show();
            WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }

        private void onResize(object sender, EventArgs e) {
            if (FormWindowState.Minimized == WindowState) {
                trayIcon.Visible = true;
                ShapeSettings.Instance.Hide();
                Hide();
            } else {
                setSizes();
                if (ShapeSettings.Instance.Showing) {
                    ShapeSettings.Instance.Show();
                }
            }
        }

        private void setSizes() {
            if (canvasPanel == null)
                return;

            if (screenshot != null) {
                bmpWidth = screenshot.Shot.Width*ZoomFactor/100;
                bmpHeight = screenshot.Shot.Height*ZoomFactor/100;
            }

            Size s = CanvasSize;
            int w = s.Width;
            int h = s.Height;

            if (canvasHolderPanel.Width > w)
                w = canvasHolderPanel.Width;
            if (canvasHolderPanel.Height > h)
                h = canvasHolderPanel.Height;

            canvasPanel.Size = new Size(w, h);
            canvasPanel.Location = new Point(-canvasHolderPanel.HorizontalScroll.Value, -canvasHolderPanel.VerticalScroll.Value);

            if (backBuffer != null) {
                backBuffer.Dispose();
                backBuffer = null;
            }

            setViewParams();

            foreach (SettingsChangeListener l in settigsListeners)
                l.canvasResized(canvasPanel.Size);

            canvasPanel.Invalidate();
        }

        private class CropChangeCommand : Command {
            private RectangleF from, to;
            private readonly Mazio mazio;

            public CropChangeCommand(Mazio mazio, RectangleF from, RectangleF to) {
                this.mazio = mazio;
                this.from = from;
                this.to = to;
            }

            public string Name {
                get { return "Resize Crop Frame"; }
            }

            public void execute() {
                mazio.cropBox.moveHandle("start", to.Location);
                mazio.cropBox.moveHandle("end", new PointF(to.Right, to.Bottom));
                mazio.invalidateView();
            }

            public void unexecute() {
                mazio.cropBox.moveHandle("start", from.Location);
                mazio.cropBox.moveHandle("end", new PointF(from.Right, from.Bottom));
                mazio.invalidateView();
            }
        }

//        private void hideSettingsPanel()
//        {
//            if (btnShowShapeSettings.Checked)
//            {
//                btnShowShapeSettings.Checked = false;
//                buttonShowShapeSettings_Click(null, null);
//            }
//        }

        //
        // WARNING
        //
        // The algorithms below (onMouseDown, onMouseUp, onMouseMove)
        // are an absolute steaming pile of shit, 
        // arrived at by experimentation and trial&error
        // It needs to be properly designed to not suck
        // and start using state machine
        //

        private void onMouseDown(object sender, MouseEventArgs e) {
            PointF point = viewParams.toRelative(e.Location);

            canvasPanel.Controls.Remove(grabButton);
            canvasPanel.Controls.Remove(startDrawingButton);

            foreach (Shape s in shapes)
                s.deactivate();

            if ((cropBox != null) && cropBox.Editing && cropBox.isHit(point)) {
                cropChangeStart = cropBox.CropRectangle;
                invalidateAnnotatedScreenshot(sender, e);
                invalidateView();
                return;
            }

            moveFrom = point;

            if (editingShape != null) {
                string handle = editingShape.getHandleAtPoint(point);
                if (handle != null) {
                    editingShape.SelectedHandle = handle;
                    editingHandle = handle;
//                    Debug.WriteLine("onMouseDown() - editingHandle=" + editingHandle);
                }
            }

            if (editingHandle == null) {
                if (shapeHit != null) {
                    if (editingShape != null) {
                        ShapeSettings.Instance.Contents = null;
                        editingShape.Editing = false;
                    }
                    editingShape = shapeHit;
                    foreach (Shape s in shapes) {
                        if (s != editingShape) s.Highlight = false;
                    }

                    ShapeSettings.Instance.Contents = editingShape.SettingsPanel;
                    editingShape.Highlight = true;
                    editingShape.Editing = true;
                    editingShape.startMoving(moveFrom);

                    currentColor = Color.FromArgb(0xff, editingShape.ShapeColorNoHilight);
                    trackbarOpacity.Value = editingShape.ShapeColorNoHilight.A;
                    setOpacity();
                    trackbarWidth.Value = editingShape.Thickness;
                }
            }

            updateUiItems();
            invalidateAnnotatedScreenshot(sender, e);
            invalidateView();

            if (shapeHit != null || editingHandle != null) return;

            if (editingShape != null) {
                editingShape.Editing = false;
                editingShape = null;
            }
            addNewShape(point);
        }

        private void addNewShape(PointF p) {
            if (!CanAddNewShape) {
                CanAddNewShape = true;
                return;
            }

//            hideSettingsPanel();

            foreach (CheckBox ch in shapeButtons.Keys) {
                if (!ch.Checked || shapeButtons[ch].create == null) continue;

                ShapeSettingsPanel panel = shapeButtons[ch].panelCreate != null
                                               ? shapeButtons[ch].panelCreate()
                                               : null;
                addingShape = shapeButtons[ch].create(p, panel);
                ShapeSettings.Instance.Contents = panel;
                invalidateAnnotatedScreenshot(null, null);
                break;
            }
        }

        private void onMouseUp(object sender, MouseEventArgs e) {
            invalidateAnnotatedScreenshot(sender, e);

            if (editingShape != null) {
//                Debug.WriteLine("onMouseUp() - editingShape=" + editingShape);
                moveTo = viewParams.toRelative(e.Location);
                if (moveFrom.Equals(moveTo))
                    return;

                if (editingHandle == null) {
                    Command c = new MoveShapeCommand(this, editingShape, moveFrom, moveTo);
                    undoRedo.addCommand(c);
                } else {
                    Command c = new MoveShapeHandleCommand(this, editingShape, editingHandle, moveFrom, moveTo);
                    undoRedo.addCommand(c);
                }
            }

            editingHandle = null;

            if (cropBox != null && cropBox.Editing) {
                cropBox.finished();
                cropChangeEnd = cropBox.CropRectangle;
                Command c = new CropChangeCommand(this, cropChangeStart, cropChangeEnd);
                undoRedo.addCommand(c);
            }

            if (addingShape != null) {
                addingShape.finished();
                if (addingShape.isValid()) {
                    Command c = new AddShapeCommand(this, addingShape);
                    shapes.AddLast(addingShape);
                    undoRedo.addCommand(c);
                    editingShape = addingShape;
                    editingShape.Highlight = true;
                    editingShape.Editing = true;
                    updateUiItems();
                }
                addingShape = null;
            }
            invalidateView();
        }

        private void onMouseMove(object sender, MouseEventArgs e) {
            PointF point = viewParams.toRelative(e.Location);

            if (cropBox != null && cropBox.Editing) {
                if (cropHit && (e.Button == MouseButtons.Left)) {
                    cropBox.dragging(point);
                    invalidateView();
                } else {
                    bool hit = cropBox.isHit(point);
                    if (hit != cropHit) {
                        cropHit = hit;
                        invalidateView();
                    }
                }
            }

            if (cropHit) return;

            if (addingShape != null) {
                if (!addingShape.dragging(point)) {
                    invalidateView();
                }
            } else if (editingShape != null) {
                if (editingHandle != null) {
                    editingShape.moveHandle(editingHandle, point);
                } else if (e.Button == MouseButtons.Left) {
                    editingShape.moveTo(point);
                }

                editingShape.SelectedHandle = editingShape.getHandleAtPoint(point);

                invalidateView();
            }
            Shape newHit = findShapeUnderCursor(point);

            if (newHit == shapeHit) return;

            shapeHit = newHit;
//                Debug.WriteLine("shapeHit=" + shapeHit);
            invalidateView();
        }

        private Shape findShapeUnderCursor(PointF point) {
            int i = 0;

            Shape shape = null;

            Shape[] sTable = new Shape[shapes.Count];
            foreach (Shape s in shapes) {
                sTable[i] = s;
                if ((editingShape == null) || (s != editingShape)) {
                    s.Highlight = false;
                }
                ++i;
            }

            for (i = shapes.Count - 1; i >= 0; --i) {
                bool sHit = sTable[i].isHit(point);
                if (!sHit) {
                    continue;
                }
                shape = sTable[i];
                shape.Highlight = true;
                break;
            }
            return shape;
        }

        private void onMouseDoubleClick(object sender, MouseEventArgs e) {
            PointF point = viewParams.toRelative(e.Location);
            Shape shape = findShapeUnderCursor(point);
            
            if (shape == null) return;

            editingShape = shape;
            editingShape.onDoubleClick(sender, e);

            editingShape = null;
        }

        private void canvasPanel_Paint(object sender, PaintEventArgs e) {
            if (backBuffer == null)
                backBuffer = new Bitmap(canvasHolderPanel.Width, canvasHolderPanel.Height);

            Graphics g = Graphics.FromImage(backBuffer);

            paintPicture(g, true, viewParams);

            if (cropBox != null)
                cropBox.getView().paint(g, false, viewParams);

            string hint = null;
            foreach (CheckBox c in shapeButtons.Keys)
                if (c.Checked) {
                    hint = shapeButtons[c].hint;
                    break;
                }
            drawHint(g, hint);

            e.Graphics.DrawImageUnscaled(backBuffer, canvasHolderPanel.HorizontalScroll.Value,
                                         canvasHolderPanel.VerticalScroll.Value);

            g.Dispose();
        }

        private const string TXT_GRAB = "Get Screenshot";
        private const string TXT_START = "Begin  Drawing";

        private static void setButtonParams(ButtonBase b) {
            b.Font = new Font("Arial", 20.0F, FontStyle.Bold, GraphicsUnit.Pixel, 238);
            b.ForeColor = Color.DarkGray;
            b.BackColor = Color.White;
            b.FlatAppearance.BorderSize = 0;
            b.FlatStyle = FlatStyle.Flat;
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.Cursor = Cursors.Hand;
        }

        private void setupStartingButtons() {
            grabButton = new Button {Text = TXT_GRAB};
            setButtonParams(grabButton);
            grabButton.Click += buttonGrab_Click;

            startDrawingButton = new Button {Text = TXT_START};
            setButtonParams(startDrawingButton);
            startDrawingButton.Click += buttonStartDrawing_Click;

            grabButton.ClientSize = new Size(200, 30);
            startDrawingButton.ClientSize = new Size(200, 30);

            canvasPanel.Controls.Add(grabButton);
            canvasPanel.Controls.Add(startDrawingButton);
        }

        private void removeStartingButtons() {
            if (canvasPanel.Controls.Contains(grabButton)) {
                canvasPanel.Controls.Remove(grabButton);
            }
            if (canvasPanel.Controls.Contains(startDrawingButton)) {
                canvasPanel.Controls.Remove(startDrawingButton);
            }
        }

        private void buttonStartDrawing_Click(object sender, EventArgs e) {
            removeStartingButtons();
        }

        private void drawLogo(ViewParameters p) {
            if (shapes.Count == 0) {
                int locX = (p.Viewport.Width - 400)/2;
                int locY = (p.Viewport.Height - startDrawingButton.Height)/2;

                if (canvasPanel.Controls.Contains(grabButton)) {
                    int x = locX;
                    int y = locY;
                    grabButton.Location = new Point(x, y);
                }
                if (canvasPanel.Controls.Contains(startDrawingButton)) {
                    int x = locX + 400 - startDrawingButton.Width;
                    int y = locY;
                    startDrawingButton.Location = new Point(x, y);
                }
            } else {
                removeStartingButtons();
            }
        }

/*
        private const string HINT_TXT =
            "Grab a screenshot,\r\nopen an image (JPG or PNG),\r\nor drag and drop a file\r\n\r\n"
            + "Drag a label in lower-left\r\nor upload to web when done";
*/

/*
        private void drawUsageHints(Graphics g)
        {
            Font f = new Font("Arial", 12, FontStyle.Bold);
            Color c = SystemColors.ActiveCaption;

            if (ConfigDialog.Instance.ShowUsageHints)
            {
                Size s = g.MeasureString(HINT_TXT, f).ToSize();
                Brush b1 = new SolidBrush(Color.FromArgb(0x7f, c));
                Brush b2 = new SolidBrush(c);
                int x1 = (canvasPanel.Width - s.Width - 20) / 2;
                int y1 = (canvasPanel.Height - s.Height - 20) / 2;
                g.FillRectangle(b1, x1, y1, s.Width + 20, s.Height + 20);
                g.DrawString(HINT_TXT, f, b2, x1 + 10, y1 + 10);
                b1.Dispose();
                b2.Dispose();
            }
            if (runtimeArgs != null)
            {
                Brush b3 = new SolidBrush(c);
                g.DrawString("Runtime argument: " + runtimeArgs, f, b3, 0, 0);
                b3.Dispose();
            }
            if (clickOnceArgs != null)
            {
                string str = "";
                foreach (string name in clickOnceArgs)
                    str += "[" + name + ", " + clickOnceArgs[name] + "] ";
                if (str.Length > 0)
                {
                    Brush b3 = new SolidBrush(c);
                    g.DrawString("ClickOnce arguments: " + str, f, b3, 0, 0);
                    b3.Dispose();
                }
            }
            f.Dispose();
        }
*/

        private void paintPicture(Graphics g, bool drawHandlesAndHints, ViewParameters parameters) {
            if (screenshot != null) {
                removeStartingButtons();
                g.Clear(SystemColors.Desktop);
                screenshot.getView().paint(g, false, parameters);
            } else {
                g.Clear(Color.White);
                if (drawHandlesAndHints) {
                    //drawUsageHints(g, parameters);
                    drawLogo(parameters);
                }
            }

            foreach (Shape s in shapes)
                s.getView().paint(g, drawHandlesAndHints, parameters);
            if (addingShape != null)
                addingShape.getView().paint(g, drawHandlesAndHints, parameters);
        }

/*
        private class ResizeHandle
        {
            private int X;
            private int Y;
            public string name;

            private Mazio parent;
            private Point p;

            public Point get()
            {
                return p;
            }

            public Point getStart()
            {
                return new Point(X - 5, Y - 5);
            }

            public ResizeHandle(Mazio parent, string name, int x, int y)
            {
                this.parent = parent;
                this.name = name;
                X = x;
                Y = y;
                p = new Point(x, y);
            }

            public bool isIn(int x, int y)
            {
                int xo = (parent.canvasHolderPanel.Width - parent.bmpWidth) / 2;
                int yo = (parent.canvasHolderPanel.Height - parent.bmpHeight) / 2;

                if ((Math.Abs(X - x + xo) < 5) && (Math.Abs(Y - y + yo) < 5))
                {
                    return true;
                }
                return false;
            }
        }
*/

        private void buttonColor_Click(object sender, EventArgs e) {
            colorDialog.ShowDialog(this);
            currentColor = colorDialog.Color;
            btnColor.BackColor = CurrentColor;
            if (editingShape != null) {
                editingShape.ShapeColor = CurrentColor;
            }

            foreach (SettingsChangeListener l in settigsListeners)
                l.colorChanged(currentColor);

            if (editingShape != null) {
                invalidateView();
            }

            try {
                SettingsWrapper.saveSettingValue(REG_COLOR_R, (int) colorDialog.Color.R);
                SettingsWrapper.saveSettingValue(REG_COLOR_G, (int) colorDialog.Color.G);
                SettingsWrapper.saveSettingValue(REG_COLOR_B, (int) colorDialog.Color.B);
            } catch (NullReferenceException) {}

            invalidateAnnotatedScreenshot(sender, e);
        }

        private void fileName_TextChanged(object sender, EventArgs e) {
            if (boxFileName.Text.Length > 0 && imageFileName != null) {
                imageFileName = imageFileName.Substring(0, imageFileName.LastIndexOf('\\') + 1) + boxFileName.Text + comboFileType.SelectedItem;
            }
            updateUiItems();
        }

        private void setDragLabelEnableState(bool enabled) {
            labelDrag.Visible = enabled;
        }

        private void setWindowTitle() {
            if (imageFileName != null) {
                string name = imageFileName;
                int idx = imageFileName.LastIndexOf('\\');
                if (idx > 0)
                    name = imageFileName.Substring(idx + 1);
                else {
                    idx = imageFileName.LastIndexOf('/');
                    if (idx > 0)
                        name = imageFileName.Substring(idx + 1);
                }
                idx = name.LastIndexOf('.');
                if (idx > 0)
                    name = name.Substring(0, idx);
                Text = "Mazio - " + name;
            } else {
                Text = "Mazio";
            }
        }

        private void updateUiItems() {
            bool enabled = (boxFileName.Text.Length != 0) && (screenshot != null || shapes.Count > 0);
            setDragLabelEnableState(enabled);
            btnUpload.Enabled = enabled;
            btnTightCrop.Enabled = enabled;

            btnClear.Enabled = shapes.Count > 0;

            bool moreThanOne = shapes.Count > 1;

            LinkedListNode<Shape> node = shapes.Find(editingShape);

            btnTop.Enabled = editingShape != null && node != null && node.Next != null && moreThanOne;
            btnUp.Enabled = editingShape != null && node != null && node.Next != null && moreThanOne;
            btnDown.Enabled = editingShape != null && node != null && node.Previous != null && moreThanOne;
            btnBottom.Enabled = editingShape != null && node != null && node.Previous != null && moreThanOne;

            btnZoomReset.Enabled = screenshot != null;
            trackbarZoom.Enabled = screenshot != null;
            btnFit.Enabled = screenshot != null;
            btnCenter.Enabled = screenshot != null;
            labelZoom.Enabled = screenshot != null;
            checkSaveAtFullSize.Enabled = screenshot != null;
            btnTightCrop.Enabled = screenshot != null;

            btnCopy.Enabled = screenshot != null || shapes.Count > 0;

            btnCrop.Enabled = screenshot != null;

            btnTransform.Enabled = screenshot != null;

            setWindowTitle();
        }

        private void uncheckShapeButtons() {
            foreach (CheckBox b in shapeButtons.Keys) {
                b.Checked = false;
            }

            if (cropBox != null) {
                cropBox.Editing = false;
            }
            cropHit = false;
        }

        private static void saveSelectedTool(int tool) {
            try {
                SettingsWrapper.saveSettingValue(REG_SELECTED_TOOL, tool);
            } catch (NullReferenceException) {}
        }

        private void buttonLine_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnLine.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnLine].panelCreate();
            saveSelectedTool(shapeButtons[btnLine].idx);
            updateHilightedShapes();
        }

        private void buttonArrow_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnArrow.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnArrow].panelCreate();
            saveSelectedTool(shapeButtons[btnArrow].idx);
            updateHilightedShapes();
        }

        private void buttonCircle_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnCircle.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnCircle].panelCreate();
            saveSelectedTool(shapeButtons[btnCircle].idx);
            updateHilightedShapes();
        }

        private void buttonRectangle_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnRectangle.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnRectangle].panelCreate();
            saveSelectedTool(shapeButtons[btnRectangle].idx);
            updateHilightedShapes();
        }

        private void buttonText_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnText.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnText].panelCreate();
            saveSelectedTool(shapeButtons[btnText].idx);
            updateHilightedShapes();
        }

        private void buttonMagnifier_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnMagnifier.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnMagnifier].panelCreate();
            saveSelectedTool(shapeButtons[btnMagnifier].idx);
            updateHilightedShapes();
        }

        private void buttonPonyVille_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnPonyVille.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnPonyVille].panelCreate();
            saveSelectedTool(shapeButtons[btnPonyVille].idx);
            updateHilightedShapes();
        }

        private void checkImage_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            checkImage.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[checkImage].panelCreate();
            saveSelectedTool(shapeButtons[checkImage].idx);
            updateHilightedShapes();
        }

        private void buttonCensor_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnCensor.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnCensor].panelCreate();
            saveSelectedTool(shapeButtons[btnCensor].idx);
            updateHilightedShapes();
        }

        private void buttonPencil_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnPencil.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnPencil].panelCreate();
            saveSelectedTool(shapeButtons[btnPencil].idx);
            updateHilightedShapes();
        }

        private void buttonCrop_Click(object sender, EventArgs e) {
            uncheckShapeButtons();
            btnCrop.Checked = true;
            ShapeSettings.Instance.Contents = shapeButtons[btnCrop].panelCreate();
            if (cropBox != null) {
                cropBox.Editing = true;
            }
            saveSelectedTool(shapeButtons[btnCrop].idx);
            updateHilightedShapes();
        }

        private void updateHilightedShapes() {
            if (editingShape != null) {
                editingShape.Editing = false;
                editingShape = null;
            }
            foreach (Shape s in shapes) {
                s.Highlight = false;
            }
            shapeHit = null;
            invalidateView();
            updateUiItems();
        }

        public void timerTick(object sender, EventArgs eArgs) {
            if (Visible && sender.Equals(delayer)) return;

            delayer.Stop();
            delayer = null;
            delayerTo.Stop();
            delayerTo = null;

            grabber.ShowDialog();
            if ((grabber.DialogResult == DialogResult.OK) && grabber.CheckForWindow()) {
                Bitmap bmpScreenshot = new Bitmap(grabber.GrabWidth, grabber.GrabHeight, PixelFormat.Format32bppArgb);

                Graphics.FromImage(bmpScreenshot).CopyFromScreen(grabber.GrabX, grabber.GrabY,
                                                                 0, 0,
                                                                 new Size(grabber.GrabWidth, grabber.GrabHeight),
                                                                 CopyPixelOperation.SourceCopy);

                cropBox = null;
                screenshot = new ScreenShot(this, bmpScreenshot);
                scaleToFit(canvasHolderPanel.Width, canvasHolderPanel.Height);

                setSizes();
                ClientSize = new Size(bmpWidth + ConfigDialog.Instance.GrabMargin*ZoomFactor/100 + BORDER_SIZE,
                                      bmpHeight + ConfigDialog.Instance.GrabMargin*ZoomFactor/100 + BORDER_SIZE);

                cropBox = new CropBox(this, btnCrop.Checked, CropBox.createSettingsPanel());

                buttonFit_Click(null, null);

                updateUiItems();
            }

            Opacity = 1.0;
            grabber = null;

            Show();
        }

        private void scaleToFit(int width, int height) {
            int w, h;
            if (cropBox != null) {
                RectangleF r = Shape.getShapeRectangleF(cropBox.Handles["start"], cropBox.Handles["end"]);
                w = (int) r.Width;
                h = (int) r.Height;
            } else {
                w = screenshot.Shot.Width;
                h = screenshot.Shot.Height;
            }
            w += 2 * ConfigDialog.Instance.GrabMargin;
            h += 2 * ConfigDialog.Instance.GrabMargin;

            if (w > width || h > height) {
                double ratioW = width/(double) w;
                double ratioH = height/(double) h;
                int result = (int) (0.5 + 100*(ratioW < ratioH ? ratioW : ratioH));
                trackbarZoom.Value = Math.Min(trackbarZoom.Maximum, Math.Max(trackbarZoom.Minimum, result));
            } else {
                trackbarZoom.Value = 100;
            }
        }

        private void centerCropBox() {
            if (cropBox == null) return;

            canvasHolderPanel.HorizontalScroll.Value = 0;
            canvasHolderPanel.VerticalScroll.Value = 0;
            activateScrollMove();

            setViewParams();

            Rectangle r = Shape.ShapeView.getShapeRectangle(cropBox.Handles["start"], cropBox.Handles["end"],
                                                            CurrentViewParameters);

            int x = canvasHolderPanel.HorizontalScroll.Value;
            if (r.Left < 0 || r.Right > x + canvasHolderPanel.Width)
                x = r.Left + r.Width/2 - canvasHolderPanel.ClientSize.Width/2;
            canvasHolderPanel.HorizontalScroll.Value = x;

            int y = canvasHolderPanel.VerticalScroll.Value;
            if (r.Top < 0 || r.Bottom > y + canvasHolderPanel.Height)
                y = r.Top + r.Height/2 - canvasHolderPanel.ClientSize.Height/2;
            canvasHolderPanel.VerticalScroll.Value = y;

            activateScrollMove();
        }

        private void activateScrollMove() {
            // need to do this voodoo to actually make the panel repaint itself properly. 
            // Beats me why nothing else works. I guess I am too stupid for .NET
            Invoke(new MethodInvoker(delegate {
                                         Size = new Size(Size.Width + 1, Size.Height + 1);
                                         Invoke(new MethodInvoker(
                                            delegate { Size = new Size(Size.Width - 1, Size.Height - 1); }));
                                     }));
        }

        private void visibleChanged(object sender, EventArgs e) {
            if (grabber == null) return;

            delayer = new System.Windows.Forms.Timer {Interval = 1000};
            delayer.Tick += timerTick;
            delayer.Start();
            delayerTo = new System.Windows.Forms.Timer {Interval = 5000};
            delayerTo.Tick += timerTick;
            delayerTo.Start();
        }

        private void buttonClear_Click(object sender, EventArgs e) {
            foreach (Shape sh in shapes) {
                sh.deactivate();
            }

            GroupCommand c = new GroupCommand("Clear Shapes");

            for (LinkedListNode<Shape> node = shapes.Last; node != null; node = node.Previous) {
                c.add(new RemoveShapeCommand(this, node.Value));
            }

            undoRedo.addCommand(c);

            shapes.Clear();

            invalidateView();
            updateUiItems();
        }


        private void buttonAbout_Click(object sender, EventArgs e) {
            About a = new About();
            a.ShowDialog();
        }

        private void trackbarWidth_ValueChanged(object sender, EventArgs e) {
            labelSize.Text = trackbarWidth.Value.ToString();

            if (editingShape != null) {
                editingShape.Thickness = trackbarWidth.Value;
            }

            foreach (SettingsChangeListener l in settigsListeners) {
                l.thicknessChanged(trackbarWidth.Value);
            }

            if (editingShape != null) {
                invalidateView();
            }

            try {
                SettingsWrapper.saveSettingValue(REG_LINE_WIDTH, trackbarWidth.Value);
            } catch (NullReferenceException) {}

            invalidateAnnotatedScreenshot(sender, e);
        }

        private void trackbarOpacity_ValueChanged(object sender, EventArgs e) {
            setOpacity();

            if (editingShape != null) {
                editingShape.ShapeColor = CurrentColor;
            }

            foreach (SettingsChangeListener l in settigsListeners) {
                l.colorChanged(currentColor);
            }

            if (editingShape != null) {
                invalidateView();
            }

            SettingsWrapper.saveSettingValue(REG_OPACITY, trackbarOpacity.Value);

            invalidateAnnotatedScreenshot(sender, e);
        }

        private void setViewParams() {
            viewParams = new ViewParameters(ZoomFactor, CanvasOffset, canvasPanel.Size,
                                            new Point(canvasHolderPanel.HorizontalScroll.Value,
                                                      canvasHolderPanel.VerticalScroll.Value),
                                            new Size(canvasHolderPanel.Size.Width, canvasHolderPanel.Size.Height), false);
        }

        private void trackbarZoom_ValueChanged(object sender, EventArgs e) {
            labelZoom.Text = ZoomFactor + "%";

            setSizes();

            foreach (SettingsChangeListener l in settigsListeners) {
                l.screenshotZoomed(ZoomFactor);
            }
            invalidateView();
        }

        private Color CurrentColor {
            get { return Color.FromArgb(trackbarOpacity.Value, currentColor); }
        }

        private void buttonUpload_Click(object sender, EventArgs e) {
            string name = saveBitmapToTemp();

            UploadType type = (UploadType) comboUploadSite.SelectedItem;

            List<string> strings = new List<string>();
            foreach (Shape s in shapes) {
                // ugly as shit, but it is not worthwhile currently to make it any nicer
                if (s is TextShape) {
                    strings.Add(((TextShape) s).Text);
                }
            }

            Form uploader = type.getDialog(name, strings, CommandLineParser.Instance.getArguments(runtimeArgs));
            uploader.ShowDialog();
        }

        private void openFile(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog
                                 {
                                     CheckFileExists = true,
                                     CheckPathExists = true,
                                     Multiselect = false,
                                     Filter = "Image files (*.png, *.jpg)|*.jpg;*.png|Mazio files (*.maz)|*.maz"
                                 };

            if (ofd.ShowDialog() != DialogResult.OK) {
                return;
            }

            try {
                Stream stream = ofd.OpenFile();
                imageFileName = ofd.FileName;
                loadBitmap(stream, imageFileName);
                stream.Close();
            } catch (Exception ex) {
                imageFileName = null;
                MessageBox.Show("Error: Could not read file from disk: " + ex.Message);
            }
        }

        private void saveFile(object sender, EventArgs e) {
            if (imageFileName != null) {
                saveBitmap(imageFileName);
            }
        }

        private void saveFileAs(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog {CheckFileExists = false, CheckPathExists = true};
            if (imageFileName != null)
                sfd.FileName = imageFileName.Substring(imageFileName.LastIndexOf('\\') + 1);
            sfd.Filter = "Image files (*.png, *.jpg)|*.jpg;*.png|Mazio files (*.maz)|*.maz";
            if (sfd.ShowDialog() != DialogResult.OK) {
                return;
            }
            imageFileName = sfd.FileName;
            if (imageFileName == null) {
                return;
            }
            saveBitmap(imageFileName);
            updateUiItems();
        }

        private void buttonCopy_Click(object sender, EventArgs e) {
            copy();
        }

        private void copy() {
            Bitmap bmp = getImageToSave();

            Clipboard.SetImage(bmp);

            bmp.Dispose();
        }


        private void buttonPaste_Click(object sender, EventArgs e) {
            paste(btnPaste, new Point(0, btnPaste.Height));
        }

        private void paste(Control where, Point offset) {
            ContextMenu menu = new ContextMenu();
            MenuItem asBackground = new MenuItem("As New Screenshot", (s, e) => pasteAsBackground());
            menu.MenuItems.Add(asBackground);
            MenuItem asShape = new MenuItem("As Additional Image", (s, e) => pasteAsShape());
            menu.MenuItems.Add(asShape);
            menu.Show(where, offset);
        }

        private void pasteAsShape() {
            if (!Clipboard.ContainsImage()) {
                return;
            }
            Image img = Clipboard.GetImage();
            if (img == null) {
                return;
            }
            PointF point = viewParams.toRelative(new Point(canvasHolderPanel.Width/2, canvasHolderPanel.Height/2));
            Shape pic = new Picture(img, this, CurrentColor, trackbarWidth.Value, point, Picture.createSettingsPanel());
            Command c = new AddShapeCommand(this, pic);
            shapes.AddLast(pic);
            undoRedo.addCommand(c);
            Invalidate();
        }

        private void pasteAsBackground() {
            if (!Clipboard.ContainsImage()) {
                return;
            }
            Image img = Clipboard.GetImage();
            if (img == null) {
                return;
            }
            Bitmap bmp = new Bitmap(img);
            loadBitmap(bmp);
        }

        private void setPasteTimer() {
            pasteTimer = new System.Windows.Forms.Timer {Interval = 1000};
            pasteTimer.Tick += pasteTimer_Tick;
            pasteTimer.Start();
        }

        public void pasteTimer_Tick(object sender, EventArgs eArgs) {
            btnPaste.Enabled = Clipboard.ContainsImage();
        }

        private void btnFile_Click(object sender, EventArgs e) {
            ContextMenu menu = new ContextMenu();
            MenuItem openItem = new MenuItem("Open...", openFile) {Shortcut = Shortcut.CtrlO};
            menu.MenuItems.Add(openItem);
            MenuItem saveItem = new MenuItem("Save", saveFile)
                                {
                                    Shortcut = Shortcut.CtrlS,
                                    Enabled = imageFileName != null && (HaveScreenshot || shapes.Count > 0)
                                };
            menu.MenuItems.Add(saveItem);
            MenuItem saveAsItem = new MenuItem("Save As...", saveFileAs)
                                  {
                                      Shortcut = Shortcut.CtrlShiftS,
                                      Enabled = HaveScreenshot || shapes.Count > 0
                                  };
            menu.MenuItems.Add(saveAsItem);
            menu.Show(btnFile, new Point(0, btnFile.Height));
        }

        private void buttonZoomReset_Click(object sender, EventArgs e) {
            trackbarZoom.Value = 100;
        }

        private void buttonFit_Click(object sender, EventArgs e) {
            if (screenshot == null) return;

            scaleToFit(canvasHolderPanel.Width, canvasHolderPanel.Height);
            centerCropBox();
        }

        private void buttonCenter_Click(object sender, EventArgs e) {
            if (screenshot != null) {
                centerCropBox();
            }
        }

        private void buttonRedo_MouseEnter(object sender, EventArgs e) {
            tip.SetToolTip(btnRedo, "Redo " + undoRedo.LastRedo + " (Ctrl+Y)");
        }

        private void buttonUndo_MouseEnter(object sender, EventArgs e) {
            tip.SetToolTip(btnUndo, "Undo " + undoRedo.LastUndo + " (Ctrl+Z)");
        }

        private void dragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] names = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (names.Length != 1) return;

            string ext = names[0].Substring(names[0].LastIndexOf('.') + 1);
            if (!(ext.ToLower().Equals("jpg") || ext.ToLower().Equals("png") || ext.ToLower().Equals("maz"))) return;

            e.Effect = DragDropEffects.Copy;
        }

        private void dropFile(object sender, DragEventArgs e) {
            string[] names = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            Stream stream = File.Open(names[0], FileMode.Open);
            imageFileName = names[0];
            loadBitmap(stream, names[0]);
            updateUiItems();
            stream.Close();
        }

        private void buttonUndo_Click(object sender, EventArgs e) {
            undoRedo.undo();
        }

        private void buttonRedo_Click(object sender, EventArgs e) {
            undoRedo.redo();
        }

        private void buttonTightCrop_Click(object sender, EventArgs e) {
            if (cropBox == null) return;

            cropChangeStart = cropBox.CropRectangle;
            cropBox.cropTight();
            cropChangeEnd = cropBox.CropRectangle;
            undoRedo.addCommand(new CropChangeCommand(this, cropChangeStart, cropChangeEnd));
            invalidateView();
        }

        private readonly ToolTip tip = new ToolTip();

        private void setupToolTips() {
            tip.AutoPopDelay = 5000;
            tip.InitialDelay = 1000;
            tip.ReshowDelay = 500;
            tip.ShowAlways = true;

            tip.SetToolTip(btnLine, "Draw Straight Line");
            tip.SetToolTip(btnArrow, "Draw Arrow");
            tip.SetToolTip(btnCircle, "Draw Oval");
            tip.SetToolTip(btnRectangle, "Draw Rectangle");
            tip.SetToolTip(btnPencil, "Draw a Line withe the Pencil");
            tip.SetToolTip(btnText, "Type a Line of Text");
            tip.SetToolTip(btnMagnifier, "Draw a Magnifying Glass");
            tip.SetToolTip(btnPonyVille, "Draw with Ponies");
            tip.SetToolTip(checkImage, "Insert Additional Image");
            tip.SetToolTip(btnCensor, "Draw a Censor");
            tip.SetToolTip(btnTightCrop, "UnCrop");
            tip.SetToolTip(btnCrop, "Crop");
            tip.SetToolTip(btnColor, "Select Color");
            tip.SetToolTip(trackbarWidth, "Set Line Width");
            tip.SetToolTip(trackbarOpacity, "Set Shape Opacity");
            tip.SetToolTip(btnTop, "Move Shape to the Front");
            tip.SetToolTip(btnUp, "Move Shape Up");
            tip.SetToolTip(btnDown, "Move Shape Down");
            tip.SetToolTip(btnBottom, "Move Shape to the Back");
            tip.SetToolTip(btnClear, "Clear All Shapes");
            tip.SetToolTip(btnShowShapeSettings, "Selected Tool Settings");
            tip.SetToolTip(btnCopy, "Copy Image To Clipboard (Ctrl+C)");
            tip.SetToolTip(btnPaste, "Paste Background From Clipboard (Ctrl+V)");
            tip.SetToolTip(btnTransform, "Transform Image");
            tip.SetToolTip(labelDrag, "Drag this box to the desktop or to a folder to save the drawing as a file");
        }

        private void comboUploadSite_SelectedIndexChanged(object sender, EventArgs e) {
            SettingsWrapper.saveSettingValue(REG_SELECTED_UPLOADER, comboUploadSite.SelectedIndex);
        }

        private void comboFileType_SelectedIndexChanged(object sender, EventArgs e) {
            if (imageFileName != null) {
                int idx = imageFileName.LastIndexOf('.');
                if (idx > 0) {
                    imageFileName = imageFileName.Substring(0, idx) + comboFileType.SelectedItem;
                }
            }

            SettingsWrapper.saveSettingValue(REG_FILE_TYPE, comboFileType.SelectedIndex);
        }

        private void buttonSettings_Click(object sender, EventArgs e) {
            ConfigDialog config = ConfigDialog.Instance;
            if (config.Visible) {
                config.Focus();
            } else {
                config.Show();
            }
        }

        private void labelNewVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            NewVersionInfo.showNewVersionInfo();
        }

        private void checkNewVersion() {
            Thread.Sleep(1000);

            bool isNew = NewVersionInfo.haveNewVersion();

            if (isNew) {
                Invoke(new MethodInvoker(delegate { labelNewVersion.Visible = true; labelNewVersion.Enabled = true; }));
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e) {
            if ((e.KeyCode == Keys.Delete) && (editingShape != null)) {
                LinkedList<Shape> newShapes = new LinkedList<Shape>();
                foreach (Shape s in shapes) {
                    if (s != editingShape) {
                        newShapes.AddLast(s);
                    }
                }

                editingShape.Highlight = false;
                editingShape.Editing = false;

                Command c = new RemoveShapeCommand(this, editingShape);
                undoRedo.addCommand(c);

                shapes.Clear();
                shapes = newShapes;

                editingShape = null;

                invalidateView();
                updateUiItems();
            } else if (e.Control && e.KeyCode == Keys.Z) {
                undoRedo.undo();
            } else if (e.Control && e.KeyCode == Keys.Y) {
                undoRedo.redo();
            } else if (e.Control && e.KeyCode == Keys.C) {
                copy();
            } else if (e.Control && e.KeyCode == Keys.V) {
                paste(canvasHolderPanel, new Point(canvasHolderPanel.Width / 2, canvasHolderPanel.Height / 2));
            }
        }

        private void drawHint(Graphics g, string text) {
            if (!ConfigDialog.Instance.ShowUsageHints) return;

            if (text == null) return;

            StringFormat format = new StringFormat {Alignment = StringAlignment.Center};

            Font f = new Font("Arial Bold", 10);
            Brush b = new SolidBrush(Color.Red);
            Brush bgB = new SolidBrush(Constants.TEXT_BG_COLOR);
            Size s = g.MeasureString(text, f).ToSize();

            Point p = new Point((canvasHolderPanel.Width)/2, canvasHolderPanel.Height - s.Height - SystemInformation.HorizontalScrollBarHeight);
            g.FillRectangle(bgB, p.X - s.Width / 2, canvasHolderPanel.Height - s.Height - SystemInformation.HorizontalScrollBarHeight, s.Width, s.Height);
            g.DrawString(text, f, b, p, format);

            bgB.Dispose();
            b.Dispose();
            f.Dispose();
        }

        private void labelSize_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            Pen p = new Pen(Color.Black, trackbarWidth.Value);
            Size s = g.MeasureString("" + trackbarWidth.Maximum, labelSize.Font).ToSize();
            g.DrawLine(p, s.Width, labelSize.Height/2, labelSize.Width, labelSize.Height/2);
            p.Dispose();
        }

        private void buttonTop_Click(object sender, EventArgs e) {
            if (editingShape == null) return;
            
            LinkedListNode<Shape> node = shapes.Find(editingShape);

            Command c = new PutShapeOnTopCommand(this, editingShape);
            undoRedo.addCommand(c);

            if (node != null) {
                shapes.Remove(node);
                shapes.AddLast(node);
            }
            updateUiItems();
            invalidateView();
        }

        private void buttonUp_Click(object sender, EventArgs e) {
            if (editingShape == null) return;

            LinkedListNode<Shape> node = shapes.Find(editingShape);
            if (node != null) {
                LinkedListNode<Shape> next = node.Next;

                Command c = new PutShapeHigherCommand(this, editingShape);
                undoRedo.addCommand(c);

                shapes.Remove(node);
                shapes.AddAfter(next, node);
            }
            updateUiItems();
            invalidateView();
        }

        private void buttonDown_Click(object sender, EventArgs e) {
            if (editingShape == null) return;

            LinkedListNode<Shape> node = shapes.Find(editingShape);
            if (node != null) {
                LinkedListNode<Shape> prev = node.Previous;

                Command c = new PutShapeLowerCommand(this, editingShape);
                undoRedo.addCommand(c);

                shapes.Remove(node);
                shapes.AddBefore(prev, node);
            }
            updateUiItems();
            invalidateView();
        }

        private void buttonBottom_Click(object sender, EventArgs e) {
            if (editingShape == null) return;

            LinkedListNode<Shape> node = shapes.Find(editingShape);

            Command c = new PutShapeOnBottomCommand(this, editingShape);
            undoRedo.addCommand(c);

            if (node != null) {
                shapes.Remove(node);
                shapes.AddFirst(node);
            }
            updateUiItems();
            invalidateView();
        }

        public void notifyUndoRedoState(bool undoEmpty, bool redoEmpty) {
            btnUndo.Enabled = !undoEmpty;
            btnRedo.Enabled = !redoEmpty;
            updateUiItems();
        }

        private class AddShapeCommand : Command {
            private readonly Shape s;
            private readonly Mazio parent;

            public AddShapeCommand(Mazio parent, Shape s) {
                this.parent = parent;
                this.s = s;
            }

            public void execute() {
                parent.shapes.AddLast(s);
                parent.invalidateView();
            }

            public void unexecute() {
                parent.shapes.Remove(s);
                parent.invalidateView();
            }

            public string Name { get { return "Add " + s.Name; } }
        }

        private class MoveShapeCommand : Command {
            private readonly Shape s;
            private readonly Mazio parent;
            private readonly PointF from;
            private readonly PointF to;

            public MoveShapeCommand(Mazio parent, Shape s, PointF from, PointF to) {
                this.parent = parent;
                this.from = from;
                this.to = to;
                this.s = s;
            }

            public void execute() {
                s.startMoving(from);
                s.moveTo(to);
                parent.invalidateView();
            }

            public void unexecute() {
                s.startMoving(to);
                s.moveTo(from);
                parent.invalidateView();
            }

            public string Name { get { return "Move " + s.Name; } }
        }

        private class MoveShapeHandleCommand : Command {
            private readonly Shape s;
            private readonly Mazio parent;
            private readonly string handle;
            private readonly PointF from;
            private readonly PointF to;

            public MoveShapeHandleCommand(Mazio parent, Shape s, string handle, PointF from, PointF to) {
                this.parent = parent;
                this.handle = handle;
                this.from = from;
                this.to = to;
                this.s = s;
            }

            public void execute() {
                s.moveHandle(handle, to);
                parent.invalidateView();
            }

            public void unexecute() {
                s.moveHandle(handle, from);
                parent.invalidateView();
            }

            public string Name { get { return "Edit " + s.Name; } }
        }

        private class RemoveShapeCommand : Command {
            private readonly Shape s;
            private readonly Mazio parent;
            private readonly Shape prev;

            public RemoveShapeCommand(Mazio parent, Shape s) {
                this.parent = parent;
                this.s = s;
                prev = findPreviousShape();
            }

            public void execute() {
                if (parent.shapes.Count == 0)
                    return;

                if (!parent.shapes.Contains(s)) {
                    return;
                }
                parent.shapes.Remove(s);
                parent.invalidateView();
            }

            private Shape findPreviousShape() {
                for (LinkedListNode<Shape> node = parent.shapes.First; node != null; node = node.Next) {
                    if (!node.Value.Equals(s)) continue;

                    if (node.Previous != null) {
                        return node.Previous.Value;
                    }
                }
                return null;
            }

            public void unexecute() {
                if (prev == null) {
                    parent.shapes.AddFirst(s);
                } else {
                    LinkedListNode<Shape> prevNode = parent.shapes.Find(prev);
                    if (prevNode != null) {
                        parent.shapes.AddAfter(prevNode, s);
                    } else {
                        Debug.WriteLine("Can't find parent shape to undo removing shape");
                    }
                }
                parent.invalidateView();
            }

            public string Name { get { return "Remove " + s.Name; } }
        }

        private abstract class ChangeShapeZOrderCommand : Command {
            protected readonly Mazio parent;
            private readonly Shape from;
            protected readonly Shape s;

            protected ChangeShapeZOrderCommand(Mazio parent, Shape s) {
                this.parent = parent;
                this.s = s;
// ReSharper disable PossibleNullReferenceException
                LinkedListNode<Shape> p = parent.shapes.Find(s).Previous;
// ReSharper restore PossibleNullReferenceException
                from = p != null ? p.Value : null;
            }

            public void unexecute() {
                parent.shapes.Remove(s);
                if (from == null)
                    parent.shapes.AddFirst(s);
                else
// ReSharper disable AssignNullToNotNullAttribute
                    parent.shapes.AddAfter(parent.shapes.Find(from), s);
// ReSharper restore AssignNullToNotNullAttribute
                parent.invalidateView();
            }

            public abstract void execute();
            public abstract string Name { get; }
        }

        private class PutShapeOnTopCommand : ChangeShapeZOrderCommand {
            public PutShapeOnTopCommand(Mazio parent, Shape s) : base(parent, s) {}

            public override void execute() {
                parent.shapes.Remove(s);
                parent.shapes.AddLast(s);
                parent.invalidateView();
            }

            public override string Name { get { return "Move " + s.Name + " to Top"; } }
        }

        private class PutShapeOnBottomCommand : ChangeShapeZOrderCommand {
            public PutShapeOnBottomCommand(Mazio parent, Shape s) : base(parent, s) {}

            public override void execute() {
                parent.shapes.Remove(s);
                parent.shapes.AddFirst(s);
                parent.invalidateView();
            }

            public override string Name { get { return "Move " + s.Name + " to Bottom"; } }
        }

        private class PutShapeHigherCommand : ChangeShapeZOrderCommand {
            public PutShapeHigherCommand(Mazio parent, Shape s) : base(parent, s) {}

            public override void execute() {
// ReSharper disable PossibleNullReferenceException
                LinkedListNode<Shape> next = parent.shapes.Find(s).Next;
// ReSharper restore PossibleNullReferenceException
                parent.shapes.Remove(s);
                if (next != null)
                    parent.shapes.AddAfter(next, s);
                else
                    parent.shapes.AddLast(s);
                parent.invalidateView();
            }

            public override string Name { get { return "Move " + s.Name + " Up"; } }
        }

        private class PutShapeLowerCommand : ChangeShapeZOrderCommand {
            public PutShapeLowerCommand(Mazio parent, Shape s) : base(parent, s) {}

            public override void execute() {
// ReSharper disable PossibleNullReferenceException
                LinkedListNode<Shape> prev = parent.shapes.Find(s).Previous;
// ReSharper restore PossibleNullReferenceException
                parent.shapes.Remove(s);
                if (prev != null)
                    parent.shapes.AddBefore(prev, s);
                else
                    parent.shapes.AddFirst(s);
                parent.invalidateView();
            }

            public override string Name { get { return "Move " + s.Name + " Down"; } }
        }

        private void buttonShowShapeSettings_Click(object sender, EventArgs e) {
            if (!ShapeSettings.Instance.Visible) {
                ShapeSettings.Instance.Show();
            } else {
                ShapeSettings.Instance.BringToFront();
            }
        }

        private void onMove(object sender, EventArgs e) {}

        private void mazioLoad(object sender, EventArgs e) {
            string startupFileName = CommandLineParser.Instance.getStartupFileName(runtimeArgs);
            if (startupFileName == null) return;

            if (!File.Exists(startupFileName)) return;

            using (FileStream fileStream = File.OpenRead(startupFileName)) {
                imageFileName = startupFileName;
                updateUiItems();

                loadBitmap(fileStream, fileStream.Name);
            }
        }

        private void btnTransform_Click(object sender, EventArgs e) {
            BackgroundTransformator.Instance.openSettings();
        }
    }
}