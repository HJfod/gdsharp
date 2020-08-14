using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace GDSharp {
    namespace Pages {
        public partial class Import : Tab {
            public Import() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Import";

                Elements.Container C = new Elements.Container();

                var ImportButton = new Elements.GButton();
                ImportButton.Text = "Import";
                ImportButton.Click += new EventHandler(OnClick);

                Elements.Div ImportLeveLArea = new Elements.Div();

                C.Controls.Add(ImportButton);
                C.Controls.Add(ImportLeveLArea);

                Controls.Add(C);
            }

            private void OnClick(object sender, EventArgs e) {
                Console.WriteLine("bbbbb");
            }
        }
    }
}