// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerForm.cs" company="Rubius">
//   All rights reserved. 2008-2012
// </copyright>
// <summary>
//   Defines the ServerForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Server
{
    using System;
    using System.Windows.Forms;

    public interface IServerView
    {
        void WriteLog(string log);

        void ShowException(Exception ex);
    }

    public partial class ServerForm : Form, IServerView
    {
        private ChatServer _chatServer;

        public ServerForm()
        {
            InitializeComponent();

            stopToolStripMenuItem.Enabled = false;
            txtServerName.Text = Properties.Settings.Default.ServerName;
            txtServerPort.Text = Properties.Settings.Default.ServerPort;
        }

        private void OnStartServerButtonClick(object sender, EventArgs e)
        {
            try
            {
                var name = txtServerName.Text;

                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Server name is not correct.");
                }

                int port;

                if (!int.TryParse(txtServerPort.Text, out port))
                {
                    throw new Exception("Server port is not correct.");
                }

                _chatServer = new ChatServer(port, name, this);

                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;

                Properties.Settings.Default.ServerName = txtServerName.Text;
                Properties.Settings.Default.ServerPort = txtServerPort.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowDialog(this, ex);
            }
        }

        private void OnStopServerButtonClick(object sender, EventArgs e)
        {
            try
            {
                _chatServer.Close();

                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_chatServer != null)
            {
                _chatServer.Close();
            }

            base.OnClosed(e);
        }

        public void WriteLog(string log)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((MethodInvoker)delegate
                        {
                            txtLog.AppendText(log);
                            txtLog.AppendText(Environment.NewLine);
                        });
            }
            else
            {
                txtLog.AppendText(log);
                txtLog.AppendText(Environment.NewLine);
            }
        }

        public void ShowException(Exception ex)
        {
            Common.ErrorHandling.ShowExceptionThreadSafe(this, ex);
        }
    }
}
