using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Domain.DTO;

namespace WebTextEditor.BLL.Services
{
    public sealed class DocumentContentService : IDocumentContentService
    {
        private readonly IDocumentContentRepository _contentRepository;
        private readonly IMappingEngine _mapper;

        public DocumentContentService(IDocumentContentRepository contentRepository, IMappingEngine mapper)
        {
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public Task AddAsync(DocumentContent content)
        {
            var entity = _mapper.Map<DocumentContent, DocumentContentEntity>(content);

            return _contentRepository.AddAsync(entity);
        }

        public Task RemoveAsync(DocumentContent content)
        {
            var entity = _mapper.Map<DocumentContent, DocumentContentEntity>(content);

            return _contentRepository.RemoveAsync(entity);
        }

        public async Task<Dictionary<string, char>> GetCurrentContentAsync(string documentId)
        {
            var content = await _contentRepository.GetAllAsync(documentId);

            return content.ToDictionary(p => p.Id, p => p.Value);
        }
    }
}