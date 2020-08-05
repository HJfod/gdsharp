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
        private FlowLayoutPanel MainPanel;

        public Main() {
            InitializeComponent();
        }

        class SelectMenu {
            public int ID { get; set; }
            public string Text { get; set; }
        }

        private void InitializeComponent() {
            Text = "GDSharp";
            ClientSize = new Size(Dimensions.Width, Dimensions.Height);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            MainPanel = new FlowLayoutPanel();

            GDShare GDShare = new GDShare();

            var text = new Label();
            text.Text = "Welcome to GDSharp!";
            text.Font = Style.GetHeaderFont();
            text.AutoSize = true;
            text.Width = 300;
            text.ForeColor = Style.Color(Style.Colors.Text);

            var button = new Elements.GButton();
            button.Text = "Export";
            button.Click += new EventHandler(OnClick);

            MainPanel.Controls.Add(text);
            MainPanel.Controls.Add(button);
            MainPanel.Width = Dimensions.Width;
            MainPanel.Height = Dimensions.Height;
            MainPanel.BackColor = Style.Color(Style.Colors.Background);
            Controls.Add(MainPanel);

            CenterToScreen();
        }

        void OnClick(object sender, EventArgs e) {
            Console.WriteLine("what do i look like, a blue ball?");
        }
    }
}
