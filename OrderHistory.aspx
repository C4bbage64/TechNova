<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="technova.OrderHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Your Order History</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" />

    <div class="order-history-section">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="admin-table">
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order #" />
                <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Price (RM)" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="Total" HeaderText="Total (RM)" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="OrderStatus" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
