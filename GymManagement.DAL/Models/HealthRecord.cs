namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {



        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }


        public int MemberId { get; set; }
        public Member Member { get; set; } = null;

    }
}
