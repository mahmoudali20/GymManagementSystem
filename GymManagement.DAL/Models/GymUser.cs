using GymManagement.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Models
{
    public abstract class GymUser : BaseEntity
    {


        public string Name { get; set; } = null!;


        public string Email { get; set; } = null!;


        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        // Owned value object — flattened into the DB row
        public Address Address { get; set; } = null!;

    }

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;
    }

}
