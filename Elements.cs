using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDSharp {
    public class Elements {
        public partial class GButton : Button {
            Timer hoverTimer;
            Color ButtonBGColor = Style.Color(Style.Colors.Light);
            static bool hovering = false;

            public GButton() {
                AutoSize = true;
                Font = Style.GetFont();
                ForeColor = Style.Color(Style.Colors.Text);
                BackColor = Color.FromArgb(0,0,0,0);
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                Padding = new Padding(Style.Padding * 4, Style.Padding, Style.Padding * 4, Style.Padding);
                MouseEnter += HoverIn;
                MouseLeave += HoverOut;

                hoverTimer = new Timer();
                hoverTimer.Interval = 100;
                hoverTimer.Tick += timer_Tick;
            }
            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                using (Brush b = new SolidBrush(ButtonBGColor)) {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    e.Graphics.FillPath(b, RoundedRect(this.ClientRectangle, Style.CornerSize));
                    TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, flags);
                    //e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), ClientRectangle);
                }

                //base.OnPaint(pevent);
            }
            private void HoverIn(object sender, EventArgs e) {
                if (hoverTimer.Enabled) { hoverTimer.Stop(); }
                hoverTimer.Start();

                hovering = true;
               // this.BackColor = Style.Color(Style.Colors.Lighter);
            }
            private void HoverOut(object sender, EventArgs e) {
                if (hoverTimer.Enabled) { hoverTimer.Stop(); }
                hoverTimer.Start();

                hovering = false;
               // this.BackColor = Style.Color(Style.Colors.Light);
            }
            private void timer_Tick(object sender, EventArgs e) {

                ButtonBGColor = Style.Color(Style.Colors.Lighter);

                hoverTimer.Stop();
            }
        }
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius) {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
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
        }
    }
}