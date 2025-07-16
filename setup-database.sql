-- PantryLink Database Setup Script for MariaDB
-- Run this script as root user

-- Create the database
CREATE DATABASE IF NOT EXISTS PantryLinkDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Create a dedicated user for the application
CREATE USER IF NOT EXISTS 'pantrylink'@'localhost' IDENTIFIED BY 'PantryLink2025!';

-- Grant privileges to the user
GRANT ALL PRIVILEGES ON PantryLinkDb.* TO 'pantrylink'@'localhost';

-- Flush privileges to ensure they take effect
FLUSH PRIVILEGES;

-- Display the created database and user
SHOW DATABASES;
SELECT User, Host FROM mysql.user WHERE User = 'pantrylink';

-- Test connection (you can run this separately)
-- USE PantryLinkDb;
-- SHOW TABLES;
