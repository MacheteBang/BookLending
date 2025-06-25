Create Table dbo.RoleClaims
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_RoleClaims_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_RoleClaims_CreatedBy Default SUser_SName(),
    RoleClaimId UniqueIdentifier Not Null Constraint DF_RoleClaims_RoleClaimId Default NewSequentialId(),
    RoleId UniqueIdentifier Not Null,
    ClaimType NVarChar(max) Null,
    ClaimValue NVarChar(max) Null,

    Constraint PK_RoleClaims Primary Key (RoleClaimId),
    Constraint FK_RoleClaims_Roles_RoleId Foreign Key (RoleId) References dbo.Roles(RoleId) On Delete Cascade
)

Go

Create Index IX_RoleClaims_RoleId On dbo.RoleClaims(RoleId);
