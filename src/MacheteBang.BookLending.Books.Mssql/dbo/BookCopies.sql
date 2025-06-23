Create Table dbo.BookCopies
(
   BookCopyId UniqueIdentifier Not Null Constraint DF_BookCopies_BookCopyId Default NewSequentialId(),
   BookId UniqueIdentifier Not Null,
   Condition VarChar(16) Not Null,
   Constraint PK_BookCopies Primary Key (BookCopyId),
   Constraint FK_BookCopies_Books Foreign Key (BookId) References dbo.Books(BookId) On Delete Cascade
)
