<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewRequests.aspx.cs" Inherits="TPS2.Customer.ViewRequests" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div>
        <asp:ListBox runat="server" ID="RequestDates" AutoPostBack="True" OnSelectedIndexChanged="RequestDates_OnSelectedIndexChanged"></asp:ListBox>
    </div>
    <div id="DetailsPanel" runat="server" Visible="False">
        <p>Required Skills</p>
        <asp:ListBox runat="server" ID="RequiredSkillListBox" SelectionMode="Multiple"/>
        <p>Requested Skills</p>
        <asp:ListBox runat="server" ID="RequestedSkillListBox" SelectionMode="Multiple"/>
        <p>Education Level</p>
        <asp:DropDownList runat="server" ID="EducationLevel"/>
        <p>Required?</p>
        <asp:CheckBox runat="server" ID="EducationRequired"/>
        <p>Starting Salary</p>
        <asp:TextBox runat="server" ID="StartingSalary"/>
        <p>Address Line 1:</p>
        <asp:TextBox runat="server" ID="Address1TextBox"></asp:TextBox>
        <p>Address Line 2:</p>
        <asp:TextBox runat="server" ID="Address2TextBox"></asp:TextBox>
        <p>City:</p>
        <asp:TextBox runat="server" ID="CityTextBox"></asp:TextBox>
        <p>State:</p>
        <asp:DropDownList runat="server" ID="StatesListBox"/>
        <p>Zip:</p>
        <asp:TextBox runat="server" ID="ZipTextBox"></asp:TextBox>
        <p>Telecommute Available?:</p>
        <asp:CheckBox runat="server" ID="TelecommuteCheckBox"/>
    </div>
</asp:Content>
