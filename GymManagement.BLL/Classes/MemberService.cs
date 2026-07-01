using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> createMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var EmailExist = await _unitOfWork.GetRepo<Member>().AnyAsync(x => x.Email == model.Email);


            var PhoneExist = await _unitOfWork.GetRepo<Member>().AnyAsync(x => x.Phone == model.Phone);

            if (EmailExist)
                return Result.Validation("Email already exists.");

            if (PhoneExist)
                return Result.Validation("Phone already exists.");

            var member = _mapper.Map<Member>(model);

            await _unitOfWork.GetRepo<Member>().AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to create member.");
        }

        public async Task<Result> UpdateAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {

            var EmailExist = await _unitOfWork.GetRepo<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id);

            var PhoneExist = await _unitOfWork.GetRepo<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id);

            if (EmailExist || PhoneExist) return Result.Validation("Email or Phone already exists."); ;

            var member = await _unitOfWork.GetRepo<Member>().GetByIdAsync(id);

            if (member is null) return Result.Fail("Member not found.");

            _mapper.Map(model, member);

            _unitOfWork.GetRepo<Member>().Update(member);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to update member.");
        }

        public async Task<Result<MemberToUpdateViewModel?>> GetForUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepo<Member>().FirstOrDefaultAsync(x => x.Id == memberId, tracking: false, ct);

            if (member == null) return Result<MemberToUpdateViewModel>.Fail("Member not found.");

            var model = _mapper.Map<MemberToUpdateViewModel>(member);

            return Result<MemberToUpdateViewModel>.OK(model);

        }



        public async Task<Result> DeleteAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepo<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return Result.Fail("Member not found.");
            _unitOfWork.GetRepo<Member>().Delete(member);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete member.");
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {

            var Members = await _unitOfWork.GetRepo<Member>().GetAllAsync(ct: ct);

            if (!Members.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberViewModel>>(Members);

        }

        public async Task<Result<MemberViewModel>> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {

            var member = await _unitOfWork.GetRepo<Member>().GetByIdAsync(MemberId, ct);

            if (member == null) return Result<MemberViewModel>.Fail("Member not found.");

            var model = _mapper.Map<MemberViewModel>(member);

            // Check If Member has Acive Membership or not
            var ActiveMembership = await _unitOfWork.GetRepo<Membership>().FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate > DateTime.Now);

            if (ActiveMembership is not null)
            {
                var ActivePlan = await _unitOfWork.GetRepo<Plan>().GetByIdAsync(ActiveMembership.PlanId, ct);
                model.PlanName = ActivePlan.Name;
                model.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMembership.EndDate.ToString();
            }

            return Result<MemberViewModel>.OK(model);
        }

        public async Task<Result<HealthRecordViewModel>> GetMemberHealthRecord(int MemberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepo<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == MemberId, ct: ct);

            if (record is null) return Result<HealthRecordViewModel>.Fail("Health record not found.");

            var result = _mapper.Map<HealthRecordViewModel>(record);
            return Result<HealthRecordViewModel>.OK(result);

        }




    }
}
