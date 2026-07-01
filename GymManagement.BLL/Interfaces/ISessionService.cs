using GymManagement.BLL.Commn;
using GymManagement.BLL.ViewModels.Sessions;

namespace GymManagement.BLL.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel session, CancellationToken ct = default);
        Task<Result> DeleteAsync(int sessionId, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetForUpdateAsync(int sessionId, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDown(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryrForDropDown(CancellationToken ct = default);
    }
}
