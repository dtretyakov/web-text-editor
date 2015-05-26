using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using WebTextEditor.BLL.Services;
using WebTextEditor.BLL.Tests.Mocks;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Domain.DTO;
using WebTextEditor.Domain.Exceptions;
using Xunit;

namespace WebTextEditor.BLL.Tests.Services
{
    public sealed class DocumentServiceTests
    {
        [Fact]
        public async Task GetDocumentTest()
        {
            var documentEntity = new DocumentEntity
            {
                Id = "Id",
                Created = new DateTime(2015, 05, 25),
                Name = "Document Name",
                UserId = "UserId"
            };

            var content = new Dictionary<string, string>
            {
                {"1", "a"},
                {"2", "b"},
                {"3", "c"}
            };

            var collaborators = new List<DocumentCollaborator>
            {
                new DocumentCollaborator {Id = 1, CaretPosition = 1, ConnectionId = "1", DocumentId = "1", UserId = "1"},
                new DocumentCollaborator {Id = 2, CaretPosition = 2, ConnectionId = "2", DocumentId = "2", UserId = "2"},
                new DocumentCollaborator {Id = 3, CaretPosition = 3, ConnectionId = "3", DocumentId = "3", UserId = "3"}
            };

            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(documentEntity));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            documentContentServiceMock.Setup(p => p.GetCurrentContentAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(content));

            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();
            documentCollaboratorServiceMock.Setup(p => p.FindByDocumentAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(collaborators));

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            var document = await documentService.GetAsync("1");

            Assert.NotNull(document);
            Assert.NotNull(document.Content);
            Assert.NotNull(document.Collaborators);

