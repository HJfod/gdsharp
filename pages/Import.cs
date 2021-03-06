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

            Elements.Div ImportLeveLArea;

            private void InitializeComponent() {
                Name = "Import";

                Elements.Container C = new Elements.Container();

                var ImportButton = new Elements.GButton(null, "Click to Import Level(s)", true);
                ImportButton.Click += new EventHandler(OnClick);

                ImportLeveLArea = new Elements.Div();

                C.Controls.Add(ImportButton);
                C.Controls.Add(ImportLeveLArea);

                Controls.Add(C);
            }

            public void AddImport(string file) {
                Elements.GDLevel Level = new Elements.GDLevel(file);

                EventHandler ImportThis = (object s, EventArgs e) => {
                    string res = GDShare.ImportLevel(file);
                    if (res != null) MessageBox.Show($"Error: {res}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); else Level.Dispose();
                };

                EventHandler CloseThis = (object s, EventArgs e) => Level.Dispose();

                ContextMenuStrip CM = new ContextMenuStrip();
                CM.Items.Add(new ToolStripMenuItem("Import", null, ImportThis ));
                CM.Items.Add(new ToolStripMenuItem("Close", null, CloseThis ));

                Level.ContextMenuStrip = CM;

                dynamic LevelInfo = GDShare.GetLevelInfo(file);
                
                string Info = "";

                foreach (PropertyInfo i in LevelInfo.GetType().GetProperties()) {
                    if (i.Name != "Name" && i.Name != "Creator" && i.Name != "Description")
                        Info += $"{i.Name.Replace("_", " ")}: {i.GetValue(LevelInfo)}\n";
                }

                Color c = Style.Color(Style.Colors.TitlebarText);

                Level.Add(new Elements.Header(LevelInfo.Name, c));
                Level.Add(new Elements.NewLine());
                Level.Add(new Elements.Text($"by {LevelInfo.Creator}", c));
                Level.Add(new Elements.NewLine());
                Level.Add(new Elements.TextDark($"\"{LevelInfo.Description}\"", Color.FromArgb(150, c)));
                Level.Add(new Elements.NewLine());
                Level.Add(new Elements.Text($"{Info}", c));
                Level.Add(new Elements.NewLineBig());

                Elements.GButton ImportButton = new Elements.GButton(Style.Color(Style.Colors.Main), "Import");
                ImportButton.Click += ImportThis;

                Elements.GButton CloseButton = new Elements.GButton(Style.Color(Style.Colors.Main), "Close");
                CloseButton.Click += CloseThis;

                Level.Add(ImportButton);
                Level.Add(CloseButton);

                ImportLeveLArea.Controls.Add(Level);
            }

            private void OnClick(object sender, EventArgs e) {
                using (OpenFileDialog ofd = new OpenFileDialog()) {
                    ofd.InitialDirectory = "c:\\";
                    ofd.Filter = $"Level files (*.{GDShare.Ext.LevelAlt};*.{GDShare.Ext.Level})|*.{GDShare.Ext.LevelAlt};*.{GDShare.Ext.Level}|All files (*.*)|*.*";
                    ofd.FilterIndex = 1;
                    ofd.RestoreDirectory = true;
                    ofd.Multiselect = true;

                    if (ofd.ShowDialog() == DialogResult.OK) {
                        foreach (string file in ofd.FileNames) {
                            AddImport(file);
                        }
                    }
                }
            }
        }
    }
}