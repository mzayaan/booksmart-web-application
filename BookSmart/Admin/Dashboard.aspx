<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPages/mainLayout.Master"
    AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BookSmart.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>📊 Admin Dashboard</h2>

    <div class="row mt-4">
        <div class="col-md-3">
            <div class="card text-bg-primary mb-3">
                <div class="card-body text-center">
                    <h5 class="card-title">Total Books</h5>
                    <asp:Label ID="lblBooks" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card text-bg-secondary mb-3">
                <div class="card-body text-center">
                    <h5 class="card-title">Total Users</h5>
                    <asp:Label ID="lblUsers" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card text-bg-success mb-3">
                <div class="card-body text-center">
                    <h5 class="card-title">Total Orders</h5>
                    <asp:Label ID="lblOrders" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card text-bg-warning mb-3">
                <div class="card-body text-center">
                    <h5 class="card-title">Revenue</h5>
                    <asp:Label ID="lblRevenue" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>
        <asp:Button ID="btnViewReports" runat="server" Text="📄 View Full Reports" CssClass="btn btn-outline-primary mt-4" OnClick="btnViewReports_Click" />
    </div>
</asp:Content>
