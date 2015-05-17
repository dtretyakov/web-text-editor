using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public interface IDocumentCollaboratorsService
    {
        /// <summary>
        ///     Adds a new collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="userId">User identifier.</param>
        Task AddCollaboratorAsync(string documentId, string userId);

        /// <summary>
        ///     Removes a collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="userId">User identifier.</param>
        Task RemoveCollaboratorAsync(string documentId, string userId);

        /// <summary>
        ///     Sets a document caret position for a specific user.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="caretPosition">Caret position.</param>
        Task SetDocumentCaretPositionAsync(string documentId, string userId, int? caretPosition);

        /// <summary>
        ///     Gets a list of document collaborators.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>List of collaborators.</returns>
        Task<List<DocumentCollaborator>> GetCollaboratorsAsync(string documentId);
    }
}