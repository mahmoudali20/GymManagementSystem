using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.FluentConfigration
{
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 And 25");
                TB.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");

            });
        }

    }
}
