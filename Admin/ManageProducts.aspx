<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="technova.ManageProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Products</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" />

    <!-- Add Product -->
    <div class="admin-section">
        <h3>Add New Product</h3>
        <asp:TextBox ID="txtName" runat="server" Placeholder="Product Name" CssClass="input" />
        <asp:TextBox ID="txtPrice" runat="server" Placeholder="Price" CssClass="input" />
        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input" />
        <asp:TextBox ID="txtImage" runat="server" Placeholder="Image Path" CssClass="input" />
        <asp:Button ID="btnAdd" runat="server" Text="Add Product" OnClick="btnAdd_Click" CssClass="btn-primary" />
    </div>

    <!-- Product List -->
    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
        OnRowEditing="gvProducts_RowEditing"
        OnRowUpdating="gvProducts_RowUpdating"
        OnRowCancelingEdit="gvProducts_RowCancelingEdit"
        OnRowDeleting="gvProducts_RowDeleting"
        CssClass="admin-table">
        <Columns>
            <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="true" />

            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <%# Eval("ProductName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNameEdit" runat="server" Text='<%# Bind("ProductName") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    RM <%# Eval("Price", "{0:0.00}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPriceEdit" runat="server" Text='<%# Bind("Price") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Category">
                <ItemTemplate>
                    <%# Eval("CategoryName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlCategoryEdit" runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <%# Eval("ImagePath") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtImageEdit" runat="server" Text='<%# Bind("ImagePath") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
