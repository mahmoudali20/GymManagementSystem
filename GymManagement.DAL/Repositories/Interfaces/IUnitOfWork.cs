using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepo<T>() where T : BaseEntity, new();

        Task<int> SaveChangesAsync();

        public ISessionRepository SessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }
    }
}
