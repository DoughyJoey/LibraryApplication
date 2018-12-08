namespace LibraryApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedProductDimensionsFromBookModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "ProductDimensions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ProductDimensions", c => c.String(nullable: false));
        }
    }
}
