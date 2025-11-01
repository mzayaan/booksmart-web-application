<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BookSmart.Admin.Admin" MasterPageFile="~/MasterPages/mainLayout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>📘 Book Inventory</h2>

    <asp:Label ID="lblStatus" runat="server" CssClass="text-success d-block mb-2" />
    <!-- Search Bar -->
<div class="input-group mb-3">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by Title or Category..." />
    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" />
    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-outline-secondary ms-2" OnClick="btnClear_Click" />
</div>

    <asp:DataGrid ID="dgBooks" runat="server" AutoGenerateColumns="False"
        BorderWidth="1px" BorderStyle="Solid" CssClass="table table-bordered"
        OnEditCommand="dgBooks_EditCommand"
        OnCancelCommand="dgBooks_CancelCommand"
        OnUpdateCommand="dgBooks_UpdateCommand"
        OnDeleteCommand="dgBooks_DeleteCommand">
        
        <Columns>
            <asp:BoundColumn DataField="ID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundColumn DataField="Title" HeaderText="Title" />
            <asp:BoundColumn DataField="Author" HeaderText="Author" />
            <asp:BoundColumn DataField="Category" HeaderText="Category" />
            <asp:BoundColumn DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
            <asp:BoundColumn DataField="Stock" HeaderText="Stock" />
            <asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" />
            <asp:ButtonColumn CommandName="Delete" Text="Delete" />
        </Columns>
    </asp:DataGrid>

    <hr />
<h4>Add New Book</h4>
<div class="row g-3">
    <div class="col-md-3">
        <asp:TextBox ID="txtNewTitle" runat="server" CssClass="form-control" Placeholder="Title" />
    </div>
    <div class="col-md-2">
        <asp:TextBox ID="txtNewAuthor" runat="server" CssClass="form-control" Placeholder="Author" />
    </div>
    <div class="col-md-2">
        <asp:TextBox ID="txtNewCategory" runat="server" CssClass="form-control" Placeholder="Category" />
    </div>
    <div class="col-md-2">
        <asp:TextBox ID="txtNewPrice" runat="server" CssClass="form-control" Placeholder="Price" />
    </div>
    <div class="col-md-2">
        <asp:TextBox ID="txtNewStock" runat="server" CssClass="form-control" Placeholder="Stock" />
    </div>
    <div class="col-md-1">
        <asp:Button ID="btnAddBook" runat="server" CssClass="btn btn-success w-100" Text="Add" OnClick="btnAddBook_Click" />
    </div>
</div>

</asp:Content>
