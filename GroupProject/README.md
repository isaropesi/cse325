# 💰 Budget4U

Budget4U is a comprehensive personal finance tracking application built with **Blazor Server** and **.NET 10**. It empowers users to take control of their finances with absolute privacy by tracking income, expenses, and setting monthly budgets.

*Created by Isabella Silva for CSE 325.*

---

## ✨ Features

- **User Authentication:** Secure registration and login powered by ASP.NET Core Identity. Data is isolated per user.
- **Transactions Management:** Log, edit, and delete daily income and expenses.
- **Spending Categories:** Create custom categories with color-coded badges to organize your spending.
- **Monthly Budget Limits:** Set custom spending caps per category, per month.
- **Live Dashboard:** A real-time overview of your finances including total income, total expenses, net balance, and visual progress bars showing how close you are to your budget limits.
- **Transaction History Filter:** Filter your transaction history by specific months and years.

---

## 🛠️ Technology Stack

- **Framework:** .NET 10 / Blazor Server
- **Authentication:** ASP.NET Core Identity (Cookie-based)
- **Database:** Entity Framework Core with SQLite
- **Styling:** Custom Vanilla CSS (No external CSS frameworks required)

---

## 🚀 Running the Project Locally

To run the project on your local machine, ensure you have the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed.

### 1. Clone the Repository
```bash
git clone https://github.com/isaropesi/cse325.git
cd cse325/GroupProject
```

### 2. Build the Application
Restore dependencies and compile the project:
```bash
dotnet build
```

### 3. Run the Application
Start the development server. The database (`budget4u.db`) will be automatically created and migrated on startup if it doesn't exist.
```bash
dotnet run
```

### 4. Access the App
Open your browser and navigate to the URL provided in your terminal (typically `http://localhost:5000` or `https://localhost:5001`).

---

## 📸 Screenshots

*(Add screenshots of your running application here before W07 final submission!)*
- Landing Page
- Dashboard
- Transactions
- Categories
