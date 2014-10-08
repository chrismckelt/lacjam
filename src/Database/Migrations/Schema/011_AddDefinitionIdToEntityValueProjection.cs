using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(11, TransactionBehavior.None)]
    public class AddDefinitionIdToEntityValueProjection : Migration
    {
        public override void Up()
        {
            Alter.Table("EntityValueProjection")
                 .AddColumn("MetadataDefinitionIdentity")
                 .AsGuid()
                 .NotNullable();

        }

        public override void Down()
        {

        }
    }
}