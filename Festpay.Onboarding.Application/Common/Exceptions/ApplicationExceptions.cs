using Festpay.Onboarding.Application.Common.Constants;
using FluentValidation.Results;

namespace Festpay.Onboarding.Application.Common.Exceptions;

public class ApplicationExceptions : Exception
{
    public ApplicationExceptions(string message)
        : base(message) { }

    public ApplicationExceptions(string message, Exception innerException)
        : base(message, innerException) { }
}

public class NotFoundException(string entityName)
    : ApplicationExceptions(string.Format(ErrorMessageConstants.NotFound, entityName))
{ }

public class ValidationException(IEnumerable<ValidationFailure> failures)
    : ApplicationExceptions(string.Join(" / ", failures.Select(f => f.ErrorMessage)))
{
    public IEnumerable<ValidationFailure> Failures { get; } = failures;
}

public class EntityAlreadyExistsException(string entityName)
    : ApplicationExceptions(
        string.Format(ErrorMessageConstants.EntityAlreadyExists, entityName)
    )
{ }

public class TransactionDoesNotBelongToAccountException(string idAccount)
    : ApplicationException(
        $"The transaction does not belong to the account with ID '{idAccount}'."
    )
{
    public string IdAccount { get; } = idAccount;
}
