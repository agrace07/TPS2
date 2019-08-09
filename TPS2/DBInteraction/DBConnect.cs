using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.Owin.Security;
using TPS2.Models;

namespace TPS2.DBInteraction
{
    public class Parameter
    {
        public string ParameterName;
        public string ParameterValue;
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
        public string ReqDate { get; set; }
        public string Display { get; set; }
        
        public Request(string v1, string v2, string v3)
        {
            Id = v1;
            Email = v2;
            ReqDate = v3;
            Display = Email + " " + ReqDate;
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }

        public Employee(string fn, string ln, string id, string e)
        {
            Name = fn + " " + ln;
            Email = e;
            Id = id;
        }
    }

    public class Roles
    {
        public bool Employee { get; set; }
        public bool Customer { get; set; }
        public bool Manager { get; set; }
    }

    public class ClientRequest
    {
        public int Id { get; set; }
        public List<Tuple<int,bool>> Skills { get; set; }
        public int IdEducation { get; set; }
        public bool EducationRequired { get; set; }
        public int Salary { get; set; }
        public Address Location { get; set; }
        public string RequesterId { get; set; }
        public bool Complete { get; set; }
        public DateTime RequestDate { get; set; }
        public bool TelecommuteAvail { get; set; }
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
            InsertEmployeeSkills,
            UpdateEmployeeInfo,
            MatchRequest,
            ClearSkills
        }

        public ClientRequest GetRequest(string id)
        {
            var req = new ClientRequest();
            req.Location = new Address();
            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select * from ClientRequest cr JOIN RequestAddress ra ON cr.IdLocation = ra.Id where cr.Id =" + id, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //public int Id { get; set; }
                    req.Id = (int)reader["Id"];
                    //public List<Skill> Skills { get; set; }
                    //public int IdEducation { get; set; }
                    req.IdEducation = (int) reader["IdEducation"];
                    //public bool EducationRequired { get; set; }
                    req.EducationRequired = (int)reader["EducationRequired"] != 0;
                    //public int Salary { get; set; }
                    req.Salary = (int) reader["StartingSalary"];
                    //public Address Location { get; set; }
                    req.Location.AddressLine1 = reader["AddressLine1"].ToString();
                    req.Location.AddressLine2 = reader["AddressLine2"].ToString();
                    req.Location.City = reader["City"].ToString();
                    req.Location.Zip = reader["Zip"].ToString();
                    //public string RequesterId { get; set; }
                    //public bool Complete { get; set; }
                    req.Complete = (int) reader["Complete"] != 0;
                    //public DateTime RequestDate { get; set; }
                    req.RequestDate = DateTime.Parse(reader["RequestDate"].ToString());
                }

                cmd.Connection.Close();
                cmd.Connection.Open();
                cmd.CommandText = "SELECT * FROM RequestSkills WHERE RequestID = " + req.Id;

                reader = cmd.ExecuteReader();

                req.Skills = new List<Tuple<int, bool>>();

                while (reader.Read())
                {
                    req.Skills.Add(new Tuple<int, bool>((int)reader["SkillId"], (int)reader["Required"] != 0));
                }

