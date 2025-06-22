# 📖 API Usage Examples

This document provides examples of how to use the MacheteBang.BookLending API endpoints.

## Adding a Book

```http
POST /books
Content-Type: application/json

{
  "title": "1984",
  "author": "George Orwell"
}
```

## Retrieving All Books

```http
GET /books
```

## Getting a Specific Book

```http
GET /books/{id}
```

## Removing a Book

```http
DELETE /books/{id}
```
