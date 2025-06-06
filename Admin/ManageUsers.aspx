<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="technova.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Users</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />

    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
        CssClass="admin-table"
        OnRowEditing="gvUsers_RowEditing"
        OnRowUpdating="gvUsers_RowUpdating"
        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
        OnSelectedIndexChanged="gvUsers_SelectedIndexChanged"
        OnRowCommand="gvUsers_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="true" />

            <asp:TemplateField HeaderText="Username">
                <ItemTemplate>
                    <%# Eval("Username") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditUsername" runat="server" Text='<%# Bind("Username") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <%# Eval("Email") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Admin">
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsAdmin" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdmin")) %>' Enabled="false" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkEditIsAdmin" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdmin")) %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Reset Password">
                <ItemTemplate>
                    <asp:Button ID="btnResetPwd" runat="server" Text="Reset" CommandName="ResetPwd" CommandArgument='<%# Eval("UserID") %>' CssClass="btn-secondary" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True" ShowSelectButton="True" SelectText="View Orders" />
        </Columns>
    </asp:GridView>

    <asp:Panel ID="pnlOrders" runat="server" Visible="false">
        <h3>Order History for: <asp:Label ID="lblSelectedUser" runat="server" /></h3>
        <div class="admin-section">
            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="admin-table">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order #" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total (RM)" DataFormatString="{0:0.00}" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>