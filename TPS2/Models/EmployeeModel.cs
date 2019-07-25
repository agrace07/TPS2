using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPS2.Models
{
    public class State
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public State(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }

    public class States //TODO add this to a code table in the DB?
    {
        public List<State> StateList = new List<State>
        {
            new State("Alabama", "AL"),
            new State("Alaska", "AK"),
            new State("Arizona", "AZ"),
            new State("Arkansas", "AR"),
            new State("Armed Forces America", "AA"),
            new State("Armed Forces Europe", "AE"),
            new State("Armed Forces Pacific", "AP"),
            new State("California", "CA"),
            new State("Colorado", "CO"),
            new State("Connecticut", "CT"),
            new State("Delaware", "DE"),
            new State("District of Columbia", "DC"),
            new State("Florida", "FL"),
            new State("Georgia", "GA"),
            new State("Hawaii", "HI"),
            new State("Idaho", "ID"),
            new State("Illinois", "IL"),
            new State("Indiana", "IN"),
            new State("Iowa", "IA"),
            new State("Kansas", "KS"),
            new State("Kentucky", "KY"),
            new State("Louisiana", "LA"),
            new State("Maine", "ME"),
            new State("Maryland", "MD"),
            new State("Massachusetts", "MA"),
            new State("Michigan", "MI"),
            new State("Minnesota", "MN"),
            new State("Mississippi", "MS"),
            new State("Missouri", "MO"),
            new State("Montana", "MT"),
            new State("Nebraska", "NE"),
            new State("Nevada", "NV"),
            new State("New Hampshire", "NH"),
            new State("New Jersey", "NJ"),
            new State("New Mexico", "NM"),
            new State("New York", "NY"),
            new State("North Carolina", "NC"),
            new State("North Dakota", "ND"),
            new State("Ohio", "OH"),
            new State("Oklahoma", "OK"),
            new State("Oregon", "OR"),
            new State("Pennsylvania", "PA"),
            new State("Rhode Island", "RI"),
            new State("South Carolina", "SC"),
            new State("South Dakota", "SD"),
            new State("Tennessee", "TN"),
            new State("Texas", "TX"),
            new State("Utah", "UT"),
            new State("Vermont", "VT"),
            new State("Virginia", "VA"),
            new State("Washington", "WA"),
            new State("West Virginia", "WV"),
            new State("Wisconsin", "WI"),
            new State("Wyoming", "WY")
        };
    }

    public class Address
    {
        public int AddressId { get; set; } //TODO do we need this?
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public string Zip { get; set; }
    }

    public class Experience
    {
        public int ExperienceId { get; set; }
        public string Description { get; set; }
        public int Months { get; set; }
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
        public string ResumeLocation { get; set; } //TODO will point to a location on the server where the resume is stored.  
        public string Picture { get; set; } //TODO same as resume
        public DateTime AvailabilityDate { get; set; }
    }
}