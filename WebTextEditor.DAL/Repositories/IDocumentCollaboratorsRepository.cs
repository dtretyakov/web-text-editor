using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    /// <summary>
    ///     Provides access to the document collaborators database collection.
    /// </summary>
    public interface IDocumentCollaboratorsRepository
    {
        /// <summary>
        ///     Adds a new collaborator.
        /// </summary>
        /// <param name="collaborator">Collaborator.</param>
        Task AddAsync(DocumentCollaboratorEntity collaborator);

        /// <summary>
        ///     Removes a collaborator.
        /// </summary>
        /// <param name="collaborator">Collaborator.</param>
        Task RemoveAsync(DocumentCollaboratorEntity collaborator);

        /// <summary>
        ///     Updates a collaborator.
        /// </summary>
        /// <param name="collaborator">Collaborator.</param>
        Task UpdateAsync(DocumentCollaboratorEntity collaborator);

        /// <summary>
        ///     Retrieves a list of collaborators by document identifier.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Collaborators.</returns>
        Task<List<DocumentCollaboratorEntity>> FindByDocumentAsync(string documentId);

        /// <summary>
        ///     Retrieves a collaborator by connection identifier.
        /// </summary>
        /// <param name="connectionId">Connection identifier.</param>
        /// <returns>Collaborator.</returns>
        Task<List<DocumentCollaboratorEntity>> FindByConnectionAsync(string connectionId);
    }
}