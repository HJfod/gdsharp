using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace GDSharp {
    public class Dimensions {
        public static int Width = 440;
        public static int Height = 550;
        public static float Scale = 1;
    }

    public class Settings {
        public static int VersionNum = 10;
        public static string VersionString = "v0.1.0";
        public static string BuildString = "Build WIP 090820";
        public static string AppName = "GDSharp";
    }

    public class Program {
        public static void ShowSplash() {
            Application.Run(BootupSplash);
            
            BootupClose.Cancel();
        }

        public static BootupSplash BootupSplash = new BootupSplash();

        public static CancellationTokenSource BootupClose = new CancellationTokenSource();

        public static Task Bootup = new Task(ShowSplash, BootupClose.Token);

        [DllImport( "kernel32.dll" )]
        static extern bool AttachConsole( int dwProcessId );

        [STAThread]
        static void Main() {
            AttachConsole( -1 );

            Console.WriteLine("% Booting up...");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Dimensions.Scale = ( (new Elements.NewLine()).CreateGraphics().DpiX / 96 );
            Dimensions.Width = (int)((float)Dimensions.Width * Dimensions.Scale);
            Dimensions.Height = (int)((float)Dimensions.Height * Dimensions.Scale);

            Style.InitializeFonts();

            Bootup.Start();
            
            Application.Run(new Main());
        }
    }
}
