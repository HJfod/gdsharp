using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GDSharp {
    public class Dimensions {
        public static int Width = 440;
        public static int Height = 550;
    }
    public class Program {
        [DllImport( "kernel32.dll" )]
        static extern bool AttachConsole( int dwProcessId );

        [STAThread]
        static void Main() {
            AttachConsole( -1 );

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
