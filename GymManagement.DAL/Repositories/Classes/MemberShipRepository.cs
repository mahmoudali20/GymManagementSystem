using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<Membership>, IMemberShipRepository
    {

        private readonly GymDbContext _dbContext;
        public MemberShipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Membership>> GetMembershipsWithMemberAndPlan()
        {
            var memberships = _dbContext.Memberships.AsNoTracking().Include(m => m.Member).Include(m => m.Plan);
            return await memberships.ToListAsync();
        }
    }
}
