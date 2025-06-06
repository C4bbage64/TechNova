<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="technova.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Profile</h2>

    <asp:Label ID="lblStatus" runat="server" ForeColor="Green" Font-Bold="true" />

    <div class="profile-form">
        <label>Username:</label><br />
        <asp:TextBox ID="txtUsername" runat="server" ReadOnly="true" CssClass="input" /><br /><br />

        <label>Email:</label><br />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" /><br /><br />

        <label>New Password (optional):</label><br />
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="input" /><br /><br />

        <label>Confirm New Password:</label><br />
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input" /><br /><br />

        <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" OnClick="btnUpdate_Click" CssClass="btn-primary" />
    </div>
</asp:Content>
