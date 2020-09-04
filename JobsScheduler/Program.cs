using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using JobsScheduler.Jobs;

namespace JobsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            JobsController.ProcessJobs();
            Console.Read();
        }
    }
}
