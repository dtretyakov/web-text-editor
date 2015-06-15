using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public interface IDocumentContentService
    {
        /// <summary>
        ///     Adds a new document content.
        /// </summary>
        /// <param name="content">Document content.</param>
        Task AddAsync(DocumentContent content);
        
        /// <summary>
        ///     Adds a new document contents.
        /// </summary>
        /// <param name="contents">Document contents.</param>
        Task AddAsync(IEnumerable<DocumentContent> contents);

        /// <summary>
        ///     Removes a document content.
        /// </summary>
        /// <param name="content">Document content.</param>
        Task RemoveAsync(DocumentContent content);
        
        /// <summary>
        ///     Removes a document contents.
        /// </summary>
        /// <param name="contents">Document contents.</param>
        Task RemoveAsync(IEnumerable<DocumentContent> contents);

        /// <summary>
        ///     Gets a document content.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Content.</returns>
        Task<Dictionary<string, string>> GetCurrentContentAsync(string documentId);

        /// <summary>
        ///     Removes a whole document contents.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        Task RemoveAllAsync(string documentId);
    }
}