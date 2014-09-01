using FluentMigrator;

namespace Lacjam.Database.Migrations.Schema
{

    [Migration(3, TransactionBehavior.None)]
    public class InitialSchema : Migration
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
                 .WithColumn("Version")
                 .AsInt32()
                 .NotNullable()
                 .WithColumn("EventData")
                 .AsCustom("NVARCHAR(MAX)")
                 .Nullable();

            Create.Table("EventAudit")
                 .WithColumn("Id")
                 .AsGuid()
                 .PrimaryKey()
                 .NotNullable()
                 .WithColumn("Seq")
                 .AsInt64()
                 .NotNullable()
                 .WithColumn("EventId")
                 .AsGuid()
                 .NotNullable()
                 .WithColumn("CreatedUtcDate")
                 .AsDateTime()
                 .NotNullable()
                 .WithDefault(SystemMethods.CurrentUTCDateTime)
                 .WithColumn("Result")
                 .AsAnsiString(50)
                 .NotNullable()
                 .WithColumn("Message")
                 .AsCustom("NVARCHAR(MAX)")
                 .Nullable()
                 .WithColumn("EventProcessedUtcDate")
                 .AsDateTime()
                 .NotNullable();

            Create.Table("EventHandlerError")
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
                  .WithColumn("Version")
                  .AsInt32()
                  .NotNullable()
                  .WithColumn("EventData")
                  .AsCustom("NVARCHAR(MAX)")
                  .Nullable();

            Create.Table("HandlerSequence")
                  .WithColumn("Dispatcher")
                  .AsString(150)
                  .PrimaryKey()
                  .NotNullable()
                  .WithColumn("Pointer")
                  .AsInt64()
                  .NotNullable();

        }

        public override void Down()
        {

        }
    }
}