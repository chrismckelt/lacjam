using FluentMigrator;

namespace Database.Migrations
{
    [Migration(3)]
    public class CreateEventExceptionsTable : Migration
    {
        public override void Up()
        {
            Create.Table("EventExceptions")
                .WithColumn("Id")
                .AsGuid()
                .PrimaryKey()
                .NotNullable()
                .WithColumn("EventId")
                .AsGuid()
                .NotNullable()
                .WithColumn("Seq")
                .AsInt64()
                .NotNullable()
                .WithColumn("ExceptionUtcDateTime")
                .AsDateTime()
                .NotNullable()
                .WithColumn("ExceptionType")
                .AsString(200)
                .WithColumn("Exception")
                .AsCustom("NVARCHAR(MAX)")
                .WithColumn("Payload")
                .AsCustom("NVARCHAR(MAX)")
                .NotNullable();
        }

        public override void Down()
        {
            Delete.Table("EventExceptions");
        }
    }
}