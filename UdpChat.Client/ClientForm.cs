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
        private ChatClient _chatClient;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void OnSendButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (lstChatters.SelectedItem != null)
                {
                    var contact = lstChatters.SelectedItem as Contact;

                    _chatClient.SendChatMessage(contact, txtMessage.Text);

                    txtMessage.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            ShowLoginDialog();

            txtMessage.Select();

            base.OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _chatClient.Logout();

            _chatClient.Close();

            base.OnClosed(e);
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            this.ShowLoginDialog();
        }

        private void ShowLoginDialog()
        {
            try
            {
                var dialog = new LoginDialog();

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var endPoint = new IPEndPoint(IPAddress.Parse(dialog.ServerIP), int.Parse(dialog.ServerPort));

                    this._chatClient = new ChatClient(endPoint, this);

                    this._chatClient.Login(dialog.User);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        txtChatBox.Clear();
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

        private void OnLogoutButtonClick(object sender, EventArgs e)
        {
            try
            {
                _chatClient.Logout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
