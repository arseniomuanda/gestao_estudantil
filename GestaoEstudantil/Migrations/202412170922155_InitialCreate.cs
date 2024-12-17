namespace GestaoEstudantil.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cursoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disciplinas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        CodigoDisciplina = c.String(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        Curso_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .ForeignKey("dbo.Cursoes", t => t.Curso_Id)
                .Index(t => t.ProfessorId)
                .Index(t => t.Curso_Id);
            
            CreateTable(
                "dbo.Estudantes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeCompleto = c.String(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        NumeroBI = c.String(nullable: false),
                        EmailEstudante = c.String(nullable: false),
                        CursoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cursoes", t => t.CursoId, cascadeDelete: true)
                .Index(t => t.CursoId);
            
            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        IdNota = c.Int(nullable: false, identity: true),
                        Valor = c.Double(nullable: false),
                        EstudanteId = c.Int(nullable: false),
                        FrequenciaId = c.Int(nullable: false),
                        DisciplinaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdNota)
                .ForeignKey("dbo.Disciplinas", t => t.DisciplinaId, cascadeDelete: true)
                .ForeignKey("dbo.Estudantes", t => t.EstudanteId, cascadeDelete: true)
                .ForeignKey("dbo.Frequencias", t => t.FrequenciaId, cascadeDelete: true)
                .Index(t => t.EstudanteId)
                .Index(t => t.FrequenciaId)
                .Index(t => t.DisciplinaId);
            
            CreateTable(
                "dbo.Frequencias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Professors",
                c => new
                    {
                        IdProfessor = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        NumeroMc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdProfessor);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EstudanteDisciplinas",
                c => new
                    {
                        Estudante_Id = c.Int(nullable: false),
                        Disciplina_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Estudante_Id, t.Disciplina_Id })
                .ForeignKey("dbo.Estudantes", t => t.Estudante_Id, cascadeDelete: true)
                .ForeignKey("dbo.Disciplinas", t => t.Disciplina_Id, cascadeDelete: true)
                .Index(t => t.Estudante_Id)
                .Index(t => t.Disciplina_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Disciplinas", "Curso_Id", "dbo.Cursoes");
            DropForeignKey("dbo.Disciplinas", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Notas", "FrequenciaId", "dbo.Frequencias");
            DropForeignKey("dbo.Notas", "EstudanteId", "dbo.Estudantes");
            DropForeignKey("dbo.Notas", "DisciplinaId", "dbo.Disciplinas");
            DropForeignKey("dbo.EstudanteDisciplinas", "Disciplina_Id", "dbo.Disciplinas");
            DropForeignKey("dbo.EstudanteDisciplinas", "Estudante_Id", "dbo.Estudantes");
            DropForeignKey("dbo.Estudantes", "CursoId", "dbo.Cursoes");
            DropIndex("dbo.EstudanteDisciplinas", new[] { "Disciplina_Id" });
            DropIndex("dbo.EstudanteDisciplinas", new[] { "Estudante_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Notas", new[] { "DisciplinaId" });
            DropIndex("dbo.Notas", new[] { "FrequenciaId" });
            DropIndex("dbo.Notas", new[] { "EstudanteId" });
            DropIndex("dbo.Estudantes", new[] { "CursoId" });
            DropIndex("dbo.Disciplinas", new[] { "Curso_Id" });
            DropIndex("dbo.Disciplinas", new[] { "ProfessorId" });
            DropTable("dbo.EstudanteDisciplinas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Professors");
            DropTable("dbo.Frequencias");
            DropTable("dbo.Notas");
            DropTable("dbo.Estudantes");
            DropTable("dbo.Disciplinas");
            DropTable("dbo.Cursoes");
        }
    }
}
