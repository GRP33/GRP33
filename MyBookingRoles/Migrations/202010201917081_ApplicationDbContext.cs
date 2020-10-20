namespace MyBookingRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Speciality = c.Int(nullable: false),
                        RatePerHour = c.Double(nullable: false),
                        DisableNewBookings = c.Boolean(nullable: false),
                        PhoneNum = c.String(nullable: false, maxLength: 10),
                        ProfilePic = c.Binary(),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        CustomerID = c.Int(nullable: false),
                        CustomerName = c.String(),
                        ArtistID = c.Int(nullable: false),
                        Location = c.String(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        end = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        BookingFee = c.Double(nullable: false),
                        ArtistRateFee = c.Double(nullable: false),
                        PackageCost = c.Double(nullable: false),
                        EventFee = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        TimeBlockHelper = c.String(),
                        Status = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ArtistID)
                .Index(t => t.PackageId)
                .Index(t => t.ServiceId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageId = c.Int(nullable: false, identity: true),
                        PackageType = c.String(),
                        PackagePrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PackageId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceType = c.String(),
                        ServicePrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        BookingID = c.Int(nullable: false),
                        Answer = c.String(),
                        Comment = c.String(),
                        ArtistName = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FeedbackID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.BookPayments",
                c => new
                    {
                        BookPayymentID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        DiscountID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        PaymentType = c.Boolean(nullable: false),
                        BalanceDue = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        MembershipDiscount = c.Double(nullable: false),
                        PaymetDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Discount_DiscId = c.Int(),
                    })
                .PrimaryKey(t => t.BookPayymentID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.Discounts", t => t.Discount_DiscId)
                .Index(t => t.BookingID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Discount_DiscId);
            
            AddColumn("dbo.Orders", "UserId", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Orders", "City", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.Orders", "PostalCode", c => c.String(nullable: false, maxLength: 8));
            AddColumn("dbo.Orders", "Country", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.Orders", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Experation", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "CreditCard", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "CreditCardNumber", c => c.String());
            AddColumn("dbo.Orders", "CcType", c => c.String());
            AddColumn("dbo.Orders", "SaveInfo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
            AddForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookPayments", "Discount_DiscId", "dbo.Discounts");
            DropForeignKey("dbo.BookPayments", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.BookPayments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Feedbacks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Bookings", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Bookings", "ArtistID", "dbo.Artists");
            DropForeignKey("dbo.Bookings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BookPayments", new[] { "Discount_DiscId" });
            DropIndex("dbo.BookPayments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.BookPayments", new[] { "BookingID" });
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Feedbacks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Feedbacks", new[] { "BookingID" });
            DropIndex("dbo.Bookings", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Bookings", new[] { "ServiceId" });
            DropIndex("dbo.Bookings", new[] { "PackageId" });
            DropIndex("dbo.Bookings", new[] { "ArtistID" });
            DropColumn("dbo.Orders", "ApplicationUser_Id");
            DropColumn("dbo.Orders", "SaveInfo");
            DropColumn("dbo.Orders", "CcType");
            DropColumn("dbo.Orders", "CreditCardNumber");
            DropColumn("dbo.Orders", "CreditCard");
            DropColumn("dbo.Orders", "Experation");
            DropColumn("dbo.Orders", "Total");
            DropColumn("dbo.Orders", "Country");
            DropColumn("dbo.Orders", "PostalCode");
            DropColumn("dbo.Orders", "City");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "UserId");
            DropTable("dbo.BookPayments");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Services");
            DropTable("dbo.Packages");
            DropTable("dbo.Bookings");
            DropTable("dbo.Artists");
            DropTable("dbo.Administrations");
        }
    }
}
