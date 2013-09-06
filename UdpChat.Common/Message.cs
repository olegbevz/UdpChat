namespace UdpChat.Common
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    public class Contact
    {
        public string Name { get; set; }

        public IPEndPoint EndPoint { get; set; }
    }

    public class LoginMessage : Message
    {
        public string Sender { get; set; }

        public override MessageType Type
        {
            get
            {
                return MessageType.Login;
            }
        }
    }

    public class LogoutMessage : Message
    {
        public string Sender { get; set; }

        public override MessageType Type
        {
            get
            {
                return MessageType.Logout;
            }
        }
    }

    public class ContactsMessage : Message
    {
        public List<Contact> Contacts { get; set; }

        public override MessageType Type
        {
            get
            {
                return MessageType.Contacts;
            }
        }
    }

    public class ChatMessage : Message
    {
        public Contact Sender { get; set; }

        public Contact Receiver { get; set; }

        public string Content { get; set; }

        public override MessageType Type
        {
            get
            {
                return MessageType.ChatMessage;
            }
        }
    }

    public abstract class Message
    {
        public static Message FromBytes()
        {
            return null;
        }

        protected Message()
        {
        }

        protected Message(byte[] data)
        {
            ////The first four bytes are for the Command
            //this.Type = (MessageType)BitConverter.ToInt32(data, 0);

            ////The next four store the length of the name
            //int nameLen = BitConverter.ToInt32(data, 4);

            ////The next four store the length of the message
            //int msgLen = BitConverter.ToInt32(data, 8);

            ////This check makes sure that strName has been passed in the array of bytes
            //if (nameLen > 0)
            //    this.User = Encoding.UTF8.GetString(data, 12, nameLen);
            //else
            //    this.User = null;

            ////This checks for a null message field
            //if (msgLen > 0)
            //    this.Content = Encoding.UTF8.GetString(data, 12 + nameLen, msgLen);
            //else
            //    this.Content = null;
        }

        public abstract MessageType Type { get; }

        public virtual byte[] ToBytes()
        {
            //List<byte> result = new List<byte>();

            ////First four are for the Command
            //result.AddRange(BitConverter.GetBytes((int)this.Type));

            ////Add the length of the name
            //if (this.User != null)
            //    result.AddRange(BitConverter.GetBytes(this.User.Length));
            //else
            //    result.AddRange(BitConverter.GetBytes(0));

            ////Length of the message
            //if (this.Content != null)
            //    result.AddRange(BitConverter.GetBytes(this.Content.Length));
            //else
            //    result.AddRange(BitConverter.GetBytes(0));

            ////Add the name
            //if (this.User != null)
            //    result.AddRange(Encoding.UTF8.GetBytes(this.User));

            ////And, lastly we add the message text to our array of bytes
            //if (this.Content != null)
            //    result.AddRange(Encoding.UTF8.GetBytes(this.Content));

            //return result.ToArray();
        }
    }
}