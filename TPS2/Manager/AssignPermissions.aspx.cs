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
        public Roles Roles { get; set; }

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
                Form.Action = ResolveUrl("~/Manager/AssignPermissions");

                SuccessMessage =
                    message == "Success" ? "Permissions updated."
                        //follow this format to add additional messages:
                        //: message == "SetPwdSuccess" ? "Your password has been set."
                        : String.Empty;
                successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            }

            if (Roles != null)
            {
                employeeChkBox.Checked = Roles.Employee;
                customerChkBox.Checked = Roles.Customer;
                managerChkBox.Checked = Roles.Manager;
            }

            if (!IsPostBack)
            {
                UserList.DataSource = _databaseConnection.GetAllEmployees();
                UserList.DataValueField = "ID";
                UserList.DataTextField = "Name";
                UserList.DataBind();
            }
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            var newRoles = new Roles
            {
                Customer = customerChkBox.Checked,
                Employee = employeeChkBox.Checked,
                Manager = managerChkBox.Checked
            };

            _databaseConnection.UpdateUserRoles(UserList.SelectedValue, newRoles);
            Response.Redirect("/Manager/AssignPermissions.aspx?m=Success");
        }

        protected void UserList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            customerChkBox.Checked = employeeChkBox.Checked = managerChkBox.Checked = false;
            Roles = _databaseConnection.GetUsersRoles(UserList.SelectedValue);

            if (Roles == null)
                return;
            if (Roles.Customer)
                customerChkBox.Checked = true;
            if (Roles.Employee)
                employeeChkBox.Checked = true;
            if (Roles.Manager)
                managerChkBox.Checked = true;
        }
    }
}