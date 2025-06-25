Create Table dbo.Users
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_Users_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_Users_CreatedBy Default SUser_SName(),
    UserId UniqueIdentifier Not Null Constraint DF_Users_UserId Default NewSequentialId(),
    UserName NVarChar(256) Null,
    NormalizedUserName NVarChar(256) Null,
    Email NVarChar(256) Null,
    NormalizedEmail NVarChar(256) Null,
    EmailConfirmed Bit Not Null,
    PasswordHash NVarChar(max) Null,
    SecurityStamp NVarChar(max) Null,
    ConcurrencyStamp NVarChar(max) Null,
    PhoneNumber NVarChar(max) Null,
    PhoneNumberConfirmed Bit Not Null,
    TwoFactorEnabled Bit Not Null,
    LockoutEnd DateTimeOffset Null,
    LockoutEnabled Bit Not Null,
    AccessFailedCount Int Not Null,

    Constraint PK_Users Primary Key NonClustered (UserId)
)

Go

Create Index EmailIndex On dbo.Users(NormalizedEmail);

Go

Create Unique Clustered Index UserNameIndex On dbo.Users(NormalizedUserName);
