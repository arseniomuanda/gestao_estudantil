CREATE TABLE AuditLog (
    Id INT PRIMARY KEY IDENTITY,
    Action NVARCHAR(50),
    TableName NVARCHAR(50),
    Date DATETIME,
    UserId NVARCHAR(128) -- Ou o tipo que corresponda ao seu UserId
);
