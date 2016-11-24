namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedClasses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ABV", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Reviews", "Aroma", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Aroma");
            DropColumn("dbo.Products", "ABV");
        }
    }
}
