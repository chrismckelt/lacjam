using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(9, TransactionBehavior.None)]
    public class CreateEntityValueProjection : Migration
    {
        public override void Up()
        {
            Create.Table("EntityValueProjection")
                 .WithColumn("Identity")
                 .AsGuid()
                 .PrimaryKey()
                 .WithColumn("EntityIdentity")
                 .AsGuid()
                 .NotNullable()
                 .WithColumn("Name")
                 .AsString(200)
                 .NotNullable()
                 .WithColumn("DataType")
                 .AsString(200)
                 .NotNullable()
                 .WithColumn("Regex")
                 .AsString(200)
                 .Nullable()
                 .WithColumn("Value")
                 .AsCustom("NVARCHAR(MAX)")
                 .NotNullable()
                 .WithColumn("CreatedUtcDate")
                 .AsDateTime()
                 .WithDefaultValue(DateTime.UtcNow)
                 .WithColumn("LastModifiedUtcDate")
                 .AsDateTime()
                 .Nullable()
                 .WithDefaultValue(DateTime.UtcNow);
        }

        public override void Down()
        {

        }
    }
}