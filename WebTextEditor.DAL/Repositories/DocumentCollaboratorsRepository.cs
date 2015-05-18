using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Repositories
{
    public sealed class DocumentCollaboratorsRepository : IDocumentCollaboratorsRepository
    {
        public async Task<List<DocumentCollaboratorEntity>> FindByDocumentAsync(string documentId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentCollaborators.Where(p => p.DocumentId == documentId).ToListAsync();
            }
        }

        public async Task<List<DocumentCollaboratorEntity>> FindByConnectionAsync(string connectionId)
        {
            using (var db = new DataContext())
            {
                return await db.DocumentCollaborators.Where(p => p.ConnectionId == connectionId).ToListAsync();
            }
        }

        public async Task AddAsync(DocumentCollaboratorEntity collaborator)
        {
            using (var db = new DataContext())
            {
                db.DocumentCollaborators.AddOrUpdate(collaborator);
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
                db.DocumentCollaborators.Attach(document);
                db.DocumentCollaborators.Remove(document);
                await db.SaveChangesAsync();
            }
        }
    }
}