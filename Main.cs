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

namespace GDSharp {
    public partial class Main : Form {
        private Panel MainPanel;

        public static GDShare GDShare = new GDShare();

        public Main() {
            string data = GDShare.DecodeCCFile(GDShare.GetCCPath("GameManager"));
            Console.WriteLine(data.Substring(0, 100));

            InitializeComponent();
        }

        class SelectMenu {
            public int ID { get; set; }
            public string Text { get; set; }
        }

        List<FlowLayoutPanel> TabsList = new List<FlowLayoutPanel> {
            new Pages.Home(),
            new Pages.Backups(),
            new Pages.Export(),
            new Pages.Import(),
            new Pages.Settings()
        };

        private Panel Tabs;
        private Panel Pages;

        private void InitializeComponent() {
            Text = $"{Settings.AppName} {Settings.VersionString}";
            ClientSize = new Size(Dimensions.Width, Dimensions.Height);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Style.Color(Style.Colors.Background);
            Click += Nullify;

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

            foreach (var i in TabsList) {
                var Tab = new Elements.Tab();
                Tab.Text = i.Name;
                int offset = 0;
                foreach (Control p in Tabs.Controls) {
                    offset += p.Size.Width + Style.Margin;
                }
                if (offset == 0) Tab.Selected = true;
                Tab.Click += TabSelect;
                Tab.Click += (Object sender, EventArgs e) => Tab.Selected = true;
                Tab.Click += (Object sender, EventArgs e) => i.Show();
                Tab.Location = new Point(offset, 0);
                Tabs.Controls.Add(Tab);
                Pages.Controls.Add(i);
            }

            Icon = new Icon("resources/icon.ico");

            MainPanel.Size = new Size(Dimensions.Width, Dimensions.Height);
            MainPanel.Padding = new Padding(0);
            MainPanel.Margin = new Padding(0);
            MainPanel.Controls.Add(Tabs);
            MainPanel.Controls.Add(Pages);

            Controls.Add(MainPanel);

            CenterToScreen();
        }

        void Nullify(Object sender, EventArgs e) {
            ActiveControl = null;
        }
        
        void TabSelect(Object sender, EventArgs e) {
            foreach (Elements.Tab p in Tabs.Controls) {
                p.Selected = false;
            }
            foreach (Pages.Tab p in Pages.Controls) {
                p.Hide();
            }
        }
    }
}
