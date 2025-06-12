using Festpay.Onboarding.Domain.Exceptions;
using Festpay.Onboarding.Domain.Extensions;

namespace Festpay.Onboarding.Domain.Entities;

public class Account : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public decimal Balance { get; private set; } = 0;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new RequiredFieldException(nameof(Name));

        if (!Document.IsValidDocument())
            throw new InvalidDocumentNumberException(Document);

        if (!Email.IsValidEmail())
            throw new InvalidEmailFormatException(Email);

        if (!Phone.IsValidPhone())
            throw new InvalidPhoneNumberException(Phone);
    }
    
    public void Deposit(decimal value)
    {
        if (value < 0)
            throw new InvalidUpdateBalanceAccountException(value);
        Balance += value;
    }

    public void Withdraw(decimal value)
    {
        if(value<0)
            throw new InvalidUpdateBalanceAccountException(value);
        if (Balance<value)
            throw new InvalidRemoveBalanceAccountException(value);
        Balance -= value;
    }
    public class Builder
    {
        private readonly Account _account = new();

        public Builder WithName(string name)
        {
            _account.Name = name;
            return this;
        }

        public Builder WithDocument(string document)
        {
            _account.Document = document;
            return this;
        }

        public Builder WithEmail(string email)
        {
            _account.Email = email;
            return this;
        }

        public Builder WithPhone(string phone)
        {
            _account.Phone = phone;
            return this;
        }

        public Account Build()
        {
            _account.Validate();
            return _account;
        }
    }
}
