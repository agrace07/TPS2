<%@ Page Title="Customer Dashboard" Language="C#" MasterPageFile="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<h2><%: Title %>.</h2>

<div>
    <ul>
        <li><a runat="server" href="NewRequestForm.aspx" title="New Staffing Request">Create a New Request</a></li>
        <li><a runat="server" href="SelectPerson.aspx" title="Select Candidate">Select an Employee</a></li>
        <li><a runat="server" href="ViewRequests.aspx" title="View Staffing Requests">View Existing Requests</a></li>
    </ul>
</div>
</asp:Content>