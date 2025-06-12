using Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Domain.Exceptions;

public class DomainException : ArgumentException
{
    public string? FieldName { get; }

    protected DomainException(string message)
        : base(message)
    {
        FieldName = string.Empty;
    }

    protected DomainException(string value, string fieldName)
        : base($"{fieldName}: {value}")
    {
        FieldName = fieldName;
    }
}

public class InvalidEmailFormatException(string email)
    : DomainException($"The email '{email}' is not in a valid format.", nameof(email))
{
    public string Email { get; } = email;
}

public class InvalidPhoneNumberException(string phone)
    : DomainException($"The phone number '{phone}' is not valid.", nameof(phone))
{
    public string Phone { get; } = phone;
}

public class InvalidDocumentNumberException(string document)
    : DomainException($"The document number '{document}' is not valid.", nameof(document))
{
    public string Document { get; } = document;
}

public class RequiredFieldException(string fieldName)
    : DomainException($"The field '{fieldName}' is required and cannot be empty.", fieldName);

public class InvalidEnumValueException(string enumName, string value)
    : DomainException($"The value '{value}' is not valid for the enum '{enumName}'.", enumName)
{
    public string EnumName { get; } = enumName;
    public string Value { get; } = value;
}

public class InvalidDateRangeException(DateTime startDate, DateTime endDate)
    : DomainException(
        $"The end date '{endDate}' cannot be earlier than the start date '{startDate}'.",
        nameof(endDate)
    )
{
    public DateTime StartDate { get; } = startDate;
    public DateTime EndDate { get; } = endDate;
}

public class InvalidHourlyRateException(decimal hourlyRate)
    : DomainException(
        $"The hourly rate '{hourlyRate}' must be greater than zero.",
        nameof(hourlyRate)
    )
{
    public decimal HourlyRate { get; } = hourlyRate;
}

public class InvalidValueToTransaction(decimal value)
    : DomainException(
        $"The value of transaction must be greater than zero, value is '{value}'",
        nameof(value)
    )
{
    public decimal value { get; } = value;
}

public class TransactionAlreadyCancelledException(bool isCanceled)
    : DomainException(
        $"The transaction has already been cancelled",
        nameof(isCanceled)
    )
{
    public bool isCanceled { get; } = isCanceled;
}

public class DestinationInvalidException(string accountid)
    : DomainException(
        $"Account source and account destinando must be diferentes",
        nameof(accountid)
    )
{
    public string accountid { get; } = accountid;
}

public class InvalidCancellationDateException(DateTime date)
    : DomainException(
        $"Cancellation transaction with a past date are not allowed, date of transaction was created is '{date}'",
        nameof(date)
    )
{
    public DateTime date { get; } = date;
}

public class InvalidRemoveBalanceAccountException(decimal Value)
    : DomainException(
        $"The account does not have sufficient funds to perform this operation.Attempted withdrawal amount:'{Value}'",
        nameof(Value)
    )
{
    public decimal Value { get; } = Value;
}

public class InvalidUpdateBalanceAccountException(decimal value)
    : DomainException(
        $"The value to update the account balance must be greater than 0. Received: '{value}'.",
        nameof(value)
    )
{
    public decimal Value { get; } = value;
}


