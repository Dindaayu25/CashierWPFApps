namespace CRUDBootcamp32.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelTransTransactionItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_m_transactionitem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Item_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_item", t => t.Item_Id)
                .ForeignKey("dbo.tb_m_transaction", t => t.Transaction_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.tb_m_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_m_transactionitem", "Transaction_Id", "dbo.tb_m_transaction");
            DropForeignKey("dbo.tb_m_transactionitem", "Item_Id", "dbo.tb_m_item");
            DropIndex("dbo.tb_m_transactionitem", new[] { "Transaction_Id" });
            DropIndex("dbo.tb_m_transactionitem", new[] { "Item_Id" });
            DropTable("dbo.tb_m_transaction");
            DropTable("dbo.tb_m_transactionitem");
        }
    }
}
