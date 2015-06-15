using System.Collections.Generic;
using System.Linq;
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

        public Task AddAsync(IEnumerable<DocumentContentEntity> contents)
        {
            return _context.AddAsync(contents);
        }

        public Task RemoveAsync(DocumentContentEntity content)
        {
            return _context.RemoveAsync(content);
        }

        public Task RemoveAsync(IEnumerable<DocumentContentEntity> contents)
        {
            return _context.RemoveAsync(contents);
        }

        public Task<List<DocumentContentEntity>> GetAllAsync(string documentId)
        {
            return _context.ToListAsync(p => p.DocumentId == documentId);
        }

        public Task RemoveAllAsync(string documentId)
        {
            return Task.Run(() => { _context.Remove(_context.Where(p => p.DocumentId == documentId)); });
        }
    }
}