using System;

namespace WebTextEditor.Domain.Exceptions
{
    /// <summary>
    ///     The exception that is trows when entity was not found.
    /// </summary>
    [Serializable]
    public sealed class NotFoundException : ApplicationException
    {
        /// <summary>
        ///     Default constructor.
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        ///     Constructs an exception with message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NotFoundException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}