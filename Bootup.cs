using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System;

namespace GDSharp {
    public partial class LoadingPanel : Panel {
        public LoadingPanel(int iWidth, int iHeight, Point loc) {
            Width = iWidth;
            Height = iHeight;
            Location = loc;
        }
    }

    public partial class BootupSplash : Form {
        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Label Progress;
        public Panel ProgressBar;

        private static int ProgressBarWidth = 350;
        private static int ProgressBarHeight = 7;

        public void SetProgress(float percentage) {
            ProgressBar.Width = (int)((float)ProgressBarWidth * (percentage / 100f));
        }

        public BootupSplash() {
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            Padding = new Padding(0);
            TransparencyKey = Color.Black;

            Image AppIcon = Image.FromFile(@"resources\icon.png");

            PictureBox Pic = new PictureBox();
            Pic.Image = ResizeImage(AppIcon, 170, 170);
            Pic.Location = new Point(10, 10);
            Pic.AutoSize = true;

            LoadingPanel ProgressBG = new LoadingPanel(ProgressBarWidth, 32, new Point(220,100));

            Progress = new Label();
            Progress.Text = "Loading...";
            Progress.Font = Style.GetFont();
            Progress.TextAlign = ContentAlignment.MiddleCenter;
            Progress.Dock = DockStyle.Fill;
            Progress.ForeColor = Style.Color(Style.Colors.Text);

            ProgressBar = new Panel();
            ProgressBar.BackColor = Style.Color(Style.Colors.Main);
            ProgressBar.Height = ProgressBarHeight;
            ProgressBar.Width = ProgressBarWidth;
            ProgressBar.Location = new Point(220, 132);

            Label Title = new Label();
            Title.Location = new Point(220, 16);
            Title.Text = Settings.AppName;
            Title.Font = Style.GetHeaderFont(50);
            Title.AutoSize = true;
            Title.ForeColor = Style.Color(Style.Colors.Text);

            Label Subtitle = new Label();
            Subtitle.Text = $"{GDSharp.Settings.VersionString} ({GDSharp.Settings.VersionNum}) {GDSharp.Settings.BuildString}";
            Subtitle.AutoSize = true;
            Subtitle.ForeColor = Style.Color(Style.Colors.Text);
            Subtitle.Font = Style.GetFont(8);
            Subtitle.Location = new Point(220, 149);

            Width = 570;
            Height = 200;

            MinimumSize = Size;
            MaximumSize = Size;

            Icon = new Icon("resources/icon.ico");

            ProgressBG.Controls.Add(Progress);
            Controls.Add(ProgressBG);
            Controls.Add(Pic);
            Controls.Add(ProgressBar);
            Controls.Add(Subtitle);
            Controls.Add(Title);

            Console.WriteLine("+ Showed Splash Screen");
        }
    }
}