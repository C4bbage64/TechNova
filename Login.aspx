<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="technova.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="login-container">
        <h2>Login to TechNova</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" /><br />

        <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" /><br />

        <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" placeholder="Password" TextMode="Password" /><br /><br />

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-primary" OnClick="btnLogin_Click" /><br /><br />

        <div class="register-redirect">
            Don’t have an account?
            <a href="Register.aspx">Register here</a>
        </div>

    </div>
</asp:Content>
