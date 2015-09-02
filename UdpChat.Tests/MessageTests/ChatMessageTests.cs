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
        public void ChatMessageWithSimpleSenderTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void WelcomeMessageTest()
        {
            var chatMessage = new ChatMessage(
                "SERVER",
                "Hello, Harry Potter! Welcome to chat on server 'SERVER'!");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWitNullSenderTest()
        {
            var chatMessage = new ChatMessage(
                null,
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithEmptySenderTest()
        {
            var chatMessage = new ChatMessage(
                string.Empty,
                "Message Content");

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithEmptyContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                string.Empty);

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithNullContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                null);

            this.AssertMessage(chatMessage);
        }

        [TestCase]
        public void ChatMessageWithBigContentTest()
        {
            var chatMessage = new ChatMessage(
                "Sender",
                new string('a', 9999));

            this.AssertMessage(chatMessage);
        }
    }
}