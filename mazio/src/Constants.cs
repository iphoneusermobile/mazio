using System.Drawing;

namespace mazio
{
    public static class Constants
    {
        public static string REG_KEY = "Software\\Kalamon\\Mazio";
        public static string INI_NAME = "mazio_portable.ini";
        public static string INI_SECTION = "mazio";
        public static Color SHADOW_COLOR = Color.FromArgb(0x3f, Color.Black);
        public static Color TEXT_BG_COLOR = Color.FromArgb(0x7f, Color.Yellow);
        public static int DEFAULT_GRAB_MARGIN = 100;
        public static int DEFAULT_JPEG_QUALITY = 80;
        public static int SHADOW_X = 1;
        public static int SHADOW_Y = -2;
        public static double HILIGHT_BOOST = 0.25;
        public static int MIN_MAGNIFIER_ZOOM = 100;
        public static int MAX_MAGNIFIER_ZOOM = 400;
        public static int DEFAULT_MAGNIFIER_ZOOM = 200;
    }
}
