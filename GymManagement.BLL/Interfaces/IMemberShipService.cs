using GymManagement.BLL.Commn;
using GymManagement.BLL.ViewModels.MemberShips;

namespace GymManagement.BLL.Interfaces
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MemberShipViewModel>> GetMembershipsAsync();
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDown(CancellationToken ct = default);
        Task<Result> DeleteMembershipAsync(int id);
    }
}
