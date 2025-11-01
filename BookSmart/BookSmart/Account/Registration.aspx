<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs"
    Inherits="BookSmart.Account.Registration"
    MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4">Create an Account</h2>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" HeaderText="Please fix the following:" />

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtName">Full Name:</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtEmail">Email:</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>

        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtPassword">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="ddlCountry">Country:</label>
            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Select Country --" Value="" />
                <asp:ListItem Text="Mauritius" Value="Mauritius" />
                <asp:ListItem Text="India" Value="India" />
                <asp:ListItem Text="UK" Value="UK" />
                <asp:ListItem Text="USA" Value="USA" />
            </asp:DropDownList>
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtAddress1">Address Line 1:</label>
            <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtAddress2">Address Line 2:</label>
            <asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtCity">City:</label>
            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtState">State:</label>
            <asp:TextBox ID="txtState" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Panel runat="server" CssClass="mb-3">
            <label for="txtPostal">Postal Code:</label>
            <asp:TextBox ID="txtPostal" runat="server" CssClass="form-control" />
        </asp:Panel>

        <asp:Button ID="btnRegister" runat="server" Text="Register"
            CssClass="btn btn-success" OnClick="btnRegister_Click" />
    </div>
</asp:Content>
