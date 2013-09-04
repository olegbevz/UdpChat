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
    using System.Text;

    using UdpChat.Common;

    public class ChatClient : IContract
    {
        private UdpClient _udpClient;

        private IPEndPoint _serverEndPoint;

        private IAsyncResult _asyncResult;

        private List<string> _contacts = new List<string>();

        public EventHandler ContactsChanged;

        public EventHandler MessageWasSend;

        private EventHandler _getContactsCallback;

        public ChatClient(IPEndPoint endPoint)
        {
            _udpClient = new UdpClient();

            _udpClient.Connect(endPoint);

            this.StartReceiving();
        }

        public IEnumerable<string> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public void GetContacts(EventHandler callback)
        {
            _getContactsCallback = callback;

            var message = new Message { Type = MessageType.Contacts };

            SendMessage(message);
        }

        public string[] GetContacts()
        {
            throw new NotImplementedException();
        }

        public void Login(string user)
        {
            var message = new Message { Type = MessageType.Login, User = user, Content = user };

            SendMessage(message);
        }

        public void Logout()
        {
            var message = new Message { Type = MessageType.Logout };

            SendMessage(message);
        }

        public void SendMessage(string user, string content)
        {
            var message = new Message { Type = MessageType.Message, User = user, Content = content };

            SendMessage(message);

            var bytes = _udpClient.Receive(ref _serverEndPoint);

            var response = new Message(bytes);
        }

        private void SendMessage(Message message)
        {
            var bytes = message.ToBytes();

            _udpClient.Send(bytes, bytes.Length);
        }

        private void StartReceiving()
        {
            _asyncResult = _udpClient.BeginReceive(this.OnReceive, null);
        }

        public void OnReceive(IAsyncResult asyncResult)
        {
            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            var bytes = _udpClient.EndReceive(asyncResult, ref endPoint);

            var message = new Message(bytes);

            switch (message.Type)
            {
                case MessageType.Login:
                    _contacts.Add(message.User);
                    OnContactsChanged();
                    break;
                case MessageType.Logout:
                    _contacts.Remove(message.User);
                    OnContactsChanged();
                    break;
                case MessageType.Message:
                    //TODO: Show message
                    this.OnMessageSent();
                    break;
                case MessageType.Contacts:
                    _contacts = new List<string>(message.Content == null ? new string[0] : message.Content.Split(','));
                    _getContactsCallback.Invoke(this, EventArgs.Empty);
                    OnContactsChanged();
                    break;
                case MessageType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpClient.BeginReceive(this.OnReceive, null);
        }

        public void OnContactsChanged()
        {
            if (ContactsChanged != null)
            {
                ContactsChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnMessageSent()
        {
            if (MessageWasSend != null)
            {
                MessageWasSend.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
