using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class Select : ListBox {
            public class SelectItem {
                public string Text { get; set; }
                public int Index { get; set; }
            }

            public Select(ContextMenuStrip _Menu = null) {
                this.Width = Dimensions.Width - Style.PaddingSizeBig * 2;
                this.Height = 200;
                this.BackColor = Style.Color(Style.Colors.Light);
                this.ForeColor = Style.Color(Style.Colors.Text);
                this.Padding = Style.Padding;
                this.Margin = Style.Padding;
                this.BorderStyle = BorderStyle.None;
                this.SelectionMode = SelectionMode.MultiSimple;
                this.DisplayMember = "Text";
                this.ValueMember = "Text";
                this.DrawItem += DrawSelectItem;
                this.ItemHeight = Style.GetFont().Height * 4;
                this.Font = Style.GetFont();
                this.ContextMenuStrip = _Menu;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);

                SolidBrush b = new SolidBrush(this.BackColor);
                e.Graphics.FillPath(b, Shapes.RoundedRect(this.ClientRectangle, new int[1] { Style.CornerSize }));
            }

            private void DrawSelectItem(object sender, DrawItemEventArgs e) {
                if (e.Index < 0) return;

                Rectangle r = new Rectangle(
                    e.Bounds.Left,
                    e.Bounds.Top,
                    e.Bounds.Width + Style.PaddingSize * 2,
                    e.Bounds.Height + Style.PaddingSize * 2
                );
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                    e = new DrawItemEventArgs(
                        e.Graphics,
                        Style.GetFont(),
                        r,
                        e.Index,
                        e.State ^ DrawItemState.Selected,
                        e.ForeColor,
                        Style.Color(Style.Colors.Main)
                    );
                }

                e.DrawBackground();
                e.Graphics.DrawString(this.Items[e.Index].ToString(), Style.GetFont(), new SolidBrush(this.ForeColor), r, StringFormat.GenericDefault);
            }

            public bool AddItem(string Text) {
                this.Items.Add(new SelectItem() {
                    Text = Text,
                    Index = this.Items.Count
                });

                return true;
            }
        }
    }
}