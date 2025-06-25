Create Table dbo.UserLogins
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_UserLogins_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_UserLogins_CreatedBy Default SUser_SName(),
    LoginProvider NVarChar(450) Not Null,
    ProviderKey NVarChar(450) Not Null,
    ProviderDisplayName NVarChar(max) Null,
    UserId UniqueIdentifier Not Null,

    Constraint PK_UserLogins Primary Key (LoginProvider, ProviderKey),
    Constraint FK_UserLogins_Users_UserId Foreign Key (UserId) References dbo.Users(UserId) On Delete Cascade
)

Go

Create Index IX_UserLogins_UserId On dbo.UserLogins(UserId);
