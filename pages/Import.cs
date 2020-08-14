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

                Elements.GDLevel TestLevel = new Elements.GDLevel();

                ImportLeveLArea.Controls.Add(TestLevel);

                dynamic UserInfo = GDShare.GetLevelInfo("Aspire");

                string Stats = "";

                foreach (PropertyInfo i in UserInfo.GetType().GetProperties()) {
                    Stats += $"{i.Name.Replace("_", " ")}: {i.GetValue(UserInfo)}\n";
                }

                TestLevel.Add((new Elements.Text(Stats, Style.Color(Style.Colors.TitlebarText))));
                TestLevel.Add(new Elements.NewLineBig());
                TestLevel.Add(((new Elements.GButton(Style.Color(Style.Colors.Main), "click for vore"))));

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