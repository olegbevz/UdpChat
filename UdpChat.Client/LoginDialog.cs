// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginDialog.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LoginDialog type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Client
{
    using System;
    using System.Windows.Forms;

    using UdpChat.Common;

    public partial class LoginDialog : Form
    {
        public LoginDialog()
        {
            InitializeComponent();

            this.LoadSettings();
        }

        public string User
        {
            get
            {
                return txtName.Text;
            }
        }

        public string ServerIP 
        { 
            get
            {
                return txtServerIP.Text;
            }
        }

        public string ServerPort
        {
            get
            {
                return txtPort.Text;
            }
        }

        private void LoadSettings()
        {
            this.txtName.Text = Properties.Settings.Default.UserName;
            this.txtServerIP.Text = Properties.Settings.Default.ServerIP;
            this.txtPort.Text = Properties.Settings.Default.ServerPort;
            this.checkBox1.Checked = Properties.Settings.Default.ServerOnThisComputer;
        }

        private void OnLoad(object sender, EventArgs e)
        {

        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            SaveSetting();

            Close();
        }

        private void SaveSetting()
        {
            Properties.Settings.Default.UserName = this.txtName.Text;
            Properties.Settings.Default.ServerIP = this.txtServerIP.Text;
            Properties.Settings.Default.ServerPort = this.txtPort.Text;
            Properties.Settings.Default.ServerOnThisComputer = this.checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            int port;

            btnOK.Enabled = !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(ServerIP)
                            && int.TryParse(ServerPort, out port);
        }

        private void OnCheckBoxChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ErrorHandling.ShowException(this, ex);
            }
        }

       
    }
}
