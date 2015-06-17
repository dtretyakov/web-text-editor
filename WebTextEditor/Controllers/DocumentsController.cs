using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR.Infrastructure;
using WebTextEditor.BLL.Services;
using WebTextEditor.Domain.DTO;
using WebTextEditor.Hubs;
using WebTextEditor.Infrastructure;

namespace WebTextEditor.Controllers
{
    /// <summary>
    ///     Provides a document management facilities.
    /// </summary>
    [Authorize]
    [ValidateArguments]
    [RoutePrefix("api/documents")]
    public sealed class DocumentsController : ApiController
    {
        private readonly IDocumentService _documentsService;
        private readonly IConnectionManager _connectionManager;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public DocumentsController(IDocumentService documentsService, IConnectionManager connectionManager)
        {
            _documentsService = documentsService;
            _connectionManager = connectionManager;
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
        public async Task Delete(string documentId)
        {
            await _documentsService.DeleteAsync(User.Identity.Name, documentId);

            // Notify cliens to leave removed document
            var documentsHub = _connectionManager.GetHubContext<DocumentsHub>();
            await documentsHub.Clients.Group(documentId).leaveDocument();
        }
    }
}