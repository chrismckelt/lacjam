USE [master]


IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{DatabaseUsername}')
CREATE LOGIN [{DatabaseUsername}] WITH PASSWORD='{DatabasePassword}'


USE [Lacjam]


IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{DatabaseUsername}')
CREATE USER [{DatabaseUsername}] FOR LOGIN [{DatabaseUsername}]


EXEC sp_addrolemember 'db_owner', N'{DatabaseUsername}'