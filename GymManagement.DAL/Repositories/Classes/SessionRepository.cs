using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {


        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountOfBookedSlotsAsync(int sessionid, CancellationToken ct = default) => await _dbContext.Bookings.AsNoTracking().CountAsync(X => X.SessionId == sessionid);


        public async Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory(CancellationToken ct = default)
        {
            var query = _dbContext.Sessions.AsNoTracking().Include(X => X.Trainer).Include(X => X.Category);
            return await query.ToListAsync(ct);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategory(int sessionid, CancellationToken ct = default) => await _dbContext.Sessions.AsNoTracking().Include(X => X.Trainer).Include(X => X.Category).FirstOrDefaultAsync(X => X.Id == sessionid);


    }
}
