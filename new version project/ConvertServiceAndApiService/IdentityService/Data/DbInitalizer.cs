

namespace IdentityService.Data
{
    public class DbInitalizer
    {
        public static void Initialize(AuthDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
