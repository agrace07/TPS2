<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPS2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Taylor's Professional Services</h1>
        <p class="lead">Taylor's Professional Services(TPS) is here to connect the best canidate for the best position.</p>
        <p><a runat="server" href="~/Account/Register.aspx">Get Registered &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting Started</h2>
            <p>
            Post your contact information and availability if you are looking for a job. Or, post the postions you need filled if you are looking for an employee(s).
            </p>
            <p>
                <a runat="server" href="~/Account/Register.aspx">Register Here &raquo;</a>
                <a runat="server" href="~/Account/Login.aspx">Login Here &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>About Us</h2>
            <p>
                Come see how we better businesses around us and how we strive for a greater future.
            </p>
            <p>
                <a runat="server" href="~/About">About Us &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Contact Us</h2>
            <p>
                If you have any questions or suggestions contact us.
            </p>
            <p>
                <a runat="server" href="~/Contact">Contact Us &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
