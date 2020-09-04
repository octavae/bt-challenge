using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsWebAPI.Models
{
    public class Job
    {
        public long JobId { get; set; }
        public string JobName { get; set; }
        public string JobType { get; set; }
        public string JobStatus { get; set; }
    }
}