namespace UdpChat.Common
{
    using System;
    using System.Net;

    using Newtonsoft.Json;

    /// <summary>
    /// Данные о пользователе чата
    /// </summary>
    public class Contact
    {
        public Contact(string name, IPEndPoint endPoint)
        {
            Name = name;
            IP = endPoint.Address.ToString();
            Port = endPoint.Port;
        }

        public Contact()
        {
            
        }

        /// <summary>
        /// Gets or sets the name.
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the end point.
        /// Адрес пользователя
        /// </summary>
        [JsonIgnore]
        public IPEndPoint EndPoint 
        { 
            get
            {
                return new IPEndPoint(IPAddress.Parse(IP), Port);
            }
        }

        public string IP { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the last active time.
        /// Время последней активности клиента
        /// </summary>
        [JsonIgnore]
        public DateTime LastActiveTime { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var contact = obj as Contact;
            if (contact != null)
            {
                return contact.Name == Name && contact.EndPoint.Equals(EndPoint);
            }

            return false;
        }
    }
}