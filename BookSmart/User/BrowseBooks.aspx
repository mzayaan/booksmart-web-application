<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseBooks.aspx.cs" Inherits="BookSmart.User.BrowseBooks" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mt-4 mb-4">Available Books</h2>

    <div class="row mb-3">
        <div class="col-md-3">
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                <asp:ListItem Text="-- All Categories --" Value="" />
            </asp:DropDownList>
        </div>
        <div class="col-md-5">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search books..." />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
        </div>
    </div>

    <asp:GridView ID="gvBooks" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered"
        OnRowCommand="gvBooks_RowCommand"
        DataKeyNames="ID">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Author" HeaderText="Author" />
            <asp:BoundField DataField="Category" HeaderText="Category" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />

            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <button class="btn btn-sm btn-outline-primary"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#collapseImage<%# Eval("ID") %>">
                        View Image
                    </button>

                    <div class="collapse mt-2" id="collapseImage<%# Eval("ID") %>">
                        <img src='<%# ResolveUrl("/" + Eval("ImageURL")) %>'
                            class="img-thumbnail mt-2"
                            style="max-width: 120px; max-height: 120px;"
                            alt="Book Image" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <button class="btn btn-sm btn-outline-secondary"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#collapseDesc<%# Eval("ID") %>">
                        View Description
                    </button>

                    <div class="collapse mt-2" id="collapseDesc<%# Eval("ID") %>">
                        <p class="mt-2"><%# Eval("Description") %></p>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField CommandName="AddToCart" Text="Add to Cart" ButtonType="Button" />
            <asp:ButtonField CommandName="AddToWishlist" Text="Add to Wishlist" ButtonType="Button" />

        </Columns>
    </asp:GridView>
</asp:Content>
