namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HostEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Server = c.String(),
                        Port = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        Title = c.String(),
                        Body = c.String(),
                        ScheduledSendDateTime = c.DateTime(nullable: false),
                        SendDateTime = c.DateTime(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SenderEntities", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.RecipientEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SenderEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipient2MessageEntity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecipientId = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MessageEntities", t => t.MessageId, cascadeDelete: true)
                .ForeignKey("dbo.RecipientEntities", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.RecipientId)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.RecipientEntityMessageEntities",
                c => new
                    {
                        RecipientEntity_Id = c.Int(nullable: false),
                        MessageEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecipientEntity_Id, t.MessageEntity_Id })
                .ForeignKey("dbo.RecipientEntities", t => t.RecipientEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.MessageEntities", t => t.MessageEntity_Id, cascadeDelete: true)
                .Index(t => t.RecipientEntity_Id)
                .Index(t => t.MessageEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipient2MessageEntity", "RecipientId", "dbo.RecipientEntities");
            DropForeignKey("dbo.Recipient2MessageEntity", "MessageId", "dbo.MessageEntities");
            DropForeignKey("dbo.MessageEntities", "SenderId", "dbo.SenderEntities");
            DropForeignKey("dbo.RecipientEntityMessageEntities", "MessageEntity_Id", "dbo.MessageEntities");
            DropForeignKey("dbo.RecipientEntityMessageEntities", "RecipientEntity_Id", "dbo.RecipientEntities");
            DropIndex("dbo.RecipientEntityMessageEntities", new[] { "MessageEntity_Id" });
            DropIndex("dbo.RecipientEntityMessageEntities", new[] { "RecipientEntity_Id" });
            DropIndex("dbo.Recipient2MessageEntity", new[] { "MessageId" });
            DropIndex("dbo.Recipient2MessageEntity", new[] { "RecipientId" });
            DropIndex("dbo.MessageEntities", new[] { "SenderId" });
            DropTable("dbo.RecipientEntityMessageEntities");
            DropTable("dbo.Recipient2MessageEntity");
            DropTable("dbo.SenderEntities");
            DropTable("dbo.RecipientEntities");
            DropTable("dbo.MessageEntities");
            DropTable("dbo.HostEntities");
        }
    }
}
