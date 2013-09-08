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
    using System;

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
            var jsonString = string.Empty;

            try
            {
                jsonString = Cryptography.Decrypt(bytes);

                    return JsonConvert.DeserializeObject<Message>(
                      jsonString,
                      new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                    "Error while reading message from JSON string \"{0}\".", jsonString), ex);
            }
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

            //var jsonStringForCheck = Cryptography.Decrypt(bytes);

            //if (json != jsonStringForCheck)
            //{
            //    throw new Exception();
            //}

            //var messageForCheck = JsonConvert.DeserializeObject<Message>(
            //      jsonStringForCheck,
            //      new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            //if (!messageForCheck.Equals(this))
            //{
            //    throw new Exception();
            //}

            return bytes;
        }
    }
}