using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPS2.DBInteraction;

namespace TPS2.Manager
{
    public partial class RequestMatcher : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();

        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Render success message
            var message = Request.QueryString["m"];
            if (message != null)
            {
                // Strip the query string from action
                Form.Action = ResolveUrl("~/Manager/RequestMatcher");

                SuccessMessage =
                    message == "MatchSuccess" ? "Employee matched to request."
                    //follow this format to add additional messages:
                    //: message == "SetPwdSuccess" ? "Your password has been set."
                    : String.Empty;
                successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            }

            if (!IsPostBack || message != null)
            {
                ActiveRequests.DataSource = _databaseConnection.GetUnfilledRequests();
                ActiveRequests.DataValueField = "ID";
                ActiveRequests.DataTextField = "Email";
                ActiveRequests.DataBind();

                //TODO get QUALIFIED employees, look at skills in the DB
                //People.DataSource = _databaseConnection.GetAllEmployees();
                //People.DataSource = _databaseConnection.GetQualifiedEmployees();
                //People.DataValueField = "ID";
                //People.DataTextField = "Name";
                //People.DataBind();

                //PersonSkills.DataSource = _databaseConnection.GetSkillList();
                //PersonSkills.DataValueField = "Id";
                //PersonSkills.DataTextField = "Name";
                //PersonSkills.DataBind();
            }
        }

        protected void EnableSubmit(object sender, EventArgs e)
        {
            //PersonSkills.ClearSelection();

            //var emp = _databaseConnection.GetEmployeeModel(People.SelectedValue);

            //foreach (var skill in emp.WorkExperience)
            //    PersonSkills.Items.FindByText(skill.Description).Selected = true;

            if (ActiveRequests.SelectedIndex > -1)
            {
                People.DataSource = _databaseConnection.GetQualifiedEmployees(ActiveRequests.SelectedValue);
                People.DataValueField = "ID";
                People.DataTextField = "Name";
                People.DataBind();

                if (People.Items.Count > 0)
                {
                    People.Visible = true;
                    NoQualified.Visible = false;
                }
                else
                {
                    People.Visible = false;
                    NoQualified.Visible = true;
                }

                if (People.GetSelectedIndices().Length == 3)
                {
                    Submit.Enabled = true;
                }
            }
        }

        //this needs to make sure we do a post back
        protected void Submit_OnClick(object sender, EventArgs e)
        {
            //var peopleList = new List<ParameterList>();

            foreach (var item in People.Items.Cast<ListItem>().Where(item => item.Selected))
            {
                var requestMatch = new List<ParameterList>
                { 
                    //ID of the person
                    new ParameterList {ParameterName = "@AspNetUserId", Parameter = item.Value},
                    //ID of the request
                    new ParameterList {ParameterName = "@RequestId", Parameter = ActiveRequests.SelectedItem.Value}
                };

                _databaseConnection.RunStoredProc(DBConnect.StoredProcs.MatchRequest, requestMatch);

                People.Visible = false;
                NoQualified.Visible = false;
            }
            Response.Redirect("/Manager/RequestMatcher.aspx?m=MatchSuccess");
        }
    }
}