<%@ Page Title="Manager Dashboard" Language="C#" MasterPageFile="~/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <div>
        <ul>
            <li><a runat="server" href="RequestMatcher.aspx" title="Match Requests">Match Requests</a></li>
            <li><a runat="server" href="AssignPermissions.aspx" title="Assign Permissions">Assign Permissions</a></li>
        </ul>
    </div>
</asp:Content>