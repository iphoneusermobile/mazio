using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace mazio.util
{
    class Win32
    {
        public const int WS_POPUP = unchecked((int)0x80000000);
        public const int WS_CAPTION = unchecked(0x00C00000);
        public const int WS_EX_DLGMODALFRAME = unchecked(0x00000001);
        public const int WS_EX_TOOLWINDOW = unchecked(0x00000080);
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                X = x;
                Y = y;
            }

            public static implicit operator Point(POINT p) {
                return new Point(p.X, p.Y);
            }

            public static implicit operator POINT(Point p) {
                return new POINT(p.X, p.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool ReleaseCapture();

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom) {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public void Inflate(int width, int height) {
                this.left -= width;
                this.top -= height;
                this.right += width;
                this.bottom += height;
            }

            /// LONG->int
            public int left;

            /// LONG->int
            public int top;

            /// LONG->int
            public int right;

            /// LONG->int
            public int bottom;
        }
        
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            /// DWORD->unsigned int
            public uint cbSize;

            /// RECT->tagRECT
            public RECT rcWindow;

            /// RECT->tagRECT
            public RECT rcClient;

            /// DWORD->unsigned int
            public uint dwStyle;

            /// DWORD->unsigned int
            public uint dwExStyle;

            /// DWORD->unsigned int
            public uint dwWindowStatus;

            /// UINT->unsigned int
            public uint cxWindowBorders;

            /// UINT->unsigned int
            public uint cyWindowBorders;

            /// ATOM->WORD->unsigned short
            public ushort atomWindowType;

            /// WORD->unsigned short
            public ushort wCreatorVersion;
        }

        [DllImport("user32")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        // cursor ops from http://blog.paranoidferret.com/index.php/2008/01/30/csharp-tutorial-how-to-use-custom-cursors/
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("gdi32.dll")]
        public static extern int SetROP2(IntPtr hdc, int enDrawMode);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll")]
        public static extern void LineTo(IntPtr hdc, int x, int y);

        [DllImport("gdi32.dll")]
        public static extern void MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct DLLVersionInfo
		{
			public int cbSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformID;
		}

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("UxTheme.dll", CharSet=CharSet.Auto)]
		public static extern bool IsAppThemed();

		[DllImport("UxTheme.dll", CharSet=CharSet.Auto)]
		public static extern bool IsThemeActive();

		[DllImport("comctl32.dll", CharSet=CharSet.Auto)]
		public static extern int DllGetVersion(ref DLLVersionInfo version);

		[DllImport("uxtheme.dll", ExactSpelling=true, CharSet=CharSet.Unicode)]
		public static extern IntPtr OpenThemeData(IntPtr hWnd, String classList);

		[DllImport("uxtheme.dll", ExactSpelling=true)]
		public extern static Int32 CloseThemeData(IntPtr hTheme);

		[DllImport("uxtheme", ExactSpelling=true)]
		public extern static Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId,
			int iStateId, ref RECT pRect, IntPtr pClipRect);

		[DllImport("uxtheme", ExactSpelling=true)]
		public extern static int IsThemeBackgroundPartiallyTransparent(IntPtr hTheme, int iPartId, int iStateId);

        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static Int32 GetThemeBackgroundContentRect(
            IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pBoundingRect, out RECT pContentRect);

		[DllImport("uxtheme", ExactSpelling=true)]
		public extern static Int32 DrawThemeParentBackground(IntPtr hWnd, IntPtr hdc, ref RECT pRect);

		[DllImport("uxtheme", ExactSpelling=true)]
		public extern static Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId,
			int iStateId, ref RECT pRect, ref RECT pClipRect);

		public const int S_OK = 0x0;

		public const int EP_EDITTEXT = 1;
		public const int ETS_DISABLED = 4;
		public const int ETS_NORMAL = 1;
		public const int ETS_READONLY = 6;

		public const int WM_THEMECHANGED = 0x031A;
		public const int WM_NCPAINT = 0x85;
		public const int WM_NCCALCSIZE = 0x83;

		public const int WS_EX_CLIENTEDGE = 0x200;
		public const int WVR_HREDRAW = 0x100;
		public const int WVR_VREDRAW = 0x200;
		public const int WVR_REDRAW = (WVR_HREDRAW | WVR_VREDRAW);

		[StructLayout(LayoutKind.Sequential)]
		public struct NCCALCSIZE_PARAMS
		{
			public RECT rgrc0, rgrc1, rgrc2;
			public IntPtr lppos;
		}
    }
}
