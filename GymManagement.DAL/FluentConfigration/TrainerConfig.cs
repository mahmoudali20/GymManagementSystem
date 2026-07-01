using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.FluentConfigration
{
    public class TrainerConfig : GymUserConfig<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);


            builder.Property(M => M.CreatedAt)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GetDate()");


        }
    }
}
