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

        private void OnReceive(IAsyncResult asyncResult)
        {
            if (_udpServer.Client == null)
            {
                return;
            }

            var endPoint = new IPEndPoint(IPAddress.Any, 0);

            var bytes = _udpServer.EndReceive(asyncResult, ref endPoint);

            var message = Message.FromBytes(bytes);

            switch (message.Type)
            {
                case MessageType.Login:
                    var loginMessage = message as LoginMessage;

                    _contacts.Add(new Contact(loginMessage.Sender, endPoint));

                   BroadcastContactsMessage();

                    _view.WriteLog(string.Format("\'{0}\' has joined the chat.", loginMessage.Sender));
                    break;
                case MessageType.Logout:
                    var logoutContact = GetContactByEndPoint(endPoint);

                    if (logoutContact != null)
                    {
                        _contacts.Remove(logoutContact);

                        this.BroadcastContactsMessage();

                        _view.WriteLog(string.Format("\'{0}\' has escaped the chat.", logoutContact.Name));
                    }
                    break;
                case MessageType.ChatMessage:
                    var chatMessage = message as ChatMessage;
                    
                    var receiver = GetContactByEndPoint(chatMessage.Receiver.EndPoint);

                    SendMessage(message, receiver.EndPoint);

                    _view.WriteLog(string.Format("\'{0}\' has written a message \'{1}\'.", receiver.Name, chatMessage.Content));
                
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _udpServer.BeginReceive(this.OnReceive, null);
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

        private void BroadcastContactsMessage()
        {
            foreach (var contact in _contacts)
            {
                this.SendContractsMessage(contact.EndPoint);
            }
        }

        private void SendContractsMessage(IPEndPoint endPoint)
        {
            this.SendMessage(new ContactsMessage(_contacts), endPoint);
        }

        private void SendMessage(Message message, IPEndPoint endPoint)
        {
            var bytes = message.ToBytes();

            _udpServer.Send(bytes, bytes.Length, endPoint);
        }
    }
}
