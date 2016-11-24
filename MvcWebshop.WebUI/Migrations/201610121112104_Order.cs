namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        OrderLineId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderId = c.String(),
                        Order_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderLineId)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.Order_OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Shipped = c.Boolean(nullable: false),
                        UserInfoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.UserInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.OrderLines", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "UserInfoId" });
            DropIndex("dbo.OrderLines", new[] { "Order_OrderId" });
            DropIndex("dbo.OrderLines", new[] { "ProductId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderLines");
        }
    }
}
