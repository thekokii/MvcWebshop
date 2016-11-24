namespace MvcWebshop.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedClassesAsTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Reviews", newName: "BeerReviews");
            AlterColumn("dbo.BeerReviews", "Aroma", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BeerReviews", "Aroma", c => c.Int());
            RenameTable(name: "dbo.BeerReviews", newName: "Reviews");
        }
    }
}
