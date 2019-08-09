using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPS2.DBInteraction;
using Parameter = TPS2.DBInteraction.Parameter;

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
                ActiveRequests.DataTextField = "Display";
                ActiveRequests.DataBind();
            }
        }

        protected void EnableSubmit(object sender, EventArgs e)
        {
            if (ActiveRequests.SelectedIndex > -1)
            {
                People.DataSource = _databaseConnection.GetQualifiedEmployees(ActiveRequests.SelectedValue);
                People.DataValueField = "ID";
                People.DataTextField = "Name";
                People.DataBind();

                if (People.Items.Count > 0)
                {
                    peopleCaption.Visible = true;
                    People.Visible = true;
                    NoQualified.Visible = false;
                }
                else
                {
                    peopleCaption.Visible = false;
                    People.Visible = false;
                    NoQualified.Visible = true;
                }

                if (People.GetSelectedIndices().Length == 3)
                {
                    Submit.Enabled = true;
                }
            }
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            foreach (var item in People.Items.Cast<ListItem>().Where(item => item.Selected))
            {
                var requestMatch = new List<Parameter>
                { 
                    //ID of the person
                    new Parameter {ParameterName = "@AspNetUserId", ParameterValue = item.Value},
                    //ID of the request
                    new Parameter {ParameterName = "@RequestId", ParameterValue = ActiveRequests.SelectedItem.Value}
                };

                _databaseConnection.RunStoredProc(DBConnect.StoredProcs.MatchRequest, requestMatch);
                
                peopleCaption.Visible = false;
                People.Visible = false;
                NoQualified.Visible = false;
            }
            Response.Redirect("/Manager/RequestMatcher.aspx?m=MatchSuccess");
        }
    }
}