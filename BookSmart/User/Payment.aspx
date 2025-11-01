<%@ Page Title="Payment" Language="C#" MasterPageFile="~/MasterPages/mainLayout.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="BookSmart.User.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">💳 Payment Details</h2>

    <div class="mb-3">
        <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold fs-5" />
    </div>

    <div class="mb-3">
        <label for="txtCard" class="form-label">Card Number</label>
        <asp:TextBox ID="txtCard" runat="server" CssClass="form-control" />
    </div>

    <div class="mb-3">
        <label for="txtHolder" class="form-label">Card Holder</label>
        <asp:TextBox ID="txtHolder" runat="server" CssClass="form-control" />
    </div>

    <div class="mb-3">
        <label for="ddlMethod" class="form-label">Payment Method</label>
        <asp:DropDownList ID="ddlMethod" runat="server" CssClass="form-select">
            <asp:ListItem Text="-- Select --" Value="" />
            <asp:ListItem Text="Credit Card" Value="Credit Card" />
            <asp:ListItem Text="PayPal" Value="PayPal" />
            <asp:ListItem Text="Bank Transfer" Value="Bank Transfer" />
        </asp:DropDownList>
    </div>

    <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="btn btn-success" OnClick="btnPay_Click" />
    <br /><br />
    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />
</asp:Content>
