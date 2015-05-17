using System.Collections.Generic;
using System.Threading.Tasks;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public interface IDocumentContentService
    {
        /// <summary>
        ///     Removes a document content.
        /// </summary>
        /// <param name="content">Document content.</param>
        Task AddAsync(DocumentContent content);

        /// <summary>
        ///     Removes a document content.
        /// </summary>
        /// <param name="content">Document content.</param>
        Task RemoveAsync(DocumentContent content);

        /// <summary>
        ///     Gets a document content.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Content.</returns>
        Task<Dictionary<string, char>> GetCurrentContentAsync(string documentId);
    }
}