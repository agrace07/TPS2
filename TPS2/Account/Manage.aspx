<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="TPS2.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Change your account settings</h4>
                <hr />
                <dl class="dl-horizontal">
                    <dt>Password:</dt>
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Change]" Visible="false" ID="ChangePassword" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Create]" Visible="false" ID="CreatePassword" runat="server" />
                    </dd>
                    <dt>External Logins:</dt>
                    <dd><%: LoginsCount %>
                        <asp:HyperLink NavigateUrl="/Account/ManageLogins" Text="[Manage]" runat="server" />

                    </dd>
                </dl>
            </div>
            <div>
                <dl>
                    <dt>First Name:</dt>
                    <asp:TextBox runat="server" ID="FirstNameTextBox"></asp:TextBox>
                    <dt>Last Name:</dt>
                    <asp:TextBox runat="server" ID="LastNameTextBox"></asp:TextBox>
                    <dt>Phone Number:</dt>
                    <asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox>
                    <dt>Address Line 1:</dt>
                    <asp:TextBox runat="server" ID="Address1TextBox"></asp:TextBox>
                    <dt>Address Line 2:</dt>
                    <asp:TextBox runat="server" ID="Address2TextBox"></asp:TextBox>
                    <dt>City:</dt>
                    <asp:TextBox runat="server" ID="CityTextBox"></asp:TextBox>
                    <dt>State:</dt>
                    <asp:DropDownList runat="server" ID="StatesListBox"/>
                    <dt>Zip:</dt>
                    <asp:TextBox runat="server" ID="ZipTextBox"></asp:TextBox>
                    <dt>Willing to Relocate?:</dt>
                    <asp:CheckBox runat="server" ID="RelocateCheckBox"/>
                    <dt>Availability Date</dt>
                    <asp:Calendar runat="server" ID="AvailabilityDateCalendar"></asp:Calendar>
                    <dt>Skills</dt>
                    <asp:ListBox runat="server" ID="SkillListBox" SelectionMode="Multiple"/>
                    
                </dl>
                
                <asp:Button runat="server" ID="SubmitBtn" Text="Submit" OnClick="SubmitBtn_Click"/>
            </div>
        </div>
    </div>

</asp:Content>
