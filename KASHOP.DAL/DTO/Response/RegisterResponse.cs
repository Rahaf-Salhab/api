using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class RegisterResponse
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
