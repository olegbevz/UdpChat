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
        private readonly UdpClient udpClient;

        private List<Contact> contacts = new List<Contact>();

        private IClientView clientView;

        private string userName;

        private string serverName;

        public ChatClient(IClientView view)
        {
            udpClient = new UdpClient();            
            this.clientView = view;
        }

        public bool IsInChat { get; private set; }

        public void Start(IPEndPoint serverEndPoint)
        {
            udpClient.Connect(serverEndPoint);

            udpClient.BeginReceive(this.OnReceive, null);
        }

        public void Close()
        {
            udpClient.Close();
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
            userName = user;

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
        public void SendChatMessage(string content)
        {
            var message = new ChatMessage(userName, content);

            SendMessage(message);
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

            udpClient.BeginSend(bytes, bytes.Length, null, null);
        }

        #endregion

        #region - Методы по обработке входящих сообщений -

        public void OnReceive(IAsyncResult asyncResult)
        {
            try
            {
                if (udpClient.Client == null)
                {
                    return;
                }

                lock (udpClient)
                {
                    var endPoint = new IPEndPoint(IPAddress.Any, 0);

                    var bytes = udpClient.EndReceive(asyncResult, ref endPoint);

                    var message = Message.FromBytes(bytes);

                    switch (message.Type)
                    {
                        case MessageType.ChatMessage:
                            OnChatMessage(message as ChatMessage);
                            break;
                        case MessageType.Contacts:
                            OnContactsMessage(message as ContactsMessage);
                            break;
                        case MessageType.LoginAccepted:
                            OnLoginAcceptedMessage(message as LoginAcceptedMessage);
                            break;
                        case MessageType.LogoutAccepted:
                            OnLogoutAcceptedMessage(message as LogoutAcceptedMessage);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unknown message was received.");
                    }

                    udpClient.BeginReceive(this.OnReceive, null);
                }
            }
            catch (Exception ex)
            {
                clientView.ShowException(ex);
            }
        }

        private void OnLoginAcceptedMessage(LoginAcceptedMessage message)
        {
            IsInChat = true;
            this.serverName = message.ServerName;
            clientView.EnableClient();
            clientView.DisplayContacts(message.Contacts);

            DiplayMessage(message.ServerName, message.WelcomeMessage);
        }

        private void OnLogoutAcceptedMessage(LogoutAcceptedMessage message)
        {
            IsInChat = false;
            clientView.DisableClient();

            DiplayMessage(this.serverName, message.GoodByeMessage);
        }

        /// <summary>
        /// Обработчик сообщения со списком контактов
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void OnContactsMessage(ContactsMessage message)
        {
            contacts = message.Contacts;

            clientView.DisplayContacts(contacts);
        }

        /// <summary>
        /// Обработчик сообщения в чате
        /// </summary>
        /// <param name="chatMessage">
        /// The chat message
        /// </param>
        private void OnChatMessage(ChatMessage chatMessage)
        {
            DiplayMessage(chatMessage.Sender, chatMessage.Content);
        }

        private void DiplayMessage(string sender, string message)
        {
            var content = string.Format(
               "{0} {1}: {2}",
               DateTime.Now,
               sender,
               message);

            clientView.DisplayMessage(content);
        }

        #endregion
    }
}
