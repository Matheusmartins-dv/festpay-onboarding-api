using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Common.Models;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festpay.Onboarding.Application.Features.V1;

public sealed record CancelTransactionCommand(
    string IdTransaction,
    string SourceAccountId
) : IRequest<bool>;

public sealed class CancelTransactionCommandValidator : AbstractValidator<CancelTransactionCommand>
{
    public CancelTransactionCommandValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty().WithMessage("Id account source is required.");

        RuleFor(x => x.IdTransaction)
            .NotEmpty()
            .WithMessage("Id transaction is required");
    }
}

public sealed class CancelTransactionCommandHandler(FestpayContext dbContext) : IRequestHandler<CancelTransactionCommand, bool>
{
    public async Task<bool> Handle(
        CancelTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await dbContext.Transactions.FindAsync(Guid.Parse(request.IdTransaction));

        if (transaction == null)
            throw new NotFoundException("Transaction");
        if (transaction.SourceAccountId != request.SourceAccountId)
            return false;

        var sourceAccount = await dbContext.Accounts.FindAsync(Guid.Parse(request.SourceAccountId))
            ?? throw new NotFoundException("Conta");

        var destinationAccount = await dbContext.Accounts.FindAsync(Guid.Parse(transaction.DestinationAccountId))
            ?? throw new NotFoundException("Conta");

        sourceAccount.Deposit(transaction.Value);
        destinationAccount.Withdraw(transaction.Value);
        transaction.CanceledTransaction();

        dbContext.Transactions.Update(transaction);
        dbContext.Accounts.Update(destinationAccount);
        dbContext.Accounts.Update(sourceAccount);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

public sealed class CancelTransactionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Transaction}{EndpointConstants.Cancel}",
            async ([FromServices] ISender sender, [FromBody] CancelTransactionCommand command) =>
            {
                var result = await sender.Send(command);
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}
