// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogoutMessage.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение о выходе пользователя из чата
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    /// <summary>
    /// Сообщение о выходе пользователя из чата
    /// </summary>
    public class LogoutMessage : Message
    {
        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.Logout;
            }
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is LogoutMessage;
        }
    }
}