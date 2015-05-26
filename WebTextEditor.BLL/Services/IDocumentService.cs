using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    /// <summary>
    ///     Provides document management facilities.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        ///     Gets a document by identifier.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Document.</returns>
        Task<DocumentState> GetAsync(string documentId);

        /// <summary>
        ///     Gets a list of documents.
        /// </summary>
        /// <returns>List of documents.</returns>
        Task<List<Document>> GetListAsync();

        /// <summary>
        ///     Creates a new document.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>New document.</returns>
        Task<Document> AddAsync(string userId);

        /// <summary>
        ///     Removes a document by identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="documentId">Document identifier.</param>
        Task DeleteAsync(string userId, string documentId);

        /// <summary>
        ///     Updates a document.
        /// </summary>
        /// <param name="document">Document.</param>
        Task UpdateAsync(Document document);
    }
}