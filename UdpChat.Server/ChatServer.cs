using System;

namespace UdpChat.Server
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    using UdpChat.Common;

    using Message = UdpChat.Common.Message;

    public class Contact
    {
        public string User { get; set; }

        public IPEndPoint EndPoint { get; set; }
    }

    public class ChatController : IContract
    {
        public ChatController()
        {
            Contacts = new List<string>();
        }

        public List<string> Contacts { get; set; }

        public string[] GetContacts()
        {
            return Contacts.ToArray();
        }

        public void Login(string user)
        {
            Contacts.Add(user);
        }

        public void Logout()
        {
            Contacts.Remove("");
        }

        public void SendMessage(string user, string message)
        {
            throw new NotImplementedException();
        }
    }

    public class ChatServer
    {
        private UdpClient _udpServer;

        private IContract _chatController;

        private List<Contact> _contacts = new List<Contact>(); 

        public ChatServer(int port, IContract chatController)
        {
            _udpServer = new UdpClient(port);

            _udpServer.BeginReceive(OnReceive, null);

            _chatController = chatController;
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            var bytes = _udpServer.EndReceive(asyncResult, ref endPoint);

            var message = new Message(bytes);

            switch (message.Type)
            {
                case MessageType.Login:
                    _chatController.Login(message.User);

                    _contacts.Add(new Contact { User = message.User, EndPoint = endPoint });

                    foreach (var contact in _contacts)
                    {
                        this.SendMessage(message, contact.EndPoint);
                    }

                    break;
                case MessageType.Logout:
                    _chatController.Logout();

                    for (var i = 0; i < this._contacts.Count; i++)
                    {
                        if (_contacts[i].EndPoint.Equals(endPoint))
                        {
                            _contacts.RemoveAt(i);

                            foreach (var contact in _contacts)
                            {
                                this.SendMessage(message, contact.EndPoint);
                            }

                            break;
                        }
                    }
                   
                    break;
                case MessageType.Message:
                    foreach (var contact in _contacts)
                    {
                        this.SendMessage(message, contact.EndPoint);
                    }
                    break;
                case MessageType.Contacts:
                    this.SendMessageAboutContracts(endPoint);
                    break;
                case MessageType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpServer.BeginReceive(this.OnReceive, null);
        }

        private void SendMessageAboutContracts(IPEndPoint endPoint)
        {
            var message = new Message { Type = MessageType.Contacts, User = "Server" };

            var contracts = this._chatController.GetContacts();

            if (contracts.Length > 0)
            {
                foreach (var contract in contracts)
                {
                    message.Content += contract + ",";
                }

                message.Content = message.Content.Substring(0, message.Content.Length - 1);
            }

            this.SendMessage(message, endPoint);
        }

        private void SendMessage(Message message, IPEndPoint endPoint)
        {
            var bytes = message.ToBytes();

            _udpServer.Send(bytes, bytes.Length, endPoint);
        }
    }
}
