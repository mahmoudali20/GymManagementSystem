using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.FluentConfigration
{
    public class GymUserConfig<T> : IEntityTypeConfiguration<T> where T : GymUser
    {

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(50).IsRequired();

            builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Phone).HasMaxLength(11).IsRequired();
            builder.HasIndex(u => u.Phone).IsUnique();


            builder.ToTable(t => t.HasCheckConstraint(
                "CK_GymUser_Phone",
                "[Phone] LIKE '010%' OR [Phone] LIKE '011%' " +
                "OR [Phone] LIKE '012%' OR [Phone] LIKE '015%'"));


            builder.OwnsOne(u => u.Address, addr =>
            {
                addr.Property(a => a.Street).HasMaxLength(30).HasColumnName("Street");
                addr.Property(a => a.City).HasMaxLength(30).HasColumnName("City");
                addr.Property(a => a.BuildingNumber).HasMaxLength(30).HasColumnName("BuildingNumber");
            });

        }
    }
}
