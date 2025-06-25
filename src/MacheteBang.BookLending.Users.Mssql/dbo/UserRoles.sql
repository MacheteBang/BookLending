Create Table dbo.UserRoles
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_UserRoles_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_UserRoles_CreatedBy Default SUser_SName(),
    UserId UniqueIdentifier Not Null,
    RoleId UniqueIdentifier Not Null,

    Constraint PK_UserRoles Primary Key (UserId, RoleId),
    Constraint FK_UserRoles_Roles_RoleId Foreign Key (RoleId) References dbo.Roles(RoleId) On Delete Cascade,
    Constraint FK_UserRoles_Users_UserId Foreign Key (UserId) References dbo.Users(UserId) On Delete Cascade
)

Go

Create Index IX_UserRoles_RoleId On dbo.UserRoles(RoleId);
