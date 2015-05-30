using WindowsAzure.Table.EntityConverters.TypeData;
using WebTextEditor.DAL.Models;

namespace WebTextEditor.DAL.Tables.Mappings
{
    public sealed class DocumentCollaboratorMap : EntityTypeMap<DocumentCollaboratorEntity>
    {
        public DocumentCollaboratorMap()
        {
            PartitionKey(p => p.DocumentId);
            RowKey(p => p.ConnectionId);
        }
    }
}