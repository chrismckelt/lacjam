using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(6, TransactionBehavior.None)]
    public class CreateMetadataDefinitionProjection : Migration
    {
        public override void Up()
        {
            Create.Table("MetadataDefinitionProjection")
            .WithColumn("Identity")
            .AsGuid()
            .PrimaryKey()
            .WithColumn("Name")
            .AsString(200)
            .NotNullable()
            .WithColumn("Description")
            .AsCustom("NVARCHAR(MAX)")
            .Nullable()
            .WithColumn("DataType")
            .AsString(250)
            .Nullable()
            .WithDefaultValue("Text")
            .WithColumn("Regex")
            .AsString(3000)
            .Nullable()
            //.WithColumn("IsActive")
            //.AsByte()
            //.Nullable()
            //.WithDefaultValue(1)
            //.WithColumn("IsDeleted")
            //.AsByte()
            //.Nullable()
            //.WithDefaultValue(0)
            .WithColumn("CreatedUtcDate")
            .AsDateTime()
            .WithDefaultValue(DateTime.UtcNow)
            .WithColumn("LastModifiedUtcDate")
            .AsDateTime()
            .Nullable()
            .WithDefaultValue(DateTime.UtcNow)
            ;

        }

        public override void Down()
        {

        }
    }
}