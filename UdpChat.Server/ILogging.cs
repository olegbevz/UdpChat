// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogging.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ILogging type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Server
{
    /// <summary>
    /// итерфейс для вывода логов
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        /// Вывести лог
        /// </summary>
        /// <param name="log">
        /// Текст лога
        /// </param>
        void WriteLog(string log);
    }
}