<%@ Page Title="Reports" Language="C#" MasterPageFile="~/MasterPages/mainLayout.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="BookSmart.Admin.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">📊 BookSmart Reports</h2>

    <div class="row text-center mb-4">
        <div class="col-md-3">
            <div class="card border-primary">
                <div class="card-body">
                    <h5 class="card-title">Books</h5>
                    <asp:Label ID="lblBooks" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-success">
                <div class="card-body">
                    <h5 class="card-title">Users</h5>
                    <asp:Label ID="lblUsers" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-warning">
                <div class="card-body">
                    <h5 class="card-title">Orders</h5>
                    <asp:Label ID="lblOrders" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-danger">
                <div class="card-body">
                    <h5 class="card-title">Revenue (USD)</h5>
                    <asp:Label ID="lblRevenue" runat="server" CssClass="display-6" />
                </div>
            </div>
        </div>
    </div>

    <hr />

    <h4 class="mt-4"> Monthly Revenue</h4>
    <asp:GridView ID="gvMonthlyRevenue" runat="server" CssClass="table table-bordered" AutoGenerateColumns="true" />

    <hr />

    <hr>
    <h4 class="mt-4"> Top 5 Selling Books</h4>
    <asp:GridView ID="gvTopBooks" runat="server" CssClass="table table-striped" AutoGenerateColumns="true" />
    <hr />

    <h4 class="mt-4"> Recent Reviews</h4>

    <asp:GridView
        ID="gvReviews"
        runat="server"
        CssClass="table table-striped"
        AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Book Title" />
            <asp:BoundField DataField="FullName" HeaderText="Reviewer" />
            <asp:BoundField DataField="Rating" HeaderText="Rating" />
            <asp:BoundField DataField="ReviewText" HeaderText="Review" />
            <asp:BoundField DataField="ReviewDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
        </Columns>
    </asp:GridView>

    
</asp:Content>

