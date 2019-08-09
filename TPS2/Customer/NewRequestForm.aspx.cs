using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using TPS2.DBInteraction;
using Parameter = TPS2.DBInteraction.Parameter;

namespace TPS2.Customer
{
    public partial class NewRequestForm : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();
        private List<Skill> _skillList = new List<Skill>();

        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Customer/NewRequestForm");

                    SuccessMessage =
                        message == "AddRequestSuccess" ? "Your request has been added, you will receive an update within 48 hours."
                        //: message == "SetPwdSuccess" ? "Your password has been set."
                        //: message == "RemoveLoginSuccess" ? "The account was removed."
                        //: message == "AddPhoneNumberSuccess" ? "Phone number has been added"
                        //: message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
                        //: message == "UpdateInfoSuccess" ? "Your information has been updated"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }

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
            var parameters = new List<Parameter>
            {
                new Parameter
                    {ParameterName = "@EducationLevel", ParameterValue = EducationLevel.SelectedIndex.ToString()},
                new Parameter
                    {ParameterName = "@EducationRequired", ParameterValue = EducationRequired.Checked ? "1" : "0"},
                new Parameter {ParameterName = "@StartingSalary", ParameterValue = StartingSalary.Text},
                new Parameter {ParameterName = "@AddressLine1", ParameterValue = Address1TextBox.Text},
                new Parameter {ParameterName = "@AddressLine2", ParameterValue = Address2TextBox.Text},
                new Parameter {ParameterName = "@City", ParameterValue = CityTextBox.Text},
                new Parameter {ParameterName = "@Zip", ParameterValue = ZipTextBox.Text},
                new Parameter {ParameterName = "@State", ParameterValue = StatesListBox.Text},
                //new Parameter {ParameterName = "@Telecommute", ParameterValue = TelecommuteCheckBox.Checked ? "1" : "0"},
                new Parameter {ParameterName = "@RequestorID", ParameterValue = User.Identity.GetUserId()}
            };
            var clientRequestId = _databaseConnection.RunStoredProcReturnId(DBConnect.StoredProcs.InsertClientRequest, parameters);
            
            
            //insert the skills
            
            var requiredItems = RequiredSkillListBox.Items.Cast<ListItem>().Where(item => item.Selected);
            foreach (var item in requiredItems)
            {
                var skillParameters = new List<Parameter>
                {
                    new Parameter {ParameterName = "@RequestId", ParameterValue = clientRequestId.ToString()},
                    new Parameter {ParameterName = "@SkillId", ParameterValue = item.Value},
                    new Parameter {ParameterName = "@Required", ParameterValue = "1"}
                };

                _databaseConnection.RunStoredProc(DBConnect.StoredProcs.InsertClientRequestSkills, skillParameters);
            }

            var requestedItems = RequestedSkillListBox.Items.Cast<ListItem>().Where(item => item.Selected);
            foreach (var item in requestedItems)
            {
                var skillParameters = new List<Parameter>
                {
                    new Parameter {ParameterName = "@RequestId", ParameterValue = clientRequestId.ToString()},
                    new Parameter {ParameterName = "@SkillId", ParameterValue = item.Value},
                    new Parameter {ParameterName = "@Required", ParameterValue = "0"}
                };


                _databaseConnection.RunStoredProc(DBConnect.StoredProcs.InsertClientRequestSkills, skillParameters);
            }

            Response.Redirect("/Customer/NewRequestForm?m=AddRequestSuccess");
        }
    }
}