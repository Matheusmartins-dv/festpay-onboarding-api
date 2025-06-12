using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Festpay.Onboarding.Application.Common.Models;

namespace Festpay.Onboarding.Application.Features.V1.Transact;
public sealed record CreateTransactionCommand(
    string SourceAccountId,
    string DestinationAccountId,
    decimal Value
) : IRequest<bool>;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty().WithMessage("Id account source is required.");

        RuleFor(x => x.DestinationAccountId)
            .NotEmpty()
            .WithMessage("Id account destination is required.");

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Value of transaction is required");
    }
}

public sealed class CreateTransactionCommandHandler(FestpayContext dbContext) : IRequestHandler<CreateTransactionCommand, bool>
{
    public async Task<bool> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var sourceAccount = await dbContext.Accounts.FindAsync(Guid.Parse(request.SourceAccountId))
            ?? throw new NotFoundException("Conta");

        var destinationAccount = await dbContext.Accounts.FindAsync(Guid.Parse(request.DestinationAccountId))
            ?? throw new NotFoundException("Conta");

        var transaction = new Transaction.Builder()
            .WithSourceAccount( request.SourceAccountId )
            .WithDestinationAccount( request.DestinationAccountId )
            .WithValue(request.Value)
            .Build();

        sourceAccount.Withdraw(transaction.Value);
        destinationAccount.Deposit(transaction.Value);

        await dbContext.Transactions.AddAsync(transaction, cancellationToken);
        dbContext.Accounts.Update(sourceAccount);
        dbContext.Accounts.Update(destinationAccount);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

public sealed class CreateTransactionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async ([FromServices] ISender sender, [FromBody] CreateTransactionCommand command) =>
            {
                var result = await sender.Send(command);
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}
