using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace JobsScheduler.Jobs
{
    class JobsController
    {
        public static void ProcessJobs()
        {
            string connectionString = "";

            try
            {
                connectionString = ConfigurationManager.AppSettings["bt_db"];

                List<Job> jobsCollection = JobsController.LoadJobs(connectionString);

                if (jobsCollection != null)
                {
                    foreach (Job job in jobsCollection) {
                        ProcessJob(job, connectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                // log exception somewhere
            }
            finally
            {
            }
        }
        
        /*
            Loads the jobs from the database using a datareader which is the fastest way
            Alternative solution: microORM of ORM (Dapper, EntityFramework)
        */
        private static List<Job> LoadJobs(string connectionString) {
            
            SqlConnection connection = null;
            SqlCommand command = null;
            Job currentJob = null;
            List<Job> jobsCollection = new List<Job>();

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                command = new SqlCommand("Select TaskId, TaskName, TaskType from Tasks where TaskProcessed = 0", connection);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        currentJob = new Job();
                        currentJob.JobId = dr.GetInt64(0);
                        currentJob.JobName = dr.GetString(1);
                        currentJob.JobType = dr.GetString(2);
                        jobsCollection.Add(currentJob);
                    }
                }

                return jobsCollection;
            }
            catch (Exception ex)
            {
                // log exception somewhere
                return jobsCollection;
            }
            finally
            {
                // Cleanup command and connection objects.
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        private static void ProcessJob(Job job, string connectionString) {

            try
            {
                // the address of the web api
                string uri = ConfigurationManager.AppSettings["webapi_uri"] + "/" + job.JobType;

                // serialize job object
                dynamic json = JsonConvert.SerializeObject(job);

                // initialize webrequest object (sync) and set the content type to application/json
                WebRequest request = WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                // get the response from the web api
                WebResponse response = (HttpWebResponse)request.GetResponse();
                var result = "";
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                // if result is not empty
                if (result != "")
                {
                    // deserialize the json string to a Job object
                    Job djob = JsonConvert.DeserializeObject<Job>(result);

                    // if status is OK, update the database
                    if (djob.JobStatus == "OK")
                    {
                        SqlConnection connection = null;
                        SqlCommand command = null;

                        try
                        {
                            connection = new SqlConnection(connectionString);
                            connection.Open();

                            command = new SqlCommand("Update Tasks Set TaskProcessed = 1 Where TaskId = @1", connection);
                            command.CommandType = System.Data.CommandType.Text;
                            command.Parameters.Clear();
                            command.Parameters.Add("@1", System.Data.SqlDbType.Int);
                            command.Parameters["@1"].Value = djob.JobId;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }
                        finally {
                            // Cleanup command and connection objects.
                            command.Dispose();
                            connection.Close();
                            connection.Dispose();
                        }
                    }

                    Console.WriteLine(result);
                }
            }
            catch (Exception ex) { 
            }
            finally
            {
            }

        }
    }
}
