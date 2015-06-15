using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using WebTextEditor.BLL.Services;
using WebTextEditor.BLL.Tests.Mocks;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Domain.DTO;
using Xunit;

namespace WebTextEditor.BLL.Tests.Services
{
    public sealed class DocumentContentServiceTests
    {
        [Fact]
        public async Task AddContentTest()
        {
            var content = new DocumentContent
            {
                DocumentId = "documentId",
                Id = "id",
                Value = "value"
            };

            DocumentContentEntity addedEntity = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<DocumentContentEntity>()))
                .Callback((DocumentContentEntity e) => { addedEntity = e; })
                .Returns(() => Task.FromResult(0));

            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            await contentService.AddAsync(content);

            Assert.NotNull(addedEntity);
            Assert.Equal(content.DocumentId, addedEntity.DocumentId);
            Assert.Equal(content.Id, addedEntity.Id);
            Assert.Equal(content.Value, addedEntity.Value);

            documentContentRepositoryMock.Verify(p => p.AddAsync(It.IsAny<DocumentContentEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddContentsTest()
        {
            var content = new DocumentContent
            {
                DocumentId = "documentId",
                Id = "id",
                Value = "value"
            };

            var contents = new List<DocumentContent> {content};

            List<DocumentContentEntity> addedEntities = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<IEnumerable<DocumentContentEntity>>()))
                .Callback((IEnumerable<DocumentContentEntity> e) => { addedEntities = e.ToList(); })
                .Returns(() => Task.FromResult(0));

            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            await contentService.AddAsync(contents);

            Assert.Equal(1, addedEntities.Count);

            var addedEntity = addedEntities[0];
            Assert.Equal(content.DocumentId, addedEntity.DocumentId);
            Assert.Equal(content.Id, addedEntity.Id);
            Assert.Equal(content.Value, addedEntity.Value);

            documentContentRepositoryMock.Verify(p => p.AddAsync(It.IsAny<IEnumerable<DocumentContentEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetContentTest()
        {
            var contentEntities = new List<DocumentContentEntity>
            {
                new DocumentContentEntity {Id = "1", Value = "a"},
                new DocumentContentEntity {Id = "2", Value = "b"},
                new DocumentContentEntity {Id = "3", Value = "b"}
            };

            var documentId = "documentId";
            string requestedDocumentId = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.GetAllAsync(It.IsAny<string>()))
                .Callback((string id) => { requestedDocumentId = id; })
                .Returns(() => Task.FromResult(contentEntities));

            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            var content = await contentService.GetCurrentContentAsync(documentId);

            Assert.NotNull(content);
            Assert.Equal(documentId, requestedDocumentId);
            Assert.Equal(contentEntities.Select(p => p.Id).OrderBy(p => p), content.Keys.OrderBy(p => p));
            Assert.Equal(contentEntities.Select(p => p.Value).OrderBy(p => p), content.Values.OrderBy(p => p));

            documentContentRepositoryMock.Verify(p => p.GetAllAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RemoveContentTest()
        {
            var content = new DocumentContent
            {
                DocumentId = "documentId",
                Id = "id",
                Value = "value"
            };

            DocumentContentEntity removedEntity = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.RemoveAsync(It.IsAny<DocumentContentEntity>()))
                .Callback((DocumentContentEntity entity) => { removedEntity = entity; })
                .Returns(() => Task.FromResult(0));


            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            await contentService.RemoveAsync(content);

            Assert.NotNull(removedEntity);
            Assert.Equal(content.DocumentId, removedEntity.DocumentId);
            Assert.Equal(content.Id, removedEntity.Id);
            Assert.Equal(content.Value, removedEntity.Value);

            documentContentRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<DocumentContentEntity>()), Times.Once);
        }

        [Fact]
        public async Task RemoveContentsTest()
        {
            var content = new DocumentContent
            {
                DocumentId = "documentId",
                Id = "id",
                Value = "value"
            };

            var contents = new List<DocumentContent> {content};

            List<DocumentContentEntity> removedEntities = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.RemoveAsync(It.IsAny<IEnumerable<DocumentContentEntity>>()))
                .Callback((IEnumerable<DocumentContentEntity> entities) => { removedEntities = entities.ToList(); })
                .Returns(() => Task.FromResult(0));


            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            await contentService.RemoveAsync(contents);

            Assert.Equal(1, removedEntities.Count);

            var removedEntity = removedEntities[0];
            Assert.Equal(content.DocumentId, removedEntity.DocumentId);
            Assert.Equal(content.Id, removedEntity.Id);
            Assert.Equal(content.Value, removedEntity.Value);

            documentContentRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<IEnumerable<DocumentContentEntity>>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAllContentTest()
        {
            var documentId = "documentId";

            string removedDocumentId = null;

            var documentContentRepositoryMock = new Mock<IDocumentContentRepository>();
            documentContentRepositoryMock.Setup(p => p.RemoveAllAsync(It.IsAny<string>()))
                .Callback((string entity) => { removedDocumentId = entity; })
                .Returns(() => Task.FromResult(0));

            var contentService = new DocumentContentService(documentContentRepositoryMock.Object, MockObjects.GetMapper());

            await contentService.RemoveAllAsync(documentId);

            Assert.Equal(documentId, removedDocumentId);

            documentContentRepositoryMock.Verify(p => p.RemoveAllAsync(It.IsAny<string>()), Times.Once);
        }
    }
}