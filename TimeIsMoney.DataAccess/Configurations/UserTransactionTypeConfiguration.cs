using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeIsMoney.DataAccess.Entities;

namespace TimeIsMoney.DataAccess.Configurations
{
    public class UserTransactionTypeConfiguration : IEntityTypeConfiguration<UserTransactionTypeRefEntity>
    {
        public void Configure(EntityTypeBuilder<UserTransactionTypeRefEntity> builder)
        {
            builder.ToTable("UserTransactionTypes");

            builder.HasKey(x => new { x.UserId, x.TransactionTypeId });

            builder.HasOne(x => x.User).WithMany(x => x.TransactionTypes).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.TransactionType).WithMany(x => x.Users).HasForeignKey(x => x.TransactionTypeId);
        }
    }
}