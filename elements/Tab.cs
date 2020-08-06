using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class Tab : Button {
            private Timer hoverTimer;
            private Color ButtonBGColor = Style.Color(Style.Colors.Background);
            private Color ButtonHVColor = Style.Color(Style.Colors.Hover);
            private Color CurrentBGColor = Style.Color(Style.Colors.Background);
            private bool hovering = false;
            private float hoverAnimation = 0;

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

            private bool _Selected;
            public bool Selected {
                get => this._Selected;
                set {
                    this._Selected = value;
                    if (this._Selected) {
                        if (hoverTimer.Enabled) { hoverTimer.Stop(); }
                        this.ButtonBGColor = Style.Color(Style.Colors.Dark);
                        this.CurrentBGColor = this.ButtonBGColor;

                        this.Invalidate();
                    } else {
                        this.ButtonBGColor = Style.Color(Style.Colors.Background);
                        this.hovering = false;
                        this.hoverTimer.Start();

                        this.Invalidate();
                    }
                }
            }

            public Tab() {
                AutoSize = true;
                Font = Style.GetFont();
                ForeColor = Style.Color(Style.Colors.Text);
                BackColor = Color.FromArgb(0,0,0,0);
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                FlatAppearance.BorderColor = Color.FromArgb(0,0,0,0);
                MouseEnter += HoverIn;
                MouseLeave += HoverOut;
                Height = Style.TabHeight;
                SetStyle(ControlStyles.Selectable, false);

                this.hoverTimer = new Timer();
                this.hoverTimer.Interval = Style.TimerFPS;
                this.hoverTimer.Tick += timer_Tick;

                this.Selected = false;
            }

            protected override bool ShowFocusCues {
                get {
                    return false;
                }
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                using (Brush b = new SolidBrush(CurrentBGColor)) {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    e.Graphics.FillPath(b, Shapes.RoundedRect(this.ClientRectangle, new int[4] { Style.CornerSize, Style.CornerSize, 0, 0 } ));
                    Color TColor = this.Selected ? this.ForeColor : Style.Color(Style.Colors.TextDark);
                    TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, TColor, flags);
                }

                //base.OnPaint(pevent);
            }
            
            private void HoverIn(object sender, EventArgs e) {
                if (!this.Selected) {
                    if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                    this.hovering = true;

                    this.hoverTimer.Start();
                }
               // this.BackColor = Style.Color(Style.Colors.Lighter);
            }

            private void HoverOut(object sender, EventArgs e) {
                if (!this.Selected) {
                    if (this.hoverTimer.Enabled) { this.hoverTimer.Stop(); }

                    this.hovering = false;

                    this.hoverTimer.Start();
                }
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
                    this.hoverAnimation += hoverTimer.Interval * Direction;

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