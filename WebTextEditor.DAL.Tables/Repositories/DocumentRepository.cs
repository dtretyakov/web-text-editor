using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsAzure.Table;
using WindowsAzure.Table.Extensions;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;

namespace WebTextEditor.DAL.Tables.Repositories
{
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly ITableSet<DocumentEntity> _context;

        public DocumentRepository(ITableSet<DocumentEntity> context)
        {
            _context = context;
        }

        public Task<DocumentEntity> GetAsync(string documentId)
        {
            return _context.FirstOrDefaultAsync(p => p.Id == documentId);
        }

        public Task<List<DocumentEntity>> GetAllAsync()
        {
            return _context.ToListAsync();
        }

        public Task AddAsync(DocumentEntity document)
        {
            return _context.AddAsync(document);
        }

        public Task UpdateAsync(DocumentEntity document)
        {
            return _context.UpdateAsync(document);
        }

        public Task RemoveAsync(DocumentEntity document)
        {
            return _context.RemoveAsync(document);
        }
    }
}