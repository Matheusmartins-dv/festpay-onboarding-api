using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Festpay.Onboarding.Domain.Exceptions;
using Festpay.Onboarding.Domain.Extensions;

namespace Festpay.Onboarding.Domain.Entities;
public class Transaction: EntityBase
{
    public string SourceAccountId { get; private set; } = string.Empty;
    public string DestinationAccountId { get; private set; } = string.Empty;

    public decimal Value { get; private set; } = 0;
    public Boolean IsCanceled { get; private set; } = false;

    public override void Validate()
    {
        if (!Value.isValidValueTransaction())
            throw new InvalidValueToTransaction(Value);

        if (SourceAccountId.SourceIsEqualDestinationAccount(DestinationAccountId))
            throw new DestinationInvalidException(SourceAccountId);
    }
    public void CanceledTransaction()
    {
        if (DateTime.UtcNow > CreatedUtc)
        {
            if (IsCanceled)
                throw new TransactionAlreadyCancelledException(IsCanceled);
            IsCanceled = true;
            return;
        }
        throw new InvalidCancellationDateException(DateTime.UtcNow);
    }

    public class Builder
    {
        private readonly Transaction _transaction = new Transaction();
        public Builder WithValue(decimal value)
        {
            _transaction.Value = value;
            return this;
        }
        public Builder WithSourceAccount(string accountId)
        {
            _transaction.SourceAccountId = accountId;
            return this;
        }
        public Builder WithDestinationAccount(string accountId)
        {
            _transaction.DestinationAccountId = accountId;
            return this;
        }
        public Transaction Build()
        {
            _transaction.Validate();
            _transaction.IsCanceled = false;
            return _transaction;
        }
    }

    public static Transaction Restore(
        Guid id,
        string sourceAccountId,
        string destinationAccountId,
        decimal value,
        bool isCanceled,
        DateTime createdUtc,
        DateTime? deactivatedUtc
    )
    {
        var transaction = new Transaction
        {
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            Value = value,
            IsCanceled = isCanceled,
        };

        transaction.RestoreBase(id, createdUtc, deactivatedUtc);
        return transaction;
    }
}
