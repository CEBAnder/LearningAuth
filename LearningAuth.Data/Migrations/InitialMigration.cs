using FluentMigrator;

namespace LearningAuth.Data.Migrations;

[Migration(1, "Add User table")]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("User")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("PasswordHash").AsString().NotNullable()
            .WithColumn("Name").AsString().Nullable()
            .WithColumn("DateOfBirth").AsDate().Nullable();
    }

    public override void Down()
    {
        Delete.Table("User");
    }
}