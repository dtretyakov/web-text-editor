using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    public sealed class DocumentContentRepository : IDocumentContentRepository
    {
        public async Task<List<DocumentContentEntity>> GetAllAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentContents.AsNoTracking()
                    .Where(p => p.DocumentId == documentId)
                    .ToListAsync();
            }
        }

        public async Task RemoveAllAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                var query = db.DocumentContents
                    .Where(p => p.DocumentId == documentId);

                db.DocumentContents.RemoveRange(query);

                await db.SaveChangesAsync();
            }
        }

        public async Task AddAsync(DocumentContentEntity content)
        {
            using (var db = new DataContext())
            {
                db.DocumentContents.AddRange(new[] {content});

                await ((IObjectContextAdapter)db).ObjectContext.SaveChangesAsync(SaveOptions.None);
            }
        }

        public async Task RemoveAsync(DocumentContentEntity document)
        {
            using (var db = new DataContext())
            {
                var query = db.DocumentContents.Where(
                    p => p.DocumentId == document.DocumentId && p.Id == document.Id);

                db.DocumentContents.RemoveRange(query);

                await db.SaveChangesAsync();
            }
        }
    }
}