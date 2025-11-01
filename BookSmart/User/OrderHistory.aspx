<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="BookSmart.User.OrderHistory" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">📦 Order History</h2>

    <asp:Button ID="btnDownloadPDF" runat="server"
    Text="Download PDF"
    CssClass="btn btn-outline-secondary mb-3"
    OnClick="btnDownloadPDF_Click" />

    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order #" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
        </Columns>
    </asp:GridView>

    <hr />
    <asp:Label ID="lblStatus" runat="server" Text="" Visible="false" />

<div class="mt-4 text-center">
    <asp:Button ID="btnHome" runat="server" Text="Go to Homepage" CssClass="btn btn-primary me-2" OnClick="btnHome_Click" />
    <asp:Button ID="btnBrowse" runat="server" Text="Browse More Books" CssClass="btn btn-success me-2" OnClick="btnBrowse_Click" />
    <asp:Button ID="btnResetOrders" runat="server" Text="Reset Order History"
        CssClass="btn btn-danger"
        OnClick="btnResetOrders_Click"
        OnClientClick="return confirm('Are you sure you want to permanently delete your order history?');" />
</div>


</asp:Content>
