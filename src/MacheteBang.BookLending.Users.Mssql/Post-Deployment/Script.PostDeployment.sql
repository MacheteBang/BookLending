-- PostDeployment-Roles.sql
-- Ensures Administrator and Member roles exist in dbo.Roles

-- Insert Administrator role if not exists
If Not Exists (Select 1 From dbo.Roles Where NormalizedName = N'ADMINISTRATOR')
Begin
    Insert Into dbo.Roles ( Name, NormalizedName, ConcurrencyStamp)
    Values (N'Administrator', N'ADMINISTRATOR', NewId())
End;

-- Insert Member role if not exists
If Not Exists (Select 1 From dbo.Roles Where NormalizedName = N'MEMBER')
Begin
    Insert Into dbo.Roles (Name, NormalizedName, ConcurrencyStamp)
    Values (N'Member', N'MEMBER', NewId())
End;
