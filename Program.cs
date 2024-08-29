using System;
using System.Windows.Forms;
using ZakaBank_24.Main_And_Login_Forms;

namespace ZakaBank_24
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //  Application.Run(new LoginForm());

            Application.Run(new MainForm());
        }
    }
}
