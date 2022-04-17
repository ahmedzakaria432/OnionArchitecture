using Application.Samples.Dtos;
using AutoMapper;
using Core.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Samples
{
    public class SampleProfile:Profile
    {
        public SampleProfile()
        {
            CreateMap<CreateSampleDto, Sample>();
            CreateMap<UpdateSampleDto, Sample>();
            CreateMap<Sample, SampleDto>().ReverseMap();

        }
    }
}
