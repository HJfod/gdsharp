using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class GButton : Button {
            Timer hoverTimer;
            static Color ButtonBGColor = Style.Color(Style.Colors.Light);
            static Color ButtonHVColor = Style.Color(Style.Colors.Lighter);
            static Color CurrentBGColor = ButtonBGColor;
            static bool hovering = false;
            static float hoverAnimation = 0;

            private class BaseColor {
                public static int R = Convert.ToInt16(ButtonBGColor.R);
                public static int G = Convert.ToInt16(ButtonBGColor.G);
                public static int B = Convert.ToInt16(ButtonBGColor.B);
            }

            private class HoverColor {
                public static int R = Convert.ToInt16(ButtonHVColor.R);
                public static int G = Convert.ToInt16(ButtonHVColor.G);
                public static int B = Convert.ToInt16(ButtonHVColor.B);
            }

            private class HoverAnimation {
                public static float DestR = Math.Abs(HoverColor.R - BaseColor.R);
                public static float DestG = Math.Abs(HoverColor.G - BaseColor.G);
                public static float DestB = Math.Abs(HoverColor.B - BaseColor.B);
            }

            public GButton() {
                AutoSize = true;
                Font = Style.GetFont();
                ForeColor = Style.Color(Style.Colors.Text);
                BackColor = Color.FromArgb(0,0,0,0);
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                Padding = Style.ButtonPadding;
                MouseEnter += HoverIn;
                MouseLeave += HoverOut;

                hoverTimer = new Timer();
                hoverTimer.Interval = Style.TimerFPS;
                hoverTimer.Tick += timer_Tick;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                using (Brush b = new SolidBrush(CurrentBGColor)) {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    e.Graphics.FillPath(b, Shapes.RoundedRect(this.ClientRectangle, new int[1] { Style.CornerSize }));
                    TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, flags);
                    //e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), ClientRectangle);
                }

                //base.OnPaint(pevent);
            }
            
            private void HoverIn(object sender, EventArgs e) {
                if (hoverTimer.Enabled) { hoverTimer.Stop(); }

                hovering = true;

                hoverTimer.Start();
               // this.BackColor = Style.Color(Style.Colors.Lighter);
            }

            private void HoverOut(object sender, EventArgs e) {
                if (hoverTimer.Enabled) { hoverTimer.Stop(); }

                hovering = false;

                hoverTimer.Start();
               // this.BackColor = Style.Color(Style.Colors.Light);
            }

            private void timer_Tick(object sender, EventArgs e) {
                bool stopCondition = false;
                int Direction;

                if (hovering) {
                    stopCondition = (hoverAnimation >= Style.TransitionTime);
                    Direction = 1;
                } else {
                    stopCondition = (hoverAnimation <= 0);
                    Direction = -1;
                }

                if (stopCondition) {
                    hoverTimer.Stop();
                } else {
                    hoverAnimation += hoverTimer.Interval * Direction;

                    CurrentBGColor = Color.FromArgb(
                        BaseColor.R + (int)(HoverAnimation.DestR * (hoverAnimation / Style.TransitionTime)),
                        BaseColor.G + (int)(HoverAnimation.DestG * (hoverAnimation / Style.TransitionTime)),
                        BaseColor.B + (int)(HoverAnimation.DestB * (hoverAnimation / Style.TransitionTime))
                    );
                }

                this.Invalidate();
            }
        }
    }
}