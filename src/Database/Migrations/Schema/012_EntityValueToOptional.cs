using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(12, TransactionBehavior.None)]
    public class EntityValueToOptional : Migration
    {
        public override void Up()
        {
            Alter.Table("EntityValueProjection")
                .AlterColumn("Value")
                .AsCustom("NVARCHAR(MAX)")
                .Nullable();
        }

        public override void Down()
        {

        }
    }
}