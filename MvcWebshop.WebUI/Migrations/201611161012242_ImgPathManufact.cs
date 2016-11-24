namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImgPathManufact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturers", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturers", "ImgPath");
        }
    }
}
