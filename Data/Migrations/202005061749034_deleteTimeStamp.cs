namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteTimeStamp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HostEntities", "TimeStamp");
            DropColumn("dbo.MessageEntities", "TimeStamp");
            DropColumn("dbo.RecipientEntities", "TimeStamp");
            DropColumn("dbo.SenderEntities", "TimeStamp");
            DropColumn("dbo.Recipient2MessageEntity", "TimeStamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipient2MessageEntity", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.SenderEntities", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.RecipientEntities", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MessageEntities", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.HostEntities", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
    }
}
