using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(7, TransactionBehavior.None)]
    public class CreateMetadataDefinitionValueProjection : Migration
    {
        public override void Up()
        {
            Create.Table("MetadataDefinitionValueProjection")
            .WithColumn("Identity")
            .AsGuid()
            .PrimaryKey()
            .WithColumn("DefinitionId")
            .AsGuid()
            .NotNullable()
            .WithColumn("Value")
            .AsCustom("NVARCHAR(MAX)")
            .NotNullable()
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
            .WithDefaultValue(DateTime.UtcNow);

        }

        public override void Down()
        {

        }
    }
}