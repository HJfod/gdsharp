using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System;

namespace GDSharp {
    namespace Pages {
        public partial class Overlay : Panel {
            public Overlay() {
                InitializeComponent();
            }

            private Timer _Timer;
            private double _Animation = 0;
            private int _Frame = 0;
            private bool _Anim = false;
            private int _AnimationLength = 300;
            private int _AnimationEnd = Dimensions.Height;

            private void InitializeComponent() {
                this.Width = Dimensions.Width;
                this.Height = 0;
                this.Top = 0;
                this.Left = 0;
                this.BackColor = Color.FromArgb(0,0,0,0);
                this.Paint += this.Draw;
                this.DoubleBuffered = true;
                this.Font = Style.GetFont();

                this._Timer = new Timer();
                this._Timer.Interval = Style.TimerFPS;
                this._Timer.Tick += AnimationTick;
            }

            private void Draw(object sender, PaintEventArgs e) {
                Rectangle c = new Rectangle(this.Left, this.Top, this.Width, this.Height);

                LinearGradientBrush b = new LinearGradientBrush(
                    new Point(0,0),
                    new Point(c.Width, c.Height),
                    Style.Color(Style.Colors.Main),
                    Style.Color(Style.Colors.Secondary)
                );

                string Txt = "Drop files to import!";
                string SubTxt = "Supported level formats: .gmd, .lvl\n\nSupported backup formats: Directory, .gdb";

                int SubOffset = 200;

                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                int size = (int)(11F * ((float)this._Animation / (float)this._AnimationLength));

                e.Graphics.FillRectangle(b,c);
                if (size > 0) {
                    e.Graphics.DrawString(
                        Txt,
                        Style.GetHeaderFont(24),
                        new SolidBrush(Style.Color(Style.Colors.TitlebarText)),
                        c,
                        format
                    );

                    e.Graphics.DrawString(
                        SubTxt,
                        Style.GetFont(13),
                        new SolidBrush(Color.FromArgb(125, Style.Color(Style.Colors.TitlebarText))),
                        new Rectangle(c.Left, c.Top + SubOffset, c.Width, c.Height - SubOffset),
                        format
                    );
                }

                b.Dispose();
            }

            private void AnimationTick(object sender, EventArgs e) {
                int StartTime = 0;
                int Duration = _AnimationLength;
                int EndPos = this._AnimationEnd;
                
                this._Frame += (this._Anim ? 1 : -1) * Style.TimerFPS;
                
                if (this._Frame >= 0 && this._Frame <= this._AnimationLength) {
                    this._Animation = Math.Round(Math.Cos((double)((Math.PI / Duration) * (this._Frame - StartTime))) * (double)(0 - EndPos / 2) + (double)(EndPos / 2));
                } else {
                    this._Timer.Stop();
                    this._Frame = this._Frame <= 0 ? 0 : _AnimationLength;
                }

                this.Height = (int)this._Animation;

                this.Invalidate();
            }

            public void ToggleOverlay(bool show = true) {
                if (this._Timer.Enabled) this._Timer.Stop();
                this._Anim = show;
                this._Timer.Start();
            }
        }
    }
}