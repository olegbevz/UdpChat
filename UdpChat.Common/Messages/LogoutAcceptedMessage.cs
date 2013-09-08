namespace UdpChat.Common.Messages
{
    public class LogoutAcceptedMessage : Message
    {
        public LogoutAcceptedMessage(string serverName, string goodByeMessage)
        {
            ServerName = serverName;

            GoodByeMessage = goodByeMessage;
        }

        /// <summary>
        /// Gets or sets the server name.
        /// Имя сервера
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the good bye message.
        /// Сообщение с прощальным содержанием
        /// </summary>
        public string GoodByeMessage { get; set; }

        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.LogoutAccepted;
            }
        }
    }
}