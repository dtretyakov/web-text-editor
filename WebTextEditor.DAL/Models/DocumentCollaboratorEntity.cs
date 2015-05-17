using System;

namespace WebTextEditor.DAL.Models
{
    /// <summary>
    ///     Document collaborator entity model.
    /// </summary>
    public sealed class DocumentCollaboratorEntity
    {
        /// <summary>
        ///     Gets or sets a document identifier.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///     Gets or sets a user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets a document caret position for user.
        /// </summary>
        public int? CaretPosition { get; set; }

        /// <summary>
        ///     Gets or sets a last update date.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}