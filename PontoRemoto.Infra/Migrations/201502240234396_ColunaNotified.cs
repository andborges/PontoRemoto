namespace PontoRemoto.Infra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColunaNotified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkin", "Notified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Checkin", "Notified");
        }
    }
}
