<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/mainLayout.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BookSmart.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="text-center p-5 bg-light rounded shadow">
        <h2>Welcome to BookSmart 📚</h2>
        <p class="lead">Your one-stop academic bookstore in Mauritius.</p>
        <asp:HyperLink ID="hlBrowse" runat="server" NavigateUrl="~/User/BrowseBooks.aspx" CssClass="btn btn-primary btn-lg" Text="Browse Books" />
    </div>

    <div class="mt-5">
        <h2 class="mb-4">Popular Categories</h2>
        <div class="row">
            <div class="col-md-4 mb-4">
                <a href="<%= ResolveUrl("~/User/BrowseBooks.aspx?category=Programming") %>" class="text-decoration-none text-reset">
                    <div class="card shadow-sm h-100">
                        <div class="card-body text-center">
                            <i class="bi bi-code-slash fs-1 text-primary"></i>
                            <h5 class="card-title mt-3">Programming</h5>
                            <p class="card-text">Explore the latest books in software development and coding.</p>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-md-4 mb-4">
                <a href="<%= ResolveUrl("~/User/BrowseBooks.aspx?category=Design") %>" class="text-decoration-none text-reset">
                    <div class="card shadow-sm h-100">
                        <div class="card-body text-center">
                            <i class="bi bi-pencil-square fs-1 text-success"></i>
                            <h5 class="card-title mt-3">Design</h5>
                            <p class="card-text">Master UI/UX, graphic design, and creative tools.</p>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-md-4 mb-4">
                <a href="<%= ResolveUrl("~/User/BrowseBooks.aspx?category=Academic") %>" class="text-decoration-none text-reset">
                    <div class="card shadow-sm h-100">
                        <div class="card-body text-center">
                            <i class="bi bi-book-half fs-1 text-warning"></i>
                            <h5 class="card-title mt-3">Academic</h5>
                            <p class="card-text">Get essential textbooks for your studies and research.</p>
                        </div>
                    </div>
                </a>
            </div>
        </div>
    </div>

    <div id="featuredBooksCarousel" class="carousel slide mt-5" data-bs-ride="carousel">
        <div class="carousel-inner">

            <div class="carousel-item active">
                <img src="images/aspnet1.jpg" class="d-block w-100 rounded" style="max-height: 400px; object-fit: contain;" alt="ASP.NET Fundamentals">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-50 rounded p-2">
                    <h5>ASP.NET Fundamentals</h5>
                    <p>Kickstart your web development journey with this essential guide.</p>
                </div>
            </div>

            <div class="carousel-item">
                <img src="images/sql1.jpg" class="d-block w-100 rounded" style="max-height: 400px; object-fit: contain;" alt="SQL Server Essentials">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-50 rounded p-2">
                    <h5>SQL Server Essentials</h5>
                    <p>Master database design and queries with ease.</p>
                </div>
            </div>

            <div class="carousel-item">
                <img src="images/bootstrap1.jpg" class="d-block w-100 rounded" style="max-height: 400px; object-fit: contain;" alt="Bootstrap 5 Guide">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-50 rounded p-2">
                    <h5>Bootstrap 5 Guide</h5>
                    <p>Learn the secrets to designing delightful user experiences.</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="images/booksmart.jpg" class="d-block w-100 rounded" style="max-height: 400px; object-fit: contain;" alt="Bootstrap 5 Guide">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-50 rounded p-2">
                    <h5>BookSmart</h5>
                    <p>Developing Website using ASP.NET.</p>
                </div>
            </div>

        </div>

        <button class="carousel-control-prev" type="button" data-bs-target="#featuredBooksCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon"></span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#featuredBooksCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon"></span>
        </button>
    </div>


</asp:Content>
