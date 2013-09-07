// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsMessage.cs" company="">
//   
// </copyright>
// <summary>
//   Сообщение со списком контактов
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// Сообщение со списком контактов
    /// </summary>
    public class ContactsMessage : Message
    {
        public ContactsMessage(List<Contact> contacts)
        {
            this.Contacts = contacts;
        }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public List<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets the type.
        /// Тип сообщения
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.Contacts;
            }
        }

        public override bool Equals(object obj)
        {
            var contactsMessage = obj as ContactsMessage;

            if (contactsMessage != null)
            {
                if (contactsMessage.Contacts == null && this.Contacts == null)
                {
                    return true;
                }

                if (contactsMessage.Contacts == null ^ this.Contacts == null)
                {
                    return false;
                }

                if (contactsMessage.Contacts.Count == this.Contacts.Count)
                {
                    for (var i = 0; i < this.Contacts.Count; i++)
                    {
                        if (!contactsMessage.Contacts[i].Equals(this.Contacts[i]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}