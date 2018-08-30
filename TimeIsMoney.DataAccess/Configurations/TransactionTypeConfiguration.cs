using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeIsMoney.DataAccess.Entities;

namespace TimeIsMoney.DataAccess.Configurations
{
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionTypeEntity> builder)
        {
            builder.ToTable("TransactionTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Icon).IsRequired();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}