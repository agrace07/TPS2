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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActiveRequests.DataSource = _databaseConnection.GetUnfilledRequests();
                ActiveRequests.DataValueField = "ID";
                ActiveRequests.DataTextField = "Email";
                ActiveRequests.DataBind();

                People.DataSource = _databaseConnection.GetAllEmployees();
                People.DataValueField = "Name";
                People.DataTextField = "Name";
                People.DataBind();
            }
        }

        protected void EnableSubmit(object sender, EventArgs e)
        {
            if (ActiveRequests.SelectedIndex > -1 && People.SelectedIndex > -1)
            {
                Submit.Enabled = true;
            }
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}