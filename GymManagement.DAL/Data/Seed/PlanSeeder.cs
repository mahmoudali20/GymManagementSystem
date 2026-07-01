using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GymManagement.DAL.Data.Seed
{
    public static class PlanSeeder
    {
        public static async Task SeedAsync(GymDbContext context)
        {
            if (await context.Plans.AnyAsync()) // The seeder checks if the Plans table is empty
                return;

            var path = Path.Combine(AppContext.BaseDirectory, "Data", "plans.json");

            if (!File.Exists(path))
                return;

            var json = await File.ReadAllTextAsync(path);

            var plans = JsonSerializer.Deserialize<List<Plan>>(json);

            if (plans is null || !plans.Any())
                return;

            context.Plans.AddRange(plans);

            await context.SaveChangesAsync();
        }
    }
}
