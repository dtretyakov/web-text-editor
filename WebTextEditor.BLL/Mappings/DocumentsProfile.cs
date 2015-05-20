using AutoMapper;
using WebTextEditor.DAL.Models;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Mappings
{
    public sealed class DocumentsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<DocumentEntity, Document>();
            CreateMap<Document, DocumentEntity>();
            CreateMap<DocumentEntity, DocumentState>();
            CreateMap<DocumentContentEntity, DocumentContent>();
            CreateMap<DocumentContent, DocumentContentEntity>();
            CreateMap<DocumentCollaboratorEntity, DocumentCollaborator>();
            CreateMap<DocumentCollaborator, DocumentCollaboratorEntity>();
        }
    }
}