using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    /// <summary>
    ///     Provides access to the documents database collection.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        ///     Gets a docuemnt by identifier.
        /// </summary>
        /// <param name="documentId">Docuemnt identifier.</param>
        /// <returns>Document entity.</returns>
        Task<DocumentEntity> GetAsync(string documentId);

        /// <summary>
        ///     Gets a list of user's documents.
        /// </summary>
        /// <returns>List of document entities.</returns>
        Task<List<DocumentEntity>> GetAllAsync();

        /// <summary>
        ///     Adds a new document.
        /// </summary>
        /// <param name="document">Document entity.</param>
        Task AddAsync(DocumentEntity document);

        /// <summary>
        ///     Updates a document.
        /// </summary>
        /// <param name="document">Document entity.</param>
        Task UpdateAsync(DocumentEntity document);

        /// <summary>
        ///     Removes a document by identifier.
        /// </summary>
        /// <param name="document">Document.</param>
        Task RemoveAsync(DocumentEntity document);
    }
}