<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="BookSmart.User.Cart" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mt-4 mb-4">Your Cart</h2>

    <asp:GridView 
        ID="gvCart" 
        runat="server" 
        AutoGenerateColumns="False" 
        CssClass="table table-bordered"
        DataKeyNames="ID"
        OnRowDeleting="gvCart_RowDeleting"
        OnRowEditing="gvCart_RowEditing"
        OnRowCancelingEdit="gvCart_RowCancelingEdit"
        OnRowUpdating="gvCart_RowUpdating">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Book Title" ReadOnly="true" />
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate><%# Eval("Quantity") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Quantity") %>' Width="60px" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>

    <br />
    <asp:Label ID="Label1" runat="server" CssClass="fw-bold" />
    <br />
    <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold fs-5" />
    <br />
    <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="btn btn-success mt-3" OnClick="btnCheckout_Click" />
</asp:Content>
