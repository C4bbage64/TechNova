CREATE TABLE Users (
	UserID INT PRIMARY KEY IDENTITY,
	Username VARCHAR(50) UNIQUE NOT NULL,
	Email VARCHAR(100),
	PasswordHash VARCHAR(255) NOT NULL,
	IsAdmin BIT DEFAULT 0
);

CREATE TABLE Categories (
	CategoryID INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(100) NOT NULL,
);

-- Insert Tech Categotries
INSERT INTO Categories (CategoryName) VALUES
('Laptops'),
('Smartphones'),
('Accessories');

CREATE TABLE Products (
	ProductID INT PRIMARY KEY IDENTITY,
	ProductName VARCHAR(100) NOT NULL,
	Description VARCHAR(255),
	Price DECIMAL(10, 2) NOT NULL,
	ImagePath VARCHAR(255),
	CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
);

CREATE TABLE Orders (
	OrderID INT PRIMARY KEY IDENTITY,
	UserID INT FOREIGN KEY REFERENCES Users(UserID),
	OrderDate DATETIME DEFAULT GETDATE(),
);

CREATE TABLE OrderItems (
	OrderItemID INT PRIMARY KEY IDENTITY,
	OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
	ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
	Quantity INT NOT NULL,
	UnitPrice DECIMAL(10,2) NOT NULL
);

INSERT INTO Products (ProductName, Description, Price, ImagePath, CategoryID) VALUES
('Dell XPS 13 (9315)', '13.4” FHD, Intel i7-1250U, 16GB RAM, 512GB SSD', 5699.00, 'images/laptops/xps13.jpg', 1),
('Apple MacBook Air M2', '13.6” Retina, Apple M2 chip, 256GB SSD, 8GB RAM', 5299.00, 'images/laptops/macbookair.jpg', 1),
('HP Spectre x360 14', 'Convertible OLED touch, i7, 16GB, 1TB SSD', 6499.00, 'images/laptops/spectre.jpg', 1),
('ASUS ROG Zephyrus G14', 'Gaming laptop with Ryzen 9, RTX 4060', 6999.00, 'images/laptops/rog.jpg', 1),
('Lenovo IdeaPad Slim 3i', 'Affordable 15.6” laptop, Intel i5, 512GB SSD', 2999.00, 'images/laptops/ideapad.jpg', 1);

INSERT INTO Products (ProductName, Description, Price, ImagePath, CategoryID) VALUES
('iPhone 15 Pro', '6.1” Super Retina XDR, A17 Pro chip, 128GB', 5499.00, 'images/smartphones/iphone15pro.jpg', 2),
('Samsung Galaxy S24', '6.2” Dynamic AMOLED, Snapdragon 8 Gen 3', 5199.00, 'images/smartphones/s24.jpg', 2),
('Google Pixel 8', 'Pure Android, Tensor G3, 128GB, 6.2” OLED', 3999.00, 'images/smartphones/pixel8.jpg', 2),
('Xiaomi 13T Pro', '120Hz AMOLED, Dimensity 9200+, 12GB/512GB', 2499.00, 'images/smartphones/13tpro.jpg', 2),
('OnePlus 12R', 'Snapdragon 8 Gen 2, 120Hz AMOLED, 5000mAh', 2299.00, 'images/smartphones/oneplus12r.jpg', 2);

INSERT INTO Products (ProductName, Description, Price, ImagePath, CategoryID) VALUES
('Apple AirPods Pro 2', 'Wireless ANC earbuds with MagSafe case', 1099.00, 'images/accessories/airpods.jpg', 3),
('Samsung Galaxy Watch 6', 'Smartwatch with heart rate and sleep tracking', 1399.00, 'images/accessories/watch6.jpg', 3),
('Anker PowerCore 10000', 'Portable 10,000mAh power bank, fast charge', 199.00, 'images/accessories/anker.jpg', 3),
('Logitech MX Master 3S', 'Ergonomic wireless mouse with USB-C', 399.00, 'images/accessories/mxmaster.jpg', 3),
('Ugreen USB-C Hub 7-in-1', 'Hub with HDMI, USB 3.0, SD, 100W PD', 129.00, 'images/accessories/ugreenhub.jpg', 3);

SELECT * FROM Products;

SELECT * FROM Categories;

