using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Domain.DTO;
using WebTextEditor.Domain.Exceptions;

namespace WebTextEditor.BLL.Services
{
    /// <summary>
    ///     Provides document management facilities.
    /// </summary>
    public sealed class DocumentsService : IDocumentsService
    {
        private const string NewDocumentName = "New Document";
        private readonly IDocumentCollaboratorsService _collaboratorsService;
        private readonly IDocumentContentService _documentContentService;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IMappingEngine _mapper;

        public DocumentsService(
            IDocumentsRepository documentsRepository,
            IDocumentContentService documentContentService,
            IDocumentCollaboratorsService collaboratorsService,
            IMappingEngine mapper)
        {
            _documentsRepository = documentsRepository;
            _documentContentService = documentContentService;
            _collaboratorsService = collaboratorsService;
            _mapper = mapper;
        }

        public async Task<DocumentState> GetAsync(string documentId)
        {
            var document = await _documentsRepository.GetAsync(documentId);
            if (document == null)
            {
                throw new NotFoundException(string.Format("Document {0} was not found.", documentId));
            }

            // Gets content & collaborators
            var contentTask = _documentContentService.GetCurrentContentAsync(documentId);
            var collaboratorsTask = _collaboratorsService.FindByDocumentAsync(documentId);

            await Task.WhenAll(contentTask, collaboratorsTask);

            // Construct final data
            var documentState = _mapper.Map<DocumentEntity, DocumentState>(document);
            documentState.Content = contentTask.Result;
            documentState.Collaborators = collaboratorsTask.Result;

            return documentState;
        }

        public async Task<List<Document>> GetListAsync()
        {
            var documents = await _documentsRepository.GetAllAsync();

            return _mapper.Map<List<DocumentEntity>, List<Document>>(documents);
        }

        public async Task<Document> AddAsync(string userId)
        {
            var document = new DocumentEntity
            {
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString("N"),
                Name = NewDocumentName,
                UserId = userId
            };

            await _documentsRepository.AddAsync(document);

            return _mapper.Map<DocumentEntity, Document>(document);
        }

        public async Task DeleteAsync(string userId, string documentId)
        {
            var document = await _documentsRepository.GetAsync(documentId);
            if (document == null)
            {
                throw new NotFoundException(string.Format("Document {0} not found.", documentId));
            }

            if (document.UserId != userId)
            {
                throw new ForiddenException(string.Format("Access to the document {0} forbidden.", documentId));
            }

            await Task.WhenAll(
                _documentsRepository.RemoveAsync(document),
                _documentContentService.RemoveAllAsync(document.Id));

        }

        public async Task UpdateAsync(Document document)
        {
            var entity = await _documentsRepository.GetAsync(document.Id);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("Document {0} was not found.", document.Id));
            }

            entity.Name = document.Name;

            await _documentsRepository.UpdateAsync(entity);
        }
    }
}