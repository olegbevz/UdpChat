// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogoutMessageTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LogoutMessageTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Tests.MessageTests
{
    using NUnit.Framework;

    using UdpChat.Common.Messages;

    [TestFixture]
    public class LogoutMessageTests : MessageTests
    {
        [TestCase]
        public void LogoutMessageTest()
        {
            var logoutMessage = new LogoutMessage();
            this.AssertMessage(logoutMessage);
        }
    }
}