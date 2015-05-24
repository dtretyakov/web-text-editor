using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    /// <summary>
    ///     Provides access to the document content database collection.
    /// </summary>
    public interface IDocumentContentRepository
    {
        /// <summary>
        ///     Adds a new content.
        /// </summary>
        /// <param name="content">Content.</param>
        Task AddAsync(DocumentContentEntity content);

        /// <summary>
        ///     Removes a content.
        /// </summary>
        /// <param name="collaborator">Content.</param>
        Task RemoveAsync(DocumentContentEntity collaborator);

        /// <summary>
        ///     Retrieves a document content.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Content.</returns>
        Task<List<DocumentContentEntity>> GetAllAsync(string documentId);

        /// <summary>
        ///     Removes a whole document content.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        Task RemoveAllAsync(string documentId);
    }
}