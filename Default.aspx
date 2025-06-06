<%@ Page Title="" Language="C#" MasterPageFile="~/technova.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="technova.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <section class="hero-section">
        <h1>Welcome to TechNova</h1>
        <p>Your one-stop shop for the latest laptops, smartphones, and accessories.</p>
        <a class="btn-primary" href="Products.aspx">Shop Now</a>
    </section>

    <!-- ✅ Moved Up: About Us Section -->
    <section class="about-section">
    <div class="about-content">
        <h2>About Us</h2>
        <p>
            At TechNova, we’re passionate about bringing tomorrow’s tech to your hands today.
            Whether you're a gamer, a student, or a professional, our carefully curated selection of laptops,
            smartphones, and accessories is designed to meet your lifestyle and budget.
        </p>
        <p>
            Founded in 2025, TechNova is committed to quality, affordability, and exceptional customer service.
            We partner with top brands and provide a seamless online shopping experience to help you stay ahead
            in the fast-moving world of technology.
        </p>
    </div>
    </section>

    <section class="categories-section">
        <h2>Explore Categories</h2>
        <div class="category-grid">
            <div class="category-card">
                <img src="images/laptops/xps13.jpg" alt="Laptops" />
                <h3>Laptops</h3>
                <p>Performance, portability, and power. Browse our top picks.</p>
                <a href="Products.aspx">View Products</a>
            </div>
            <div class="category-card">
                <img src="images/smartphones/oneplus12r.jpg" alt="Smartphones" />
                <h3>Smartphones</h3>
                <p>Latest models with cutting-edge features. Stay connected in style.</p>
                <a href="Products.aspx">View Products</a>
            </div>
            <div class="category-card">
                <img src="images/accessories/airpods.jpg" alt="Accessories" />
                <h3>Accessories</h3>
                <p>Everything you need to enhance your tech lifestyle.</p>
                <a href="Products.aspx">View Products</a>
            </div>
        </div>
    </section>
</asp:Content>
