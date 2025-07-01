# Book Lending API Endpoints

This document provides a reference for all endpoints in the Book Lending API, including their domain, summary, path, and authorization level.

## API Endpoints

| Domain | Endpoint Summary         | Endpoint Path                      | Authorization Level |
| ------ | ------------------------ | ---------------------------------- | ------------------- |
| Users  | Register User            | POST /users/register               | Anonymous           |
| Users  | Login User               | POST /users/login                  | Anonymous           |
| Users  | Promote User             | PATCH /users/{userId}/promote      | Administrator       |
| Users  | Demote User              | PATCH /users/{userId}/demote       | Administrator       |
| Books  | Get All Books            | GET /books                         | Member              |
| Books  | Get a Book by ID         | GET /books/{id}                    | Member              |
| Books  | Add a New Book           | POST /books                        | Administrator       |
| Books  | Remove a Book by ID      | DELETE /books/{id}                 | Administrator       |
| Books  | Get All Book Copies      | GET /books/{id}/copies             | Member              |
| Books  | Get a Book Copy by ID    | GET /books/{id}/copies/{copyId}    | Member              |
| Books  | Add a New Book Copy      | POST /books/{id}/copies            | Administrator       |
| Books  | Remove a Book Copy by ID | DELETE /books/{id}/copies/{copyId} | Administrator       |

## Authorization Levels

- **Anonymous**: No authentication required
- **Member**: Requires user to be authenticated with the Member role
- **Administrator**: Requires user to be authenticated with the Administrator role

## Notes

- All authenticated endpoints require a valid JWT token
- Administrator users also have all Member permissions
- All endpoints return appropriate HTTP status codes and problem details when errors occur
