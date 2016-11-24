namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Extend1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeerReviews", "Appearance", c => c.Int(nullable: false));
            AddColumn("dbo.BeerReviews", "Taste", c => c.Int(nullable: false));
            AddColumn("dbo.BeerReviews", "Palate", c => c.Int(nullable: false));
            AddColumn("dbo.BeerReviews", "Overall", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BeerReviews", "Overall");
            DropColumn("dbo.BeerReviews", "Palate");
            DropColumn("dbo.BeerReviews", "Taste");
            DropColumn("dbo.BeerReviews", "Appearance");
        }
    }
}
