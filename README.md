# Inventory - Order Management API

A backend-focused ASP.NET Core Web API project that simulating a simple order management system.
Designed to practice API design with authentication, logging, error handling, and relational data.

## Features
### Authentication & Authorization
  - JWT token-based authentication
  - Role-based authorization (e.g., Admin vs. User)
  - Secure password hashing
  - Email Validation

### Product Management
  - Create, update, delete products
  - Validate stock before fulfilling orders

 ### Order Management
  - Create orders containing existing products
  - Stock deduction when order is placed
  - Prevent order submission in case of insufficient stock

### Audit Logging  
  - Tracks all key user actions (e.g., product changes, order creation,..)
  - Stores who did what and when

### Error Handling/ Validation/ Logging
  - Prevent any app crash by properly handling any exceptions
  - Logging any unsuccessful attempts/ encountered errors
  - Model validation using FluentValidation

### Data seeding
  - Initial testing data are stored in development environments

## Data Model Overview

| Entity	| Relationships |
|---------|---------|
| User |	Has many Orders, has many AuditLogs, has many EmailVerificationTokens |
| EmailVerificationToken | Belongs to single User, records created and expiration date |
| Product |	Appears in many OrderItems, tracks stock quantity and price |
| Order |	Created by one User, contains many OrderItems, tracks created date, order status and price |
| OrderItem |	Links one Order and one Product, tracks ordered quantity and unit price |
| AuditLog |	Belongs to one User, records action & timestamp |

## Tech Stack
  - BE: ASP.NET Core Web API
  - Auth: JWT Bearer Tokens
  - DB: Entity Framework Core + SQL Server
  - Testing: xUnit + Moq
  - Validation: FluentValidation
  - Logging: Serilog

## Tools used
  - Postman - for endpoints tests
  - PaperCut SMTP - for email tests

## Areas for Improvement:
  - Add functionality to re-send verification emails
  - Add background tasks to clear non-verified user and expired email validation tokens
  - Implement product search & filtering
  - Build small React frontend for visualization
  - and many more
