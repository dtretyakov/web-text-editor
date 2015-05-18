namespace WebTextEditor.Domain.DTO
{
    /// <summary>
    ///     Document DTO.
    /// </summary>
    public sealed class DocumentContent
    {
        /// <summary>
        ///     Gets or sets a document identifier.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        ///     Gets or sets document content identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a conent value.
        /// </summary>
        public char Value { get; set; }
    }
}