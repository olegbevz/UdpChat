using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UdpChat.Client
{
    public partial class LoginDialog : Form
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        public string User { get; set; }

        public string ServerIP { get; set; }

        public string ServerPort { get; set; }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            User = txtName.Text;
            ServerIP = txtServerIP.Text;
            ServerPort = txtPort.Text;

            this.Close();
        }

        private void OnUserNameTextChanged(object sender, EventArgs e)
        {
            User = txtName.Text;
        }

        private void OnServerIpTextChanged(object sender, EventArgs e)
        {
            ServerIP = txtServerIP.Text;
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        private void OnCheckBoxChanged(object sender, EventArgs e)
        {
            txtServerIP.Enabled = !checkBox1.Checked;

            if (checkBox1.Checked)
            {
                txtServerIP.Tag = txtServerIP.Text;

                txtServerIP.Text = "127.0.0.1";
            }
            else
            {
                txtServerIP.Text = txtServerIP.Tag as string ?? string.Empty;
            }
        }
    }
}
