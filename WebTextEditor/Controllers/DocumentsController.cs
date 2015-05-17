using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebTextEditor.BLL.Services;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.Controllers
{
    /// <summary>
    ///     Provides a document management facilities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/documents")]
    public sealed class DocumentsController : ApiController
    {
        private readonly IDocumentsService _documentsService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        /// <summary>
        ///     Retrieve a list of user documents.
        /// </summary>
        /// <returns>Documents list.</returns>
        [Route]
        public Task<List<Document>> Get()
        {
            return _documentsService.GetListAsync();
        }

        /// <summary>
        ///     Retrieve a document by identifier.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <returns>Document.</returns>
        [Route("{documentId}")]
        public Task<DocumentState> Get(string documentId)
        {
            return _documentsService.GetAsync(documentId);
        }

        /// <summary>
        ///     Create a new document.
        /// </summary>
        /// <returns>Document.</returns>
        [Route]
        public Task<Document> Post()
        {
            return _documentsService.AddAsync(User.Identity.Name);
        }

        /// <summary>
        ///     Update a document.
        /// </summary>
        [Route("{id}")]
        public Task Put(Document document)
        {
            return _documentsService.UpdateAsync(document);
        }

        /// <summary>
        ///     Delete a document.
        /// </summary>
        /// <returns>Document.</returns>
        [Route("{documentId}")]
        public Task Delete(string documentId)
        {
            return _documentsService.DeleteAsync(User.Identity.Name, documentId);
        }
    }
}