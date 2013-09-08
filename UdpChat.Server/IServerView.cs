// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServerView.cs" company="">
//   
// </copyright>
// <summary>
//   Интерфейс для работы с рабочим окном сервера
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Server
{
    using System;

    /// <summary>
    /// Интерфейс для работы с рабочим окном сервера
    /// </summary>
    public interface IServerView
    {
        /// <summary>
        /// Отображение сообщения об ошибке
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        void ShowException(Exception ex);
    }
}