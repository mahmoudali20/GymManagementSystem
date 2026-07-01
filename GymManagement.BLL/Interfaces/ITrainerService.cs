using GymManagement.BLL.Commn;
using GymManagement.BLL.ViewModels.Trainers;

namespace GymManagement.BLL.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct);

        Task<Result> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct);

        Task<Result<TrainerViewModel>> GetTrainerDetailsByIdAsync(int id, CancellationToken ct);

        Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct);

        Task<Result<TrainerToUpdateViewModel>> GetForUpdateAsync(int TrainerId, CancellationToken ct = default);

        Task<Result> DeleteTrainerAsync(int id, CancellationToken ct);



    }
}
