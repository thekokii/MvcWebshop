namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeerReviews", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BeerReviews", "DateTime");
        }
    }
}
