# 📚 Book Lending API – Project Scope

## 🧾 Core Requirements

### Books

- Add/edit/delete books
- Track available copies
- Search by title, author, genre

### Users

- Register/login
- Roles: **Admin** (full access), **Member** (borrow/return only)

### Borrowing

- Borrow a book (if available)
- Return a book
- Track due dates, borrow history, and current status

---

## 🛠 Suggested Endpoints

- `GET /books`
- `POST /books`
- `PUT /books/{id}`
- `DELETE /books/{id}`

- `POST /users/register`
- `POST /users/login`

- `POST /borrow`
- `POST /return`

- `GET /users/{id}/history`

---

## 🧠 Bonus Ideas (Optional)

- Late return detection & fees
- Email notifications for due dates
- Waitlist for unavailable books
- Admin dashboard/reporting endpoints
- Rate or review books
