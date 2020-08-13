using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Import : Tab {
            public Import() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Import";

                Elements.Container C = new Elements.Container();

                var button = new Elements.GButton();
                button.Text = "Import";
                button.Click += new EventHandler(OnClick);

                var button2 = new Elements.GButton();
                button2.Text = "Another button";
                button2.Click += new EventHandler(OnClick);

                C.Controls.Add(button);
                C.Controls.Add(button2);

                Controls.Add(C);
            }

            private void OnClick(object sender, EventArgs e) {
                Console.WriteLine("bbbbb");
            }
        }
    }
}