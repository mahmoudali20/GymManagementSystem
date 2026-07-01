using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepo<Plan>().GetAllAsync(ct: ct);
            if (!plans.Any())
                return []; ;
            var planVM = _mapper.Map<IEnumerable<PlanViewModel>>(plans);
            return planVM;
        }

        public async Task<Result<PlanViewModel>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(id, ct: ct);
            if (plan == null) return Result<PlanViewModel>.NotFound("Plan not found");
            var planVM = _mapper.Map<PlanViewModel>(plan);
            return Result<PlanViewModel>.OK(planVM);
        }


        public async Task<Result> ToggleStatusAsync(int id, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return Result.NotFound("Plan not found");
            if (plan.IsActive)
            {
                var hasActiveMemberShips = await _unitOfWork.GetRepo<Membership>().AnyAsync(m => m.PlanId == id && m.EndDate > DateTime.Now, ct);
                if (hasActiveMemberShips) return Result.Fail("Cannot deactivate a plan with active memberships.\"");
            }
            plan.IsActive = !plan.IsActive;
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("");

        }


        public async Task<Result<UpdatePlanViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(id, ct: ct);
            if (plan == null) return Result<UpdatePlanViewModel>.NotFound("Plan not found");

            var planVM = _mapper.Map<UpdatePlanViewModel>(plan);
            return Result<UpdatePlanViewModel>.OK(planVM);
        }



        public async Task<Result> UpdateAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return Result.NotFound("Plan not found");

            if (plan.Name != model.Name) return Result.Fail("Plan name cannot be changed");

            var hasActiveMemberShips = await _unitOfWork.GetRepo<Membership>().AnyAsync(m => m.PlanId == id && m.EndDate > DateTime.Now, ct);
            if (hasActiveMemberShips) return Result.Fail("Cannot update plan with active memberships");

            _mapper.Map(model, plan);
            _unitOfWork.GetRepo<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Update failed");
        }
    }
}
