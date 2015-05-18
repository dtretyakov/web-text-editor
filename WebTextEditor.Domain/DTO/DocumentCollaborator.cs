namespace WebTextEditor.Domain.DTO
{
    /// <summary>
    ///     Document collaborator DTO.
    /// </summary>
    public sealed class DocumentCollaborator
    {
        /// <summary>
        ///     Gets or sets a document identifier.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///     Gets or sets a connection identifier.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     Gets or sets a user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets a document caret position.
        /// </summary>
        public int? CaretPosition { get; set; }
    }
}