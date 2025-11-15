# Mini Library Management System – API Documentation

Version: v1  
Base route pattern: `api/v{version:apiVersion}/{controller}`  
Example base URL (local): `https://localhost:5001/api/v1/`

## Notes
- Controllers are versioned; examples below use `v1`.
- Authentication: All endpoints require a JWT bearer token in the `Authorization` header except the `Login` endpoint which is marked `[AllowAnonymous]`.
- Responses use a common envelope `ReturnRecord` with properties: `data`, `message`, `status` (enum shown as string, e.g. `"Success"` or `"Failure"`).

## Authentication
**Obtain token:**
- **POST** `api/v1/Login`
- **Body:** 
  ```
  {
    "Username": "admin",
    "Password": "123"
  }
  ```
- **Success response** `200 OK`:
  ```
  {
    "data": { "token": "<JWT token>" },
    "message": "Token Generated Successfully",
    "status": "Success"
  }
  ```
- **Failure response** `401 Unauthorized`:
  ```
  {
    "data": null,
    "message": "Invalid credentials",
    "status": "Failure"
  }
  ```

**Use the token:**
- Header: `Authorization: Bearer <token>`
- Content-Type: `application/json` for request bodies

## Common Response Examples

**Success:**
```
{
  "data": { /* resource data */ },
  "message": "Operation completed successfully",
  "status": "Success"
}
```

**Failure:**
```
{
  "data": null,
  "message": "Error description",
  "status": "Failure"
}
```

---

# Endpoints
---
## LoginController
Base: `api/v1/Login`
- **POST** `/api/v1/Login`
- **Auth:** None
- **Request (JSON):**
  ```
  {
    "Username": "admin",
    "Password": "123"
  }
  ```
- **Success:** `200 OK` with `data.token` containing a JWT.
- **Failure:** `401 Unauthorized` (invalid credentials).
---
---

## BookManagementController
Base: `api/v1/BookManagement`

#### GET All (no paging)
- **GET** `/api/v1/BookManagement/All`
- **Auth:** Bearer
- **Response `data`:** List of books

#### GET with paging + filters
- **GET** `/api/v1/BookManagement`
- **Auth:** Bearer
- **Response `data`:** List of books
- **Query parameters:**
  - `pageNumber` (int, default 1)
  - `size` (int, default 5)
  - `title` (string, optional)
  - `category` (string, optional)
  - `isbn` (string, optional)
- **Example:**
  ```
  GET /api/v1/BookManagement?pageNumber=1&size=10
  GET /api/v1/BookManagement?pageNumber=1&size=10&title=Prog
  GET /api/v1/BookManagement?pageNumber=1&size=10&category=Prog
  GET /api/v1/BookManagement?pageNumber=1&size=10&isbn=978
  ```

#### GET by id
- **GET** `/api/v1/BookManagement/{id}`
- **Auth:** Bearer

#### Create book
- **POST** `/api/v1/BookManagement`
- **Auth:** Bearer
- **Body:**
  ```
  {
    "Title": "The Pragmatic Programmer",
    "Author": "Andrew Hunt",
    "Isbn": "978-0201616224",
    "Category": "Programming",
    "CopiesAvailable": 5,
    "PublishedYear": 1999
  }
  ```
- **Success:** `200 OK` with `data` = new `BookId`.

#### Update book
- **PUT** `/api/v1/BookManagement/{id}`
- **Auth:** Bearer
- **Body:** Fields similar to create (all fields of Book model).
  ```
  {
    "BookId": "1"
    "Title": "Updated Title",
    "Author": "Updated Author",
    "Isbn": "978-0201616224",
    "Category": "Programming",
    "CopiesAvailable": 3,
    "PublishedYear": 1999
  }
  ```
- **Success:** `200 OK` with `data` = updated `BookId`.

#### Delete book
- **DELETE** `/api/v1/BookManagement/{id}`
- **Auth:** Bearer
- **Success:** `200 OK` - Soft delete (sets delete flag)

**Notes:**
- Validation errors return `400 Bad Request` with `status: "Failure"`.
- Deleting sets soft-delete flag.

---
---

## MemberManagementController
Base: `api/v1/MemberManagement`

#### GET All (no paging)
- **GET** `/api/v1/MemberManagement/All`
- **Auth:** Bearer

#### GET with paging + filters
- **GET** `/api/v1/MemberManagement`
- **Auth:** Bearer
- **Query parameters:**
  - `pageNumber` (int, default 1)
  - `size` (int, default 5)
  - `fullname` (string, optional)
  - `email` (string, optional)
  - `phone` (string, optional)
  - **Example:**
  ```
  GET /api/v1/MemberManagement?pageNumber=1&size=10
  GET /api/v1/MemberManagement?pageNumber=1&size=10&fullname=Mon
  GET /api/v1/MemberManagement?pageNumber=1&size=10&email=Prog
  GET /api/v1/MemberManagement?pageNumber=1&size=10&phone=013
  ```

#### GET by id
- **GET** `/api/v1/MemberManagement/{id}`
- **Auth:** Bearer

#### Create member
- **POST** `/api/v1/MemberManagement`
- **Auth:** Bearer
- **Body:**
  ```
  {
    "FullName": "Alice Johnson",
    "Email": "alice.johnson@example.com",
    "Phone": "01710000001",
    "JoinDate": "2023-01-10T00:00:00",
    "IsActive": 1
  }
  ```

