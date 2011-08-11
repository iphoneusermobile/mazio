using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace mazio.util
{
    // taken from http://www.dotnetjunkies.com/WebLog/johnwood/archive/2006/07/04/transparent_richtextbox.aspx
    //
    // kalamon: well, turns out that this control does not work properly
    // - font becomes transparent when I move the mouse outside of the 
    // control's area. If somebody knows how to fix this, let me know
    //

    public class RichEdit50 : RichTextBox
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams prams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    prams.ExStyle |= 0x020; // transparent
                    prams.ClassName = "RICHEDIT50W";
                }
                return prams;
            }
        }
    }
}
