using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace GDSharp {
    public partial class Main : Form {
        private Panel MainPanel;

        public static GDShare GDShare = new GDShare();

        public static void ShowLoadingMessage(string msg, int prog) {
            Program.BootupSplash.Progress.Text = msg;
            Program.BootupSplash.SetProgress((float)prog);
        }

        public Main() {
            string data = GDShare.DecodeCCFile(GDShare.GetCCPath("GameManager"), ShowLoadingMessage);
            string LLdata = GDShare.DecodeCCFile(GDShare.GetCCPath("LocalLevels"), ShowLoadingMessage);
            Console.WriteLine("+ Decrypted GD data");

            InitializeComponent();
        }

        class SelectMenu {
            public int ID { get; set; }
            public string Text { get; set; }
        }

        public dynamic TabList = new {};

        private Panel Tabs;
        private Panel Pages;
        public Pages.Overlay Overlay;
        private VScrollBar ScrollBar;

        private void InitializeComponent() {
            Text = $"{Settings.AppName} {Settings.VersionString}";
            ClientSize = new Size(Dimensions.Width, Dimensions.Height);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Style.Color(Style.Colors.Background);

            MainPanel = new Panel();

            Tabs = new Panel();

            Tabs.Location = new Point(0,0);
            Tabs.Size = new Size(Dimensions.Width, Style.TabHeight);
            Tabs.BackColor = Color.FromArgb(0,0,0,0);
            Tabs.ForeColor = Style.Color(Style.Colors.Text);

            Pages = new Panel();
            
            Pages.Location = new Point(0, Style.TabHeight);
            Pages.Size = new Size(Dimensions.Width, Dimensions.Height - Style.TabHeight);
            Pages.BackColor = Style.Color(Style.Colors.Dark);

            DragEnter += DropFileHover;
            DragLeave += FileUnHover;
            DragDrop += DropFile;
            AllowDrop = true;

            Overlay = new Pages.Overlay();
            Overlay.Location = new Point(0,0);

            ScrollBar = new VScrollBar();
            ScrollBar.Location = new Point(Dimensions.Width - ScrollBar.Width, 0);

            /*
            foreach (var i in new List<Panel> {
                new Pages.Home(),
                new Pages.Backups(),
                new Pages.Export(),
                new Pages.Import(),
                new Pages.Settings()
            })
            */

            TabList = new {
                Home = new Pages.Home(),
                Backups = new Pages.Backups(),
                Export = new Pages.Export(),
                Import = new Pages.Import(),
                Settings = new Pages.Settings()
            };

            foreach (PropertyInfo pi in TabList.GetType().GetProperties()) {
                Panel i = pi.GetValue(TabList);

                var Tab = new Elements.Tab();
                Tab.Text = i.Name;
                int offset = 0;
                foreach (Control p in Tabs.Controls) {
                    offset += p.Size.Width + Style.Margin;
                }
                if (offset == 0) Tab.Selected = true;
                Tab.Name = i.Name;
                Tab.Click += (object sender, EventArgs e) => SelectTab(i.Name);
                Tab.Location = new Point(offset, 0);
                Tabs.Controls.Add(Tab);
                Pages.Controls.Add(i);
            }

            ContextMenuStrip CM = new ContextMenuStrip();
            CM.Items.Add(new ToolStripMenuItem("Help", null, (object s, EventArgs e) => MessageBox.Show("help dialog") ));
            CM.Items.Add(new ToolStripMenuItem("Quit (Alt+F4)", null, (object s, EventArgs e) => this.Close() ));

            ContextMenuStrip = CM;

            Icon = new Icon("resources/icon.ico");

            MainPanel.Size = new Size(Dimensions.Width, Dimensions.Height);
            MainPanel.Padding = new Padding(0);
            MainPanel.Margin = new Padding(0);
            MainPanel.Location = new Point(0,0);
            MainPanel.Controls.Add(Tabs);
            MainPanel.Controls.Add(Pages);

            Pages.Controls.Add(this.ScrollBar);

            Controls.Add(MainPanel);
            Controls.Add(Overlay);

            Overlay.BringToFront();

            CenterToScreen();

            Program.BootupSplash.Close();

            Console.WriteLine("+ Splash Closed");
            
            Show();
            Focus();
            Activate();

            Console.WriteLine("+ App succesfully booted up!");
        }

        public void ShowScrollBar(bool show = true) {
            //ScrollBar.Visible = show;
            //ScrollBar.Scroll -= null;
            //ScrollBar.Scroll += (sender, e) => { this.VerticalScroll.Value = ScrollBar.Value; };
        }
        
        public void SelectTab(string tab) {
            foreach (Control p in Pages.Controls) {
                if (p is Pages.Tab) {
                    if (p.Name == tab) p.Show(); else p.Hide();
                }
            }
            foreach (Elements.Tab p in Tabs.Controls) {
                if (p is Elements.Tab) {
                    p.Selected = p.Name == tab ? true : false;
                }
            }
        }

        void DropFileHover(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
                Overlay.SlideOverlay(true);
            }
        }

        void FileUnHover(object sender, EventArgs e) {
            Overlay.SlideOverlay(false);
        }

        void DropFile(object sender, DragEventArgs e) {
            string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string i in s) {
                TabList.Import.AddImport(i);
                SelectTab("Import");
            }
            Overlay.SlideOverlay(false);
        }
    }
}
