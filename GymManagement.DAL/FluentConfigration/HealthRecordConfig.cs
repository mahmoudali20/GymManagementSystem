using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.FluentConfigration
{
    public class HealthRecordConfig : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(h => h.Height)
                   .HasColumnType("decimal(5,2)");

            builder.Property(h => h.Weight)
                   .HasColumnType("decimal(5,2)");

            builder.Property(h => h.BloodType)
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(h => h.Note)
                   .HasMaxLength(500);


        }
    }
}
