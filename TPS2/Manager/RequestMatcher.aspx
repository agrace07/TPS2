<%@ Page Title="Match Requests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestMatcher.aspx.cs" Inherits="TPS2.Manager.RequestMatcher" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <div>
        <asp:ListBox runat="server" ID="ActiveRequests" OnSelectedIndexChanged="EnableSubmit" AutoPostBack="True"/>
        <asp:ListBox runat="server" ID="People" OnSelectedIndexChanged="EnableSubmit" AutoPostBack="True" SelectionMode="Multiple"/>
    </div>
    <div>
        <asp:Button runat="server" ID="Submit" Text="Submit" OnClick="Submit_OnClick" Enabled="False"/>
    </div>
</asp:Content>