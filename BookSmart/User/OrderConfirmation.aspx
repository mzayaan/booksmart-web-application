<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="BookSmart.User.OrderConfirmation" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">Confirm Your Order</h2>

    <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Book Title" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold fs-5" />

    <div class="mt-3">
        <asp:Button ID="btnConfirm" runat="server" Text="Confirm Order" CssClass="btn btn-primary" OnClick="btnConfirm_Click" />
    </div>
</asp:Content>
