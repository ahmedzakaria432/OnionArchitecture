using Application.Samples.Dtos;
using Application.Shared.Interfaces;
using Core.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Samples.Interfaces
{
    public interface ISampleService:IService<Sample,SampleDto,CreateSampleDto,UpdateSampleDto>
    {
    }
}
