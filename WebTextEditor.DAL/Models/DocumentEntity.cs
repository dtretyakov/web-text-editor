using System;

namespace WebTextEditor.DAL.Models
{
    /// <summary>
    ///     Document entity model.
    /// </summary>
    public sealed class DocumentEntity
    {
        /// <summary>
        ///     Document identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Document name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Document author identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Creation date.
        /// </summary>
        public DateTime Created { get; set; }
    }
}