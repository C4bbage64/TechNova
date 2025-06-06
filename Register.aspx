<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="technova.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="register-container">
        <h2>Create Your TechNova Account</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" /><br />

        <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" /><br />

        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" placeholder="Email" TextMode="Email" /><br />

        <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" placeholder="Password" TextMode="Password" /><br /><br />

        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn-primary" OnClick="btnRegister_Click" />
    </div>
</asp:Content>
