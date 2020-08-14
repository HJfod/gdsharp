using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class GButton : Button {
            private Timer hoverTimer;
            private Color ButtonBGColor = Style.Color(Style.Colors.Light);
            private Color ButtonHVColor = Style.Color(Style.Colors.Lighter);
            private Color CurrentBGColor = Style.Color(Style.Colors.Light);
            private bool hovering = false;
            private float hoverAnimation = 0;
            private Color BGColor;

            private float GetDest(string which) {
                int HR = Convert.ToInt16(this.ButtonHVColor.R);
                int HG = Convert.ToInt16(this.ButtonHVColor.G);
                int HB = Convert.ToInt16(this.ButtonHVColor.B);

                int BR = Convert.ToInt16(this.ButtonBGColor.R);
                int BG = Convert.ToInt16(this.ButtonBGColor.G);
                int BB = Convert.ToInt16(this.ButtonBGColor.B);

                switch (which) {
                    case "R": return Math.Abs(HR - BR);
                    case "G": return Math.Abs(HG - BG);
                    case "B": return Math.Abs(HB - BB);
                }
                return 0F;
            }

            public GButton(Color? _BG = null, string _Text = null) {
                this.AutoSize = true;
                this.Font = Style.GetFont();
                this.ForeColor = Style.Color(Style.Colors.Text);
                this.BackColor = Color.FromArgb(0,0,0,0);
                this.FlatStyle = FlatStyle.Flat;
                this.FlatAppearance.BorderSize = 0;
                this.Padding = Style.ButtonPadding;
                this.MouseEnter += this.HoverIn;
                this.MouseLeave += this.HoverOut;
                this.TabStop = false;

                this.BGColor = _BG is Color ? (Color) _BG : Style.Color(Style.Colors.Dark);

                if (_Text != null) this.Text = _Text;

                this.hoverTimer = new Timer();
                this.hoverTimer.Interval = Style.TimerFPS;
                this.hoverTimer.Tick += this.timer_Tick;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                using (Brush b = new SolidBrush(this.CurrentBGColor)) {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    e.Graphics.FillRectangle(new SolidBrush(this.BGColor), this.ClientRectangle);
                    e.Graphics.FillPath(b, Shapes.RoundedRect(this.ClientRectangle, new int[1] { Style.CornerSize }));
                    TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, flags);
                    //e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), ClientRectangle);
                }

                //base.OnPaint(pevent);
            }
            
            private void HoverIn(object sender, EventArgs e) {
                if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                this.hovering = true;

                this.hoverTimer.Start();
               // this.BackColor = Style.Color(Style.Colors.Lighter);
            }

            private void HoverOut(object sender, EventArgs e) {
                if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                this.hovering = false;

                this.hoverTimer.Start();
               // this.BackColor = Style.Color(Style.Colors.Light);
            }

            private void timer_Tick(object sender, EventArgs e) {
                bool stopCondition = false;
                int Direction;

                if (this.hovering) {
                    stopCondition = (this.hoverAnimation >= Style.TransitionTime);
                    Direction = 1;
                } else {
                    stopCondition = (this.hoverAnimation <= 0);
                    Direction = -1;
                }

                if (stopCondition) {
                    this.hoverTimer.Stop();
                } else {
                    this.hoverAnimation += this.hoverTimer.Interval * Direction;

                    this.CurrentBGColor = Color.FromArgb(
                        Convert.ToInt16(ButtonBGColor.R) + (int)(GetDest("R") * (this.hoverAnimation / Style.TransitionTime)),
                        Convert.ToInt16(ButtonBGColor.G) + (int)(GetDest("G") * (this.hoverAnimation / Style.TransitionTime)),
                        Convert.ToInt16(ButtonBGColor.B) + (int)(GetDest("B") * (this.hoverAnimation / Style.TransitionTime))
                    );
                }

                this.Invalidate();
            }
        }
    }
}