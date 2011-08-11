using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace mazio
{
    public abstract class UploadType
    {
        public abstract Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline);
        public abstract string getName();

        public override string ToString()
        {
            return getName();
        }
    }
}
