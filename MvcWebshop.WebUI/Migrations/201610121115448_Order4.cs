namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderLines", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "Order_OrderId" });
            DropColumn("dbo.OrderLines", "OrderId");
            RenameColumn(table: "dbo.OrderLines", name: "Order_OrderId", newName: "OrderId");
            AlterColumn("dbo.OrderLines", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderLines", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderLines", "OrderId");
            AddForeignKey("dbo.OrderLines", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "OrderId" });
            AlterColumn("dbo.OrderLines", "OrderId", c => c.Int());
            AlterColumn("dbo.OrderLines", "OrderId", c => c.String());
            RenameColumn(table: "dbo.OrderLines", name: "OrderId", newName: "Order_OrderId");
            AddColumn("dbo.OrderLines", "OrderId", c => c.String());
            CreateIndex("dbo.OrderLines", "Order_OrderId");
            AddForeignKey("dbo.OrderLines", "Order_OrderId", "dbo.Orders", "OrderId");
        }
    }
}
