<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TPS2.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Contact Us.</h3>
    <address>
        100 On A Street<br />
        Somewhere, AZ 86303-6158<br />
        <abbr title="Phone">P:</abbr>
        800.HELP.YOU(800.435.7968)<br />
        <abbr title="Hours">Hrs:</abbr>
        7:00AM-8:00PM PST
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@tps.com">Support@tps.com</a>
    </address>
</asp:Content>
