using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionController : Controller
    {
        public readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var session = await _sessionService.GetAllAsync(ct);
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await DropDownList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct = default)
        {

            if (!ModelState.IsValid)
            {
                await DropDownList();
                return View("Create", model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                await DropDownList(ct);
                return View(model);
            }
            TempData["SuccessMessage"] = "Session Created Succesfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var session = await _sessionService.GetForUpdateAsync(id, ct);
            await DropDownList(ct);
            if (!session.success)
            {
                TempData["ErrorMessage"] = session.error;
                return RedirectToAction("Index");

            }
            return View(session.Value);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {

            if (!ModelState.IsValid)
            {

                await DropDownList(ct);
                return View("Edit", model);
            }

            var result = await _sessionService.UpdateAsync(id, model, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                await DropDownList(ct);
                return View(model);
            }
            TempData["SuccessMessage"] = "Session Updated Successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (!result.success)
                return NotFound();
            return View(result.Value);
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.DeleteAsync(id, ct);
            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            return RedirectToAction("Index");
        }





        private async Task DropDownList(CancellationToken ct = default)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDown(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryrForDropDown(ct), "Id", "CategoryName");
        }
    }
}
