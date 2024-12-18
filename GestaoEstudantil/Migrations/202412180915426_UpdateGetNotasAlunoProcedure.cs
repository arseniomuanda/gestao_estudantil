﻿namespace GestaoEstudantil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateGetNotasAlunoProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            CREATE PROCEDURE GetNotasAluno
                @EstudanteId INT
            AS
            BEGIN
                DECLARE @cols AS NVARCHAR(MAX),
                        @query AS NVARCHAR(MAX);

                -- Obter a lista de nomes de frequência dinamicamente
                SELECT @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(Nome)
                                      FROM Frequencias
                                      FOR XML PATH(''), TYPE
                                      ).value('.', 'NVARCHAR(MAX)'), 1, 1, '');

                -- Construir a consulta dinâmica
                SET @query = '
                SELECT ' + @cols + '
                FROM
                (
                    SELECT n.EstudanteId, f.Nome, COALESCE(CAST(n.Valor AS NVARCHAR), NULL) AS Valor
                    FROM Notas n
                    RIGHT JOIN Frequencias f ON n.FrequenciaId = f.Id
                    WHERE n.EstudanteId = @EstudanteId
                ) x
                PIVOT
                (
                    MAX(Valor)
                    FOR Nome IN (' + @cols + ')
                ) p';

                -- Executar a consulta dinâmica
                -- EXEC sp_executesql @query, N''@EstudanteId INT'', @EstudanteId;
            END
        ");
        }

        public override void Down()
        {
            Sql("DROP PROCEDURE IF EXISTS GetNotasAluno");
        }
    }

}