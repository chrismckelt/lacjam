using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(4, TransactionBehavior.None)]
    public class CreateMetadataDefinitionGroupProjection : Migration
{
    public override void Up()
    {
        Create.Table("MetadataDefinitionGroupProjection")
             .WithColumn("Identity")
             .AsGuid()
             .PrimaryKey()
             .WithColumn("Name")
             .AsString(200)
             .NotNullable()
             .WithColumn("Description")
             .AsCustom("NVARCHAR(MAX)")
             .Nullable()
             .WithColumn("IsActive")
             .AsByte()
             .WithDefaultValue(1)
             .WithColumn("IsDeleted")
             .AsByte()
             .WithDefaultValue(0)
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