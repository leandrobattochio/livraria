// using System.Net;
// using Livraria.Domain.Shared.Enum;
// using Moq;
//
// namespace Livraria.Tests.Services.Queries.Users;
//
// public class GetUserQueryHandlerTests(GetUserQueryHandlerFixture fixture) : IClassFixture<GetUserQueryHandlerFixture>
// {
//     [Fact]
//     public async Task GetUserQueryHandler_ShouldReturnUser()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var query = new GetUserQuery(userId);
//         query.SetCallerInfo(new CallerInfo(userId, UserRole.SaaSAdmin, null, null));
//
//         fixture.UserRepositoryMock.Setup(c => c.GetUserReadOnlyAsync(userId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new UserReadModel(userId, "test@test.com", "FirstName", "LastName", UserRole.SaaSAdmin,
//                 null, null, UserRegistrationStatus.Complete));
//
//         // Act
//         var result = await fixture.Handler.Handle(query, CancellationToken.None);
//
//         // Assert
//         result.IsSuccess.ShouldBeTrue();
//         result.StatusCode.ShouldBe(HttpStatusCode.OK);
//         result.Data.ShouldNotBeNull();
//         result.Data.Id.ShouldBe(userId);
//         result.Data.UserRole.ShouldBe(UserRole.SaaSAdmin);
//         result.Data.Email.ShouldBe("test@test.com");
//         result.Data.ClicheriaId.ShouldBeNull();
//         result.Data.CompanyId.ShouldBeNull();
//         
//         fixture.UserRepositoryMock.Verify(c => c.GetUserReadOnlyAsync(userId, CancellationToken.None), Times.Once);
//     }
//     
//     [Fact]
//     public async Task GetUserQueryHandler_InvalidUserId_ShouldReturnNotFound()
//     {
//         // Arrange
//         var userId = Guid.NewGuid();
//         var wrongUserId = Guid.NewGuid();
//         
//         var query = new GetUserQuery(wrongUserId);
//         query.SetCallerInfo(new CallerInfo(userId, UserRole.SaaSAdmin, null, null));
//
//         fixture.UserRepositoryMock.Setup(c => c.GetUserReadOnlyAsync(userId, It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new UserReadModel(userId, "test@test.com", "FirstName", "LastName", UserRole.SaaSAdmin,
//                 null, null, UserRegistrationStatus.Complete));
//
//         // Act
//         var result = await fixture.Handler.Handle(query, CancellationToken.None);
//
//         // Assert
//         result.IsSuccess.ShouldBeTrue();
//         result.Data.ShouldBeNull();
//         result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//         
//         fixture.UserRepositoryMock.Verify(c => c.GetUserReadOnlyAsync(wrongUserId, CancellationToken.None), Times.Once);
//     }
// }