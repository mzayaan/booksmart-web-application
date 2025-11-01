<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="BookSmart.Account.ForgotPassword" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Reset Password</h2>

    <asp:Label ID="lblResult" runat="server" CssClass="text-danger" />

    <div class="mb-3">
        <label>Enter your email:</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>

    <div id="resetPanel" runat="server" visible="false">
        <div class="mb-3">
            <label>New Password:</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>

        <div class="mb-3">
            <label>Confirm New Password:</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
    </div>

    <asp:Button ID="btnRecover" runat="server" Text="Continue"
        CssClass="btn btn-warning" OnClick="btnRecover_Click" />

    <asp:Button ID="btnReset" runat="server" Text="Reset Password"
        CssClass="btn btn-primary mt-3" OnClick="btnReset_Click" Visible="false" />
</asp:Content>
