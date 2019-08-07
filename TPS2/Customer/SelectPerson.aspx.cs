using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using TPS2.DBInteraction;

namespace TPS2.Customer
{
    public partial class SelectPerson : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SelectDiv.Visible = false;
            noCandidates.Visible = false;
        
            var message = Request.QueryString["m"];
            if (message != null)
            {
                // Strip the query string from action
                Form.Action = ResolveUrl("~/Customer/SelectPerson");

                SuccessMessage =
                    message == "SelectSuccess" ? "Your selection has been noted.  Your new employee will be contacting you soon."
                        //: message == "SetPwdSuccess" ? "Your password has been set."
                        : String.Empty;
                successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            }

            if (!IsPostBack || message != null)
            {

                var filledIds = new List<string>();
                foreach (var id in _databaseConnection.GetFilledRequests(User.Identity.GetUserId()))
                {
                    filledIds.Add(id.ToString());
                }

                if (filledIds.Count > 0)
                {
                    FilledRequests.DataSource = filledIds;
                    FilledRequests.DataBind();
                }
                else
                {
                    candidatesAvailable.Visible = false;
                    noCandidates.Visible = true;
                }
            }
        }

        protected void FilledRequests_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectDiv.Visible = true;
            var candidates = _databaseConnection.GetCandidateList(FilledRequests.SelectedValue);
            
            CandidateList.DataSource = candidates;
            CandidateList.DataValueField = "Id";
            CandidateList.DataTextField = "Name";
            CandidateList.DataBind();
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            _databaseConnection.SelectEmployee(CandidateList.SelectedValue, FilledRequests.SelectedValue);
            Response.Redirect("/Customer/SelectPerson?m=SelectSuccess");
        }
    }
}