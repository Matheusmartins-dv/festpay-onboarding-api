using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Common.Models;
using Festpay.Onboarding.Infra.Context;
using FluentValidation;
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

public sealed record GetTransactionQueryResponse(
    Guid Id,
    string SourceAccountId,
    string DestinationaccountId,
    decimal Value,
    bool IsCanceled,
    DateTime CreatedAt,
    DateTime? DeactivatedAt
);

public sealed record GetTransactionsQuery : IRequest<ICollection<GetTransactionQueryResponse>>;

public sealed class GetTransactionCommandHandler(FestpayContext dbContext) : IRequestHandler<GetTransactionsQuery, ICollection<GetTransactionQueryResponse>>
{
    public async Task<ICollection<GetTransactionQueryResponse>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await dbContext.Transactions.ToListAsync(cancellationToken);

        return transactions
            .Select(a => new GetTransactionQueryResponse(
                a.Id,
                a.SourceAccountId,
                a.DestinationAccountId,
                a.Value,
                a.IsCanceled,
                a.CreatedUtc,
                a.DeactivatedUtc
            ))
            .ToList();
    }
}

public sealed class GetTransactionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetTransactionsQuery());
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}
