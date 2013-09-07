// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsMessageTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ContactsMessageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests.MessageTests
{
    using System.Collections.Generic;
    using System.Net;

    using NUnit.Framework;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    [TestFixture]
    public class ContactsMessageTests : MessageTests
    {
        [TestCase]
        public void ContactsMessageWithEmptyContactsTest()
        {
            var contactsMessage = new ContactsMessage(new List<Contact>());

            this.AssertMessage(contactsMessage);
        }

        [TestCase]
        public void ContactsMessageWithNullContactsTest()
        {
            var contactsMessage = new ContactsMessage(null);

            this.AssertMessage(contactsMessage);
        }

        [TestCase]
        public void ContactsMessageWithSeveralContactsTest()
        {
            var contactsMessage =
                new ContactsMessage(
                    new List<Contact>
                        {
                            new Contact("John", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 80)),
                            new Contact("Harry", new IPEndPoint(IPAddress.Parse("192.168.0.1"), 88)),
                            new Contact("Marry", new IPEndPoint(IPAddress.Parse("192.168.0.2"), 88))
                        });

            this.AssertMessage(contactsMessage);
        }

        [TestCase]
        public void ContactsMessageWithTooManyContactsTest()
        {
            var contact = new Contact("John", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 80));
            var contacts = new List<Contact>();

            for (var i = 0; i < 9999; i++)
            {
                contacts.Add(contact);
            }

            var contactsMessage = new ContactsMessage(contacts);
            this.AssertMessage(contactsMessage);
        }
    }
}