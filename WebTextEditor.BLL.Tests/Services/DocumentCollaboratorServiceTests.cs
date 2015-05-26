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
    public sealed class DocumentCollaboratorServiceTests
    {
        [Fact]
        public async Task AddCollaboratorTest()
        {
            var collaborator = new DocumentCollaborator
            {
                Id = 1,
                UserId = "userId",
                DocumentId = "documentId",
                CaretPosition = 2,
                ConnectionId = "3"
            };

            DocumentCollaboratorEntity addedEntity = null;

            var collaboratorRepositoryMock = new Mock<IDocumentCollaboratorRepository>();
            collaboratorRepositoryMock.Setup(p => p.AddAsync(It.IsAny<DocumentCollaboratorEntity>()))
                .Callback((DocumentCollaboratorEntity entity) => { addedEntity = entity; })
                .Returns(() => Task.FromResult(0));

            var collaboratorService = new DocumentCollaboratorService(collaboratorRepositoryMock.Object, MockObjects.GetMapper());

            await collaboratorService.AddAsync(collaborator);

            Assert.NotNull(addedEntity);
            Assert.Equal(collaborator.Id, addedEntity.Id);
            Assert.Equal(collaborator.CaretPosition, addedEntity.CaretPosition);
            Assert.Equal(collaborator.ConnectionId, addedEntity.ConnectionId);
            Assert.Equal(collaborator.DocumentId, addedEntity.DocumentId);
            Assert.Equal(collaborator.UserId, addedEntity.UserId);

            collaboratorRepositoryMock.Verify(p => p.AddAsync(It.IsAny<DocumentCollaboratorEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCollaboratorTest()
        {
            var collaborator = new DocumentCollaborator
            {
                Id = 1,
                UserId = "userId",
                DocumentId = "documentId",
                CaretPosition = 2,
                ConnectionId = "3"
            };

            DocumentCollaboratorEntity updatedEntity = null;

            var collaboratorRepositoryMock = new Mock<IDocumentCollaboratorRepository>();
            collaboratorRepositoryMock.Setup(p => p.UpdateAsync(It.IsAny<DocumentCollaboratorEntity>()))
                .Callback((DocumentCollaboratorEntity entity) => { updatedEntity = entity; })
                .Returns(() => Task.FromResult(0));

            var collaboratorService = new DocumentCollaboratorService(collaboratorRepositoryMock.Object, MockObjects.GetMapper());

            await collaboratorService.UpdateAsync(collaborator);

            Assert.NotNull(updatedEntity);
            Assert.Equal(collaborator.Id, updatedEntity.Id);
            Assert.Equal(collaborator.CaretPosition, updatedEntity.CaretPosition);
            Assert.Equal(collaborator.ConnectionId, updatedEntity.ConnectionId);
            Assert.Equal(collaborator.DocumentId, updatedEntity.DocumentId);
            Assert.Equal(collaborator.UserId, updatedEntity.UserId);

            collaboratorRepositoryMock.Verify(p => p.UpdateAsync(It.IsAny<DocumentCollaboratorEntity>()), Times.Once);
        }

        [Fact]
        public async Task RemoveCollaboratorTest()
        {
            var collaborator = new DocumentCollaborator
            {
                Id = 1,
                UserId = "userId",
                DocumentId = "documentId",
                CaretPosition = 2,
                ConnectionId = "3"
            };

            DocumentCollaboratorEntity removedEntity = null;

            var collaboratorRepositoryMock = new Mock<IDocumentCollaboratorRepository>();
            collaboratorRepositoryMock.Setup(p => p.RemoveAsync(It.IsAny<DocumentCollaboratorEntity>()))
                .Callback((DocumentCollaboratorEntity entity) => { removedEntity = entity; })
                .Returns(() => Task.FromResult(0));

            var collaboratorService = new DocumentCollaboratorService(collaboratorRepositoryMock.Object, MockObjects.GetMapper());

            await collaboratorService.RemoveAsync(collaborator);

            Assert.NotNull(removedEntity);
            Assert.Equal(collaborator.Id, removedEntity.Id);
            Assert.Equal(collaborator.CaretPosition, removedEntity.CaretPosition);
            Assert.Equal(collaborator.ConnectionId, removedEntity.ConnectionId);
            Assert.Equal(collaborator.DocumentId, removedEntity.DocumentId);
            Assert.Equal(collaborator.UserId, removedEntity.UserId);

            collaboratorRepositoryMock.Verify(p => p.RemoveAsync(It.IsAny<DocumentCollaboratorEntity>()), Times.Once);
        }

        [Fact]
        public async Task FindCollaboratorByConnectionTest()
        {
            var connectionId = "connectionId";

            string requestedConnectionId = null;

            var collaboratorRepositoryMock = new Mock<IDocumentCollaboratorRepository>();
            collaboratorRepositoryMock.Setup(p => p.FindByConnectionAsync(It.IsAny<string>()))
                .Callback((string id) => { requestedConnectionId = id; })
                .Returns((string id) => Task.FromResult(new List<DocumentCollaboratorEntity>
                {
                    new DocumentCollaboratorEntity {ConnectionId = id}
                }));

            var collaboratorService = new DocumentCollaboratorService(collaboratorRepositoryMock.Object, MockObjects.GetMapper());

            var collaborators = await collaboratorService.FindByConnectionAsync(connectionId);

            Assert.Equal(connectionId, requestedConnectionId);
            Assert.Equal(connectionId, collaborators.Single().ConnectionId);

            collaboratorRepositoryMock.Verify(p => p.FindByConnectionAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task FindCollaboratorByDocumentTest()
        {
            var documentId = "documentId";

            string requestedDocumentId = null;

            var collaboratorRepositoryMock = new Mock<IDocumentCollaboratorRepository>();
            collaboratorRepositoryMock.Setup(p => p.FindByDocumentAsync(It.IsAny<string>()))
                .Callback((string id) => { requestedDocumentId = id; })
                .Returns((string id) => Task.FromResult(new List<DocumentCollaboratorEntity>
                {
                    new DocumentCollaboratorEntity {DocumentId = id}
                }));

            var collaboratorService = new DocumentCollaboratorService(collaboratorRepositoryMock.Object, MockObjects.GetMapper());

            var collaborators = await collaboratorService.FindByDocumentAsync(documentId);

            Assert.Equal(documentId, requestedDocumentId);
            Assert.Equal(documentId, collaborators.Single().DocumentId);

            collaboratorRepositoryMock.Verify(p => p.FindByDocumentAsync(It.IsAny<string>()), Times.Once);
        }
    }
}