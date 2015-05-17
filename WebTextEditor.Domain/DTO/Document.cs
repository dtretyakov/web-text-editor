using System;

namespace WebTextEditor.Domain.DTO
{
    /// <summary>
    ///     Document DTO.
    /// </summary>
    public class Document
    {
        /// <summary>
        ///     Gets or sets a document identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a document name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a create date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets user identifier.
        /// </summary>
        public string UserId { get; set; }
    }
}