using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {

        Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory(CancellationToken ct = default); // Get all sessions with their associated trainer and category
        Task<Session?> GetSessionWithTrainerAndCategory(int sessionid, CancellationToken ct = default); // Get a specific session by ID with its associated trainer and category
        Task<int> CountOfBookedSlotsAsync(int sessionid, CancellationToken ct = default); // Get the count of booked slots for a specific session



    }
}
