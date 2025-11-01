<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="BookSmart.Account.Login"
    MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Login to BookSmart</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />

    <div class="mb-3">
        <label>Email:</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>
    <div class="mb-3">
        <label>Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
    </div>

    <asp:Button ID="btnLogin" runat="server" Text="Login"
        CssClass="btn btn-primary" OnClick="btnLogin_Click" />

    <p class="mt-3 text-center">
    Don’t have an account?
    <asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="~/Account/Registration.aspx">Register here</asp:HyperLink>
    </p>
</asp:Content>
