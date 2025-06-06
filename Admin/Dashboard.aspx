<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="technova.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Admin Dashboard</h2>

    <div class="dashboard-grid">
        <div class="dashboard-card">
            <h3>📦 Products</h3>
            <asp:Label ID="lblProducts" runat="server" CssClass="dash-stat" />
        </div>
        <div class="dashboard-card">
            <h3>👤 Users</h3>
            <asp:Label ID="lblUsers" runat="server" CssClass="dash-stat" />
        </div>
        <div class="dashboard-card">
            <h3>🛒 Orders</h3>
            <asp:Label ID="lblOrders" runat="server" CssClass="dash-stat" />
        </div>
        <div class="dashboard-card">
            <h3>💰 Revenue</h3>
            <asp:Label ID="lblRevenue" runat="server" CssClass="dash-stat" />
        </div>
    </div>

    <h3>📅 Recent Orders</h3>
    <asp:GridView ID="gvRecentOrders" runat="server" AutoGenerateColumns="False" CssClass="admin-table">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order #" />
            <asp:BoundField DataField="Username" HeaderText="User" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="Total" HeaderText="Total (RM)" DataFormatString="{0:0.00}" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnExportCSV" runat="server" Text="⬇️ Export to CSV" OnClick="btnExportCSV_Click" CssClass="btn-secondary" />

</asp:Content>
