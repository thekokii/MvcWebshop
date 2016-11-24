namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserInfoId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserInfoId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserInfoId)
                .Index(t => t.UserInfoId);
            
            AddColumn("dbo.BeerReviews", "UserInfoId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BeerReviews", "UserInfoId");
            AddForeignKey("dbo.BeerReviews", "UserInfoId", "dbo.UserInfoes", "UserInfoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BeerReviews", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoes", "UserInfoId", "dbo.AspNetUsers");
            DropIndex("dbo.UserInfoes", new[] { "UserInfoId" });
            DropIndex("dbo.BeerReviews", new[] { "UserInfoId" });
            DropColumn("dbo.BeerReviews", "UserInfoId");
            DropTable("dbo.UserInfoes");
        }
    }
}
