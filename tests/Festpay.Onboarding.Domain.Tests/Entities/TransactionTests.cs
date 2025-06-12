using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Domain.Exceptions;
using Xunit;

namespace Festpay.Onboarding.Domain.Tests.Entities;

public class TransactionTests
{
    [Fact]
    public void Should_Create_Transaction_When_Data_Is_Valid()
    {
        var transaction = new Transaction.Builder()
            .WithSourceAccount("e1638cdc-3177-4d5a-8b0e-cb9af718b815")
            .WithDestinationAccount("60f57842-a566-40c4-a883-d6c5385ebe6d")
            .WithValue(150.00m)
            .Build();

        Assert.Equal("e1638cdc-3177-4d5a-8b0e-cb9af718b815", transaction.SourceAccountId);
        Assert.Equal("60f57842-a566-40c4-a883-d6c5385ebe6d", transaction.DestinationAccountId);
        Assert.Equal(150.00m, transaction.Value);
        Assert.False(transaction.IsCanceled);
    }

    [Fact]
    public void Should_Throw_InvalidTransactionValueException_When_Value_Is_Negative()
    {
        var exception = Assert.Throws<InvalidValueToTransaction>(() =>
            new Transaction.Builder()
                .WithSourceAccount("e1638cdc-3177-4d5a-8b0e-cb9af718b815")
                .WithDestinationAccount("60f57842-a566-40c4-a883-d6c5385ebe6d")
                .WithValue(-100)
                .Build()
        );

        Assert.Equal(-100.00m, exception.value);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_SourceAccountId_Is_Null()
    {
        var exception = Assert.Throws<RequiredFieldException>(() =>
            new Transaction.Builder()
                .WithSourceAccount(null)
                .WithDestinationAccount("60f57842-a566-40c4-a883-d6c5385ebe6d")
                .WithValue(50)
                .Build()
        );

        Assert.Equal("SourceAccountId", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_DestinationAccountId_Is_Null()
    {
        var exception = Assert.Throws<RequiredFieldException>(() =>
            new Transaction.Builder()
                .WithSourceAccount("e1638cdc-3177-4d5a-8b0e-cb9af718b815")
                .WithDestinationAccount(null!)
                .WithValue(10.00m)
                .Build()
        );

        Assert.Equal("DestinationAccountId", exception.FieldName);
    }
}

