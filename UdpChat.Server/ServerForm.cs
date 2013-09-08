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

    using UdpChat.Common;

    public partial class ServerForm : Form, IServerView, ILogging
    {
        private readonly ChatServer _chatServer;

        public ServerForm()
        {
            InitializeComponent();

            stopToolStripMenuItem.Enabled = false;
            txtServerName.Text = Properties.Settings.Default.ServerName;
            txtServerPort.Text = Properties.Settings.Default.ServerPort;

            var eventLogging = new EventLogging("Udp Chat", "Application");

            _chatServer = new ChatServer(this, new ILogging[] { this, eventLogging });
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

                _chatServer.Start(port, name);

                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;

                Properties.Settings.Default.ServerName = txtServerName.Text;
                Properties.Settings.Default.ServerPort = txtServerPort.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowException(this, ex);
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

        private void OnExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            try
            {
                if (_chatServer != null)
                {
                    _chatServer.DisconnectInactiveContacts();
                }
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.WriteLog("Server is not started yet.");

            InactiveContactsTimer.Start();

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (_chatServer != null)
                {
                    _chatServer.Close();
                }
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
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
            ErrorHandling.ShowExceptionThreadSafe(this, ex);
        }
    }
}
