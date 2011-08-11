using System;
using System.Drawing.Imaging;
using Encoder = System.Drawing.Imaging.Encoder;

namespace mazio {

    public abstract class Format {
        public abstract ImageCodecInfo getCodecInfo();
        public abstract EncoderParameters getParameters();

        public static ImageCodecInfo getEncoderInfo(String mimeType) {
            int j;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j) {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }

    public class JpgFormat : Format {
        public override ImageCodecInfo getCodecInfo() {
            return getEncoderInfo("image/jpeg");
        }

        public override EncoderParameters getParameters() {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.Quality, ConfigDialog.Instance.JpegQuality);
            return p;
        }

        public override string ToString() {
            return ".jpg";
        }
    }

    public class PngFormat : Format {
        public override ImageCodecInfo getCodecInfo() {
            return getEncoderInfo("image/png");
        }

        public override EncoderParameters getParameters() {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.ColorDepth, 32L);
            return p;
        }

        public override string ToString() {
            return ".png";
        }
    }

    public class MazioFormat : Format {
        public override ImageCodecInfo getCodecInfo() {
            return getEncoderInfo("image/mazio");
        }

        public override EncoderParameters getParameters() {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.ColorDepth, 32L);
            return p;
        }

        public override string ToString() {
            return ".maz";
        }
    }
}
