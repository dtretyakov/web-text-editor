using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public sealed class DocumentCollaboratorsService : IDocumentCollaboratorsService
    {
        private readonly IDocumentCollaboratorsRepository _collaboratorsRepository;
        private readonly IMappingEngine _mapper;

        public DocumentCollaboratorsService(IDocumentCollaboratorsRepository collaboratorsRepository, IMappingEngine mapper)
        {
            _collaboratorsRepository = collaboratorsRepository;
            _mapper = mapper;
        }

        public Task AddAsync(DocumentCollaborator collaborator)
        {
            var entity = _mapper.Map<DocumentCollaborator, DocumentCollaboratorEntity>(collaborator);

            return _collaboratorsRepository.AddAsync(entity);
        }

        public Task UpdateAsync(DocumentCollaborator collaborator)
        {
            var entity = _mapper.Map<DocumentCollaborator, DocumentCollaboratorEntity>(collaborator);

            return _collaboratorsRepository.UpdateAsync(entity);
        }

        public Task RemoveAsync(DocumentCollaborator collaborator)
        {
            var entity = _mapper.Map<DocumentCollaborator, DocumentCollaboratorEntity>(collaborator);

            return _collaboratorsRepository.RemoveAsync(entity);
        }

        public async Task<List<DocumentCollaborator>> FindByDocumentAsync(string documentId)
        {
            var collaborators = await _collaboratorsRepository.FindByDocumentAsync(documentId);

            return _mapper.Map<List<DocumentCollaboratorEntity>, List<DocumentCollaborator>>(collaborators);
        }

        public async Task<List<DocumentCollaborator>> FindByConnectionAsync(string connectionId)
        {
            var collaborators = await _collaboratorsRepository.FindByConnectionAsync(connectionId);

            return _mapper.Map<List<DocumentCollaboratorEntity>, List<DocumentCollaborator>>(collaborators);
        }
    }
}