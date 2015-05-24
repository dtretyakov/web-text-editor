using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    public sealed class DocumentsRepository : IDocumentsRepository
    {
        public async Task<DocumentEntity> GetAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                return await db.Documents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == documentId);
            }
        }

        public async Task<List<DocumentEntity>> GetAllAsync()
        {
            using (var db = new DataContext())
            {
                return await db.Documents
                    .OrderByDescending(p => p.Created)
                    .ToListAsync();
            }
        }

        public async Task AddAsync(DocumentEntity document)
        {
            using (var db = new DataContext())
            {
                db.Documents.AddRange(new[] {document});

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(DocumentEntity document)
        {
            using (var db = new DataContext())
            {
                db.Documents.Attach(document);

                db.Entry(document).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(DocumentEntity document)
        {
            using (var db = new DataContext())
            {
                var query = db.Documents.Where(p => p.Id == document.Id);

                db.Documents.RemoveRange(query);

                await db.SaveChangesAsync();
            }
        }
    }
}