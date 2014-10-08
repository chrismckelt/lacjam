using System;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(8, TransactionBehavior.None)]
    public class CreateEntityProjection : Migration
{
    public override void Up()
    {
        Create.Table("EntityProjection")
             .WithColumn("Identity")
             .AsGuid()
             .PrimaryKey()
             .WithColumn("Name")
             .AsString(200)
             .NotNullable()
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