using Festpay.Onboarding.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festpay.Onboarding.Infra.Configurations;
public class TransactionConfiguration : ConfigurationBase<Transaction>, IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        ConfigureEntityBase(builder);
    }
}
