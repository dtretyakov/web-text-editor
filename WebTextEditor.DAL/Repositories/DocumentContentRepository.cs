using System.Collections.Generic;
using System.Data.Entity;
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
                return await db.DocumentContents.Where(p => p.DocumentId == documentId).ToListAsync();
            }
        }

        public async Task RemoveAllAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                db.DocumentContents.RemoveRange(db.DocumentContents.Where(p => p.DocumentId == documentId));
                await db.SaveChangesAsync();
            }
        }

        public async Task AddAsync(DocumentContentEntity content)
        {
            using (var db = new DataContext())
            {
                db.DocumentContents.Add(content);
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(DocumentContentEntity document)
        {
            using (var db = new DataContext())
            {
                db.DocumentContents.Attach(document);
                db.DocumentContents.Remove(document);
                await db.SaveChangesAsync();
            }
        }
    }
}