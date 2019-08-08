<%@ Page Title="Select Candidate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectPerson.aspx.cs" Inherits="TPS2.Customer.SelectPerson" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>
    <div ID="candidatesAvailable" runat="server">
        <p>Your filled requests.  Please select which request you would like to view candidates for.</p>
        <div>
            <asp:RadioButtonList runat="server" ID="FilledRequests" OnSelectedIndexChanged="FilledRequests_OnSelectedIndexChanged" AutoPostBack="True"/>
        </div>
    </div>
    <div ID="noCandidates" runat="server">
        <p>You have no filled requests at this time.  If it has been more than 48 hours since you submitted your request, please contact us at 555-555-5555 or via email at supprt@tps.com</p>
    </div>
    <div ID="SelectDiv" runat="server">
        <p>Possible Candidates:</p>
        <asp:ListBox runat="server" ID="CandidateList" OnSelectedIndexChanged="CandidateList_OnSelectedIndexChanged" AutoPostBack="True"/>
        <p>Candidate skills:</p>
        <asp:ListBox runat="server" ID="CandidateSkillList" SelectionMode="Multiple"/>
        <div>
            <asp:Button runat="server" ID="Submit" Text="Submit" OnClick="Submit_OnClick" Enabled="True"/>
        </div>
    </div>
</asp:Content>
