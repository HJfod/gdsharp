using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

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

                ContextMenuStrip CM = new ContextMenuStrip();
                CM.Items.Add(new ToolStripMenuItem("View &selected info", null, ViewInfo ));
                CM.Items.Add(new ToolStripMenuItem("Export selected", null, ExportLevel ));

                selectLevel = new Elements.Select(CM);

                Program.BootupSplash.Progress.Text = "Loading levels...";

                foreach (dynamic lvl in GDShare.GetLevelList(null, Main.ShowLoadingMessage)) {
                    selectLevel.AddItem(lvl.Name);
                }

                C.Controls.Add(selectLevel);
                C.Controls.Add(button);
                C.Controls.Add(new Elements.Text("Right-click level list for more options"));
                Controls.Add(C);
            }

            private void ViewInfo(object sender, EventArgs e) {
                try {
                    if (selectLevel.SelectedItems.Count > 0) {
                        Elements.Select.SelectItem lvl = (Elements.Select.SelectItem)selectLevel.SelectedItems[0];
                        dynamic LevelInfo = GDShare.GetLevelInfo(lvl.Text);
                        
                        string Info = "";

                        foreach (PropertyInfo i in LevelInfo.GetType().GetProperties()) {
                            if (i.Name != "Name" && i.Name != "Creator" && i.Name != "Description")
                                Info += $"{i.Name.Replace("_", " ")}: {i.GetValue(LevelInfo)}\n";
                        }

                        MessageBox.Show(Info, $"Info for {lvl.Text}", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                } catch(Exception err) {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void ExportLevel(object sender, EventArgs e) {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult dr = fbd.ShowDialog();

                if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)) {
                    string failure = "";
                    string success = "";
                    foreach (Elements.Select.SelectItem i in selectLevel.SelectedItems) {
                        string ExportTry = GDShare.ExportLevel(i.Text, fbd.SelectedPath);
                        if (ExportTry != null) {
                            failure += ExportTry;
                        } else {
                            success += $"{i.Text}; ";
                        }
                    }
                    if (failure.Length > 0) {
                        Console.WriteLine($"- Error exporting {failure}");
                        MessageBox.Show($"Error: {failure}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        Console.WriteLine($"+ Succesfully exported {success}");
                        MessageBox.Show($"Succesfully exported {success}");
                    }
                } else if (dr != DialogResult.Cancel) {
                    Console.WriteLine($"- Selected path for exporting not accepted.");
                    MessageBox.Show("Selected path not accepted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}