// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Contact type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    using Newtonsoft.Json;

    /// <summary>
    /// Базовая модель сообщения передаваемая между сервером и клиентом
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        [JsonIgnore]
        public abstract MessageType Type { get; }

        /// <summary>
        /// Получение сообщения из массива байтов
        /// </summary>
        /// <param name="bytes">
        /// Массив байтов
        /// </param>
        /// <returns>
        /// Модель сообщения
        /// </returns>
        public static Message FromBytes(byte[] bytes)
        {
            var jsonString = Cryptography.Decrypt(bytes);

            return JsonConvert.DeserializeObject<Message>(
                jsonString, 
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        }

        /// <summary>
        /// Получение массива байтов из сообщения
        /// </summary>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public virtual byte[] ToBytes()
        {
            var json = JsonConvert.SerializeObject(
                this, 
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            var bytes = Cryptography.Encrypt(json);

            return bytes;
        }
    }
}