using WindowsAzure.Table.EntityConverters.TypeData;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Tables.Mappings
{
    public sealed class DocumentMap : EntityTypeMap<DocumentEntity>
    {
        public DocumentMap()
        {
            PartitionKey(p => p.Id);
        }
    }
}