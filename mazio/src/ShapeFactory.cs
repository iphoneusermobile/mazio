using System.Xml.XPath;
using mazio.shapes;

namespace mazio {
    class ShapeFactory {
        public static Shape createShape(ScreenshotEditor editor, bool cropBoxActive, XPathNavigator element) {
            string type = element.GetAttribute("type", "");
            Shape s = null;
            switch (type) {
                case "Arrow":
                    s = new Arrow(editor, element);
                    break;
                case "CropBox":
                    s = new CropBox(editor, element, cropBoxActive);
                    break;
                case "Censor":
                    s = new Censor(editor, element);
                    break;
                case "Line":
                    s = new Line(editor, element);
                    break;
                case "Oval":
                    s = new Oval(editor, element);
                    break;
                case "MagnifyingGlass":
                    s = new MagnifyingGlass(editor, element);
                    break;
                case "Pencil":
                    s = new Pencil(editor, element);
                    break;
                case "Picture":
                    s = new Picture(editor, element);
                    break;
                case "PonyVille":
                    s = new PonyVille(editor, element);
                    break;
                case "RectangleShape":
                    s = new RectangleShape(editor, element);
                    break;
                case "TextShape":
                    s = new TextShape(editor, element);
                    break;
                default:
                    break;
            }
            return s;
        }
    }
}
