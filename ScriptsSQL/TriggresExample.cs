//Add-Migration AddTriggersToProfessorAndNota

public partial class AddTriggersToProfessorAndNota : DbMigration
{
    public override void Up()
    {
        // Trigger para INSERT na tabela Professor
        Sql(@"
            CREATE TRIGGER TR_Professor_Insert
            ON dbo.Professors
            AFTER INSERT
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'INSERT', 'Professors', GETDATE(), inserted.IdProfessor
                FROM inserted;
            END
        ");

        // Trigger para UPDATE na tabela Professor
        Sql(@"
            CREATE TRIGGER TR_Professor_Update
            ON dbo.Professors
            AFTER UPDATE
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'UPDATE', 'Professors', GETDATE(), inserted.UIdProfessorserId
                FROM inserted;
            END
        ");

        // Trigger para DELETE na tabela Professor
        Sql(@"
            CREATE TRIGGER TR_Professor_Delete
            ON dbo.Professors
            AFTER DELETE
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'DELETE', 'Professors', GETDATE(), deleted.IdProfessor
                FROM deleted;
            END
        ");

        // Trigger para INSERT na tabela Nota
        Sql(@"
            CREATE TRIGGER TR_Nota_Insert
            ON dbo.Notas
            AFTER INSERT
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'INSERT', 'Notas', GETDATE(), NULL
                FROM inserted;
            END
        ");

        // Trigger para UPDATE na tabela Nota
        Sql(@"
            CREATE TRIGGER TR_Nota_Update
            ON dbo.Notas
            AFTER UPDATE
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'UPDATE', 'Notas', GETDATE(), NULL
                FROM inserted;
            END
        ");

        // Trigger para DELETE na tabela Nota
        Sql(@"
            CREATE TRIGGER TR_Nota_Delete
            ON dbo.Notas
            AFTER DELETE
            AS
            BEGIN
                INSERT INTO AuditLog (Action, TableName, Date, UserId)
                SELECT 'DELETE', 'Notas', GETDATE(), NULL
                FROM deleted;
            END
        ");
    }

    public override void Down()
    {
        // Remover triggers na tabela Professor
        Sql("DROP TRIGGER TR_Professor_Insert");
        Sql("DROP TRIGGER TR_Professor_Update");
        Sql("DROP TRIGGER TR_Professor_Delete");

        // Remover triggers na tabela Nota
        Sql("DROP TRIGGER TR_Nota_Insert");
        Sql("DROP TRIGGER TR_Nota_Update");
        Sql("DROP TRIGGER TR_Nota_Delete");
    }
}
