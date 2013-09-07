// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ContactsMessageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests.MessageTests
{
    using NUnit.Framework;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    public abstract class MessageTests
    {
        public void AssertMessage(Message message)
        {
            var bytes = message.ToBytes();

            var encryptedMessage = Message.FromBytes(bytes);

            Assert.AreEqual(message, encryptedMessage);
        }
    }
}
