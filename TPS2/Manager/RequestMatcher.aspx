<%@ Page Title="Match Requests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestMatcher.aspx.cs" Inherits="TPS2.Manager.RequestMatcher" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>
    <div>
        <p>Unfilled Requests</p>
        <asp:ListBox runat="server" ID="ActiveRequests" OnSelectedIndexChanged="EnableSubmit" AutoPostBack="True"/>
        <p ID="peopleCaption" runat="server" Visible="False">Qualified employees</p>
        <asp:ListBox runat="server" ID="People" OnSelectedIndexChanged="EnableSubmit" AutoPostBack="True" SelectionMode="Multiple" Visible="False"/>
        <p runat="server" ID="NoQualified" Visible="False">Uh oh....  No qualified employees.</p>
    </div>
    <div>
        <asp:Button runat="server" ID="Submit" Text="Submit" OnClick="Submit_OnClick" Enabled="False"/>
    </div>
</asp:Content>