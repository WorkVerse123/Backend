# 💼 WorkVerse – Back-end (.NET)

Hệ thống **WorkVerse**, nền tảng tìm kiếm việc làm part-time cho học sinh, sinh viên. Ứng dụng hỗ trợ quản lý hồ sơ, công việc, ứng tuyển và gợi ý việc làm phù hợp bằng AI. Dự án được phát triển với kiến trúc **Clean Architecture** trên nền tảng **.NET**.

---

## 📁 Project Structure

```
/WorkVerse
│
├── WorkVerse.Application     # Application layer (services, use cases, DTOs)
├── WorkVerse.Domain          # Domain layer (entities, value objects, interfaces)
├── WorkVerse.Infrastructure  # Infrastructure (EF Core, database, external services)
├── WorkVerse.API             # Web API project (Controllers, Middleware, DI setup)
├── WorkVerse.sln             # Solution file
├── .gitignore
└── README.md
```

---

## 🚧 Branch Naming Convention

```
type/scope_author
```

* `feature`: Phát triển tính năng
* `bugfix`: Sửa lỗi
* `hotfix`: Sửa lỗi khẩn cấp
* `refactor`: Tối ưu code, không thay đổi logic
* `chore`: Công việc phụ trợ (cấu hình, script...)

📌 **scope**: Chức năng cụ thể (auth, job, profile, application...)
📌 **author**: Tên viết thường, không dấu (ví dụ: `trungnt`, `nhannb`)

🔹 **Ví dụ**:

```bash
feature/auth_trungnt
bugfix/job-application_trungnt 
```

---

## 📝 Commit Message Convention

```
<type>(<scope>): <short-description>
```

### ✅ type gồm:

* `Add`: Thêm mới tính năng
* `Update`: Cập nhật tính năng hoặc logic
* `Fix`: Sửa lỗi
* `Refactor`: Tối ưu code (không thay đổi logic)
* `Delete`: Xóa bỏ chức năng / đoạn code

🔹 **Ví dụ**:

```bash
Add(auth): implement jwt authentication
Fix(job): validate job posting deadline
Refactor(profile): clean up controller logic
```

---

## 🌐 API Naming Convention

* Dùng **plural nouns**: `/jobs`, `/applications`, `/profiles`
* Dùng **lowercase** và **dấu gạch ngang (-)** trong URL
* Tối đa **2 cấp nested resource**
* Hành động đặc biệt đặt rõ ở cuối endpoint

🔹 **Ví dụ**:

```http
GET    /jobs
POST   /jobs
GET    /jobs/{id}/applications
PATCH  /applications/{id}/approve
```

---

## 📦 API Response Format

* Trả về theo định dạng **PascalCase** (chuẩn C#)
* Phía client (React, Angular, ...) có thể convert sang `camelCase` nếu cần

🔹 **Mẫu Response**:

```json
{
  "StatusCode": 200,
  "Message": "Success",
  "Data": {
    "JobId": 123,
    "Title": "Part-time Cashier",
    "Applicants": [...]
  }
}
```

---

## 🛠️ Tech Stack

* **.NET 8**
* **Entity Framework Core**
* **MySQL**
* **JWT Authentication**
* **Swagger** (API documentation)
* **Clean Architecture** Pattern

---

## 👨‍💻 Development Guide

1. Clone project:

   ```bash
   git clone https://github.com/WorkVerse/BE.git
   ```

2. Cấu hình `appsettings.Development.json` tại dự án Web API:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=workverse;User=root;Password=yourpassword;"
     },
     "Jwt": {
       "Key": "your-secret-key",
       "Issuer": "workverse-app",
       "Audience": "workverse-app"
     }
   }
   ```

3. Run migration & database:

   ```bash
   dotnet ef database update --project WorkVerse.Infrastructure
   ```

4. Run project:

   ```bash
   dotnet run --project WorkVerse.API
   ```

5. Mở Swagger UI:

   ```
   http://localhost:8080/swagger
   ```

---

## 📬 Contact

Mọi thắc mắc vui lòng liên hệ team backend: **Thế Trung, Bá Nhân**

---
