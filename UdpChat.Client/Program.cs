﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UdpChat.Client
{
    using UdpChat.Common;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ClientForm());
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowException(null, ex);
            }
        }
    }
}
