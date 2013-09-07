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

            var ip = this.GetLocalIP();

            if (ip != null)
            {
                _view.WriteLog(string.Format("Server \'{0}\' was started at {1}:{2}.", serverName, ip, port));
            }
        }

        public void Close()
        {
            _udpServer.Close();

            _view.WriteLog(string.Format("Server \'{0}\' was closed.", _serverName));
        }

        private IPAddress GetLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return null;
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            try
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
                        this.OnLoginMessage(message, endPoint);
                        break;
                    case MessageType.Logout:
                        this.OnLogoutMessage(endPoint, message);
                        break;
                    case MessageType.ChatMessage:
                        var chatMessage = message as ChatMessage;
                        this.OnChatMessage(chatMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _udpServer.BeginReceive(this.OnReceive, null);
            }
            catch (Exception ex)
            {
                _view.ShowException(ex);
            }
        }

        private void OnChatMessage(ChatMessage message)
        {
            var receiver = this.GetContactByEndPoint(message.Receiver.EndPoint);

            this.SendMessage(message, receiver.EndPoint);

            this._view.WriteLog(string.Format("\'{0}\' has written to \'{1}\' a message \'{2}\'.", message.Sender, receiver.Name, message.Content));
        }

        private void OnLogoutMessage(IPEndPoint endPoint, Message message)
        {
            var logoutContact = this.GetContactByEndPoint(endPoint);

            if (logoutContact != null)
            {
                this._contacts.Remove(logoutContact);

                // Отсылаем подтверждение об удалении пользователя на сервере
                this.SendMessage(message, endPoint);

                this.BroadcastContactsMessage();

                _view.WriteLog(string.Format("\'{0}\' has escaped the chat.", logoutContact.Name));
            }
        }

        private void OnLoginMessage(Message message, IPEndPoint endPoint)
        {
            var loginMessage = message as LoginMessage;

            var contact = new Contact(loginMessage.Sender, endPoint);

            this._contacts.Add(contact);

            // Отсылаем подтверждение о добавлении пользователя на сервере
            this.SendMessage(loginMessage, endPoint);

            this.BroadcastContactsMessage();

            this.SendWelcomeMessage(endPoint, contact);

            _view.WriteLog(string.Format("\'{0}\' has joined the chat.", loginMessage.Sender));
        }

        private void SendWelcomeMessage(IPEndPoint endPoint, Contact contact)
        {
            this.SendMessage(
                new ChatMessage(
                    this._serverName,
                    contact,
                    string.Format("Hello, {0}! Welcome to chat on server \'{1}\'!", contact.Name, this._serverName)),
                endPoint);
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
