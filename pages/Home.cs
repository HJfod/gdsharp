using System.Windows.Forms;
using System.Dynamic;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Home : Tab {
            public Home() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Home";

                dynamic UserInfo = GDShare.GetGDUserInfo(null);

                var WelcomeText = new Elements.Header();
                WelcomeText.Text = "Welcome to GDSharp!";

                var UserName = new Elements.TextDark();
                UserName.Text = $"Logged in as {UserInfo.Name} | UserID {UserInfo.UserID}";

                string Stats = "";

                foreach (PropertyInfo i in UserInfo.Stats.GetType().GetProperties()) {
                    Stats += $"{i.Name.Replace("_", " ")}: {i.GetValue(UserInfo.Stats)}\n";
                }

                var UserStats = new Elements.Text();
                UserStats.Text = $"{Stats}";
                UserStats.Hide();

                var StatsShow = new Elements.GButton();
                StatsShow.Text = "View stats";
                StatsShow.Click += (object sender, EventArgs e) => {
                    UserStats.Visible = UserStats.Visible ? false : true;
                };

                var VersionString = new Elements.TextDark();
                VersionString.Text = $"{GDSharp.Settings.AppName} {GDSharp.Settings.VersionString} ({GDSharp.Settings.VersionNum}) {GDSharp.Settings.BuildString}";
                VersionString.Dock = DockStyle.Bottom;
                VersionString.Anchor = AnchorStyles.Bottom;

                Controls.Add(WelcomeText);
                Controls.Add(new Elements.NewLine());
                Controls.Add(UserName);
                Controls.Add(new Elements.NewLineBig());
                Controls.Add(StatsShow);
                Controls.Add(new Elements.NewLine());
                Controls.Add(UserStats);
                Controls.Add(VersionString);
            }
        }
    }
}