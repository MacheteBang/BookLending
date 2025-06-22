Create Table dbo.Books
(
   Id UniqueIdentifier Not Null,
   Title NVarChar(max) Not Null,
   Author NVarChar(max) Not Null,
   Constraint PK_Books Primary Key (Id)
)
