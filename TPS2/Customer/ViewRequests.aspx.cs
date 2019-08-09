using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using TPS2.DBInteraction;

namespace TPS2.Customer
{
    public partial class ViewRequests : System.Web.UI.Page
    {
        private DBConnect _dbConnect = new DBConnect();

        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RequestDates.DataSource = _dbConnect.GetUsersRequests(User.Identity.GetUserId());
                RequestDates.DataValueField = "Item2";
                RequestDates.DataTextField = "Item1";
                RequestDates.DataBind();
            }
        }

        protected void RequestDates_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClientRequest cr = _dbConnect.GetRequest(RequestDates.SelectedValue);
            DetailsPanel.Visible = true;

            //RequestedSkillListBox.
            //RequiredSkillListBox
            //EducationLevel.SelectedIndex = 
            EducationRequired.Checked = cr.EducationRequired;
            StartingSalary.Text = cr.Salary.ToString();
            Address1TextBox.Text = cr.Location.AddressLine1;
            Address2TextBox.Text = cr.Location.AddressLine2;
            CityTextBox.Text = cr.Location.City;
            //StatesListBox
            ZipTextBox.Text = cr.Location.Zip;
            //TelecommuteCheckBox.Checked = cr.

        }
    }
}