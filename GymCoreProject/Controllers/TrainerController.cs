using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Ineterfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this._trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerService.GetAllTrainerAsync(ct);
            return View(trainers);
        }
        #region Create Trainer
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTrainerViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);

            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Trainer failed to create. Email or phone may already exist.";
            return View(model);
        }



        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
            var trainer = await _trainerService.GetAllTrainerDetailsAsync(id,ct);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found. ";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);   
        }

        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);
            var result = await _trainerService.UpdateTrainerDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer Updated successfully.";
            else
                TempData["ErrorMessage"] = "Traner not found.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            var trainer = await _trainerService.GetAllTrainerDetailsAsync(id,ct); 
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.RemoveTrainerAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            else
                TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
