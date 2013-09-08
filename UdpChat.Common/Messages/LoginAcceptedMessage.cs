namespace UdpChat.Common.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// The login accepted message.
    /// Сообщение с подтверждением входа пользователя в чат
    /// </summary>
    public class LoginAcceptedMessage : Message
    {
        public LoginAcceptedMessage(string serverName, string welcomeMessage, List<Contact> contacts)
        {
            this.ServerName = serverName;

            this.WelcomeMessage = welcomeMessage;

            this.Contacts = contacts;
        }

        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.LoginAccepted;
            }
        }

        /// <summary>
        /// Gets or sets the server name.
        /// Имя сервера
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the welcome message.
        /// Текст приветствия
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public List<Contact> Contacts { get; set; }
    }
}