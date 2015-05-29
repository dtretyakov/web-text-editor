using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Sql.Configurations
{
    public sealed class DocumentEntityConfiguration : EntityTypeConfiguration<DocumentEntity>
    {
        public DocumentEntityConfiguration()
        {
            ToTable("Document");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasMaxLength(32);

            Property(p => p.Name)
                .HasMaxLength(128);

            Property(p => p.UserId)
                .HasMaxLength(128);

            Property(p => p.Created)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute()));
        }
    }
}