using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.MemberShips;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model)
        {

            var member = await _unitOfWork.GetRepo<Member>().GetByIdAsync(model.MemberId);
            if (member == null) return Result.Validation("Member Not Found");

            var plan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(model.PlanId);
            if (plan == null) return Result.Validation("Plan Not Found");

            var hasActiveMembership = await _unitOfWork.MemberShipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now);
            if (hasActiveMembership) return Result.Validation("Member already has an active membership");

            if (!plan.IsActive) return Result.Validation("Plan is inactive");

            var membership = _mapper.Map<Membership>(model);
            var startDate = model.StartDate ?? DateTime.Now;
            membership.CreatedAt = startDate;
            membership.EndDate = startDate.AddDays(plan.DurationDays);


            await _unitOfWork.MemberShipRepository.AddAsync(membership);
            var result = await _unitOfWork.SaveChangesAsync();


            return result > 0 ? Result.OK() : Result.Fail("Failed to create membership");
        }



        public async Task<IEnumerable<MemberShipViewModel>> GetMembershipsAsync()
        {
            var memberships = await _unitOfWork.MemberShipRepository.GetMembershipsWithMemberAndPlan();
            if (!memberships.Any())
                return [];

            var membershipViewModels = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
            return membershipViewModels;
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepo<Member>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);

        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDown(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepo<Plan>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<Result> DeleteMembershipAsync(int id)
        {
            var memberShip = await _unitOfWork.MemberShipRepository.FirstOrDefaultAsync(d => d.MemberId == id);
            if (memberShip == null) return Result.NotFound();

            if (memberShip.EndDate <= DateTime.Now)
                return Result.Validation("Membership cannot be deleted because it is inactive.");


            _unitOfWork.MemberShipRepository.Delete(memberShip);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to Delete membership");
        }
    }
}
