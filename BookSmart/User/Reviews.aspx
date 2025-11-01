<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reviews.aspx.cs" Inherits="BookSmart.User.Reviews" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Book Reviews</h2>

    <div class="mb-3">
        <label>Select Book:</label>
        <asp:DropDownList ID="ddlBooks" runat="server" CssClass="form-select" />
    </div>

    <div class="mb-3">
        <label>Rating:</label>
        <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-select">
            <asp:ListItem Value="1">⭐</asp:ListItem>
            <asp:ListItem Value="2">⭐⭐</asp:ListItem>
            <asp:ListItem Value="3">⭐⭐⭐</asp:ListItem>
            <asp:ListItem Value="4">⭐⭐⭐⭐</asp:ListItem>
            <asp:ListItem Value="5">⭐⭐⭐⭐⭐</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="mb-3">
        <label>Review:</label>
        <asp:TextBox ID="txtReview" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
    </div>

    <asp:Button ID="btnSubmitReview" runat="server" Text="Submit Review" CssClass="btn btn-primary" OnClick="btnSubmitReview_Click" />

    <hr />

    <asp:GridView ID="gvReviews" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="FullName" HeaderText="User" />
            <asp:BoundField DataField="Rating" HeaderText="Rating" />
            <asp:BoundField DataField="ReviewText" HeaderText="Review" />
            <asp:BoundField DataField="ReviewDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
        </Columns>
    </asp:GridView>
</asp:Content>
