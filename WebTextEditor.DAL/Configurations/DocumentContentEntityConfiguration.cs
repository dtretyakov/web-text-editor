using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Configurations
{
    public sealed class DocumentContentEntityConfiguration : EntityTypeConfiguration<DocumentContentEntity>
    {
        public DocumentContentEntityConfiguration()
        {
            ToTable("DocumentContent");

            Property(p => p.DocumentId)
                .HasMaxLength(32)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute()));

            Property(p => p.Id)
                .HasMaxLength(32)
                .IsRequired();

            Property(p => p.UserId)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}