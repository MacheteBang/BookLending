Create Table dbo.UserTokens
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_UserTokens_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_UserTokens_CreatedBy Default SUser_SName(),
    UserId UniqueIdentifier Not Null,
    LoginProvider NVarChar(450) Not Null,
    Name NVarChar(450) Not Null,
    Value NVarChar(max) Null,

    Constraint PK_UserTokens Primary Key (UserId, LoginProvider, Name),
    Constraint FK_UserTokens_Users_UserId Foreign Key (UserId) References dbo.Users(UserId) On Delete Cascade
)
