using GymManagement.BLL.Commn;
using GymManagement.BLL.ViewModels.Members;

namespace GymManagement.BLL.Interfaces
{
    public interface IMemberService
    {

        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default);

        Task<Result> createMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);

        Task<Result<MemberViewModel>> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default);

        Task<Result<HealthRecordViewModel?>> GetMemberHealthRecord(int MemberId, CancellationToken ct = default);

        Task<Result> DeleteAsync(int memberId, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result<MemberToUpdateViewModel>> GetForUpdateAsync(int memberId, CancellationToken ct = default);
    }
}
