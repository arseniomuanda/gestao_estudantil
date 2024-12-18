namespace GestaoEstudantil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(nullable: false, maxLength: 50),
                        TableName = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuditLogs");
        }
    }
}
