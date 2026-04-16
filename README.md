# 🏦 MellonBank - E-Banking System

Το **MellonBank** είναι μια ολοκληρωμένη web εφαρμογή ηλεκτρονικής τραπεζικής (E-Banking), αναπτυγμένη με **ASP.NET Core MVC**. Υποστηρίζει ρόλους χρηστών (Πελάτες & Προσωπικό), διαχείριση λογαριασμών, μεταφορές χρημάτων σε πραγματικό χρόνο και ενσωμάτωση εξωτερικών API για ισοτιμίες συναλλάγματος.

---

## 🚀 Γρήγορη Εκκίνηση (Docker Setup)

Η εφαρμογή είναι πλήρως dockerized, επιτρέποντας εύκολη εκτέλεση χωρίς τοπική εγκατάσταση SQL Server ή .NET SDK.

### 1. Κλωνοποίηση Repository


git clone https://github.com/your-username/mellonbank.git
cd mellonbank

### 2. (Προαιρετικό) Pull SQL Server Image


docker pull mcr.microsoft.com/mssql/server:2022-latest

### 3. Εκτέλεση Εφαρμογής


docker compose up --build

📌 Με την εκκίνηση:
- Εκτελούνται αυτόματα τα **Database Migrations**
- Τρέχει ο **DbSeeder**
- Δημιουργούνται roles και αρχικά δεδομένα

---

## 🛠 Τεχνολογικό Stack

- **Backend:** ASP.NET Core 8 MVC (C#)  
- **Database:** Microsoft SQL Server  
- **ORM:** Entity Framework Core (Code-First)  
- **Authentication:** ASP.NET Core Identity  
- **Frontend:** Razor Views, Bootstrap, JavaScript, jQuery  
- **External API:** Fixer.io (USD exchange rates)

## 📋 Troubleshooting

### ❗ Database Connection Issue

Αν αποτύχει η πρώτη εκκίνηση:

- Βεβαιώσου ότι το container `db` έχει ξεκινήσει πλήρως  
- Αν χρειάζεται:


docker compose restart

---

### 🔑 API Keys

Για τις ισοτιμίες USD:

- Χρησιμοποιείται το Fixer.io API  
- Ρύθμισε το API Key στο:


appsettings.json

---

## 👨‍💻 Ανάπτυξη

**Κωνσταντίνος Βελεγράκης**  
📧 belekostac@gmail.com