Create Table dbo.Books
(
   BookId UniqueIdentifier Not Null,
   Isbn VarChar(13) Not Null,
   Title NVarChar(max) Not Null,
   Author NVarChar(max) Not Null,
   Constraint PK_Books Primary Key (BookId),
   Constraint UQ_Books_Isbn Unique (Isbn)
)
