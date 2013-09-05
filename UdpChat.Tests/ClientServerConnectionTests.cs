// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientServerConnectionTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ClientServerConnectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests
{
    using System.Net;

    using NUnit.Framework;

    using UdpChat.Client;
    using UdpChat.Server;

    [TestFixture]
    public class ClientServerConnectionTests
    {
        [TestCase]
        public void GetContactsTests()
        {
            var controller = new ChatControllerMoq();

            var chatServer = new ChatServer(8888, controller);
            var chatClient = new ChatClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

            controller.ContactsToReturn = new[] { "User1", "User2", "User3" };
            chatClient.GetContacts((sender, args) =>
                {
                    Assert.AreEqual(controller.ContactsToReturn, chatClient.Contacts);
                });

            //controller.ContactsToReturn = new[] { "User1" };
            //chatClient.GetContacts(
            //    (sender, args) =>
            //        {
            //            Assert.AreEqual(controller.ContactsToReturn, chatClient.Contacts);
            //        });
            //Assert.AreEqual(controller.ContactsToReturn, chatClient.GetContacts());

            //controller.ContactsToReturn = new[] { "1" };
            //Assert.AreEqual(controller.ContactsToReturn, chatClient.GetContacts());

            //controller.ContactsToReturn = new string[0];
            //Assert.AreEqual(controller.ContactsToReturn, chatClient.GetContacts());
        }

        [TestCase]
        public void LoginTests()
        {
            var controller = new ChatControllerMoq();
            var chatServer = new ChatServer(8888, controller);
            var chatClient = new ChatClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

            var actionWasCalled = false;
            controller.LoginAction = user => { actionWasCalled = true; Assert.AreEqual(user, "Harry"); };
            chatClient.Login("Harry");

            //CollectionAssert.AreEqual(chatServer, );

            Assert.IsTrue(actionWasCalled);
        }

        [TestCase]
        public void SendMessageTests()
        {
            var controller = new ChatControllerMoq();
            var chatServer = new ChatServer(8008, controller);
            var chatClient = new ChatClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8008));

            var actionWasCalled = false;
            controller.SendMessageAction = (user, message) => { actionWasCalled = true; Assert.AreEqual(user, "John"); };
            chatClient.SendMessage("John", "Message body");

            Assert.IsTrue(actionWasCalled);
        }
    }
}
