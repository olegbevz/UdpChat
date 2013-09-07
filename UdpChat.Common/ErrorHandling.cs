// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandling.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ErrorHandling type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common
{
    using System;
    using System.Windows.Forms;

    public static class ErrorHandling
    {
        public static void ShowExceptionThreadSafe(Form form, Exception ex)
        {
            if (form.InvokeRequired)
            {
                form.Invoke((MethodInvoker)(() => ErrorHandling.ShowException(form, ex)));
            }
            else
            {
                ShowException(form, ex);
            }
        }

        public static void ShowException(IWin32Window window, Exception ex)
        {
            MessageBox.Show(
                window,
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}