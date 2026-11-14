using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymCoreProject.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanRepository planRepository;
        //private readonly GymDbContext dbContext = new GymDbContext();
        public PlansController(IPlanRepository repository)
        {
            planRepository = repository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(id,ct);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
