using WindowsAzure.Table.EntityConverters.TypeData;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Tables.Mappings
{
    public sealed class DocumentContentMap : EntityTypeMap<DocumentContentEntity>
    {
        public DocumentContentMap()
        {
            PartitionKey(p => p.DocumentId);
            RowKey(p => p.Id);
        }
    }
}