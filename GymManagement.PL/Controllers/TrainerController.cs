using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainerController : Controller
    {

        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainer = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            var result = await _trainerService.CreateTrainerAsync(model, ct);


            if (!result.success)
                TempData["ErrorMessage"] = result.error;

            else
                TempData["SuccessMessage"] = "Trainer Created Succesfully";

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction("Index");
            }

            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _trainerService.GetForUpdateAsync(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction("Index");
            }
            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);
            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);
            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {

            var result = await _trainerService.DeleteTrainerAsync(id, ct);

            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";

            return RedirectToAction("index");
        }
    }
}