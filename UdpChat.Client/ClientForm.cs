// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientForm.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ClientForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Windows.Forms;

    using UdpChat.Common;

    public partial class ClientForm : Form, IClientView
    {
        private readonly ChatClient _chatClient;

        public ClientForm()
        {
            InitializeComponent();

            _chatClient = new ChatClient(this);
        }

        private void OnLogoutButtonClick(object sender, EventArgs e)
        {
            try
            {
                _chatClient.Logout();
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }
        }

        private void OnExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSendButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (lstChatters.SelectedItem != null)
                {
                    _chatClient.SendChatMessage(txtMessage.Text);

                    txtMessage.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            try
            {
                ShowLoginDialog();
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                ShowLoginDialog();

                txtMessage.Select();
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }

            base.OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (_chatClient != null)
                {
                    if (_chatClient.IsInChat)
                    {
                        _chatClient.Logout();
                    }

                    _chatClient.Close();
                }
            }
            catch (Exception ex)
            {
                this.ShowException(ex);
            }

            base.OnClosed(e);
        }

        private void ShowLoginDialog()
        {
            var dialog = new LoginDialog();

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var endPoint = new IPEndPoint(IPAddress.Parse(dialog.ServerIP), int.Parse(dialog.ServerPort));

                _chatClient.Start(endPoint);

                _chatClient.Login(dialog.User);
            }
        }

        public void DisplayContacts(IEnumerable<Contact> contacts)
        {
                lstChatters.Invoke((MethodInvoker)delegate 
                        {
                            lstChatters.Items.Clear();

                            foreach (var contact in contacts)
                            {
                                lstChatters.Items.Add(contact);

                                lstChatters.SelectedIndex = 0;
                            }

                            txtMessage.Select();
                        });
        }

        public void DisplayMessage(string message)
        {


            this.txtChatBox.Invoke(
                (MethodInvoker)delegate
                    {
                        txtChatBox.AppendText(message);
                        txtChatBox.AppendText(Environment.NewLine);
                    });
        }

        public void DisableClient()
        {
            Invoke(
                (MethodInvoker)delegate
                    {
                        logInToolStripMenuItem.Enabled = true;
                        logOutToolStripMenuItem.Enabled = false;
                        txtMessage.Enabled = false;
                        btnSend.Enabled = false;
                        lstChatters.Items.Clear();
                        txtMessage.Clear();
                    });
        }

        public void EnableClient()
        {
            Invoke(
               (MethodInvoker)delegate
               {
                   logInToolStripMenuItem.Enabled = false;
                   logOutToolStripMenuItem.Enabled = true;
                   txtMessage.Enabled = true;
                   btnSend.Enabled = true;
               });
        }

        public void ShowException(Exception ex)
        {
           ErrorHandling.ShowExceptionThreadSafe(this, ex);
        }
    }
}
