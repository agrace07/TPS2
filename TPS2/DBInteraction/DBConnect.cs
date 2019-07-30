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
    public class DBConnect
    {
        //TODO delete this
        private string test = "select * from AspNetUsers";

        //TODO Update to return list/accept query to run
        public bool RunSelectQuery()//(string query)
        {
            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(test, con);
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
        
        public int RunStoredProc(string spName, List<ParameterList> parameters)
        {
            int returnValue = 0;

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(spName, con)
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

        public int RunStoredProcReturnId(string spName, List<ParameterList> parameters)
        {
            int returnValue = 0;

            using (var con =
                new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand(spName, con)
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
                returnValue = cmd.ExecuteNonQuery();
                
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

        //-get existing info(if available)
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


        //TODO CustomerModel:
        //Get available skills from DB
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

        //-Add new request
        //-View existing requests
        //-Delete existing requests(use flag in DB, don't delete)
        //-Edit existing
    }

    public class Skill
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ParameterList
    {
        public string ParameterName;
        public string Parameter;
    }
}