using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeIsMoney.DataAccess.Entities;

namespace TimeIsMoney.DataAccess.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder
                .HasKey(x => x.Id);

            builder.Property(x => x.Lastname).IsRequired();
            builder.Property(x => x.Firstname).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Salt).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.HasOne(x => x.ConfirmationCode).WithOne(x => x.User);
            builder.HasMany(x => x.Transactions).WithOne(x => x.Owner);
        }
    }
}