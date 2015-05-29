using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;

namespace WebTextEditor.DAL.Sql.Repositories
{
    public sealed class DocumentContentRepository : IDocumentContentRepository
    {
        public async Task<List<DocumentContentEntity>> GetAllAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentContents
                    .AsNoTracking()
                    .Where(p => p.DocumentId == documentId)
                    .ToListAsync();
            }
        }

        public async Task RemoveAllAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "DELETE FROM DocumentContent WHERE DocumentId = {0}",
                    documentId);
            }
        }

        public async Task AddAsync(DocumentContentEntity content)
        {
            using (var db = new DataContext())
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "INSERT INTO DocumentContent (DocumentId, Id, Value) VALUES ({0}, {1}, {2})",
                    content.DocumentId, content.Id, content.Value);
            }
        }

        public async Task RemoveAsync(DocumentContentEntity content)
        {
            using (var db = new DataContext())
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "DELETE FROM DocumentContent WHERE DocumentId = {0} AND Id = {1}",
                    content.DocumentId, content.Id);
            }
        }
    }
}