            Assert.Equal(documentEntity.Id, document.Id);
            Assert.Equal(documentEntity.Name, document.Name);
            Assert.Equal(documentEntity.Created, document.Created);
            Assert.Equal(content, document.Content);
            Assert.Equal(collaborators, document.Collaborators);

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentContentServiceMock.Verify(p => p.GetCurrentContentAsync(It.IsAny<string>()), Times.Once);
            documentCollaboratorServiceMock.Verify(p => p.FindByDocumentAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetDocumentByInvalidIdTest()
        {
            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult((DocumentEntity) null));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            documentContentServiceMock.Setup(p => p.GetCurrentContentAsync(It.IsAny<string>()));

            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();
            documentCollaboratorServiceMock.Setup(p => p.FindByDocumentAsync(It.IsAny<string>()));

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await Assert.ThrowsAsync<NotFoundException>(async () => await documentService.GetAsync("1"));

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentContentServiceMock.Verify(p => p.GetCurrentContentAsync(It.IsAny<string>()), Times.Never);
            documentCollaboratorServiceMock.Verify(p => p.FindByDocumentAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetDocumentsListTest()
        {
            var documentEntities = new List<DocumentEntity>
            {
                new DocumentEntity {Id = "1"},
                new DocumentEntity {Id = "2"},
                new DocumentEntity {Id = "3"}
            };

            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(p => p.GetAllAsync())
                .Returns(() => Task.FromResult(documentEntities));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            var documents = await documentService.GetListAsync();

            Assert.NotNull(documents);
            Assert.Equal(documentEntities.Count, documents.Count);
            Assert.Equal(documentEntities.Select(p => p.Id), documents.Select(p => p.Id));

            documentRepositoryMock.Verify(p => p.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddDocumentTest()
        {
            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<DocumentEntity>()))
                .Returns(() => Task.FromResult(0));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            const string userId = "userId";
            var dateBefore = DateTime.UtcNow;

            var document = await documentService.AddAsync(userId);

            Assert.NotNull(document);
            Assert.Equal(userId, document.UserId);
            Assert.Equal("New Document", document.Name);
            Assert.False(string.IsNullOrEmpty(document.Id));
            Assert.True(dateBefore < document.Created);
            Assert.True(document.Created < DateTime.UtcNow);

            documentRepositoryMock.Verify(p => p.AddAsync(It.IsAny<DocumentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentTest()
        {
            const string userId = "userId";
            const string documentId = "documentId";
            var previousName = "Previous name";
            var newName = "New name";

            var documentEntity = new DocumentEntity
            {
                Id = documentId,
                UserId = userId,
                Name = previousName
            };

            var document = new Document
            {
                Id = documentId,
                UserId = userId,
                Name = newName
            };

            DocumentEntity updatedDocument = null;
            string requestedDocumentId = null;

            var documentRepositoryMock = new Mock<IDocumentRepository>();

            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Callback((string id) => { requestedDocumentId = id; })
                .Returns(() => Task.FromResult(documentEntity));

            documentRepositoryMock.Setup(p => p.UpdateAsync(It.IsAny<DocumentEntity>()))
                .Callback((DocumentEntity doc) => { updatedDocument = doc; })
                .Returns(() => Task.FromResult(0));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await documentService.UpdateAsync(document);

            Assert.Equal(documentId, requestedDocumentId);
            Assert.NotNull(updatedDocument);
            Assert.Equal(newName, updatedDocument.Name);

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentRepositoryMock.Verify(p => p.UpdateAsync(It.IsAny<DocumentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentWithInvalidIdTest()
        {
            var document = new Document();

            var documentRepositoryMock = new Mock<IDocumentRepository>();

            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult((DocumentEntity) null));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await Assert.ThrowsAsync<NotFoundException>(async () => await documentService.UpdateAsync(document));

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentRepositoryMock.Verify(p => p.UpdateAsync(It.IsAny<DocumentEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteDocumentTest()
        {
            var userId = "userId";
            var documentId = "docmentId";

            DocumentEntity removedDocument = null;
            string requestedDocumentId = null;
            string removedDocumentId = null;

            var documentRepositoryMock = new Mock<IDocumentRepository>();

            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Callback((string id) => { requestedDocumentId = id; })
                .Returns((string id) => Task.FromResult(new DocumentEntity {Id = id, UserId = userId}));

            documentRepositoryMock.Setup(p => p.RemoveAsync(It.IsAny<DocumentEntity>()))
                .Callback((DocumentEntity doc) => { removedDocument = doc; })
                .Returns(() => Task.FromResult(0));

            var documentContentServiceMock = new Mock<IDocumentContentService>();

            documentContentServiceMock.Setup(p => p.RemoveAllAsync(It.IsAny<string>()))
                .Callback((string id) => { removedDocumentId = id; })
                .Returns(() => Task.FromResult(0));

            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await documentService.DeleteAsync(userId, documentId);

            Assert.Equal(documentId, requestedDocumentId);
            Assert.Equal(documentId, removedDocumentId);
            Assert.Equal(documentId, removedDocument.Id);
            Assert.Equal(userId, removedDocument.UserId);

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<DocumentEntity>()), Times.Once);
            documentContentServiceMock.Verify(p => p.RemoveAllAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDocumentWithInvalidIdTest()
        {
            var userId = "userId";
            var documentId = "docmentId";

            var documentRepositoryMock = new Mock<IDocumentRepository>();

            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult((DocumentEntity) null));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await Assert.ThrowsAsync<NotFoundException>(async () => await documentService.DeleteAsync(userId, documentId));

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<DocumentEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteDocumentMadeByOtherUserTest()
        {
            var userId = "userId";
            var otherUserId = "otherUserId";
            var documentId = "docmentId";

            var documentRepositoryMock = new Mock<IDocumentRepository>();

            documentRepositoryMock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(new DocumentEntity {UserId = otherUserId}));

            var documentContentServiceMock = new Mock<IDocumentContentService>();
            var documentCollaboratorServiceMock = new Mock<IDocumentCollaboratorService>();

            var documentService = new DocumentService(
                documentRepositoryMock.Object,
                documentContentServiceMock.Object,
                documentCollaboratorServiceMock.Object,
                MockObjects.GetMapper());

            await Assert.ThrowsAsync<ForiddenException>(async () => await documentService.DeleteAsync(userId, documentId));

            documentRepositoryMock.Verify(p => p.GetAsync(It.IsAny<string>()), Times.Once);
            documentRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<DocumentEntity>()), Times.Never);
        }
    }
}