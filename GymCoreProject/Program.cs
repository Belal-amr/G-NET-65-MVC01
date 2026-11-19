using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Ineterfaces;
using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymCoreProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberServices>();
            builder.Services.AddScoped<ITrainerService, TrainerServices>();
            //builder.Services.AddSingleton<IPlanRepository, PlanRepository>();
            //builder.Services.AddTransient<IPlanRepository, PlanRepository>();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
