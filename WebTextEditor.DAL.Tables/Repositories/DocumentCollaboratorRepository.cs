using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsAzure.Table;
using WindowsAzure.Table.Extensions;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;

namespace WebTextEditor.DAL.Tables.Repositories
{
    public sealed class DocumentCollaboratorRepository : IDocumentCollaboratorRepository
    {
        private readonly ITableSet<DocumentCollaboratorEntity> _context;

        public DocumentCollaboratorRepository(ITableSet<DocumentCollaboratorEntity> context)
        {
            _context = context;
        }

        public Task AddAsync(DocumentCollaboratorEntity collaborator)
        {
            return _context.AddAsync(collaborator);
        }

        public Task RemoveAsync(DocumentCollaboratorEntity collaborator)
        {
            return _context.RemoveAsync(collaborator);
        }

        public Task UpdateAsync(DocumentCollaboratorEntity collaborator)
        {
            return _context.UpdateAsync(collaborator);
        }

        public Task<List<DocumentCollaboratorEntity>> FindByDocumentAsync(string documentId)
        {
            return _context.ToListAsync(p => p.DocumentId == documentId);
        }

        public Task<List<DocumentCollaboratorEntity>> FindByConnectionAsync(string connectionId)
        {
            return _context.ToListAsync(p => p.ConnectionId == connectionId);
        }
    }
}