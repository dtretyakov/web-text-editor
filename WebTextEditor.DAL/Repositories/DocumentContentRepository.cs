using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;
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
                await db.DocumentContents
                    .Where(p => p.DocumentId == documentId)
                    .DeleteAsync();
            }
        }

        public Task AddAsync(DocumentContentEntity content)
        {
            return ExecuteSqlAction(async () =>
            {
                using (var db = new DataContext())
                {
                    db.DocumentContents.AddRange(new[] {content});

                    await db.SaveChangesAsync();
                }
            });
        }

        public async Task RemoveAsync(DocumentContentEntity content)
        {
            using (var db = new DataContext())
            {
                await db.DocumentContents
                    .Where(p => p.DocumentId == content.DocumentId && p.Id == content.Id)
                    .DeleteAsync();
            }
        }

        /// <summary>
        ///     Executes action until successful completion.
        /// </summary>
        /// <param name="action">Action.</param>
        private static async Task ExecuteSqlAction(Func<Task> action)
        {
            while (true)
            {
                try
                {
                    await action();
                }
                catch (EntityException)
                {
                    // Caused by connection timeout
                }
            }
        }
    }
}