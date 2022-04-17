using Application.Samples.Dtos;
using Application.Shared;
using AutoMapper;
using Core.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Samples
{
    public class SampleService:Service<Sample,SampleDto,CreateSampleDto,UpdateSampleDto>
    {
        public SampleService(ISampleRepository sampleRepository,IMapper mapper)
            :base(sampleRepository,mapper)
        {

        }
    }
}
