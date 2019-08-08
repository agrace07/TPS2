<%@ Page Title="Assign Permissions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignPermissions.aspx.cs" Inherits="TPS2.Manager.AssignPermissions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>
    <div>
        <table>
            <colgroup>

            </colgroup>
            <tr><td>Select a user:</td></tr>
            <tr>
                <td rowspan="3"><asp:ListBox runat="server" ID="UserList" OnSelectedIndexChanged="UserList_OnSelectedIndexChanged" AutoPostBack="True" /></td>
            
                <td><asp:CheckBox runat="server" ID="employeeChkBox" Text="Employee" AutoPostBack="True"/></td>
            </tr>
            <tr>
                <td><asp:CheckBox runat="server" ID="customerChkBox" Text="Customer" AutoPostBack="True"/></td>
            </tr>
            <tr>
                <td><asp:CheckBox runat="server" ID="managerChkBox" Text="Manager" AutoPostBack="True"/></td>
            </tr>
        </table>
        <asp:Button runat="server" Text="Submit" OnClick="Submit_OnClick"/>
    </div>
</asp:Content>
