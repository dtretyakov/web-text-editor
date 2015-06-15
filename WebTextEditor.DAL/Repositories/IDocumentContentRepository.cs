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
        ///     Adds a new contents.
        /// </summary>
        /// <param name="contents">Contents.</param>
        Task AddAsync(IEnumerable<DocumentContentEntity> contents);

        /// <summary>
        ///     Removes a content.
        /// </summary>
        /// <param name="content">Content.</param>
        Task RemoveAsync(DocumentContentEntity content);

        /// <summary>
        ///     Removes a contents.
        /// </summary>
        /// <param name="contents">Contents.</param>
        Task RemoveAsync(IEnumerable<DocumentContentEntity> contents);

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