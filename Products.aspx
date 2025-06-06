<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="technova.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Browse Products</h2>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TechNovaDB %>" SelectCommand="SELECT * FROM [Categories]"></asp:SqlDataSource>

    <asp:Label ID="lblFilter" runat="server" Text="Filter by Category: " />
    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
    <hr />

    <asp:Repeater ID="rptProducts" runat="server">
        <ItemTemplate>
            <div class="product-card">
                <img src='<%# Eval("ImagePath") %>' alt='<%# Eval("ProductName") %>' width="150" /><br />
                <h3><%# Eval("ProductName") %></h3>
                <p><%# Eval("Description") %></p>
                <strong>RM <%# Eval("Price", "{0:0.00}") %></strong><br />
                <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" 
                            CommandArgument='<%# Eval("ProductID") %>' OnCommand="btnAddToCart_Command" CssClass="btn-primary" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
