

namespace RepositoryPersistence
{
    public class RepositoryDbInitializer
    {
        public static void Initialize(RepositoryDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
