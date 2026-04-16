# 🏦 MellonBank - E-Banking System

Το **MellonBank** είναι μια ολοκληρωμένη web εφαρμογή ηλεκτρονικής τραπεζικής (E-Banking), αναπτυγμένη με **ASP.NET Core MVC**. Υποστηρίζει ρόλους χρηστών (Πελάτες & Προσωπικό), διαχείριση λογαριασμών, μεταφορές χρημάτων σε πραγματικό χρόνο και ενσωμάτωση εξωτερικών API για ισοτιμίες συναλλάγματος.

---

## 🚀 Γρήγορη Εκκίνηση (Docker Setup)

Η εφαρμογή είναι πλήρως dockerized, επιτρέποντας εύκολη εκτέλεση χωρίς τοπική εγκατάσταση SQL Server ή .NET SDK.

<i>Σημείωση: Επειδή την εφαρμογή την ανέπτυξα στο λάπτοπ μου που είναι Linux, εάν υπάρξει κάπου κάποιο θέμα, τρέξτε το σε κάποιο wsl.
Ωστόσο για να αποφύγω τέτοια προβλήματα την έχω κάνει dockerized.</i>

### 1. Κλωνοποίηση Repository


```
git clone https://github.com/kvelee/mellonbank.git

cd mellonbank
```

### 2. (Προαιρετικό, μόνο εάν δεν το έχετε) Pull SQL Server Image

```
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Εκτέλεση Εφαρμογής

1. 
```
docker compose up --build db -d
```

2. 
```
docker compose up --build web
```
### 4. Σύνδεση
Συνδέεστε στην εφαρμογή από το
```
localhost:8080
```

📌 Με την εκκίνηση:
- Εκτελούνται αυτόματα τα **Database Migrations**
- Τρέχει ο **DbSeeder**
- Δημιουργούνται roles και αρχικά δεδομένα

---

## 📋 Troubleshooting

### ❗ Database Connection Issue

Αν αποτύχει η πρώτη εκκίνηση:

- Βεβαιωθείτε ότι το container `db` έχει ξεκινήσει πλήρως  
- Αν χρειάζεται ξανακάντε build

---

### 🔑 API Keys

Για τις ισοτιμίες USD:

- Χρησιμοποιείται το Fixer.io API  
- (Προεραιτικό) Ρύθμιστε το API Key στο: appsettings.json

---

## 👨‍💻 Ανάπτυξη

**Κωνσταντίνος Βελεγράκης**  
📧 belekostac@gmail.com
