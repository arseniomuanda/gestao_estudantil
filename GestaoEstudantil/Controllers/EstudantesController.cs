using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestaoEstudantil.Models;
using Microsoft.AspNet.Identity;

namespace GestaoEstudantil.Controllers
{
    [Authorize]
    public class EstudantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Estudantes
        public ActionResult Index()
        {
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = "Lista de Estudantes";
            var estudantes = db.Estudantes.Include(e => e.Curso);
            return View(estudantes.ToList());
        }

        private string GetCurrentUserId() { return User.Identity.GetUserId(); }

        // GET: Estudantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudante estudante = db.Estudantes.Find(id);
            if (estudante == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = estudante.NomeCompleto;

            string userId = GetCurrentUserId();

            //var professor = db.Professores.Where(e => e.UserId == 1).FirstOrDefault();

            var disciplinas = db.Database.SqlQuery<Disciplina>("SELECT Disciplinas.* FROM EstudanteDisciplinas RIGHT JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id <> @p0 OR EstudanteDisciplinas.Estudante_Id IS NULL;", id).ToList();

            ViewBag.Disciplinas_list = new SelectList(disciplinas, "Id", "Nome");

            return View(estudante);
        }

        public ActionResult EstudanteResume(int id) {
            var disciplinas = db.Database.SqlQuery<Disciplina>("SELECT Disciplinas.* FROM EstudanteDisciplinas INNER JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id = @p0", id).ToList();
            return Json(new { disciplinas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AdicionarDisciplina(int estudanteId, int disciplinaId)
        {
            try
            {
                var estudanteExiste = db.Estudantes.Any(e => e.Id == estudanteId);
                var disciplinaExiste = db.Disciplinas.Any(d => d.Id == disciplinaId);

                if (!estudanteExiste)
                {
                    return Json(new { status = "error", message = "Estudante não encontrado." }, JsonRequestBehavior.AllowGet);
                }

                if (!disciplinaExiste)
                {
                    return Json(new { status = "error", message = "Disciplina não encontrada." }, JsonRequestBehavior.AllowGet);
                }

                var sql = "INSERT INTO EstudanteDisciplinas (Disciplina_Id, Estudante_Id) VALUES (@p0, @p1)";
                db.Database.ExecuteSqlCommand(sql, disciplinaId, estudanteId);

                return Json(new { status = "success", message = "Disciplina adicionada ao estudante com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Ocorreu um erro ao adicionar a disciplina ao estudante. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult RemoverDisciplina(int estudanteId, int disciplinaId)
        {
            try
            {
                var estudanteExiste = db.Estudantes.Any(e => e.Id == estudanteId);
                var disciplinaExiste = db.Disciplinas.Any(d => d.Id == disciplinaId);

                if (!estudanteExiste)
                {
                    return Json(new { status = "error", message = "Estudante não encontrado." }, JsonRequestBehavior.AllowGet);
                }

                if (!disciplinaExiste)
                {
                    return Json(new { status = "error", message = "Disciplina não encontrada." }, JsonRequestBehavior.AllowGet);
                }

                var sql = "DELETE FROM EstudanteDisciplinas WHERE EstudanteDisciplinas.Disciplina_Id = @p0 AND EstudanteDisciplinas.Estudante_Id = @p1";
                db.Database.ExecuteSqlCommand(sql, disciplinaId, estudanteId);

                return Json(new { status = "success", message = "Disciplina removida ao estudante com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Ocorreu um erro ao remover a disciplina ao estudante. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Estudantes/Create
        public ActionResult Create()
        {
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = "Novo Estudante";
            ViewBag.CursoId = new SelectList(db.Cursos, "Id", "Nome");
            return View();
        }

        // POST: Estudantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NomeCompleto,DataNascimento,NumeroBI,EmailEstudante,CursoId")] Estudante estudante)
        {
            if (ModelState.IsValid)
            {
                db.Estudantes.Add(estudante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = "Novo Estudante";
            ViewBag.CursoId = new SelectList(db.Cursos, "Id", "Nome", estudante.CursoId);
            return View(estudante);
        }

        // GET: Estudantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudante estudante = db.Estudantes.Find(id);
            if (estudante == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = estudante.NomeCompleto;
            ViewBag.CursoId = new SelectList(db.Cursos, "Id", "Nome", estudante.CursoId);
            return View(estudante);
        }

        // POST: Estudantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NomeCompleto,DataNascimento,NumeroBI,EmailEstudante,CursoId")] Estudante estudante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = estudante.NomeCompleto;
            ViewBag.CursoId = new SelectList(db.Cursos, "Id", "Nome", estudante.CursoId);
            return View(estudante);
        }

        // GET: Estudantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudante estudante = db.Estudantes.Find(id);
            if (estudante == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Estudantes";
            ViewBag.pageContex = estudante.NomeCompleto;
            return View(estudante);
        }

        public ActionResult EstudanteRelatorio(int id) {
            var estudante = db.Estudantes.Find(id);
            return View(estudante);
        }

        
        public ActionResult GetNotasAluno(int id)
        {
            // Consulta para obter Disciplinas com RIGHT JOIN
            var disciplinasQuery = @"
            SELECT Disciplinas.* 
            FROM EstudanteDisciplinas 
            INNER JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id 
            WHERE EstudanteDisciplinas.Estudante_Id = @EstudanteId";

            var disciplinas = db.Database.SqlQuery<Disciplina>(disciplinasQuery, new SqlParameter("@EstudanteId", id)).ToList();

            // Consulta dinâmica para obter Notas
            var frequencias = db.Database.SqlQuery<Frequencia>("Select * From Frequencias").Distinct().ToList();

            string query = @"
                SELECT 
                    n.IdNota, 
                    n.Valor, 
                    n.EstudanteId, 
                    n.FrequenciaId, 
                    f.Nome AS FrequenciaNome,
                    n.DisciplinaId, 
                    d.Nome AS DisciplinaNome
                FROM 
                    Notas n
                INNER JOIN 
                    Frequencias f ON n.FrequenciaId = f.Id
                INNER JOIN 
                    Disciplinas d ON n.DisciplinaId = d.Id
                WHERE 
                    n.EstudanteId = @p0";


                var notas = db.Database.SqlQuery<NotaFrequenciaDisciplina>(query, id);

            // Retornar ambos os resultados como JSON
            return Json(new { disciplinas, notas }, JsonRequestBehavior.AllowGet);
        }

        // POST: Estudantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudante estudante = db.Estudantes.Find(id);
            db.Estudantes.Remove(estudante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}