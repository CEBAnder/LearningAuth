using FluentMigrator;

namespace LearningAuth.Data.Migrations;

[Migration(2, "Add Roles column to User table")]
public class Migration_2_AddRolesToUserTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Column("Roles")
            .OnTable("User")
            .AsString()
            .NotNullable();
    }
}