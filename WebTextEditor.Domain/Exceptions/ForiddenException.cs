using System;

namespace WebTextEditor.Domain.Exceptions
{
    /// <summary>
    ///     The exception that is trows when access to entity was forbidden.
    /// </summary>
    [Serializable]
    public sealed class ForiddenException : ApplicationException
    {
        /// <summary>
        ///     Default constructor.
        /// </summary>
        public ForiddenException()
        {
        }

        /// <summary>
        ///     Constructs an exception with message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ForiddenException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}