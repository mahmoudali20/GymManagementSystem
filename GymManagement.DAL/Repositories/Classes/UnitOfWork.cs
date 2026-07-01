using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GymDbContext _dbContext;

        private readonly Dictionary<Type, object> _repositories = []; // repositories Cache  
        public UnitOfWork(GymDbContext dbcontext, ISessionRepository sessionRepository, IMemberShipRepository memberShipRepository)
        {
            _dbContext = dbcontext;
            SessionRepository = sessionRepository;
            MemberShipRepository = memberShipRepository;
        }
        public ISessionRepository SessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }


        public IGenericRepository<T> GetRepo<T>() where T : BaseEntity, new()
        {

            //Check if repository already exists in the dictionary or not  ?? IDctionary will create it above
            // Like IGenericRepository<Member>   => need Name
            var type = typeof(T);
            // if exist in dictionary => use it 

            if (_repositories.TryGetValue(type, out object? value))
            {
                return (IGenericRepository<T>)value;
            }

            // if not exist => create repo => Add dictionary => retuen repo
            else
            {
                var repo = new GenericRepository<T>(_dbContext);
                _repositories[type] = repo;
                return repo;
            }
        }

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
