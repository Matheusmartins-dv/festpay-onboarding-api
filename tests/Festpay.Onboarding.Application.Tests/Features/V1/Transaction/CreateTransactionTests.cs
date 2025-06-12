using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Features.V1.Transact;
using Festpay.Onboarding.Application.Features.V1;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Transaction;

public class CreateTransactionCommandHandlerTests
{
    private readonly DbContextOptions<FestpayContext> _dbOptions;

    public CreateTransactionCommandHandlerTests()
    {
        _dbOptions = new DbContextOptionsBuilder<FestpayContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Source_Or_Destination_Account_Does_Not_Exist()
    {
        // Arrange
        using var context = new FestpayContext(_dbOptions);
        var command = new CreateTransactionCommand(
            SourceAccountId: "account-source-123",
            DestinationAccountId: "account-destination-456",
            Value: 1.00m
        );
        var handler = new CreateTransactionCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None)
        );

        Assert.Contains("account-source-123", exception.Message); 
    }
}
