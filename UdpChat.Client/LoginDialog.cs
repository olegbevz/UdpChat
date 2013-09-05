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

        public long ServerPort { get; set; }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
        }

        private void OnUserNameTextChanged(object sender, EventArgs e)
        {
        }

        private void OnServerIpTextChanged(object sender, EventArgs e)
        {
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }
    }
}
