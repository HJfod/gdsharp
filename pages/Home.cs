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

                Elements.Container C = new Elements.Container();
                C.Location = new Point(0,0);

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
                VersionString.Location = new Point(Style.PaddingSize, Dimensions.Height - Style.PaddingSize - Style.TabHeight - VersionString.Height);

                C.Controls.Add(WelcomeText);
                C.Controls.Add(new Elements.NewLine());
                C.Controls.Add(UserName);
                C.Controls.Add(new Elements.NewLineBig());
                C.Controls.Add(StatsShow);
                C.Controls.Add(new Elements.NewLine());
                C.Controls.Add(UserStats);
                Controls.Add(C);
                Controls.Add(VersionString);

                VersionString.BringToFront();
            }
        }
    }
}