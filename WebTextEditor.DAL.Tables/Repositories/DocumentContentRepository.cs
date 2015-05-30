using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsAzure.Table;
using WindowsAzure.Table.Extensions;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;

namespace WebTextEditor.DAL.Tables.Repositories
{
    public sealed class DocumentContentRepository : IDocumentContentRepository
    {
        private readonly ITableSet<DocumentContentEntity> _context;

        public DocumentContentRepository(ITableSet<DocumentContentEntity> context)
        {
            _context = context;
        }

        public Task AddAsync(DocumentContentEntity content)
        {
            return _context.AddAsync(content);
        }

        public Task RemoveAsync(DocumentContentEntity content)
        {
            return _context.RemoveAsync(content);
        }

        public Task<List<DocumentContentEntity>> GetAllAsync(string documentId)
        {
            return _context.ToListAsync(p => p.DocumentId == documentId);
        }

        public Task RemoveAllAsync(string documentId)
        {
            return _context.RemoveAsync(new DocumentContentEntity {DocumentId = documentId});
        }
    }
}