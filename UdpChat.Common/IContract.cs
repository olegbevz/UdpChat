// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContract.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IContract type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common
{
    using System.Collections.Generic;

    public interface IContract
    {
        string[] GetContacts();

        void Login(string user);

        void Logout();

        void SendMessage(string user, string message);
    }
}