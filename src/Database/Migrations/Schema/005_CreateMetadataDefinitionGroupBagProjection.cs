using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(5, TransactionBehavior.None)]
    public class CreateMetadataDefinitionGroupBgProjection : Migration
    {
        public override void Up()
        {
            Create.Table("MetadataDefinitionGroupBagProjection")
                 .WithColumn("Identity").AsGuid().PrimaryKey()
                 .WithColumn("AggregateIdentity")
                 .AsGuid()
                 .NotNullable()
                 .WithColumn("DefinitionId")
                 .AsGuid()
                 .NotNullable()
                 //.WithColumn("IsActive")
                 //.AsByte()
                 //.WithDefaultValue(1)
                 //.WithColumn("IsDeleted")
                 //.AsByte()
                 //.WithDefaultValue(0)
                 .WithColumn("CreatedUtcDate")
                 .AsDateTime()
                 .WithDefaultValue(DateTime.UtcNow)
                 .WithColumn("LastModifiedUtcDate")
                 .AsDateTime()
                 .Nullable()
                 .WithDefaultValue(DateTime.UtcNow)
                 ;

            Create.UniqueConstraint("UQ_MetadataDefinitionGroupBagProjection")
                  .OnTable("MetadataDefinitionGroupBagProjection")
                  .Columns(new[] { "AggregateIdentity", "DefinitionId" });

        }

        public override void Down()
        {

        }
    }
}