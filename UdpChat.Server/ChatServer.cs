using System;

namespace UdpChat.Server
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    using Message = UdpChat.Common.Messages.Message;

    public class ChatServer
    {
        private UdpClient _udpServer;

        private List<Contact> _contacts = new List<Contact>();

        private string _serverName;

        private IServerView _view;

        public ChatServer(int port, string serverName, IServerView view)
        {
            _udpServer = new UdpClient(port);

            _udpServer.BeginReceive(OnReceive, null);

            _serverName = serverName;

            _view = view;
        }

        public void Close()
        {
            _udpServer.Close();
        }

        private void WriteLog(string log)
        {
            
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            if (_udpServer.Client == null)
            {
                return;
            }

            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            var bytes = _udpServer.EndReceive(asyncResult, ref endPoint);

            var message = new Message(bytes);

            switch (message.Type)
            {
                case MessageType.Login:
                    _contacts.Add(new Contact { User = message.User, EndPoint = endPoint });

                    foreach (var contact in _contacts)
                    {
                        this.SendMessageAboutContracts(contact.EndPoint);
                    }

                    _view.WriteLog(string.Format("\'{0}\' has joined the chat.", message.User));
                    break;
                case MessageType.Logout:
                    for (var i = 0; i < this._contacts.Count; i++)
                    {
                        if (_contacts[i].EndPoint.Equals(endPoint))
                        {
                            _contacts.RemoveAt(i);

                            foreach (var contact in _contacts)
                            {
                                SendMessageAboutContracts(contact.EndPoint);
                            }

                            _view.WriteLog(string.Format("\'{0}\' has escaped the chat.", message.User));

                            break;
                        }
                    }
                    break;
                case MessageType.ChatMessage:
                    foreach (var contact in _contacts)
                    {
                        if (contact.User.Equals(message.User))
                        {
                            this.SendMessage(message, contact.EndPoint);

                            _view.WriteLog(string.Format("\'{0}\' has written a message \'{1}\'.", message.User, message.Content));
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpServer.BeginReceive(this.OnReceive, null);
        }

        private void SendMessageAboutContracts(IPEndPoint endPoint)
        {
            var message = new Message { Type = MessageType.Contacts, User = "Server" };

            if (_contacts.Count > 0)
            {
                foreach (var contract in _contacts)
                {
                    message.Content += contract.User + ",";
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
