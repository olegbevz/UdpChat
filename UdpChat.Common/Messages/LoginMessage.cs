// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginMessage.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение о входе пользователя в чат
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    /// <summary>
    /// Сообщение о входе пользователя в чат
    /// </summary>
    public class LoginMessage : Message
    {
        public LoginMessage(string sender)
        {
            this.Sender = sender;
        }

        public LoginMessage()
        {
        }

        /// <summary>
        /// Gets or sets the sender.
        /// Отправитель сообщения
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.Login;
            }
        }

        public override bool Equals(object obj)
        {
            var loginMessage = obj as LoginMessage;

            if (loginMessage != null)
            {
                return loginMessage.Sender == this.Sender;
            }

            return false;
        }
    }
}