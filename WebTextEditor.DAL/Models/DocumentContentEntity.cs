namespace WebTextEditor.DAL.Models
{
    /// <summary>
    ///     Document content entity model.
    /// </summary>
    public sealed class DocumentContentEntity
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