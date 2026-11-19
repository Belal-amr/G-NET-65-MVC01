using GymManagement.BLL.Services.Ineterfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        #region Create Member
        //Get BaseURL/Member/Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel member, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), member);
            }
            var result = await memberService.CreateMemberAsync(member, ct);
            if (!result)
            {
                TempData["ErrorMessage"] = "Email or Phone already exists.";
                ModelState.AddModelError(string.Empty, "Email or Phone already exists.");
                return View("Create", member);
            }
            else
            {
                TempData["SuccessMessage"] = "Member created successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        public async Task<IActionResult> MamberDetails(int id, CancellationToken ct)
        {
            // Get Member  by Id
            var member = await memberService.GetMemberByIdAsync(id, ct);
            // Check if Member is null => return Index with message
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            // if Member is not null => return View Data

            return View(member);
        }

        //Get BaseURL/Member/HealthRecordDetails/{id}

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            //Get HealthRecord by MemberId
            var result = await memberService.GetMemberHealthRecordAsync(id, ct);
            //Check  is null => return Index with message
            if (result is null)
            {
                TempData["ErrorMessage"] = "Health Record not found.";
                return RedirectToAction(nameof(Index));
            }
            //Record is not null => return View Data
            return View(result);
        }

        #region Edit Member
        //Get BaseURL/Member/Edit/{id}
        //Edit -- Disply Edit Form
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        //Post - BaseURL/Member/Edit/{Member}
        //Edit - Submit Form
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await memberService.UpdateMemberDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Updated successfully.";
            else
                TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View();

            
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            var result = await memberService.RemoveMemberAsync(id,ct);
            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully.";
            else
                TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }


        #endregion
    }
}
