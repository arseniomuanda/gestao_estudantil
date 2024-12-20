# Gestão Estudantil

**Gestão Estudantil** é um sistema desenvolvido em ASP.NET MVC Framework 4.7 para gerenciar dados relacionados à administração de estudantes, cursos e matrículas. Ele utiliza o Entity Framework para integração com o banco de dados, oferecendo funcionalidades como:

- Cadastro de estudantes
- Gerenciamento de cursos
- Controle de matrículas

---

## **Requisitos do Sistema**

- **.NET Framework 4.7**
- **SQL Server** (ou outra base de dados suportada pelo Entity Framework)
- **Visual Studio** (recomendado 2019 ou superior)
- **Entity Framework 6.x**

---

## **Passos para Configurar e Rodar o Projeto em Outra Máquina**

### **1. Clonar o Repositório**

Use o comando abaixo para clonar o repositório:

```bash
git clone https://github.com/arseniomuanda/gestao_estudantil.git
```

Acesse o diretório do projeto:

```bash
cd gestao_estudantil
```

---

### **2. Configurar o Banco de Dados**

1. Abra o arquivo `Web.config` no diretório raiz do projeto.
2. Atualize a string de conexão `DefaultConnection` com os dados do seu servidor SQL:
   ```xml
   <connectionStrings>
       <add name="DefaultConnection"
            connectionString="Data Source=SEU_SERVIDOR;Initial Catalog=NOME_DA_BASE;Integrated Security=True"
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

---

### **3. Restaurar Pacotes NuGet**

No **Package Manager Console** ou terminal, execute:

Instalar as dependências do projecto
```bash
nuget restore
```

---

### **4. Habilitar e Atualizar o Banco de Dados**

No **Package Manager Console**, execute os seguintes comandos:

1. Habilitar migrações (caso não esteja habilitado):

   ```bash
   Enable-Migrations
   ```

2. Atualizar o banco de dados com todas as migrações:

   ```bash
   Update-Database
   ```
3. Adicionar o procedimento para Ver as notas dos alunos
  ```bash
  Add-Migration UpdateGetNotasAlunoProcedure
  ```

  ```cs
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
   ```
---

### **5. Compilar e Rodar o Projeto**

1. Abra o projeto no **Visual Studio**.
2. Compile o projeto pressionando **Ctrl + Shift + B** ou clicando em **Build > Build Solution**.
3. Execute o projeto pressionando **F5** ou clicando em **Start**.

---

## **Comandos Resumidos**

Para facilitar, aqui estão os principais comandos:

- Clonar o repositório:

  ```bash
  git clone https://github.com/arseniomuanda/gestao_estudantil.git
  ```

- Restaurar pacotes NuGet:

  ```bash
  nuget restore
  ```

- Habilitar migrações:

  ```bash
  Enable-Migrations
  ```

- Atualizar banco de dados:

  ```bash
  Update-Database
  ```


