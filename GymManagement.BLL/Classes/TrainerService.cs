using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Trainers;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Classes
{
    public class TrainerService : ITrainerService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {

            var trainer = await _unitOfWork.GetRepo<Trainer>().GetAllAsync(ct: ct);
            if (!trainer.Any())
                return [];
            var resutl = _mapper.Map<IEnumerable<TrainerViewModel>>(trainer);

            return resutl;
        }
        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct)
        {
            var EmailExist = await _unitOfWork.GetRepo<Trainer>().AnyAsync(t => t.Email == model.Email);
            var PhoneExist = await _unitOfWork.GetRepo<Trainer>().AnyAsync(t => t.Phone == model.Phone);
            if (EmailExist)
                return Result.Validation("Email already exists.");

            if (PhoneExist)
                return Result.Validation("Phone already exists.");

            var trainer = _mapper.Map<Trainer>(model);

            await _unitOfWork.GetRepo<Trainer>().AddAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to create trainer.");
        }

        public async Task<Result<TrainerViewModel>> GetTrainerDetailsByIdAsync(int id, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepo<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return Result<TrainerViewModel>.Fail("Trainer not found.");
            var result = _mapper.Map<TrainerViewModel>(trainer);
            return Result<TrainerViewModel>.OK(result);
        }

        public async Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {

            var trainer = await _unitOfWork.GetRepo<Trainer>().GetByIdAsync(id, ct);
            var EmailExist = await _unitOfWork.GetRepo<Trainer>().AnyAsync(t => t.Email == model.Email && id != t.Id);
            var PhoneExist = await _unitOfWork.GetRepo<Trainer>().AnyAsync(t => t.Phone == model.Phone && id != t.Id);
            if (EmailExist)
                return Result.Validation("Email already exists.");

            if (PhoneExist)
                return Result.Validation("Phone already exists.");

            _mapper.Map(model, trainer);
            _unitOfWork.GetRepo<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to update trainer");
        }

        public async Task<Result<TrainerToUpdateViewModel>> GetForUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepo<Trainer>().FirstOrDefaultAsync(x => x.Id == trainerId, tracking: false, ct);
            if (trainer == null) return Result<TrainerToUpdateViewModel>.Fail("Trainer not found.");
            var model = _mapper.Map<TrainerToUpdateViewModel>(trainer);
            return Result<TrainerToUpdateViewModel>.OK(model);
        }

        public async Task<Result> DeleteTrainerAsync(int id, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepo<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return Result.Fail("Trainer not found.");

            _unitOfWork.GetRepo<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.OK() : Result.Fail("Failed to delete trainer.");

        }
    }
}
