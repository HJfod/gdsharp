using System.Windows.Forms;

namespace GDSharp {
    namespace Pages {
        public partial class Home : Tab {
            public Home() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Home";

                var text = new Elements.Header();
                text.Text = "Welcome to GDSharp!";

                var text2 = new Elements.TextDark();
                text2.Text = "Log in";

                Controls.Add(text);
                Controls.Add(new Elements.NewLine());
                Controls.Add(text2);
            }
        }
    }
}