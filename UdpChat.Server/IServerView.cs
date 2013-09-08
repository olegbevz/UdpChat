namespace UdpChat.Server
{
    using System;

    /// <summary>
    /// The ServerView interface.
    /// </summary>
    public interface IServerView
    {
        void ShowException(Exception ex);
    }
}