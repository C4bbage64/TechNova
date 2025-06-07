# 🛒 TechNova – ASP.NET eCommerce Platform

**TechNova** is a full-featured eCommerce web application built using **ASP.NET Web Forms**, **C#**, and **SQL Server**. It provides a complete online shopping experience for customers and includes a secure admin dashboard for managing products, users, and orders.

---

## 🚀 Features

### 👤 For Customers
- **User Registration & Login** (with session management)
- **Product Browsing** by categories (Laptops, Smartphones, Accessories)
- **Shopping Cart** with quantity display and order checkout
- **Order History** view for logged-in users

### 🔧 For Admins
- **Product Management** (Create, Read, Update, Delete)
- **User Management** (Edit user info, view orders, promote to admin)
- **Order Management** (View & update order status)
- **Dashboard** with total revenue, user/product/order stats, recent orders
- **CSV Export** for order data

---

## 🧰 Tech Stack

| Layer       | Technology            |
|-------------|------------------------|
| Frontend    | ASP.NET Web Forms, HTML, CSS |
| Backend     | C# (.NET Framework)    |
| Database    | Microsoft SQL Server   |
| Data Access | ADO.NET + SQL Commands |
| Auth/State  | ASP.NET Session State  |

---

## 📂 Project Structure

```
/TechNova
│
├── /Admin # Admin pages (Dashboard, ManageOrders, ManageUsers, ManageProducts)
├── /Images # Product images
├── /Styles # CSS styling
├── Cart.aspx # Shopping cart
├── Products.aspx # Product listing
├── OrderHistory.aspx # User order history
├── Login.aspx # Login form
├── Register.aspx # User registration
├── Profile.aspx # Edit user profile
├── technova.Master # Master page layout
└── Web.config # Connection strings & settings
```

---

## 🛠 Setup Instructions

1. Clone this repo  
   ```bash
   git clone https://github.com/your-username/technova.git
Open the solution in Visual Studio

Configure your SQL Server database:

Create a database named TechNovaDB

Run the provided SQL script to create tables & stored procedures (if included)

Update your connection string in Web.config:

xml
Copy
Edit
<connectionStrings>
  <add name="TechNovaDB" connectionString="Data Source=.;Initial Catalog=TechNovaDB;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
Press F5 to run the app 🚀

✅ Completed Features Summary
 User registration/login (customers & admins)

 Product categories with images

 Shopping cart with order checkout

 Order history per user

 Admin dashboard & analytics

 Product/user/order management

 Session-based access control

📌 Future Enhancements
Add search & filter functionality for products

Integrate a payment gateway (real or simulated)

Enable responsive/mobile layout using Bootstrap

Add product reviews and rating system

Persistent cart storage in the database

📷 Screenshots
(Add screenshots here if desired)

📄 License
This project is for academic purposes. © 2025 TechNova Team.

yaml
Copy
Edit

---

Let me know if you’d like me to help you:
- Add a `.sql` setup section  
- Auto-generate a table of contents  
- Or prepare a PDF version of this README for submission!
