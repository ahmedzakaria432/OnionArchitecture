using Core.Shared;
using Infrastructure.Peresistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Peresistence.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<int> CommitAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }
    }
}
