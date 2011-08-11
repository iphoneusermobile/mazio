using System.Drawing;

namespace mazio
{
    interface View
    {
        void paint(Graphics g, bool drawHandles, ViewParameters parameters);
    }
}
