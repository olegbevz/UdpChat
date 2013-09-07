﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UdpChat.Client
{
    using System.Net;

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

        private void OnLoad(object sender, EventArgs e)
        {
            txtMessage.Select();
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            try
            {
                var dialog = new LoginDialog();

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var endPoint = new IPEndPoint(IPAddress.Parse(dialog.ServerIP), int.Parse(dialog.ServerPort));

                    _chatClient = new ChatClient(endPoint, this);

                    var user = dialog.User;

                    _chatClient.Login(user);

                    logInToolStripMenuItem.Enabled = false;
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
                            }
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

        private void OnLogoutButtonClick(object sender, EventArgs e)
        {
            try
            {
                _chatClient.Logout();

                logInToolStripMenuItem.Enabled = true;

                logOutToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
