using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDSharp {
    namespace Elements {
        public partial class Container : FlowLayoutPanel {
            public Container() {
                Width = Dimensions.Width;
                Height = Dimensions.Height;
                BackColor = Color.FromArgb(0,0,0,0);
                Padding = Style.BigPadding;
                Margin = Style.BigPadding;
            }
        }

        public partial class Div : FlowLayoutPanel {
            public Div() {
                AutoSize = true;
                BackColor = Color.FromArgb(0,0,0,0);
            }
        }

        public partial class GDLevel : FlowLayoutPanel {
            public FlowLayoutPanel Contents;

            public GDLevel(string iText = "GDLevel") {
                this.Width = Dimensions.Width - Style.PaddingSizeBig * 4;
                this.Height = Style.PaddingSizeBig * 2 + Style.GetFont().Height;
                this.BackColor = Style.Color(Style.Colors.Main);
                this.ForeColor = Style.Color(Style.Colors.TitlebarText);
                this.Padding = Style.BigPadding;
                this.Font = Style.GetFont();
                this.Text = iText;

                this.Contents = new FlowLayoutPanel();
                this.Contents.Hide();

                this.Controls.Add(this.Contents);

                this.Paint += this.Draw;
                this.Click += this.Roll;
            }

            private void Draw(object sender, PaintEventArgs e) {
                Rectangle c = this.ClientRectangle;

                LinearGradientBrush b = new LinearGradientBrush(
                    new Point(c.Left, c.Top),
                    new Point(c.Right, c.Bottom),
                    Style.Color(Style.Colors.Main),
                    Style.Color(Style.Colors.Secondary)
                );

                Rectangle r = new Rectangle(new Point(this.Left, this.Top), new Size(this.Width, this.Height));
                Rectangle tr = new Rectangle(c.Left + Style.PaddingSizeBig, c.Top, c.Width - Style.PaddingSizeBig * 2, c.Height);

                e.Graphics.FillRectangle(new SolidBrush(Style.Color(Style.Colors.Dark)),c);
                e.Graphics.FillPath(b, Shapes.RoundedRect(c, new int[1] { Style.CornerSize }));
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, tr, this.ForeColor, TextFormatFlags.VerticalCenter);
            }

            public void Add (Control arg) {
                this.Contents.Controls.Add(arg);
            }

            private void Roll(object Sender, EventArgs e) {
                this.Contents.Visible = this.Contents.Visible ? false : true;
            }
        }
    }
}