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
        /// <param name="collaboratorService"></param>
        /// <param name="contentService"></param>
        public DocumentsHub(IDocumentCollaboratorsService collaboratorService, IDocumentContentService contentService)
        {
            _collaboratorService = collaboratorService;
            _contentService = contentService;
        }

        /// <summary>
        ///     Adds a new document collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        public async Task JoinDocumentEditing(string documentId)
        {
            // Join group and notify collaborators
            var userId = Context.User.Identity.Name;
            Clients.Group(documentId).addCollaborator(userId);

            await Groups.Add(Context.ConnectionId, documentId);
            await _collaboratorService.AddCollaboratorAsync(documentId, userId);
        }

        /// <summary>
        ///     Removes a document collaborator.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        public async Task LeaveDocumentEditing(string documentId)
        {
            // Leave group and notify collaborators
            var userId = Context.User.Identity.Name;
            await Groups.Remove(Context.ConnectionId, documentId);
            Clients.Group(documentId).removeCollaborator(userId);

            await _collaboratorService.RemoveCollaboratorAsync(documentId, userId);
        }

        /// <summary>
        ///     Notifies group members about new caret position.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="caretPosition">Caret position.</param>
        public Task SetCaretPosition(string documentId, int? caretPosition)
        {
            var userId = Context.User.Identity.Name;
            Clients.OthersInGroup(documentId).caretPosition(userId, caretPosition);

            return _collaboratorService.SetDocumentCaretPositionAsync(documentId, userId, caretPosition);
        }

        /// <summary>
        ///     Notifies group members about new document charater.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="charId">Charater identifier.</param>
        /// <param name="value">Character value.</param>
        public Task AddCharacter(string documentId, string charId, char value)
        {
            Clients.Others.insertCharater(charId, value);

            var content = new DocumentContent
            {
                DocumentId = documentId,
                Id = charId,
                UserId = Context.User.Identity.Name,
                Value = value
            };

            return _contentService.AddAsync(content);
        }

        /// <summary>
        ///     Notifies group members about document charater deletion.
        /// </summary>
        /// <param name="documentId">Document identifier.</param>
        /// <param name="charId">Character identifier.</param>
        public Task RemoveCharacter(string documentId, string charId)
        {
            Clients.OthersInGroup(documentId).deleteCharacter(charId);

            var content = new DocumentContent
            {
                DocumentId = documentId,
                Id = charId,
                UserId = Context.User.Identity.Name
            };

            return _contentService.RemoveAsync(content);
        }
    }
}