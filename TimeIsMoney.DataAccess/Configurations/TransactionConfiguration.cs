using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeIsMoney.DataAccess.Entities;

namespace TimeIsMoney.DataAccess.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Comment);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.HasOne(x => x.Owner).WithMany(x => x.Transactions);
            builder.HasOne(x => x.Type);
        }
    }
}