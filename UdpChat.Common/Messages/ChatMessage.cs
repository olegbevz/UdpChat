// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение в чате
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    /// <summary>
    /// Сообщение в чате
    /// </summary>
    public class ChatMessage : Message
    {
        public ChatMessage(string sender, string content)
        {
            this.Sender = sender;

            this.Content = content;
        }

        /// <summary>
        /// Gets or sets the sender.
        /// Отправитель сообщения
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// Содержание сообщения
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.ChatMessage;
            }
        }

        public override bool Equals(object obj)
        {
            var chatMessage = obj as ChatMessage;

            if (chatMessage != null)
            {
                return chatMessage.Sender == this.Sender && 
                    chatMessage.Content == this.Content;
            }

            return false;
        }
    }
}