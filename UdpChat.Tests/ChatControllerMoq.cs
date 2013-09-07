namespace UdpChat.Tests
{
    using System;

    using UdpChat.Common;

    public delegate void LoginAction(string user);

    public delegate void LogoutAction();

    public delegate void SendMessageAction(string user, string message);

    public class ChatControllerMoq : IContract
    {
        public string[] ContactsToReturn { get; set; }

        public LoginAction LoginAction { get; set; }

        public LogoutAction LogoutAction { get; set; }

        public SendMessageAction SendMessageAction { get; set; }

        public string[] GetContacts()
        {
            return this.ContactsToReturn;
        }

        public void Login(string user)
        {
            LoginAction.Invoke(user);
        }

        public void Logout()
        {
            LogoutAction.Invoke();
        }

        public void SendChatMessage(string user, string message)
        {
            SendMessageAction.Invoke(user, message);
        }
    }
}