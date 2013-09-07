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
    }

    public partial class ServerForm : Form, IServerView
    {
        private ChatServer _chatServer;

        public ServerForm()
        {
            InitializeComponent();

            stopToolStripMenuItem.Enabled = false;
        }

        private void OnStartServerButtonClick(object sender, EventArgs e)
        {
            try
            {
                var port = int.Parse(txtServerPort.Text);

                var name = txtServerName.Text;

                _chatServer = new ChatServer(port, name, this);

                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        }
    }
}
