Create Table dbo.Roles
(
    CreatedOn DateTimeOffset(7) Not Null Constraint DF_Roles_CreatedOn Default SysUtcDateTime(),
    CreatedBy VarChar(128) Not Null Constraint DF_Roles_CreatedBy Default SUser_SName(),
    RoleId UniqueIdentifier Not Null Constraint DF_Roles_RoleId Default NewSequentialId(),
    Name NVarChar(256) Null,
    NormalizedName NVarChar(256) Null,
    ConcurrencyStamp NVarChar(max) Null,

    Constraint PK_Roles Primary Key (RoleId)
)

Go

Create Unique Index RoleNameIndex On dbo.Roles(NormalizedName) Where NormalizedName Is Not Null;
