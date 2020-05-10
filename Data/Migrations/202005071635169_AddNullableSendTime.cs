namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullableSendTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MessageEntities", "ScheduledSendDateTime", c => c.DateTime());
            AlterColumn("dbo.MessageEntities", "SendDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MessageEntities", "SendDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MessageEntities", "ScheduledSendDateTime", c => c.DateTime(nullable: false));
        }
    }
}
