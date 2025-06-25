Create Table dbo.UserClaims
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_UserClaims_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_UserClaims_CreatedBy Default SUser_SName(),
    UserClaimId UniqueIdentifier Not Null Constraint DF_UserClaims_UserClaimId Default NewSequentialId(),
    UserId UniqueIdentifier Not Null,
    ClaimType NVarChar(max) Null,
    ClaimValue NVarChar(max) Null,

    Constraint PK_UserClaims Primary Key (UserClaimId),
    Constraint FK_UserClaims_Users_UserId Foreign Key (UserId) References dbo.Users(UserId) On Delete Cascade
)

Go

Create Index IX_UserClaims_UserId On dbo.UserClaims(UserId);
