using FluentValidation;
using MediatR;
using Festpay.Onboarding.Domain.Extensions;
using Festpay.Onboarding.Infra.Context;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Domain.Entities;
using Carter;
using Microsoft.AspNetCore.Routing;
using Festpay.Onboarding.Application.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Festpay.Onboarding.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Festpay.Onboarding.Application.Features.V1;


public sealed record UpdateBalanceAccountCommand(
    Guid IdAccount,
    string Operation,
    decimal Value
) : IRequest<bool>;

public sealed record UpdateBalanceAccountBody(
    string Operation,
    decimal Value
) : IRequest<bool>;

public sealed class UpdateBalanceAccountCommandValidator : AbstractValidator<UpdateBalanceAccountCommand>
{
    private static readonly string[] AllowedOperations = { "deposit", "withdraw" };

    public UpdateBalanceAccountCommandValidator()
    {
        RuleFor(x => x.Operation)
            .NotEmpty().WithMessage("Operation is required.")
            .Must(op => AllowedOperations.Contains(op.ToLower()))
            .WithMessage("Operation must be either 'deposit' or 'withdraw'.");

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("Value must be greater than zero.");
    }
}

public sealed class UpdateBalanceAccountCommandHandler(FestpayContext dbContext) : IRequestHandler<UpdateBalanceAccountCommand, bool>
{
    public async Task<bool> Handle(
        UpdateBalanceAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var account =
             await dbContext.Accounts.FindAsync(request.IdAccount)
             ?? throw new NotFoundException("Conta");

        if(request.Operation == "deposit")
            account.Deposit(request.Value);
        if(request.Operation == "withdraw")
            account.Withdraw(request.Value);

        dbContext.Accounts.Update(account);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

public sealed class UpdateBalanceAccountCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{EndpointConstants.V1}{EndpointConstants.Account}/{{id:guid}}{EndpointConstants.Balance}",
            async ([FromServices] ISender sender, [FromBody] UpdateBalanceAccountBody commandBody, [FromRoute] Guid id) =>
            {
                var command = new UpdateBalanceAccountCommand(id, commandBody.Operation, commandBody.Value);
                var result = await sender.Send(command);
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Account);
    }
}

