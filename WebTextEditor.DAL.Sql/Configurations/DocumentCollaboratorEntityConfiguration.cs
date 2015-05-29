using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Sql.Configurations
{
    public sealed class DocumentCollaboratorEntityConfiguration : EntityTypeConfiguration<DocumentCollaboratorEntity>
    {
        public DocumentCollaboratorEntityConfiguration()
        {
            ToTable("DocumentCollaborator");

            HasKey(t => new {t.DocumentId, t.ConnectionId});

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.DocumentId)
                .HasMaxLength(32)
                .IsRequired();

            Property(p => p.UserId)
                .HasMaxLength(128)
                .IsRequired();

            Property(p => p.ConnectionId)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}