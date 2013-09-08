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

    public class ChatClient
    {
        private readonly UdpClient _udpClient;

        private List<Contact> _contacts = new List<Contact>();

        private IClientView _view;

        private string _userName;

        public ChatClient(IClientView view)
        {
            _udpClient = new UdpClient();

            _view = view;
        }

        public bool IsInChat { get; private set; }

        public void Start(IPEndPoint serverEndPoint)
        {
            _udpClient.Connect(serverEndPoint);

            _udpClient.BeginReceive(this.OnReceive, null);
        }

        public void Close()
        {
            _udpClient.Close();
        }

        #region - Методы по отправлению сообщений -

        /// <summary>
        /// Отправка сообщения о входе в чат на сервер
        /// </summary>
        /// <param name="user">
        /// Имя пользователя
        /// </param>
        public void Login(string user)
        {
            _userName = user;

            SendMessage(new LoginMessage(user));
        }

        /// <summary>
        /// Отправка сообщения о выходе пользователя из чата
        /// </summary>
        public void Logout()
        {
            SendMessage(new LogoutMessage());
        }

        /// <summary>
        /// Отправка сообщения в чат
        /// </summary>
        /// <param name="receiver">
        /// Получатель сообщения
        /// </param>
        /// <param name="content">
        /// Текст сообещния
        /// </param>
        public void SendChatMessage(Contact receiver, string content)
        {
            var message = new ChatMessage(_userName, receiver, content);

            SendMessage(message);

            this.OnChatMessage(message);
        }

        /// <summary>
        /// Отправка сообщения на сервере
        /// </summary>
        /// <param name="message">
        /// Модель сообщения
        /// </param>
        private void SendMessage(Message message)
        {
            var bytes = message.ToBytes();

            _udpClient.Send(bytes, bytes.Length);
        }

        #endregion

        #region - Методы по обработке входящих сообщений -

        public void OnReceive(IAsyncResult asyncResult)
        {
            try
            {
                if (_udpClient.Client == null)
                {
                    return;
                }

                lock (_udpClient)
                {
                    var endPoint = new IPEndPoint(IPAddress.Any, 0);

                    var bytes = _udpClient.EndReceive(asyncResult, ref endPoint);

                    var message = Message.FromBytes(bytes);

                    switch (message.Type)
                    {
                        case MessageType.ChatMessage:
                            OnChatMessage(message as ChatMessage);
                            break;
                        case MessageType.Contacts:
                            this.OnContactsMessage(message as ContactsMessage);
                            break;
                        case MessageType.Login:
                            IsInChat = true;
                            _view.EnableClient();
                            break;
                        case MessageType.Logout:
                            IsInChat = false;
                            _view.DisableClient();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _udpClient.BeginReceive(this.OnReceive, null);
                }
            }
            catch (Exception ex)
            {
                _view.ShowException(ex);
            }
        }

        /// <summary>
        /// Обработчик сообщения со списком контактов
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void OnContactsMessage(ContactsMessage message)
        {
            _contacts = message.Contacts;
            _view.DisplayContacts(_contacts);
        }

        /// <summary>
        /// Обработчик сообщения в чате
        /// </summary>
        /// <param name="chatMessage">
        /// The chat message
        /// </param>
        private void OnChatMessage(ChatMessage chatMessage)
        {
            var content = string.Format(
                "{0} {1}: {2}", 
                DateTime.Now, 
                chatMessage.Sender, 
                chatMessage.Content);

            _view.DisplayMessage(content);
        }

        #endregion
    }
}
