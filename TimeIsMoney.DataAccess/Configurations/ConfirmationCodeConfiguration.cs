using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeIsMoney.DataAccess.Entities;

namespace TimeIsMoney.DataAccess.Configurations
{
    public class ConfirmationCodeConfiguration : IEntityTypeConfiguration<ConfirmationCodeEntity>
    {
        public void Configure(EntityTypeBuilder<ConfirmationCodeEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.HasOne(x => x.User).WithOne(x => x.ConfirmationCode);
        }
    }
}