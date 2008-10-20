using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTracker.Library
{
    public interface ICustomer
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
