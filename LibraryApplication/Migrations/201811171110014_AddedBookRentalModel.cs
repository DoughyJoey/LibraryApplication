namespace LibraryApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBookRentalModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookRentals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false),
                        BookID = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        ScheduledReturnDate = c.DateTime(),
                        ActualReturnDate = c.DateTime(),
                        AdditionalCharge = c.Double(),
                        RentalPrice = c.Double(nullable: false),
                        RentalDuration = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookRentals");
        }
    }
}
