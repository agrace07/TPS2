﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using TPS2.DBInteraction;
using TPS2.Models;

namespace TPS2.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        private readonly DBConnect _databaseConnection = new DBConnect();
        private EmployeeModel employee;
        //private bool dataExists = true;

        public List<StateType> StateList = new List<StateType>();

        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!IsPostBack)
            {
                //Get the skills from the DB
                SkillListBox.DataSource = _databaseConnection.GetSkillList();
                SkillListBox.DataValueField = "Id";
                SkillListBox.DataTextField = "Name";
                SkillListBox.DataBind();

                // Determine the sections to render
                if (HasPassword(manager))
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    CreatePassword.Visible = true;
                    ChangePassword.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : message == "AddPhoneNumberSuccess" ? "Phone number has been added"
                        : message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
                        : message == "UpdateInfoSuccess" ? "Your information has been updated"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
                
                StatesListBox.DataSource = _databaseConnection.GetStates();
                StatesListBox.DataValueField = "Abbreviation";
                StatesListBox.DataTextField = "Name";
                StatesListBox.DataBind();

                employee = _databaseConnection.GetEmployeeModel(User.Identity.GetUserId());
                if (employee.FirstName == null)
                {
                    return;
                }
                FirstNameTextBox.Text = employee.FirstName;
                LastNameTextBox.Text = employee.LastName;
                RelocateCheckBox.Checked = employee.WillingToRelocate;
                AvailabilityDateCalendar.SelectedDate = employee.AvailabilityDate;
                PhoneTextBox.Text = employee.PhoneNumber;
                Address1TextBox.Text = employee.Location.AddressLine1;
                Address2TextBox.Text = employee.Location.AddressLine2;
                CityTextBox.Text = employee.Location.City;
                StatesListBox.SelectedIndex = StatesListBox.Items.IndexOf(StatesListBox.Items.FindByText(employee.Location.State.Name));
                ZipTextBox.Text = employee.Location.Zip;

                foreach (Experience skill in employee.WorkExperience)
                    SkillListBox.Items.FindByValue(skill.ExperienceId.ToString()).Selected = true;
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phone number from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            //TODO Validate inputs 
            var parameters = new List<ParameterList>
            {
                new ParameterList {ParameterName = "@FirstName", Parameter = FirstNameTextBox.Text},
                new ParameterList {ParameterName = "@LastName", Parameter = LastNameTextBox.Text},
                new ParameterList {ParameterName = "@AspNetUserId", Parameter = User.Identity.GetUserId()},
                new ParameterList
                    {ParameterName = "@Relocate", Parameter = RelocateCheckBox.Checked == true ? "1" : "0"},
                new ParameterList
                {
                    ParameterName = "@AvailabilityDate",
                    Parameter = AvailabilityDateCalendar.SelectedDate.ToShortDateString()
                },
                new ParameterList {ParameterName = "@PhoneNumber", Parameter = PhoneTextBox.Text},
                new ParameterList {ParameterName = "@AddressLine1", Parameter = Address1TextBox.Text},
                new ParameterList {ParameterName = "@AddressLine2", Parameter = Address2TextBox.Text},
                new ParameterList {ParameterName = "@City", Parameter = CityTextBox.Text},
                new ParameterList {ParameterName = "@Zip", Parameter = ZipTextBox.Text},
                new ParameterList {ParameterName = "@State", Parameter = StatesListBox.Text}
            };
            _databaseConnection.RunStoredProc(DBConnect.StoredProcs.UpdateEmployeeInfo, parameters);

            var requiredItems = SkillListBox.Items.Cast<ListItem>().Where(item => item.Selected);
            foreach (var item in requiredItems)
            {
                var skillParameters = new List<ParameterList>
                {
                    new ParameterList {ParameterName = "@Id", Parameter = User.Identity.GetUserId()},
                    new ParameterList {ParameterName = "@SkillId", Parameter = item.Value}
                };

                _databaseConnection.RunStoredProc(DBConnect.StoredProcs.InsertEmployeeSkills, skillParameters);
            }

            Response.Redirect("/Account/Manage?m=UpdateInfoSuccess");
        }
    }
}