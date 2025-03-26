using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.DTOs.Log
{
    public class GetLogDto
    {
        public string Username { get; set; }

        public string Description { get; set; }

        public string CreatedAt { get; set; }
    }
}
