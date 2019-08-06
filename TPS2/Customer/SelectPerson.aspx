<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectPerson.aspx.cs" Inherits="TPS2.Customer.SelectPerson" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <div>
        Your filled requests.  Please select which request you would like to view candidates for.
        <asp:RadioButtonList runat="server" ID="FilledRequests" OnSelectedIndexChanged="FilledRequests_OnSelectedIndexChanged" AutoPostBack="True"/>
    </div>
    <div ID="SelectDiv" runat="server">
        Possible Candidates:
        <asp:ListBox runat="server" ID="CandidateList"/>
        <div>
            <asp:Button runat="server" ID="Submit" Text="Submit" OnClick="Submit_OnClick" Enabled="True"/>
        </div>
    </div>
</asp:Content>
