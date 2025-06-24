# 📖 API Usage Examples

This document provides examples of how to use the MacheteBang.BookLending API endpoints.

## Book Management

### Adding a Book

```http
POST /books
Content-Type: application/json

{
  "title": "1984",
  "author": "George Orwell"
}
```

### Retrieving All Books

```http
GET /books
```

### Getting a Specific Book

```http
GET /books/{id}
```

### Removing a Book

```http
DELETE /books/{id}
```

## Book Copy Management

### Adding a Book Copy

```http
POST /books/{bookId}/copies
Content-Type: application/json

{
  "condition": "Good"
}
```

### Retrieving All Copies of a Book

```http
GET /books/{bookId}/copies
```

### Getting a Specific Book Copy

```http
GET /books/{bookId}/copies/{copyId}
```

### Removing a Book Copy

```http
DELETE /books/{bookId}/copies/{copyId}
```
