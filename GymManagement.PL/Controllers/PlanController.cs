using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class PlanController : Controller
    {

        public readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _planService.GetAllAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planService.GetByIdAsync(id);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.error;
            }
            return View(plan.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var plan = await _planService.GetForUpdateAsync(id);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.error;
                return RedirectToAction("Index");
            }
            return View(plan.Value);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var plan = await _planService.UpdateAsync(id, model, ct);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.error;
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id, CancellationToken ct)
        {

            var result = await _planService.ToggleStatusAsync(id, ct);
            if (!result.success)
                TempData["ErrorMessage"] = result.error;

            else
                TempData["SuccessMessage"] = "Plan Status Updated Successfully";
            return RedirectToAction("Index");

        }
    }
}
