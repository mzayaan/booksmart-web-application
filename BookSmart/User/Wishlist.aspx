<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs" Inherits="BookSmart.User.Wishlist" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Your Wishlist</h2>

    <asp:GridView 
        ID="gvWishlist" 
        runat="server" 
        AutoGenerateColumns="False" 
        CssClass="table table-striped"
        DataKeyNames="WishlistID,BookID"
        OnRowCommand="gvWishlist_RowCommand">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Book Title" />
            <asp:BoundField DataField="Author" HeaderText="Author" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
            <asp:ButtonField CommandName="AddToCart" Text="Add to Cart"  ButtonType="Button" />
            <asp:ButtonField CommandName="Remove"  Text="Remove"  ButtonType="Button"  />

        </Columns>
    </asp:GridView>
</asp:Content>
