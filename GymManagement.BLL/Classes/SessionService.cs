using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Classes
{
    public class SessionService : ISessionService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllAsync(CancellationToken ct = default)
        {


            var sessions = await _unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory();
            if (!sessions.Any())
                return [];

            var result = _mapper.Map<List<SessionViewModel>>(sessions);

            foreach (var seesion in result)
            {
                seesion.AvailableSlots = seesion.Capacity - await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(seesion.Id);
            }
            return result;
        }


        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId, ct);
            if (session == null) return Result<SessionViewModel>.NotFound();

            var result = _mapper.Map<SessionViewModel>(session);

            result.AvailableSlots = result.Capacity - await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(result.Id);

            return Result<SessionViewModel>.OK(result);

        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {

            if (model.StartDate >= model.EndDate) return Result.Validation("Start date must be before end date.");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future.");
            if (model.Capacity <= 0 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25.");

            var category = await _unitOfWork.GetRepo<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("Category not found.");

            var trainer = await _unitOfWork.GetRepo<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer not found.");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, true, out var specailtyresult);
            if (!isValid || trainer.Specialty != specailtyresult) return Result.Validation("Trainer's specialty does not match the category.");

            var isBusy = await _unitOfWork.GetRepo<Session>().AnyAsync(x => x.TrainerId == model.TrainerId && model.StartDate < x.EndDate && model.EndDate > x.StartDate);
            if (isBusy) return Result.Validation("Trainer is busy during the selected time.");

            var session = _mapper.Map<Session>(model);

            await _unitOfWork.GetRepo<Session>().AddAsync(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Failed to create session.");
        }



        public async Task<Result> UpdateAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(id, ct);
            if (session == null) return Result.NotFound();

            if (model.StartDate >= model.EndDate) return Result.Validation("Start date must be before end date.");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future.");



            //var category = await _unitOfWork.GetRepo<category>().GetByIdAsync(model.CategoryId); // error ==> cause tacking issue, so we use FirstOrDefaultAsync with tracking false
            var category = await _unitOfWork.GetRepo<Category>().FirstOrDefaultAsync(x => x.Id == session.CategoryId, tracking: false, ct);
            if (category == null) return Result.NotFound("Category not found.");

            //var trainer = await _unitOfWork.GetRepo<Trainer>().GetByIdAsync(model.TrainerId); // error ==> cause tacking issue, so we use FirstOrDefaultAsync with tracking false
            var trainer = await _unitOfWork.GetRepo<Trainer>().FirstOrDefaultAsync(x => x.Id == model.TrainerId, tracking: false, ct);

            if (trainer == null) return Result.NotFound("Trainer not found.");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, true, out var specailtyresult);
            if (!isValid || trainer.Specialty != specailtyresult) return Result.Validation("Trainer's specialty does not match the category.");

            var isBusy = await _unitOfWork.GetRepo<Session>().AnyAsync(x => x.Id != id && x.TrainerId == model.TrainerId && model.StartDate < x.EndDate && model.EndDate > x.StartDate);
            if (isBusy) return Result.Validation("Trainer is busy during the selected time.");

            _mapper.Map(model, session);
            _unitOfWork.SessionRepository.Update(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Failed to update session.");
        }

        public async Task<Result<UpdateSessionViewModel>> GetForUpdateAsync(int sessionId, CancellationToken ct = default)
        {

            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId, ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound();
            if (session.EndDate < DateTime.Now) return Result<UpdateSessionViewModel>.Validation("Cannot Update Completed Session !");
            if (session.StartDate <= DateTime.Now) return Result<UpdateSessionViewModel>.Validation("Cannot Update Ongoing Session !");

            var result = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.OK(result);

        }

        public async Task<Result> DeleteAsync(int sessionId, CancellationToken ct = default)
        {

            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session == null) return Result.NotFound();
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now)
                return Result.Validation("Cannot delete an ongoing session.");
            _unitOfWork.SessionRepository.Delete(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete session.");

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDown(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepo<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryrForDropDown(CancellationToken ct = default)
        {
            var caterories = await _unitOfWork.GetRepo<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(caterories);
        }
    }
}
