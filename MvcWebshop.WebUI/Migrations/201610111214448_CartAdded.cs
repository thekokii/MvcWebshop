namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        CartLineId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UserInfoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CartLineId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.ProductId)
                .Index(t => t.UserInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartLines", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.CartLines", "ProductId", "dbo.Products");
            DropIndex("dbo.CartLines", new[] { "UserInfoId" });
            DropIndex("dbo.CartLines", new[] { "ProductId" });
            DropTable("dbo.CartLines");
        }
    }
}
