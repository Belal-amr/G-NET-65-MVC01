using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Class
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;

        public PlanRepository(GymDbContext gymDbContext)
        {
            dbContext = gymDbContext;
        }

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Add(plan);
            return await dbContext.SaveChangesAsync(ct);
        }

        public  Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Remove(plan);
            return dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            //if (tracking)
            //{
            //    return await dbContext.Plans.ToListAsync();
            //}
            //else
            //{
            //    return await dbContext.Plans.AsNoTracking().ToListAsync();
            //}

            var plans = tracking ? dbContext.Plans : dbContext.Plans.AsNoTracking();
            return await plans.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbContext.Plans.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext?.Plans.Update(plan);
            return await dbContext.SaveChangesAsync(ct);
        }
    }
}
