﻿using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(5)]
    public class Create_Event_Table : Migration
    {
        public override void Up()
        {
            Create.Table("Event")
                .WithColumn("EventId")
                .AsGuid()
                .PrimaryKey()
                .NotNullable()
                .WithColumn("Seq")
                .AsInt64()
                .NotNullable()
                .Identity()
                .WithColumn("EventType")
                .AsString(200)
                .NotNullable()
                .WithColumn("AggregateId")
                .AsGuid()
                .WithColumn("CreatedUtcDate")
                .AsDateTime()
                .NotNullable()
                .WithColumn("Author")
                .AsString(200)
                .NotNullable()
                .WithColumn("SchemaVersion")
                .AsInt32()
                .NotNullable()
                .WithColumn("ObjectVersion")
                .AsInt32()
                .NotNullable()
                .WithColumn("Payload")
                .AsCustom("NVARCHAR(MAX)")
                .Nullable();
        }

        public override void Down()
        {
            Delete.Table("Event");
        }
    }
}