using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TPS2.DBInteraction;

namespace TPS2.Customer
{
    public partial class NewRequestForm : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();
        private List<Skill> _skillList = new List<Skill>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _skillList = _databaseConnection.GetSkillList();

                //Get the skills from the DB
                RequiredSkillListBox.DataSource = _skillList;
                RequiredSkillListBox.DataValueField = "Id";
                RequiredSkillListBox.DataTextField = "Name";
                RequiredSkillListBox.DataBind();

                RequestedSkillListBox.DataSource = _skillList;
                RequestedSkillListBox.DataValueField = "Id";
                RequestedSkillListBox.DataTextField = "Name";
                RequestedSkillListBox.DataBind();

                //Get State list
                StatesListBox.DataSource = _databaseConnection.GetStates();
                StatesListBox.DataValueField = "Abbreviation";
                StatesListBox.DataTextField = "Name";
                StatesListBox.DataBind();

                //Education Level
                var items = new List<string>
                {
                    "GED",
                    "High School Diploma",
                    "Grade School",
                    "Some College",
                    "Associates Degree",
                    "Bachelors Degree",
                    "Masters Degree",
                    "Doctorate"
                };

                EducationLevel.DataSource = items;
                EducationLevel.DataBind();
            }
        }

        //Load skills into the ListBox
        
        //Submit function
        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            //insert request and address
            var parameters = new List<ParameterList>
            {
                new ParameterList {ParameterName = "@EducationLevel", Parameter = EducationLevel.SelectedIndex.ToString()},
                new ParameterList {ParameterName = "@EducationRequired", Parameter = EducationRequired.Checked ? "1" : "0"},
                new ParameterList {ParameterName = "@StartingSalary", Parameter = StartingSalary.Text},
                new ParameterList {ParameterName = "@AddressLine1", Parameter = Address1TextBox.Text},
                new ParameterList {ParameterName = "@AddressLine2", Parameter = Address2TextBox.Text},
                new ParameterList {ParameterName = "@City", Parameter = CityTextBox.Text},
                new ParameterList {ParameterName = "@Zip", Parameter = ZipTextBox.Text},
                new ParameterList {ParameterName = "@State", Parameter = StatesListBox.Text},
                new ParameterList {ParameterName = "@Telecommute", Parameter = TelecommuteCheckBox.Checked ? "1" : "0"}
            };
            var clientRequestId = _databaseConnection.RunStoredProcReturnId("InsertClientRequest", parameters);
            
            
            //insert the skills
            
            var requiredItems = RequiredSkillListBox.Items.Cast<ListItem>().Where(item => item.Selected);
            foreach (var item in requiredItems)
            {
                var skillParameters = new List<ParameterList>
                {
                    new ParameterList {ParameterName = "@RequestId", Parameter = clientRequestId.ToString()},
                    new ParameterList {ParameterName = "@SkillId", Parameter = item.Value},
                    new ParameterList {ParameterName = "@Required", Parameter = "1"}
                };


                _databaseConnection.RunStoredProc("InsertClientRequestSkills", skillParameters);
            }

            var requestedItems = RequestedSkillListBox.Items.Cast<ListItem>().Where(item => item.Selected);
            foreach (var item in requestedItems)
            {
                var skillParameters = new List<ParameterList>
                {
                    new ParameterList {ParameterName = "@RequestId", Parameter = clientRequestId.ToString()},
                    new ParameterList {ParameterName = "@SkillId", Parameter = item.Value},
                    new ParameterList {ParameterName = "@Required", Parameter = "0"}
                };


                _databaseConnection.RunStoredProc("InsertClientRequestSkills", skillParameters);
            }
        }
    }
}