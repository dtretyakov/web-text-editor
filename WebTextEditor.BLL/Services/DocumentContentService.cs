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
            var entity = _mapper.Map<DocumentContentEntity>(content);

            return _contentRepository.AddAsync(entity);
        }

        public Task AddAsync(IEnumerable<DocumentContent> contents)
        {
            var entities = contents.Select(p => _mapper.Map<DocumentContentEntity>(p));

            return _contentRepository.AddAsync(entities);
        }

        public Task RemoveAsync(DocumentContent content)
        {
            var entity = _mapper.Map<DocumentContentEntity>(content);

            return _contentRepository.RemoveAsync(entity);
        }

        public Task RemoveAsync(IEnumerable<DocumentContent> contents)
        {
            var entities = contents.Select(p => _mapper.Map<DocumentContentEntity>(p));

            return _contentRepository.RemoveAsync(entities);
        }

        public async Task<Dictionary<string, string>> GetCurrentContentAsync(string documentId)
        {
            var content = await _contentRepository.GetAllAsync(documentId);

            return content.ToDictionary(p => p.Id, p => p.Value);
        }

        public Task RemoveAllAsync(string documentId)
        {
            return _contentRepository.RemoveAllAsync(documentId);
        }
    }
}