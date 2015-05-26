using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    public sealed class DocumentCollaboratorRepository : IDocumentCollaboratorRepository
    {
        public async Task<List<DocumentCollaboratorEntity>> FindByDocumentAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentCollaborators
                    .AsNoTracking()
                    .Where(p => p.DocumentId == documentId)
                    .ToListAsync();
            }
        }

        public async Task<List<DocumentCollaboratorEntity>> FindByConnectionAsync(string connectionId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentCollaborators
                    .AsNoTracking()
                    .Where(p => p.ConnectionId == connectionId)
                    .ToListAsync();
            }
        }

        public async Task AddAsync(DocumentCollaboratorEntity collaborator)
        {
            using (var db = new DataContext())
            {
                db.DocumentCollaborators.AddRange(new[] {collaborator});

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(DocumentCollaboratorEntity collaborator)
        {
            using (var db = new DataContext())
            {
                db.DocumentCollaborators.Attach(collaborator);

                db.Entry(collaborator).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(DocumentCollaboratorEntity document)
        {
            using (var db = new DataContext())
            {
                var query = db.DocumentCollaborators
                    .Where(p => p.DocumentId == document.DocumentId && p.ConnectionId == document.ConnectionId);

                db.DocumentCollaborators.RemoveRange(query);

                await db.SaveChangesAsync();
            }
        }
    }
}