using System;
using System.Collections.Generic;
using System.Text;

namespace ServicePersistence
{
    public class DbInitializer
    {
        public static void Initialize(MyDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
