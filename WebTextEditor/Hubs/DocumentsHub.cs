using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WebTextEditor.BLL.Services;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.Hubs
{
    /// <summary>
    ///     Handles collaborative document editing.
    /// </summary>
    [Authorize]
    public class DocumentsHub : Hub
    {
        private readonly IDocumentCollaboratorsService _collaboratorService;
        private readonly IDocumentContentService _contentService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="collaboratorService">Collaborators services.</param>
        /// <param name="contentService">Documents content service.</param>
        public DocumentsHub(IDocumentCollaboratorsService collaboratorService, IDocumentContentService contentService)
        {
            _collaboratorService = collaboratorService;
            _contentService = contentService;
        }

        /// <summary>
        ///     Adds a new document collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        public async Task JoinDocument(string documentId)
        {
            // Join group and notify collaborators
            var userId = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            await Groups.Add(connectionId, documentId);

            var collaborator = new DocumentCollaborator
            {
                DocumentId = documentId,
                ConnectionId = connectionId,
                UserId = userId
            };

            Clients.Group(documentId).addCollaborator(collaborator);

            await _collaboratorService.AddAsync(collaborator);
        }

        /// <summary>
        ///     Removes a document collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        public async Task LeaveDocument(string documentId)
        {
            // Leave group and notify collaborators
            var connectionId = Context.ConnectionId;

            await Groups.Remove(connectionId, documentId);

            var collaborator = new DocumentCollaborator
            {
                DocumentId = documentId,
                ConnectionId = connectionId
            };

            await _collaboratorService.RemoveAsync(collaborator);

            Clients.Group(documentId).removeCollaborator(collaborator);
        }

        /// <summary>
        ///     Notifies group members about new caret position.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="caretPosition">Caret position.</param>
        public Task SetCaret(string documentId, int? caretPosition)
        {
            var userId = Context.User.Identity.Name;

            var collaborator = new DocumentCollaborator
            {
                DocumentId = documentId,
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                CaretPosition = caretPosition
            };

            Clients.OthersInGroup(documentId).caretPosition(collaborator);

            return _collaboratorService.UpdateAsync(collaborator);
        }

        /// <summary>
        ///     Notifies group members about new document charater.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="charId">Charater identifier.</param>
        /// <param name="value">Character value.</param>
        public Task AddChar(string documentId, string charId, string value)
        {
            Clients.Others.addChar(charId, value);

            var content = new DocumentContent
            {
                DocumentId = documentId,
                Id = charId,
                Value = value
            };

            return _contentService.AddAsync(content);
        }

        /// <summary>
        ///     Notifies group members about document charater deletion.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="charId">Character identifier.</param>
        public Task RemoveChar(string documentId, string charId)
        {
            Clients.OthersInGroup(documentId).removeChar(charId);

            var content = new DocumentContent
            {
                DocumentId = documentId,
                Id = charId
            };

            return _contentService.RemoveAsync(content);
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            var collaborators = await _collaboratorService.FindByConnectionAsync(connectionId);

            foreach (var collaborator in collaborators)
            {
                Clients.Group(collaborator.DocumentId).removeCollaborator(collaborator);
            }

            await Task.WhenAll(collaborators.Select(p => _collaboratorService.RemoveAsync(p)));
        }
    }
}