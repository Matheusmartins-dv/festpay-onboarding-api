using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Common.Models;
using Festpay.Onboarding.Infra.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festpay.Onboarding.Application.Features.V1.Transact;


public sealed record FindTransactionQueryResponse(
    Guid Id,
    string SourceAccountId,
    string DestinationAccountId,
    decimal Value,
    bool IsCanceled,
    DateTime CreatedAt,
    DateTime? DeactivatedAt
);

public sealed record FindTransactionCommand(
    Guid IdTransaction,
    Guid AccountId
) : IRequest<FindTransactionQueryResponse>;


public sealed class FindTransactionCommandHandler(FestpayContext dbContext) : IRequestHandler<FindTransactionCommand, FindTransactionQueryResponse>
{
    public async Task<FindTransactionQueryResponse> Handle(
        FindTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await dbContext.Transactions.FindAsync(request.IdTransaction);

        if (transaction is null)
            throw new Exception("Transaction not found");

        if (transaction.DestinationAccountId == request.AccountId.ToString() || transaction.SourceAccountId == request.AccountId.ToString())
        {
            return new FindTransactionQueryResponse(
                transaction.Id,
                transaction.SourceAccountId,
                transaction.DestinationAccountId,
                transaction.Value,
                transaction.IsCanceled,
                transaction.CreatedUtc,
                transaction.DeactivatedUtc
                );
        }
        throw new TransactionDoesNotBelongToAccountException(request.AccountId.ToString());
    }
}

public sealed class FindTransactionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{idTransaction:guid}}{EndpointConstants.Account}/{{IdAccount:guid}}",
            async ([FromServices] ISender sender, [FromRoute] Guid idTransaction, [FromRoute] Guid IdAccount) =>
            {
                var command = new FindTransactionCommand(idTransaction, IdAccount);
                var result = await sender.Send(command);
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}
