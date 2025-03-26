using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Entities
{
    public class Log : BaseEntity<int>
    {
        public string Username { get; set; }
        public string Description { get; set; }
    }
}
