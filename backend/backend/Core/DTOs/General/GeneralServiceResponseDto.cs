using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.General
{
    public class GeneralServiceResponseDto
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
