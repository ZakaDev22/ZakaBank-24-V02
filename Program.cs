﻿using System;
using System.Windows.Forms;

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

            Application.Run(new LoginForm());
        }
    }
}
