# MVCTest Project

### 1. Clone the Repository

```sh
git clone https://github.com/matuksd/MVCTest.git
cd MVCTest
```

### 2. Change Connection String

Edit `appsettings.json`:

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MVCTestDb;Trusted_Connection=True;"
 }
```

### 3. Restore Dependencies

```sh
dotnet restore
```

### 4. Apply Database Migrations

```sh
dotnet ef database update
```

### 5. Run the Application

```sh
dotnet run
```

### 7. Running Unit Tests

```sh
dotnet test
```

### 8. API Access

```
GET /api/ProductAudit?from=YYYY-MM-DD&to=YYYY-MM-DD
```
Example:
```
 https://localhost:7253/api/ProductAudit?from=2025-01-01&to=2025-10-31
```

