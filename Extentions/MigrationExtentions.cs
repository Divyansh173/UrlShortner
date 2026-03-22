using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;

namespace URLShortner.Extentions
{
    public static class MigrationExtentions
    {
        public static void ApplyMigrations(this WebApplication app) 
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            dbContext.Database.Migrate();
        }
    }
}
