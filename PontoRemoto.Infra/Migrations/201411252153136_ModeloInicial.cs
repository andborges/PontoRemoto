namespace PontoRemoto.Infra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModeloInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checkin",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkerId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Hash = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Worker", t => t.WorkerId, cascadeDelete: true)
                .Index(t => t.WorkerId);
            
            CreateTable(
                "dbo.Worker",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        DeviceId = c.String(nullable: false, maxLength: 256, unicode: false),
                        DeviceModel = c.String(nullable: false, maxLength: 256, unicode: false),
                        DevicePlatform = c.String(nullable: false, maxLength: 256, unicode: false),
                        DeviceAlias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Name = c.String(nullable: false, maxLength: 256, unicode: false),
                        Identification = c.String(nullable: false, maxLength: 256, unicode: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identification = c.String(nullable: false, maxLength: 256, unicode: false),
                        Name = c.String(nullable: false, maxLength: 256, unicode: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AppCode = c.String(nullable: false, maxLength: 100, unicode: false),
                        AppSecret = c.String(nullable: false, maxLength: 100, unicode: false),
                        WorkerIdentificationLabel = c.String(maxLength: 100, unicode: false),
                        UrlCheckinNotification = c.String(maxLength: 1024, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Identification, unique: true, name: "IX_Client_Identification")
                .Index(t => t.UserId)
                .Index(t => t.AppCode, unique: true, name: "IX_Client_AppCode");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Checkin", "WorkerId", "dbo.Worker");
            DropForeignKey("dbo.Worker", "ClientId", "dbo.Client");
            DropForeignKey("dbo.Client", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Client", "IX_Client_AppCode");
            DropIndex("dbo.Client", new[] { "UserId" });
            DropIndex("dbo.Client", "IX_Client_Identification");
            DropIndex("dbo.Worker", new[] { "ClientId" });
            DropIndex("dbo.Checkin", new[] { "WorkerId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Client");
            DropTable("dbo.Worker");
            DropTable("dbo.Checkin");
        }
    }
}
