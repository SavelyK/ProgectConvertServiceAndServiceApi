

namespace RepositoryPersistence
{
    public class DbInitializer
    {
        public static void Initialize(RepositoryDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
