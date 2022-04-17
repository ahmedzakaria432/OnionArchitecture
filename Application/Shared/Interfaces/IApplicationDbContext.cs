using Core.Samples;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Sample> Samples { get; set; }
       // DbSet<User> UsersAdditionalData { get; set; }
    }
}
