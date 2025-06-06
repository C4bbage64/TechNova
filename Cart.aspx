<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="technova.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Your Shopping Cart</h2>

    <div class="cart-section">
        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False"
              OnRowCommand="gvCart_RowCommand" CssClass="admin-table">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Price" HeaderText="Unit Price (RM)" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="Total" HeaderText="Total (RM)" DataFormatString="{0:0.00}" />

                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Remove"
                                    CommandName="RemoveItem"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="btn-primary" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="cart-total">
            <asp:Label ID="lblTotal" runat="server" Font-Bold="true" />
        </div>

        <asp:Button ID="btnCheckout" runat="server" Text="Checkout" CssClass="btn-primary" OnClick="btnCheckout_Click" />
    </div>
</asp:Content>