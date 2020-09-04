using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JobsWebAPI.Models;
using Newtonsoft.Json;
using System.IO;

namespace JobsWebAPI.Controllers
{
    public class JobsController : ApiController
    {
        Job[] jobs = new Job[]
        {
            new Job { JobId = 1, JobName = "Notificare", JobStatus = "OK" },
            new Job { JobId = 2, JobName = "Plata", JobStatus = "NOK" }
        };

        [HttpGet]
        [ActionName("demo")]
        public IEnumerable<Job> ListaDemo()
        {
            return jobs;
        }

        [HttpPost]
        [ActionName("notificare")]
        public Job ProceseazaNotificare([FromBody] dynamic value)
        {
            // get the id and the name of the job an create a local file
            var jobId = value.JobId.Value;
            var jobName = value.JobName.Value;
            var jobType = value.JobType.Value;
            var jobStatus = "NOK";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/files") + @"\file_" + jobId + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                try
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(jobName);
                    jobStatus = "OK";
                }
                catch (Exception ex)
                {
                    // log exeption
                }
            }

            return new Job { JobId = jobId, JobName = jobName, JobType = jobType, JobStatus = jobStatus };
        }

        [HttpPost]
        [ActionName("fisier")]
        public Job ProceseazaFisier([FromBody] dynamic value)
        {
            // get the id and the name of the job an create a local file
            var jobId = value.JobId.Value;
            var jobName = value.JobName.Value;
            var jobType = value.JobType.Value;
            var jobStatus = "NOK";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/files") + @"\file_" + jobId + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                try
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(jobName);
                    jobStatus = "OK";
                }
                catch (Exception ex)
                {
                    // log exeption
                }
            }

            return new Job { JobId = jobId, JobName = jobName, JobType = jobType, JobStatus = jobStatus };
        }

        [HttpPost]
        [ActionName("plata")]
        public Job ProceseazaPlata([FromBody] dynamic value)
        {
            // get the id and the name of the job an create a local file
            var jobId = value.JobId.Value;
            var jobName = value.JobName.Value;
            var jobType = value.JobType.Value;
            var jobStatus = "NOK";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/files") + @"\file_" + jobId + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                try
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(jobName);
                    jobStatus = "OK";
                }
                catch (Exception ex)
                {
                    // log exeption
                }
            }

            return new Job { JobId = jobId, JobName = jobName, JobType = jobType, JobStatus = jobStatus };
        }

        [HttpPost]
        [ActionName("incasare")]
        public Job ProceseazaIncasare([FromBody] dynamic value)
        {
            // get the id and the name of the job an create a local file
            var jobId = value.JobId.Value;
            var jobName = value.JobName.Value;
            var jobType = value.JobType.Value;
            var jobStatus = "NOK";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/files") + @"\file_" + jobId + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                try
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(jobName);
                    jobStatus = "OK";
                }
                catch (Exception ex)
                {
                    // log exeption
                }
            }

            return new Job { JobId = jobId, JobName = jobName, JobType = jobType, JobStatus = jobStatus };
        }

    }
}
