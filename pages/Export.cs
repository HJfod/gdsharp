using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace GDSharp {
    namespace Pages {
        public partial class Export : Tab {
            public Export() {
                InitializeComponent();
            }

            private Elements.Select selectLevel;

            private void InitializeComponent() {
                Name = "Export";

                Elements.Container C = new Elements.Container();

                var button = new Elements.GButton();
                button.Text = "Export";
                button.Click += new EventHandler(ExportLevel);

                selectLevel = new Elements.Select();

                Program.BootupSplash.Progress.Text = "Loading levels...";

                foreach (dynamic lvl in GDShare.GetLevelList(null, Main.ShowLoadingMessage)) {
                    selectLevel.AddItem(lvl.Name);
                }

                C.Controls.Add(selectLevel);
                C.Controls.Add(button);
                Controls.Add(C);
            }

            private void ExportLevel(object sender, EventArgs e) {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult dr = fbd.ShowDialog();

                if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)) {
                    string failure = "";
                    foreach (Elements.Select.SelectItem i in selectLevel.SelectedItems) {
                        string ExportTry = GDShare.ExportLevel(i.Text, fbd.SelectedPath);
                        if (ExportTry != null) {
                            failure += ExportTry;
                        }
                    }
                    if (failure.Length > 0) {
                        MessageBox.Show($"Error: {failure}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (dr != DialogResult.Cancel) {
                    MessageBox.Show("Selected path not accepted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}