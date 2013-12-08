namespace UdpChat.Common.Messages
{
    public class LogoutAcceptedMessage : Message
    {
        public LogoutAcceptedMessage(string goodByeMessage)
        {
            GoodByeMessage = goodByeMessage;
        }

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