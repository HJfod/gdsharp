using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDSharp {
    namespace Elements {
        public partial class Container : FlowLayoutPanel {
            public Container() {
                this.Width = Dimensions.Width;
                this.Height = Dimensions.Height - Style.TabHeight;
                this.BackColor = Color.FromArgb(0,0,0,0);
                this.Padding = Style.BigPadding;
                this.Margin = Style.BigPadding;
            }
        }

        public partial class Div : FlowLayoutPanel {
            public Div() {
                this.AutoSize = true;
                this.BackColor = Color.FromArgb(0,0,0,0);
            }
        }

        public partial class GDLevelContents : FlowLayoutPanel {
            public GDLevelContents(int _Width, int _Y) {
                this.Hide();
                this.AutoSize = true;
                this.MinimumSize = new Size(_Width, 0);
                this.MaximumSize = new Size(_Width, 0);
                this.BackColor = Color.FromArgb(0,0,0,0);
                this.Location = new Point(0, _Y);
                this.DoubleBuffered = true;
                this.Padding = Style.Padding;
                this.Cursor = Cursors.Default;
            }
        }

        public partial class GDLevel : Panel {
            public FlowLayoutPanel Contents;
            private int _Width;
            private int _Height;

            private Timer hoverTimer;
            private bool _Hover = false;
            private float _HoverMin = 150;
            private float _HoverAnimation = 0;

            public GDLevel(string iText = "GDLevel") {
                this._Width = Dimensions.Width - Style.PaddingSizeBig * 4;
                this._Height = Style.PaddingSizeBig * 2 + Style.GetFont().Height;

                this.BackColor = Style.Color(Style.Colors.Main);
                this.ForeColor = Style.Color(Style.Colors.TitlebarText);
                this.Padding = Style.BigPadding;
                this.Font = Style.GetFont();
                this.Text = iText;
                this.MinimumSize = new Size(this._Width, this._Height);
                this.MaximumSize = new Size(this._Width, 0);
                this.AutoSize = true;

                this.Contents = new GDLevelContents(this._Width, this._Height);

                this._HoverAnimation = this._HoverMin;

                this.Controls.Add(this.Contents);

                this.MouseEnter += HoverIn;
                this.MouseLeave += HoverOut;
                this.hoverTimer = new Timer();
                this.hoverTimer.Interval = Style.TimerFPS * 4;
                this.hoverTimer.Tick += this.timer_Tick;

                this.Cursor = Cursors.Hand;

                this.DoubleBuffered = true;
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
                Point tr = new Point(Style.PaddingSizeBig, Style.PaddingSizeBig);

                e.Graphics.FillRectangle(new SolidBrush(Style.Color(Style.Colors.Dark)),c);
                e.Graphics.FillPath(b, Shapes.RoundedRect(c, new int[1] { Style.CornerSize }));
                // (int)this._HoverAnimation
                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.FromArgb((int)this._HoverAnimation, this.ForeColor)), tr);
            }

            public void Add(Control arg) {
                this.Contents.Controls.Add(arg);
            }

            private void Roll(object Sender, EventArgs e) {
                this.Contents.Visible = this.Contents.Visible ? false : true;
                this.Invalidate();
            }

            private void HoverIn(object sender, EventArgs e) {
                if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                this._Hover = true;

                this.hoverTimer.Start();
            }

            private void HoverOut(object sender, EventArgs e) {
                if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                this._Hover = false;

                this.hoverTimer.Start();
               // this.BackColor = Style.Color(Style.Colors.Light);
            }

            private void timer_Tick(object sender, EventArgs e) {
                bool stopCondition = false;
                int Direction;
                float Max;

                if (this._Hover) {
                    stopCondition = (this._HoverAnimation >= 255);
                    Max = 255;
                    Direction = 1;
                } else {
                    stopCondition = (this._HoverAnimation <= this._HoverMin);
                    Max = this._HoverMin;
                    Direction = -1;
                }

                if (stopCondition) {
                    this._HoverAnimation = Max;
                    this.hoverTimer.Stop();
                } else {
                    this._HoverAnimation += this.hoverTimer.Interval * Direction;
                }

                if (this._HoverAnimation > 255) this._HoverAnimation = 255;
                if (this._HoverAnimation < 0) this._HoverAnimation = 0;

                this.Invalidate();
            }
        }
    }
}