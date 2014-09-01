USE [Lacjam]
GO
    if exists (select * from dbo.sysobjects where id = object_id(N'Event') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Event

    if exists (select * from dbo.sysobjects where id = object_id(N'[Setting]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Setting]

    create table [Setting] (
        Id UNIQUEIDENTIFIER not null,
       primary key (Id)
    )
