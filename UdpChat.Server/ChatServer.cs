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
        private readonly UdpClient _udpServer;

        private List<Contact> _contacts = new List<Contact>();

        private readonly string _serverName;

        private readonly IServerView _view;

        private readonly IEnumerable<ILogging> _loggers; 

        public ChatServer(int port, string serverName, IServerView view, IEnumerable<ILogging> loggers)
        {
            _serverName = serverName;

            _view = view;

            _loggers = loggers;

            WriteLog(string.Format(
                "Server \'{0}\' was started at {1}:{2}.", 
                serverName, 
                this.GetLocalIP() ?? IPAddress.None, 
                port));

            _udpServer = new UdpClient(port);

            _udpServer.BeginReceive(OnReceive, null);
        }

        public void Close()
        {
            _udpServer.Close();

            WriteLog(string.Format("Server \'{0}\' was closed.", _serverName));
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
                        this.OnLoginMessage(message as LoginMessage, endPoint);
                        break;
                    case MessageType.Logout:
                        this.OnLogoutMessage(endPoint, message);
                        break;
                    case MessageType.ChatMessage:
                        this.OnChatMessage(message as ChatMessage);
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

            WriteLog(string.Format("\'{0}\' has written to \'{1}\' a message \'{2}\'.", message.Sender, receiver.Name, message.Content));
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

                WriteLog(string.Format("\'{0}\' has escaped the chat.", logoutContact.Name));
            }
        }

        private void OnLoginMessage(LoginMessage message, IPEndPoint endPoint)
        {
            if (GetContactByEndPoint(endPoint) != null)
            {
                return;
            }

            var contact = new Contact(message.Sender, endPoint);

            this._contacts.Add(contact);

            // Отсылаем подтверждение о добавлении пользователя на сервере
            this.SendMessage(message, endPoint);

            // Отсылаем обновленный список контактов всем пользователям
            this.BroadcastContactsMessage();

            // Отсылаем сообщение зашетшему пользователю с приветствием
            this.SendWelcomeMessage(endPoint, contact);

            WriteLog(string.Format("\'{0}\' has joined the chat.", message.Sender));
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

        private void WriteLog(string log)
        {
            foreach (var logging in _loggers)
            {
                logging.WriteLog(log);
            }
        }
    }
}
