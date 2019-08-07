using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPS2.DBInteraction;

namespace TPS2.Manager
{
    public partial class AssignPermissions : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)// || message != null)
            {
                //TODO get list of all users
                UserList.DataSource = _databaseConnection.GetAllEmployees();
                UserList.DataValueField = "ID";
                UserList.DataTextField = "Name";
                UserList.DataBind();
            }
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            //TODO update the DB with the check boxes
        }

        protected void UserList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO update the checkboxes to show the user's current roles
            Roles roles = _databaseConnection.GetUsersRoles(UserList.SelectedValue);
            
            //if (roles == null)
            //    return;
            //if (roles.Customer)
            //    customerChkBox.Checked = true;
            //if (roles.Employee)
            //    employeeChkBox.Checked = true;
            //if (roles.Manager)
            //    managerChkBox.Checked = true;
        }
    }
}