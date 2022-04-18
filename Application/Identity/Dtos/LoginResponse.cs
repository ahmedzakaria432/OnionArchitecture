using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Dtos
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }


    }
}
