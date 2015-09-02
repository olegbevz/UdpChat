// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatServer.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ChatServer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Server
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    using Message = UdpChat.Common.Messages.Message;

    public class ChatServer
    {
        private UdpClient udpServer;

        /// <summary>
        /// Список контактов
        /// </summary>
        private readonly List<Contact> contacts = new List<Contact>();

        /// <summary>
        /// Имя сервера
        /// </summary>
        private string serverName;

        /// <summary>
        /// Окно сервера
        /// </summary>
        private readonly IServerView serverView;

        /// <summary>
        /// Список источников для вывода логов
        /// </summary>
        private readonly IEnumerable<ILogging> loggers;

        private readonly TimeSpan contactTimeout = TimeSpan.FromMinutes(1);

        public ChatServer(IServerView view, IEnumerable<ILogging> loggers)
        {
            this.serverView = view;

            this.loggers = loggers;
        }

        /// <summary>
        /// Запуск сервера
        /// </summary>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        public void Start(int port, string serverName)
        {
            this.serverName = serverName;

            WriteLog(string.Format(
                "Server \'{0}\' was started at {1}:{2}.",
                serverName,
                this.GetLocalIP() ?? IPAddress.None,
                port));

            udpServer = new UdpClient(port);

            udpServer.BeginReceive(OnReceive, null);
        }

        /// <summary>
        /// Закрытие сервера
        /// </summary>
        public void Close()
        {
            foreach (var contact in contacts)
            {
                SendLogoutAcceptedMessage(contact);
            }

            contacts.Clear();

            if (udpServer != null)
            {
                udpServer.Close();
            }            

            WriteLog(string.Format("Server \'{0}\' was closed.", serverName));
        }

        /// <summary>
        /// Проверка и удаление неактивных контактов
        /// </summary>
        public void DisconnectInactiveContacts()
        {
            var currentTime = DateTime.Now;

            var inactiveContacts = new List<Contact>();

            foreach (var contact in contacts)
            {
                if (currentTime - contact.LastActiveTime > contactTimeout)
                {
                    inactiveContacts.Add(contact);
                }
            }

            foreach (var inactiveContact in inactiveContacts)
            {
                var message =
                    string.Format(
                        "{0}, you were inactive for too long in our chat. Now I will disconnect you.",
                        inactiveContact.Name);

                SendChatMessage(inactiveContact, message);

                SendLogoutAcceptedMessage(inactiveContact);

                contacts.Remove(inactiveContact);
            }

            if (inactiveContacts.Count > 0)
            {
                BroadcastContactsMessage();
            }
        }

        #region - Методы по обработке получаемых сообщений -

        /// <summary>
        /// обработчик получамых сообщений
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        private void OnReceive(IAsyncResult asyncResult)
        {
            try
            {
                lock (udpServer)
                {
                    var endPoint = new IPEndPoint(IPAddress.Any, 0);

                    if (udpServer.Client == null)
                    {
                        return;
                    }

                    var bytes = udpServer.EndReceive(asyncResult, ref endPoint);

                    var message = Message.FromBytes(bytes);

                    switch (message.Type)
                    {
                        case MessageType.Login:
                            OnLoginMessage(message as LoginMessage, endPoint);
                            break;
                        case MessageType.Logout:
                            OnLogoutMessage(message, endPoint);
                            break;
                        case MessageType.ChatMessage:
                            OnChatMessage(message as ChatMessage, endPoint);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    udpServer.BeginReceive(this.OnReceive, null);
                }
            }
            catch (Exception ex)
            {
                serverView.ShowException(ex);
            }
        }

        /// <summary>
        /// Обработчик сообщения в чате
        /// </summary>
        /// <param name="message">
        /// Сообщение в чате
        /// </param>
        /// <param name="endPoint">
        /// Адрес отправителя сообщения 
        /// </param>
        private void OnChatMessage(ChatMessage message, IPEndPoint endPoint)
        {
            var sender = this.GetContactByEndPoint(endPoint);

            sender.LastActiveTime = DateTime.Now;

            BroadcastChatMessage(message);

            WriteLog(string.Format("\'{0}\' has written a message \'{1}\'.", message.Sender, message.Content));
        }

        /// <summary>
        /// Обработчик сообщения о выходе пользоватя
        /// </summary>
        /// <param name="message">
        /// Сообщение о выходе из чата
        /// </param>
        /// <param name="endPoint">
        /// Адрес отправителя сообещния
        /// </param>
        private void OnLogoutMessage(Message message, IPEndPoint endPoint)
        {
            var logoutContact = this.GetContactByEndPoint(endPoint);

            if (logoutContact != null)
            {
                contacts.Remove(logoutContact);

                // Отсылаем подтверждение об удалении пользователя на сервере
                this.SendLogoutAcceptedMessage(logoutContact);

                // Отсылаем обновленный список контактов всем пользователям
                BroadcastContactsMessage();

                WriteLog(string.Format("\'{0}\' has escaped the chat.", logoutContact.Name));
            }
            else
            {
                SendChatMessage(new Contact(string.Empty, endPoint), "There is no contact with this endpoint.");
            }
        }

        /// <summary>
        /// Обработчик сообщения входа пользователя в чат
        /// </summary>
        /// <param name="message">
        /// Сообщение о входе пользователя в чат
        /// </param>
        /// <param name="endPoint">
        /// Адрес отправителя сообщения
        /// </param>
        private void OnLoginMessage(LoginMessage message, IPEndPoint endPoint)
        {
            var contact = new Contact(message.Sender, endPoint) { LastActiveTime = DateTime.Now };
            
            if (GetContactByEndPoint(endPoint) != null)
            {
                SendChatMessage(contact, "Contact with the same endpoint already exists.");
            }
            else
            {
                contacts.Add(contact);

                // Отсылаем подтверждение о добавлении пользователя на сервере
                SendLoginAcceptedMessage(contact);

                // Отсылаем обновленный список контактов всем пользователям
                BroadcastContactsMessage(contact);

                WriteLog(string.Format("\'{0}\' has joined the chat.", message.Sender));
            }
        }

        #endregion

        #region - Методы по отправке сообщений пользователям -

        /// <summary>
        /// Отправить сообщение с прощанием пользователю
        /// </summary>
        /// <param name="contact">
        /// Получатель сообщения
        /// </param>
        private void SendLogoutAcceptedMessage(Contact contact)
        {
            var goodByeMessage = string.Format("Goodbye, {0}!", contact.Name);

            SendMessage(new LogoutAcceptedMessage(goodByeMessage), contact.EndPoint);
        }

        /// <summary>
        /// Отправить сообщение с приветствованием пользователю 
        /// </summary>
        /// <param name="contact">
        /// Получатель сообщения
        /// </param>
        private void SendLoginAcceptedMessage(Contact contact)
        {
            var welcomeMessage = string.Format(
                "Hello, {0}! Welcome to chat on server \'{1}\'!", contact.Name, serverName);

            this.SendMessage(
                new LoginAcceptedMessage(serverName, welcomeMessage, contacts), 
                contact.EndPoint);
        }

        private void BroadcastChatMessage(ChatMessage chatMessage)
        {
            foreach (var contact in contacts)
            {
                this.SendMessage(chatMessage, contact.EndPoint);
            }
        }

        /// <summary>
        /// Отправить сообщение пользователю в чат от сервера
        /// </summary>
        /// <param name="message">
        /// Текст сообщения
        /// </param>
        /// <param name="contact">
        /// Получатель сообщения
        /// </param>
        private void SendChatMessage(Contact contact, string message)
        {
            SendMessage(
               new ChatMessage(
                   serverName,
                   message),
               contact.EndPoint);
        }

        /// <summary>
        /// Отправить всем пользователям чата сообщение со списком контактов
        /// </summary>
        private void BroadcastContactsMessage()
        {
            foreach (var contact in contacts)
            {
                this.SendContractsMessage(contact.EndPoint);
            }
        }

        /// <summary>
        /// Отправить всем пользователям чата сообщение со списком контактов
        /// </summary>
        private void BroadcastContactsMessage(Contact exceptedContact)
        {
            foreach (var contact in contacts)
            {
                if (!contact.Equals(exceptedContact))
                {
                    this.SendContractsMessage(contact.EndPoint);
                }
            }
        }

        /// <summary>
        /// Отправить пользователю сообщение со списком активных пользователей чата
        /// </summary>
        /// <param name="endPoint">
        /// Адрес получателя сообщения
        /// </param>
        private void SendContractsMessage(IPEndPoint endPoint)
        {
            this.SendMessage(new ContactsMessage(contacts), endPoint);
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">
        /// Модель сообщения
        /// </param>
        /// <param name="endPoint">
        /// адрес получателя сообщения
        /// </param>
        private void SendMessage(Message message, IPEndPoint endPoint)
        {
            var bytes = message.ToBytes();

            udpServer.Send(bytes, bytes.Length, endPoint);
        }

        #endregion

        /// <summary>
        /// Получение модели контакта по его адресу
        /// </summary>
        /// <param name="endPoint">
        /// The end point.
        /// </param>
        /// <returns>
        /// The <see cref="Contact"/>.
        /// </returns>
        private Contact GetContactByEndPoint(IPEndPoint endPoint)
        {
            foreach (var contact in contacts)
            {
                if (contact.EndPoint.Equals(endPoint))
                {
                    return contact;
                }
            }

            return null;
        }

        /// <summary>
        /// Вывод логов
        /// </summary>
        /// <param name="log">
        /// Текст лога
        /// </param>
        private void WriteLog(string log)
        {
            foreach (var logging in loggers)
            {
                logging.WriteLog(log);
            }
        }

        /// <summary>
        /// Получение внешнего ip сервера
        /// </summary>
        /// <returns>
        /// IP сервера
        /// </returns>
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
    }
}