#### Update member
- **PUT** `/api/v1/MemberManagement/{id}`
- **Auth:** Bearer
- **Body:**
  ```
  {
    "MemberId" : "1"
    "FullName": "Alice Johnson",
    "Email": "alice.johnson@example.com",
    "Phone": "01710000001",
    "JoinDate": "2023-01-10T00:00:00",
    "IsActive": 1
  }
  ```

#### Delete member
- **DELETE** `/api/v1/MemberManagement/{id}`
- **Auth:** Bearer

**Notes:**
- Inactive or soft-deleted members return `Failure` when fetching by id.

---
---
## BorrowingBookController (Create book borrow record)
Base: `api/v1/BorrowingBook`

#### Create borrow
- **POST** `/api/v1/BorrowingBook`
- **Auth:** Bearer
- **Body:** `BorrowDetailsCreateRecord`
  ```
  {
    "MemberId": 3,
    "BorrowDate": "2024-03-12T00:00:00",
    "DueDate": "2024-03-22T00:00:00",
    "BorrowBookList": [
      { "BookId": 6 },
      { "BookId": 7 }
    ]
  }
  ```
- **Success:** `200 OK` with `data` = created `BorrowId`.
- **Failure reasons:**
  - Member not found or inactive
  - Member exceeded borrowing limit (max 5 active borrows)
  - Member already borrowed the same book
  - Book stock update failure

**Notes:**
- Service updates book stock when borrowing.
- Transactions are used; failures roll back.

---
---
## ReturningBookController
Base: `api/v1/ReturningBook`

#### Return borrowed books
- **POST** `/api/v1/ReturningBook`
- **Auth:** Bearer
- **Body:** `ReturnBorrowedBookRecord`
  ```
  {
    "BorrowId": 3,
    "ReturnDate": "2024-03-20T00:00:00"
  }
  ```
- **Success:** `200 OK` with `data`:
  ```
  {
    "overDue": 0,
    "penaltyAmount": 0
  }
  ```
  
**Notes:**
- Each book in the borrow list will have stock incremented.
- Overdue days and penalty calculated using `DueAmountSetting:PerDayChargeAmount` from configuration.
- If book is returned after due date, `overDue` shows number of days late and `penaltyAmount` shows the calculated fine.

---
## ReportingController
Base: `api/v1/Reporting`

#### Generate report (borrow/return stats)
- **GET** `/api/v1/Reporting?fromDate={isoDate}&toDate={isoDate}`
- **Auth:** Bearer
- **Query params:**
  - `fromDate` (ISO date, required)
  - `toDate` (ISO date, required)
- **Example:**
  ```
  GET /api/v1/Reporting?fromDate=2024-01-01T00:00:00&toDate=2024-12-31T23:59:59
  ```
- **Success `data`:**
  ```
  {
    "Total_Books_Borrowed": 42,
    "Total_Books_Returned": 30,
    "Active_Borrow_Records": 12,
    "Most_Borrowed_Book": {
      "Title": "Clean Code",
      "Category": "Programming",
      "Author": "Robert C. Martin",
      "BorrowCount": 8
    }
  }
  ```

**Note:** `Active_Borrow_Records` represents the number of borrow transactions that have not yet been returned.

#### Get due notification logs
- **GET** `/api/v1/Reporting/DueNotificationLog`
- **Auth:** Bearer
- **Success `data`:**
  ```
  {
    "TotalLogs": 1,
    "Logs": [
      {
        "email": "bob@example.com",
        "message": "Your book is overdue by 3 days. Penalty amount around 30"
      }
    ]
  }
  ```
  
**Notes:**
- These logs come from `DueAlertEmailLog` entries stored when the scheduler runs or when the email-logging service is invoked.

---

## Scheduler / Email Sending
- A scheduled job `DailyDueEmailScheduleJob` is registered in `Program.cs` (via Coravel) and configured using `SchedulerConfig:DailyAtHour` and `SchedulerConfig:DailySecond` in `appsettings.json`.
- Email sending is optional: check `EmailSettings:IsActive` in configuration; if enabled, the service will attempt to send emails using SMTP settings in `appsettings.json`.

---

## Errors & Status Codes
- **200 OK** – Success (envelope status `"Success"`)
- **400 Bad Request** – Validation error or unhandled exception (envelope status `"Failure"`)
- **401 Unauthorized** – Invalid token or login failure
- **500 Internal Server Error** – Unexpected server issues

---

## Appendix – Data Models (High-level)

### Book
```
{
  "BookId": 1,
  "Title": "Clean Code",
  "Author": "Robert C. Martin",
  "Isbn": "978-0132350884",
  "Category": "Programming",
  "CopiesAvailable": 5,
  "PublishedYear": 2008,
  "Status": 1
}
```

### Member
```
{
  "MemberId": 1,
  "FullName": "John Doe",
  "Email": "john.doe@example.com",
  "Phone": "01710000001",
  "JoinDate": "2023-01-10T00:00:00",
  "IsActive": 1
}
```

### Borrow Details
```
{
  "BorrowId": 1,
  "MemberId": 3,
  "BorrowDate": "2024-03-12T00:00:00",
  "DueDate": "2024-03-22T00:00:00",
  "ReturnDate": null,
  "BorrowBookList": [
    { "BookId": 6 },
    { "BookId": 7 }
  ]
}
```

### ReturnRecord (Response Envelope)
```
{
  "data": { /* resource data or null */ },
  "message": "Descriptive message",
  "status": "Success" // or "Failure"
}
```
