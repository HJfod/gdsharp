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

                var button = new Elements.GButton();
                button.Text = "Import";
                button.Click += new EventHandler(OnClick);

                var button2 = new Elements.GButton();
                button2.Text = "Another button";
                button2.Click += new EventHandler(OnClick);

                Controls.Add(button);
                Controls.Add(button2);
            }

            private void OnClick(object sender, EventArgs e) {
                Console.WriteLine("bbbbb");
            }
        }
    }
}