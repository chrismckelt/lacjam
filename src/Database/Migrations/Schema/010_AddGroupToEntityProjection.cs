using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(10, TransactionBehavior.None)]
    public class AddGroupToEntityProjection : Migration
    {
        public override void Up()
        {
            Alter.Table("EntityProjection")
                 .AddColumn("MetadataDefinitionGroupIdentity")
                 .AsGuid()
                 .NotNullable();

        }

        public override void Down()
        {

        }
    }
}