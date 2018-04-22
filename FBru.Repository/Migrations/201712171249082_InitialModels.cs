namespace FBru.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisements",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Order = c.Int(nullable: false),
                    Title = c.String(maxLength: 255),
                    SubTitle = c.String(maxLength: 255),
                    Description = c.String(storeType: "ntext"),
                    ImageUrl = c.String(),
                    ActionUrl = c.String(),
                    IsDisplay = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Blogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false, maxLength: 255),
                    Description = c.String(nullable: false, storeType: "ntext"),
                    PublishedDate = c.DateTime(nullable: false, storeType: "date"),
                    Author = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Icon = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Dishes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Description = c.String(storeType: "ntext"),
                    Price = c.Double(nullable: false),
                    CategoryId = c.Int(nullable: false),
                    RestaurantId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.RestaurantId);

            CreateTable(
                "dbo.Images",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Url = c.String(),
                    IsDisplay = c.Boolean(nullable: false),
                    DishId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .Index(t => t.DishId);

            CreateTable(
                "dbo.Keywords",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Restaurants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ImageUrl = c.String(),
                    Name = c.String(nullable: false, maxLength: 255),
                    Address = c.String(nullable: false, maxLength: 255),
                    Description = c.String(storeType: "ntext"),
                    PhoneNumber = c.String(maxLength: 255, unicode: false),
                    OpenTime = c.Time(nullable: false, precision: 7),
                    CloseTime = c.Time(nullable: false, precision: 7),
                    IsHalal = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserGroups",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Email = c.String(nullable: false),
                    Password = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    PhoneNumber = c.String(maxLength: 255),
                    GroupId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);

            CreateTable(
                "dbo.DishKeyword",
                c => new
                {
                    DishId = c.Int(nullable: false),
                    KeywordId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.DishId, t.KeywordId })
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Keywords", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.DishId)
                .Index(t => t.KeywordId);

            Sql("INSERT INTO UserGroups (Name) VALUES ('Administrator')");
            Sql("INSERT INTO UserGroups (Name) VALUES ('Member')");
            Sql("INSERT INTO Users (Email, Password, Name, PhoneNumber, GroupId) VALUES (N'ducmeit2016@gmail.com', N'64028f69c28087158d6856f9abffeffa', N'Duc', N'123456789', 1)");

        }

        public override void Down()
        {
            DropForeignKey("dbo.Users", "GroupId", "dbo.UserGroups");
            DropForeignKey("dbo.Dishes", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Dishes", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.DishKeyword", "KeywordId", "dbo.Keywords");
            DropForeignKey("dbo.DishKeyword", "DishId", "dbo.Dishes");
            DropForeignKey("dbo.Images", "DishId", "dbo.Dishes");
            DropIndex("dbo.DishKeyword", new[] { "KeywordId" });
            DropIndex("dbo.DishKeyword", new[] { "DishId" });
            DropIndex("dbo.Users", new[] { "GroupId" });
            DropIndex("dbo.Images", new[] { "DishId" });
            DropIndex("dbo.Dishes", new[] { "RestaurantId" });
            DropIndex("dbo.Dishes", new[] { "CategoryId" });
            DropTable("dbo.DishKeyword");
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Keywords");
            DropTable("dbo.Images");
            DropTable("dbo.Dishes");
            DropTable("dbo.Categories");
            DropTable("dbo.Blogs");
            DropTable("dbo.Advertisements");
        }
    }
}
