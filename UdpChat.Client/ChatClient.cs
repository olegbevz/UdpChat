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
    using UdpChat.Common.Messages;

    public interface IClientView
    {
        void DisplayContacts(IEnumerable<Contact> contacts);

        void DisplayMessage(string message);
    }

    public class ChatClient //: IContract
    {
        private UdpClient _udpClient;

        private IPEndPoint _serverEndPoint;

        private List<Contact> _contacts = new List<Contact>();

        private IClientView _view;

        private string _userName;

        public ChatClient(IPEndPoint endPoint, IClientView view)
        {
            _udpClient = new UdpClient();

            _udpClient.Connect(endPoint);

            _view = view;

            this.StartReceiving();
        }

        public IEnumerable<Contact> Contacts
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

        public void Login(string user)
        {
            _userName = user;

            SendMessage(new LoginMessage(user));
        }

        public void Logout()
        {
            SendMessage(new LogoutMessage());
        }

        public void SendChatMessage(Contact receiver, string content)
        {
            var message = new ChatMessage(_userName, receiver, content);

            SendMessage(message);
        }

        private void SendMessage(Message message)
        {
            var bytes = message.ToBytes();

            _udpClient.Send(bytes, bytes.Length);
        }

        private Contact GetContactByEndPoint(IPEndPoint endPoint)
        {
            foreach (var contact in _contacts)
            {
                if (contact.EndPoint.Equals(endPoint))
                {
                    return contact;
                }
            }

            return null;
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

            var message = Message.FromBytes(bytes);

            switch (message.Type)
            {
                case MessageType.ChatMessage:
                    var chatMessage = message as ChatMessage;
                    _view.DisplayMessage(chatMessage.Content);
                    break;
                case MessageType.Contacts:
                    var contactsMessage = message as ContactsMessage;
                    _contacts = contactsMessage.Contacts;
                    _view.DisplayContacts(_contacts);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpClient.BeginReceive(this.OnReceive, null);
        }
    }
}
