using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GestaoEstudantil.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    //My Models

    public class Curso
    {
        public Curso() {
            this.Disciplinas = new HashSet<Disciplina>();
            this.Estudantes = new HashSet<Estudante>();
        }
        [Key]public int Id { get; set; }
        [Required(ErrorMessage = "O nome do curso é requirido")]
        [Display(Name = "Nome do Curso")]
        public string Nome { get; set; }

        public ICollection<Disciplina> Disciplinas { get; set; }
        public ICollection<Estudante> Estudantes { get; set; }
    }

    public class Estudante
    {
        public Estudante()
        {
            this.Disciplinas = new HashSet<Disciplina>();
            this.Notas = new HashSet<Nota>();
        }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Número do BI é obrigatório")]
        [Display(Name = "Número do BI")]
        public string NumeroBI { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de e-mail válido")]
        [Display(Name = "E-mail")]
        public string EmailEstudante { get; set; }

        [Required] public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }
        public ICollection<Disciplina> Disciplinas { get; set; }    
        public ICollection<Nota> Notas { get; set; }    
    }

    public class Disciplina
    {
        public Disciplina() { this.Estudantes = new HashSet<Estudante>(); }
        [Key] public int Id { get; set; }
        [Required(ErrorMessage = "Nome da disciplina é obrigatório")]
        [Display(Name = "Nome da Disciplina")] 
        public string Nome { get; set; }
        [Required(ErrorMessage = "Código da disciplina é obrigatório")]
        [Display(Name = "Código da Disciplina")] 
        public string CodigoDisciplina { get; set; }
        [Required(ErrorMessage = "Professor responsável é obrigatório")]
        [Display(Name = "Professor Responsável")] 
        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }
        public ICollection<Estudante> Estudantes { get; set; }
    }

    public class Professor
    {
        [Key] public int IdProfessor { get; set; }
        [Required(ErrorMessage = "Nome do professor é obrigatório")]
        [Display(Name = "Nome do Professor")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Número do MC é obrigatório")]
        [Display(Name = "Número do MC")]
        public string NumeroMc { get; set; }
        public ICollection<Disciplina> Disciplinas { get; set; }
        
        public string UserId { get; set; }
    }

    public class Nota
    {
        [Key]
        public int IdNota { get; set; }

        [Required(ErrorMessage = "Valor da nota é obrigatório")]
        [Range(0, 20, ErrorMessage = "O valor da nota deve estar entre 0 e 20")]
        public double Valor { get; set; }
        public int EstudanteId { get; set; }
        public virtual Estudante Estudante { get; set; }
        [Required]public int FrequenciaId { get; set; }
        public virtual Frequencia Frequencia { get; set; }
        [Required]
        public int DisciplinaId { get; set; }
        public virtual Disciplina Disciplina { get; set; }
    }

    public class Frequencia
    {
        public Frequencia()
        {
            this.Notas = new HashSet<Nota>();
        }
        [Key]public int Id { get; set; }
        [Required]public string Nome { get; set; }
        public ICollection<Nota> Notas { get; set; }
    }

    public class AuditLog { 
        [Key] public int Id { get; set; } 
        [Required][StringLength(50)] public string Action { get; set; } 
        [Required][StringLength(50)] public string TableName { get; set; } 
        [Required] public DateTime Date { get; set; } 
        [Required][StringLength(128)] public string UserId { get; set; } 
    }

    public class NotaFrequenciaDisciplina
    {
        public int IdNota { get; set; }
        public double Valor { get; set; }
        public int EstudanteId { get; set; }
        public int FrequenciaId { get; set; }
        public string FrequenciaNome { get; set; } // Nome da frequência
        public int DisciplinaId { get; set; }
        public string DisciplinaNome { get; set; } // Nome da disciplina
    }
}