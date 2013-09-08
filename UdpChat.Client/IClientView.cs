// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClientView.cs" company="">
//   
// </copyright>
// <summary>
//   Интерфейс для работы с рабочим окном клиента
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Client
{
    using System;
    using System.Collections.Generic;

    using UdpChat.Common;

    /// <summary>
    /// Интерфейс для работы с рабочим окном клиента
    /// </summary>
    public interface IClientView
    {
        /// <summary>
        /// Отобразить список контактов
        /// </summary>
        /// <param name="contacts">
        /// Список контактов
        /// </param>
        void DisplayContacts(IEnumerable<Contact> contacts);

        /// <summary>
        /// Отобразить сообщение в чате
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void DisplayMessage(string message);

        /// <summary>
        /// Заблокировать рабочее окно от ввода 
        /// </summary>
        void DisableClient();

        /// <summary>
        /// Разблокировать рабочее окно для переписки в чате
        /// </summary>
        void EnableClient();

        /// <summary>
        /// Отобоазить сообщение обошибке
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        void ShowException(Exception ex);
    }
}