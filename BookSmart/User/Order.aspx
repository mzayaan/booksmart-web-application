<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="BookSmart.User.Order" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mt-4 mb-3">Place Your Order</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3" />
    
    <asp:Button 
        ID="btnPlaceOrder" 
        runat="server" 
        Text="Confirm Order" 
        CssClass="btn btn-primary" 
        OnClick="btnPlaceOrder_Click" />
</asp:Content>
