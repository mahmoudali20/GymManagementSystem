using GymManagement.BLL.Interfaces;
using GymManagement.BLL.ViewModels.Members;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        // Get :: BaseUrl/Members/Index => List All Members
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Member = await memberService.GetAllAsync(ct);
            return View(Member);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {

            if (!ModelState.IsValid)
                return View("Create", model);

            var result = await memberService.createMemberAsync(model, ct);

            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Member Created Succesfully";


            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> EditMember(int id)
        {
            var member = await memberService.GetForUpdateAsync(id);
            return View(member.Value);
        }


        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("EditMember", model);

            var result = await memberService.UpdateAsync(id, model, ct);

            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Member Updated Successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberDetailsByIdAsync(id, ct);

            if (!member.success)
                TempData["ErrorMessage"] = member.error;

            return View(member.Value);


        }
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {

            var HealthRecord = await memberService.GetMemberHealthRecord(id, ct);
            if (!HealthRecord.success)
                TempData["ErrorMessage"] = HealthRecord.error;
            return View(HealthRecord.Value);

        }


        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {

            var result = await memberService.DeleteAsync(id, ct);

            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else
                TempData["SuccessMessage"] = "Member Deleted Successfully";

            return RedirectToAction("index");
        }


    }
}
