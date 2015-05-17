using System;
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

        public Task AddCollaboratorAsync(string documentId, string userId)
        {
            var collaborator = new DocumentCollaboratorEntity
            {
                DocumentId = documentId,
                UserId = userId,
                Updated = DateTime.UtcNow
            };

            return _collaboratorsRepository.AddAsync(collaborator);
        }

        public Task RemoveCollaboratorAsync(string documentId, string userId)
        {
            var collaborator = new DocumentCollaboratorEntity
            {
                DocumentId = documentId,
                UserId = userId
            };

            return _collaboratorsRepository.RemoveAsync(collaborator);
        }

        public Task SetDocumentCaretPositionAsync(string documentId, string userId, int? caretPosition)
        {
            var collaborator = new DocumentCollaboratorEntity
            {
                DocumentId = documentId,
                UserId = userId,
                CaretPosition = caretPosition,
                Updated = DateTime.UtcNow
            };

            return _collaboratorsRepository.UpdateAsync(collaborator);
        }

        public async Task<List<DocumentCollaborator>> GetCollaboratorsAsync(string documentId)
        {
            var collaborators = await _collaboratorsRepository.GetAllAsync(documentId);

            return _mapper.Map<List<DocumentCollaboratorEntity>, List<DocumentCollaborator>>(collaborators);
        }
    }
}