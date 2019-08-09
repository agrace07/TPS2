using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPS2.Models
{
    public class StateType
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public StateType(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public StateType State { get; set; }
        public string Zip { get; set; }
    }

    public class Experience
    {
        public int ExperienceId { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeModel
    {
        public string AspNetUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //TODO Data type for this?
        public string PhoneNumber { get; set; }
        public Address Location { get; set; }
        public bool WillingToRelocate { get; set; }
        public List<Experience> WorkExperience { get; set; }
        public string ResumeLocation { get; set; } 
        public string PictureLocation { get; set; }
        public DateTime AvailabilityDate { get; set; }
    }
}