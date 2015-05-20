using System.Data.Entity.ModelConfiguration;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Configurations
{
    public sealed class DocumentContentEntityConfiguration : EntityTypeConfiguration<DocumentContentEntity>
    {
        public DocumentContentEntityConfiguration()
        {
            ToTable("DocumentContent");

            HasKey(p => new {p.DocumentId, p.Id});

            Property(p => p.DocumentId)
                .HasMaxLength(32)
                .IsRequired();

            Property(p => p.Id)
                .HasMaxLength(900)
                .IsRequired();

            Property(p => p.Value)
                .HasMaxLength(1)
                .IsRequired();
        }
    }
}