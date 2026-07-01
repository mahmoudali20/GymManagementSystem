using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.MemberShips;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class MemberShipController : Controller
    {

        private readonly IMemberShipService _memberShipService;
        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }

        public async Task<IActionResult> Index()
        {
            var memberShip = await _memberShipService.GetMembershipsAsync();

            return View(memberShip);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Members = new SelectList(await _memberShipService.GetMemberForDropDown(), "Id", "Name");
            ViewBag.Plans = new SelectList(await _memberShipService.GetPlanForDropDown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            var membership = await _memberShipService.CreateMembershipAsync(model);

            if (!membership.success)
                TempData["ErrorMessage"] = membership.error;
            else
                TempData["SuccessMessage"] = "MemberShip Created Succesfully";


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cancel(int id)
        {

            var membership = await _memberShipService.DeleteMembershipAsync(id);
            if (!membership.success)
                TempData["ErrorMessage"] = membership.error;
            else
                TempData["SuccessMessage"] = "Member Deleted Successfully";

            return RedirectToAction("index");



        }


    }

}
