// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginMessageTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LoginMessageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests.MessageTests
{
    using NUnit.Framework;

    using UdpChat.Common;
    using UdpChat.Common.Messages;

    [TestFixture]
    public class LoginMessageTests : MessageTests
    {
        [TestCase]
        public void MessageWithSimpleSenderTest()
        {
            var loginMessage = new LoginMessage("I am a sender");
            
            this.AssertMessage(loginMessage);
        }

        [TestCase]
        public void MessageWithEmptySenderTest()
        {
            var loginMessage = new LoginMessage(string.Empty);
            this.AssertMessage(loginMessage);
        }

        [TestCase]
        public void MessageWithNullSenderTest()
        {
            var loginMessage = new LoginMessage(null);
            this.AssertMessage(loginMessage);
        }

        [TestCase]
        public void MessageWithLongSenderTest()
        {
            var loginMessage = new LoginMessage(new string('a', 9999));
            this.AssertMessage(loginMessage);
        }
    }
}