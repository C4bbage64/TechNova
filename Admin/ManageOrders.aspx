<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="technova.Admin.ManageOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Orders</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />

    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" DataKeyNames="OrderID"
        CssClass="admin-table"
        OnRowEditing="gvOrders_RowEditing"
        OnRowUpdating="gvOrders_RowUpdating"
        OnRowCancelingEdit="gvOrders_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order #" ReadOnly="true" />
            <asp:BoundField DataField="Username" HeaderText="Customer" />
            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd MMM yyyy}" ReadOnly="true" />
            <asp:BoundField DataField="TotalAmount" HeaderText="Total (RM)" DataFormatString="{0:0.00}" ReadOnly="true" />

            <asp:TemplateField HeaderText="Order Status">
                <ItemTemplate>
                    <%# Eval("OrderStatus") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Processing</asp:ListItem>
                        <asp:ListItem>Shipped</asp:ListItem>
                        <asp:ListItem>Delivered</asp:ListItem>
                        <asp:ListItem>Cancelled</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>
