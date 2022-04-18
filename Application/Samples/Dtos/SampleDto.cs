using Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Samples.Dtos
{
    public class SampleDto:AuditableDto
    {
        public string? NameOfSample { get; set; }
    }
}
