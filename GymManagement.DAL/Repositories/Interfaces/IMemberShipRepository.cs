using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<Membership>
    {

        Task<IEnumerable<Membership>> GetMembershipsWithMemberAndPlan();

    }
}
