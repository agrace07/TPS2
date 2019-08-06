using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TPS2.Models;

namespace TPS2.DBInteraction
{
    public class ParameterList
    {
        public string ParameterName;
        public string Parameter;
    }

    public class Skill
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Request
    {
        public string Id { get; set; }
        public string Email { get; set; }
        
        public Request(string v1, string v2)
        {
            Id = v1;
            Email = v2;
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Employee(string fn, string ln, string id)
        {
            Name = fn + " " + ln;
            Id = id;
        }
    }

    //TODO Fix all the hardcoded methods/functions to use stored procs

    public class DBConnect
    {
        /// <summary>
        /// Any new Stored procs need to be added here so they can be called from the function
        /// </summary>
        public enum StoredProcs
        {
            InsertClientRequest,
            InsertClientRequestSkills,
            InsertEmployeeInfo,
            UpdateEmployeeInfo,
            MatchRequest
        }
        
        //TODO Update to return list/accept query to run
        public bool RunSelectQuery(string query)
        {
            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(query, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        //reader
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            return true;
        }

        public List<Request> GetUnfilledRequests()
        {
            var unfilledRequests = new List<Request>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select cr.Id, anu.Email from clientRequest cr join AspNetUsers anu on cr.RequestorID = anu.Id where cr.Id NOT IN(select RequestID from RequestMatch)", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unfilledRequests.Add(new Request(reader["Id"].ToString(), reader["Email"].ToString()));
                }
                cmd.Connection.Close();
            }

            return unfilledRequests;
        }

        public List<int> GetFilledRequests(string userId)
        {
            var ids = new List<int>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(
                    "select id from clientrequest where requestorID = '" + userId +
                    "' and Id in (SELECT RequestId from RequestMatch ) AND Complete = 0", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ids.Add(Int32.Parse(reader["id"].ToString()));
                }
                cmd.Connection.Close();
            }

            return ids;
        }

        public List<Employee> GetCandidateList(string reqId)
        {
            var employees = new List<Employee>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                //TODO need to figure our how to tell which of the user's requests we're going to match the employee to
                var cmd = new SqlCommand("select e.FirstName, e.LastName, e.AspNetUserId from Employee e join RequestMatch rm on e.AspNetUserId = rm.AspNetUserId where e.Expired is null and rm.RequestId = " + reqId, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee(reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["AspNetUserId"].ToString()));
                }
                cmd.Connection.Close();
            }

            return employees;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select FirstName, LastName, AspNetUserId from Employee where Expired IS NULL", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee(reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["AspNetUserId"].ToString()));
                }
                cmd.Connection.Close();
            }

            return employees;
        }

        public void SelectEmployee(string selectedId, string reqId)
        {
            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("UPDATE RequestMatch set WasSelected = 1 WHERE RequestId = " + reqId + " AND AspNetUserId = '" + selectedId + "'" , con);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "UPDATE ClientRequest SET Complete = 1 WHERE Id = " + reqId;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Runs a stored proc from the database
        /// </summary>
        /// <param name="spName">Name of the stored proc to run</param>
        /// <param name="parameters">a list of parameters to be passed to the stored proc</param>
        /// <returns></returns>
        public int RunStoredProc(StoredProcs spName, List<ParameterList> parameters)
        {
            int returnValue;

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(spName.ToString(), con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Parameter);
                }
                
                cmd.Connection.Open();
                returnValue = cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }

            return returnValue;
        }

        /// <summary>
        /// Runs a stored proc from the database and returns an ID of the record that was inserted
        /// This will add a @ReturnVal to the parameter list. Your stored proc needs to include this
        /// value as well
        /// </summary>
        /// <param name="spName">Name of the stored proc to run</param>
        /// <param name="parameters">a list of parameters to be passed to the stored proc</param>
        /// <returns>ID of the inserted record</returns>
        public int RunStoredProcReturnId(StoredProcs spName, List<ParameterList> parameters)
        {
            int returnValue = 0;

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(spName.ToString(), con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Parameter);
                }

                var param = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                
                var result = param.Value;
                returnValue = (int) result;
                cmd.Connection.Close();
            }

            return returnValue;
        }
        
        //TODO Employee stuff:
        //TODO Finish the Insert new info for employee
        //-Edit/update info

        public List<State> GetStates()
        {
            var states = new List<State>();

            using (var con = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select * from cd_state", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new State(reader["StateName"].ToString(),reader["StateCd"].ToString()));
                }
            }
            return states;
        }

        /// <summary>
        /// Fills the user data in the Manage screen
        /// </summary>
        /// <param name="aspNetUserId">current user's ID</param>
        /// <returns>All employee data</returns>
        public EmployeeModel GetEmployeeModel(string aspNetUserId)
        {
            var employee = new EmployeeModel();
            //var employeeLocation = new Address();
            employee.Location = new Address();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var sql = "SELECT *FROM employee e JOIN EmployeeAddress ea on e.AddressID = ea.Id WHERE AspNetUserID = '" + aspNetUserId + "' AND Expired IS NULL";
                var cmd = new SqlCommand(sql, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.WillingToRelocate = reader["Relocate"].ToString() != "0";
                        employee.ResumeLocation = reader["ResumeLocation"].ToString();
                        employee.Picture = reader["PictureLocation"].ToString();
                        employee.AvailabilityDate = Convert.ToDateTime(reader["AvailabilityDate"]);
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.Location.AddressLine1 = reader["AddressLine1"].ToString();
                        employee.Location.AddressLine2 = reader["AddressLine2"].ToString();
                        employee.Location.City = reader["City"].ToString();
                        //TODO get this working
                        //employee.Location.State = reader["AddressLine2"].ToString();
                        employee.Location.Zip = reader["Zip"].ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            return employee;
        }

        /// <summary>
        /// Get available skills from DB
        /// </summary>
        /// <returns>Skill list</returns>
        public List<Skill> GetSkillList()
        {
            var skillList = new List<Skill>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var sql = "SELECT * FROM Skills";

                var cmd = new SqlCommand(sql, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        skillList.Add(new Skill {Id = reader["Id"].ToString(), Name = reader["Name"].ToString()});
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            return skillList;
        }

        //-View existing requests
        //-Delete existing requests(use flag in DB, don't delete)
        //-Edit existing
    }
}