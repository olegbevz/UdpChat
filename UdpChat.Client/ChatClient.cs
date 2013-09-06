// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatClient.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MessageType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    using UdpChat.Common;

    public interface IClientView
    {
        void DisplayContacts(IEnumerable<string> contacts);

        void DisplayMessage(string message);
    }

    public class ChatClient : IContract
    {
        private UdpClient _udpClient;

        private IPEndPoint _serverEndPoint;

        private List<string> _contacts = new List<string>();

        private IClientView _view;

        private string _userName;

        public ChatClient(IPEndPoint endPoint, IClientView view)
        {
            _udpClient = new UdpClient();

            _udpClient.Connect(endPoint);

            _view = view;

            this.StartReceiving();
        }

        public IEnumerable<string> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public void Close()
        {
            _udpClient.Close();
        }

        public void GetContacts(EventHandler callback)
        {
            var message = new Message { Type = MessageType.Contacts };

            SendMessage(message);
        }

        public string[] GetContacts()
        {
            return null;
        }

        public void Login(string user)
        {
            _userName = user;

            var message = new Message { Type = MessageType.Login, User = user, Content = user };

            SendMessage(message);
        }

        public void Logout()
        {
            var message = new Message { Type = MessageType.Logout, Content = _userName };

            SendMessage(message);
        }

        public void SendMessage(string user, string content)
        {
            var message = new Message { Type = MessageType.ChatMessage, User = user, Content = content };

            SendMessage(message);
        }

        private void SendMessage(Message message)
        {
            var bytes = message.ToBytes();

            _udpClient.Send(bytes, bytes.Length);
        }

        private void StartReceiving()
        {
            _udpClient.BeginReceive(this.OnReceive, null);
        }

        public void OnReceive(IAsyncResult asyncResult)
        {
            if (_udpClient.Client == null)
            {
                return;
            }

            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            var bytes = _udpClient.EndReceive(asyncResult, ref endPoint);

            var message = new Message(bytes);

            switch (message.Type)
            {
                case MessageType.ChatMessage:
                    _view.DisplayMessage(message.Content);
                    break;
                case MessageType.Contacts:
                    _contacts = new List<string>(message.Content == null ? new string[0] : message.Content.Split(','));
                    _view.DisplayContacts(_contacts);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpClient.BeginReceive(this.OnReceive, null);
        }
    }
}
