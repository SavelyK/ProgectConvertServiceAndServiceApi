using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            :base($"Entity \"{name}\" ({key}) not found.") { }
    }
}
