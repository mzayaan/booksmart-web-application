<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="BookSmart.Account.Logout" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-3">Logout</h2>
    <asp:Label ID="lblMessage" runat="server" Text="Click below to log out from your account." CssClass="alert alert-info d-block" />
    <asp:Button ID="btnLogout" runat="server" Text="Log Out Now" CssClass="btn btn-danger mt-3" OnClick="btnLogout_Click" />
</asp:Content>
