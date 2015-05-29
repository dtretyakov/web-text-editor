namespace WebTextEditor.DAL.Models
{
    /// <summary>
    ///     Document collaborator entity model.
    /// </summary>
    public sealed class DocumentCollaboratorEntity
    {
        /// <summary>
        /// Gets or sets a connection identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets a document identifier.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///     Connection identifier.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     Gets or sets a user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets a document caret position for user.
        /// </summary>
        public int? CaretPosition { get; set; }
    }
}