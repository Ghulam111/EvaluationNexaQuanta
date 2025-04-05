# .NET Core MVC + React Application Setup

This repository contains a .NET Core MVC application integrated with React. The application is used to manage and search products. The frontend is built with React, and the backend is powered by .NET Core MVC.

## Setup Instructions

Follow the steps below to set up the project:


### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone <repository-url>
cd <repository-folder>

```
### 2.  Install .NET Dependencies

Make sure you have .NET Core SDK installed. Restore the .NET Core dependencies by running the following command:

```bash
dotnet restore
```

### 3. Install Node.js Dependencies

Navigate to the ClientApp directory where the React app resides

```bash
cd ClientApp
npm install
```

### 4. Database setup

Make sure you have SQL Server Instance up and running use the provided SQL script file to create necessary tables and procedures.
The project uses Dapper to interact with the database

### 5. Build and Run the Application

Once all the dependencies are installed, you can build and run the application using the following command:

```bash
cd ..
dotnet build
dotnet run
```


### Application features :

Search Bar: A search bar to filter products by name, implemented using React and Fetch API.

Product Management: Allows CRUD operations for products.

Product Listing: Products are fetched from the backend and displayed in a table.

