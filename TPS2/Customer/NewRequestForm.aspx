<%@ Page Title="New Employee Request" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewRequestForm.aspx.cs" Inherits="TPS2.Customer.NewRequestForm" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>
    <div>
        <dl>
            <dt>Required Skills</dt>
            <asp:ListBox runat="server" ID="RequiredSkillListBox" SelectionMode="Multiple"/>
            <dt>Requested Skills</dt>
            <asp:ListBox runat="server" ID="RequestedSkillListBox" SelectionMode="Multiple"/>
            <dt>Education Level</dt>
            <asp:DropDownList runat="server" ID="EducationLevel"/>
            <dt>Required?</dt>
            <asp:CheckBox runat="server" ID="EducationRequired"/>
            <dt>Starting Salary</dt>
            <asp:TextBox runat="server" ID="StartingSalary"/>
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
            <dt>Telecommute Available?:</dt>
            <asp:CheckBox runat="server" ID="TelecommuteCheckBox"/>
        </dl>
        <asp:Button runat="server" ID="SubmitBtn" Text="Submit" OnClick="SubmitBtn_Click"/>
    </div>
</asp:Content>
