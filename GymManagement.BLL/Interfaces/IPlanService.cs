using GymManagement.BLL.Commn;
using GymManagement.BLL.ViewModels.Plans;

namespace GymManagement.BLL.Interfaces
{
    public interface IPlanService
    {

        Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result<PlanViewModel>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);
        Task<Result<UpdatePlanViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> ToggleStatusAsync(int id, CancellationToken ct);

    }
}
