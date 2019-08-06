using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using TPS2.DBInteraction;

namespace TPS2.Customer
{
    public partial class SelectPerson : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            SelectDiv.Visible = false;

            if (!IsPostBack)
            {
                var filledIds = new List<string>();
                foreach (var id in _databaseConnection.GetFilledRequests(User.Identity.GetUserId()))
                {
                    filledIds.Add(id.ToString());
                }

                FilledRequests.DataSource = filledIds;
                FilledRequests.DataBind();
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
        }
    }
}