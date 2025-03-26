using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Constants
{
    public class StaticUserRole
    {
        public const string ADMIN = "ADMIN";

        public const string EMPLOYER = "EMPLOYER";

        public const string CANDIDATE = "CANDIDATE";

        public const string ADMIN_CANDIDATE = "ADMIN, CANDIDATE";

        public const string ADMIN_EMPLOYER = "ADMIN, EMPLOYER";

        public const string CANDIDATE_EMPLOYER = "CANDIDATE, EMPLOYER";
    }
}
