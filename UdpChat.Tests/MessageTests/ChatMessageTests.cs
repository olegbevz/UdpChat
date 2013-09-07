// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatMessageTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ChatMessageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests.MessageTests
{
    using System.Net;

    using NUnit.Framework;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    [TestFixture]
    public class ChatMessageTests : MessageTests
    {
        [TestCase]
        public void ChatMessageWithSimpleSenderAndReceiverTest()
        {
            var chatMessage = new ChatMessage(
                "Sender", 
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)), 
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithNullReceiverTest()
        {
            var chatMessage = new ChatMessage("Sender", null, "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWitNullSenderTest()
        {
            var chatMessage = new ChatMessage(
                null,
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)),
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithEmptySenderTest()
        {
            var chatMessage = new ChatMessage(
                string.Empty,
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)),
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithEmptyContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)),
                string.Empty);

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithNullContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)),
                null);

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithBigContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                new Contact("Receiver", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)),
                new string('a', 9999));

            this.AssertMessage(chatMessage);
        }
    }
}