using FluentMigrator;

namespace DataModel.Migrations
{
    [Migration(20180717165837)]
    public class Create_Users_Tables : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("User_Id").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("User_Name").AsString().NotNullable()
                .WithColumn("User_Surname").AsString().NotNullable()
                .WithColumn("User_ApiKey").AsString().NotNullable()
                .WithColumn("User_Email").AsString().NotNullable()
                .WithColumn("User_Password").AsString().NotNullable()
                .WithColumn("User_Active").AsBoolean().NotNullable()
                .WithColumn("Current_User").AsBoolean().NotNullable();

            Create.Table("Movies")
                .WithColumn("UId").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("Movie_Id").AsInt64().NotNullable()
                .WithColumn("Vote_Count").AsInt64().NotNullable()
                .WithColumn("Video").AsBoolean().NotNullable()
                .WithColumn("Vote_Average").AsDouble().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Popularity").AsDouble().NotNullable()
                .WithColumn("Poster_Path").AsString().NotNullable()
                .WithColumn("Original_Language").AsString().NotNullable()
                .WithColumn("Original_Title").AsString().NotNullable()
                .WithColumn("Backdrop_Path").AsString().NotNullable()
                .WithColumn("Adult").AsBoolean().NotNullable()
                .WithColumn("Overview").AsString().NotNullable()
                .WithColumn("Release_Date").AsString().NotNullable();
            
            Create.Table("User_Movies")
                .WithColumn("User_Id").AsInt64().PrimaryKey()
                .WithColumn("UId").AsInt64().PrimaryKey()
                .WithColumn("Favourite").AsBoolean()
                .WithColumn("Watchlist").AsBoolean();
        }

        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("Movies");
            Delete.Table("User_Movies");
        }
    }
}