                cmd.Connection.Close();
            }

            return req;
        }

        public List<Tuple<DateTime,int>> GetUsersRequests(string id)
        {
            var dates = new List<Tuple<DateTime,int>>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select Id, RequestDate from ClientRequest where RequestorId ='" + id + "'", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //dates.Add(DateTime.Parse(reader["RequestDate"].ToString()));
                    dates.Add(new Tuple<DateTime, int>(DateTime.Parse(reader["RequestDate"].ToString()),(int)reader["Id"]));
                }
                cmd.Connection.Close();
            }

            return dates;
        }

        public void UpdateUserRoles(string id, Roles roles)
        {
            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("DELETE FROM AspNetUserRoles WHERE UserId = '" + id + "'", con);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                const string text = "INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('";
                if (roles.Employee)
                {
                    cmd.CommandText = text + id + "', '" + 1 + "')";
                    cmd.ExecuteNonQuery();
                }
                if (roles.Customer)
                {
                    cmd.CommandText = text + id + "', '" + 2 + "')";
                    cmd.ExecuteNonQuery();
                }
                if (roles.Manager)
                {
                    cmd.CommandText = text + id + "', '" + 3 + "')";
                    cmd.ExecuteNonQuery();
                }

                cmd.Connection.Close();
            }
        }

        public Roles GetUsersRoles(string id)
        {
            var roles = new Roles();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select RoleId from AspNetUserRoles where UserId ='" + id + "'", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    switch (reader["RoleId"])
                    {
                        case "1":
                            roles.Employee = true;
                            break;
                        case "2":
                            roles.Customer = true;
                            break;
                        case "3":
                            roles.Manager = true;
                            break;
                    }
                }
                cmd.Connection.Close();
            }

            return roles;
        }

        public List<int> GetEmployeeSkills(string id)
        {
            var skillIds = new List<int>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select SkillId from EmployeeSkills where Id ='" + id + "'", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    skillIds.Add((int) reader["SkillId"]);
                }
                cmd.Connection.Close();
            }

            return skillIds;
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
                var cmd = new SqlCommand("select cr.Id, anu.Email, cr.RequestDate from clientRequest cr join AspNetUsers anu on cr.RequestorID = anu.Id where cr.Id NOT IN(select RequestID from RequestMatch)", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unfilledRequests.Add(new Request(reader["Id"].ToString(), reader["Email"].ToString(), reader["RequestDate"].ToString()));
                }
                cmd.Connection.Close();
            }

            return unfilledRequests;
        }

        public List<Tuple<int,string>> GetFilledRequests(string userId)
        {
            var ids = new List<Tuple<int, string>>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(
                    "select id, RequestDate from clientrequest where requestorID = '" + userId +
                    "' and Id in (SELECT RequestId from RequestMatch ) AND Complete = 0", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //ids.Add(Int32.Parse(reader["id"].ToString()),reader["RequestDate"].ToString());
                    ids.Add(new Tuple<int, string>(Int32.Parse(reader["id"].ToString()), reader["RequestDate"].ToString()));
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
                var cmd = new SqlCommand("select e.FirstName, e.LastName, e.AspNetUserId from Employee e join RequestMatch rm on e.AspNetUserId = rm.AspNetUserId where e.Expired is null and rm.RequestId = " + reqId, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee(reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["AspNetUserId"].ToString(), reader["Email"].ToString()));
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
                var cmd = new SqlCommand("select FirstName, LastName, AspNetUserId, Email from Employee e JOIN AspNetUsers anu on e.AspNetUserId = anu.Id where Expired IS NULL", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee(reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["AspNetUserId"].ToString(), reader["Email"].ToString()));
                }
                cmd.Connection.Close();
            }

            return employees;
        }

        public List<Employee> GetQualifiedEmployees(string requestId)
        {
            var employees = new List<Employee>();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select FirstName, LastName, AspNetUserId from Employee e JOIN EmployeeSkills es on e.AspNetUserId = es.Id where Expired IS NULL AND SkillId in (SELECT SkillId FROM RequestSkills WHERE RequestId = " + requestId + " AND[Required] = 1)", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee(reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["AspNetUserId"].ToString(), String.Empty));
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
        public int RunStoredProc(StoredProcs spName, List<Parameter> parameters)
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
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
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
        public int RunStoredProcReturnId(StoredProcs spName, List<Parameter> parameters)
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
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
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
        
        public List<StateType> GetStates()
        {
            var states = new List<StateType>();

            using (var con = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand("select * from cd_state", con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new StateType(reader["StateName"].ToString(),reader["StateCd"].ToString()));
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
            employee.Location = new Address();

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                //TODO Clean up this query so we're only selecting values that we're using
                var sql = "SELECT * FROM employee e JOIN EmployeeAddress ea on e.AddressID = ea.Id JOIN CD_State cs on ea.StateCD = cs.StateCD WHERE AspNetUserID = '" + aspNetUserId + "' AND Expired IS NULL";
                var cmd = new SqlCommand(sql, con);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.LastName = reader["LastName"].ToString();
                    employee.WillingToRelocate = reader["Relocate"].ToString() != "0";
                    employee.ResumeLocation = reader["ResumeLocation"].ToString();
                    employee.PictureLocation = reader["PictureLocation"].ToString();
                    employee.AvailabilityDate = Convert.ToDateTime(reader["AvailabilityDate"]);
                    employee.PhoneNumber = reader["PhoneNumber"].ToString();
                    employee.Location.AddressLine1 = reader["AddressLine1"].ToString();
                    employee.Location.AddressLine2 = reader["AddressLine2"].ToString();
                    employee.Location.City = reader["City"].ToString();
                    employee.Location.State = new StateType(reader["StateName"].ToString(), reader["StateCd"].ToString());
                    employee.Location.Zip = reader["Zip"].ToString();
                    employee.ResumeLocation = reader["ResumeLocation"].ToString();
                    employee.PictureLocation = reader["PictureLocation"].ToString();
                }

                cmd.Connection.Close();

                cmd.Connection.Open();
                sql = "select s.Id, s.Name from EmployeeSkills es JOIN Skills s on es.SkillId = s.Id where es.Id = '" + aspNetUserId + "'";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();

                employee.WorkExperience = new List<Experience>();

                while (reader.Read())
                {
                    employee.WorkExperience.Add(new Experience {ExperienceId = (int) reader["Id"], Description = reader["Name"].ToString()});
                }

                cmd.Connection.Close();
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