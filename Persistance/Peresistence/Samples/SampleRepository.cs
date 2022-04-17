using Core.Samples;
using Infrastructure.Peresistence.Data;
using Infrastructure.Peresistence.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Peresistence.Samples
{
    public class SampleRepository : Repository<Sample>, ISampleRepository
    {
        public SampleRepository(ApplicationDbContext dbContext):base(dbContext)
        {

        }
    }
}
