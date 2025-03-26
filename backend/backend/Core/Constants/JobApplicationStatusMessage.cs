using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Constants
{
    public class JobApplicationStatusMessage
    {
        public const string Pending = "Your application is still pending.";

        public const string Shortlisted = "Congratulations, you've been selected for further process. We will get in touch with you soon.";

        public const string Rejected = "We are sorry to inform you that you have not been selected for further process.";
    }
}
