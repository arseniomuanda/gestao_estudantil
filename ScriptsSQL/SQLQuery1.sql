use GestaoEscolar;
SELECT Disciplinas.*, EstudanteDisciplinas.Estudante_Id FROM EstudanteDisciplinas INNER JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id = 1 AND Disciplinas.ProfessorId = 1;

SELECT * from AspNetUsers;
Select * from Professors;

SELECT Disciplinas.* FROM EstudanteDisciplinas RIGHT JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id <> 1 OR EstudanteDisciplinas.Estudante_Id IS NULL;

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
) x
PIVOT
(
    MAX(Valor)
    FOR Nome IN (' + @cols + ')
) p';

-- Executar a consulta dinâmica
EXEC sp_executesql @query;


select Frequencias.Nome, Notas.Valor from Notas inner join Frequencias on Notas.FrequenciaId = Frequencias.Id
