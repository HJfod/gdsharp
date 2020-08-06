using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDSharp {
    namespace Elements {
        public class Shapes {
            public static GraphicsPath RoundedRect(Rectangle bounds, int[] radius) {
                if (radius.Length == 1) {
                    int rad = radius[0];
                    int diameter = rad * 2;
                    Size size = new Size(diameter, diameter);
                    Rectangle arc = new Rectangle(bounds.Location, size);
                    GraphicsPath path = new GraphicsPath();

                    if (rad == 0) {
                        path.AddRectangle(bounds);
                        return path;
                    }

                    // top left arc  
                    path.AddArc(arc, 180, 90);

                    // top right arc  
                    arc.X = bounds.Right - diameter;
                    path.AddArc(arc, 270, 90);

                    // bottom right arc  
                    arc.Y = bounds.Bottom - diameter;
                    path.AddArc(arc, 0, 90);

                    // bottom left arc 
                    arc.X = bounds.Left;
                    path.AddArc(arc, 90, 90);

                    path.CloseFigure();
                    return path;
                } else {
                    GraphicsPath path = new GraphicsPath();
                    
                    for (var i = 0; i < radius.Length; i++) {
                        int diameter = radius[i] * 2;
                        Size size = new Size(diameter, diameter);
                        Rectangle arc = new Rectangle(bounds.Location, size);

                        if (radius[i] > 0) {
                            switch (i) {
                                case 0:
                                    path.AddArc(arc, 180, 90);
                                    break;
                                case 1:
                                    arc.X = bounds.Right - diameter;
                                    path.AddArc(arc, 270, 90);
                                    break;
                                case 2:
                                    arc.X = bounds.Right - diameter;
                                    arc.Y = bounds.Bottom - diameter;
                                    path.AddArc(arc, 0, 90);
                                    break;
                                case 3:
                                    arc.Y = bounds.Bottom - diameter;
                                    path.AddArc(arc, 90, 90);
                                    break;
                            }
                        } else {
                            Point p = new Point();
                            switch (i) {
                                case 0:
                                    p = new Point(0,0);
                                    break;
                                case 1:
                                    p = new Point(bounds.Right, 0);
                                    break;
                                case 2:
                                    p = new Point(bounds.Right, bounds.Bottom);
                                    break;
                                case 3:
                                    p = new Point(0, bounds.Bottom);
                                    break;
                                default:
                                    p = new Point(0,0);
                                    break;
                            }
                            path.AddLine(p, p);
                        }
                    }
                    path.CloseFigure();

                    return path;
                }
            }
        }
    }
}