using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public interface IDocumentCollaboratorService
    {
        /// <summary>
        ///     Adds a new collaborator.
        /// </summary>
        /// <param name="collaborator">Document collaborator.</param>
        Task AddAsync(DocumentCollaborator collaborator);

        /// <summary>
        ///     Updates a specific user.
        /// </summary>
        /// <param name="collaborator">Document collaborator.</param>
        Task UpdateAsync(DocumentCollaborator collaborator);

        /// <summary>
        ///     Removes a collaborator.
        /// </summary>
        /// <param name="collaborator">Document collaborator.</param>
        Task RemoveAsync(DocumentCollaborator collaborator);

        /// <summary>
        ///     Gets a list of document collaborators.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>List of collaborators.</returns>
        Task<List<DocumentCollaborator>> FindByDocumentAsync(string documentId);

        /// <summary>
        /// Gets a collaborator by connection identifier.
        /// </summary>
        /// <param name="connectionId">Connection identifier.</param>
        /// <returns>List of collaborators.</returns>
        Task<List<DocumentCollaborator>> FindByConnectionAsync(string connectionId);
    }
}