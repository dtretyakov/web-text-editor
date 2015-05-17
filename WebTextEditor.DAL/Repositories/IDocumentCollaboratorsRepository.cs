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
        ///     Retrieves a list of collaborators.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Collaborators.</returns>
        Task<List<DocumentCollaboratorEntity>> GetAllAsync(string documentId);
    }
}