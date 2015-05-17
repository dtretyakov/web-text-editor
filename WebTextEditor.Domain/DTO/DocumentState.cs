using System.Collections.Generic;

namespace WebTextEditor.Domain.DTO
{
    /// <summary>
    ///     Document state DTO.
    /// </summary>
    public sealed class DocumentState : Document
    {
        /// <summary>
        ///     Gets or sets a list of collaborators.
        /// </summary>
        public List<DocumentCollaborator> Collaborators { get; set; }

        /// <summary>
        ///     Gets or sets a document content in logoot representation.
        /// </summary>
        public Dictionary<string, char> Content { get; set; }
    }
}