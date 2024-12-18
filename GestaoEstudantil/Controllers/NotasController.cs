using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestaoEstudantil.Models;

namespace GestaoEstudantil.Controllers
{
    [Authorize]
    public class NotasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notas
        public ActionResult Index()
        {
            ViewBag.page = "Notas";
            ViewBag.pageContex = "Lista de Notas";
            var notas = db.Notas.Include(n => n.Disciplina).Include(n => n.Estudante).Include(n => n.Frequencia);
            return View(notas.ToList());
        }

        // GET: Notas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Perfil da nota";
            return View(nota);
        }

        // GET: Notas/Create
        public ActionResult Create()
        {
            ViewBag.DisciplinaId = new SelectList(db.Disciplinas, "Id", "Nome");
            ViewBag.EstudanteId = new SelectList(db.Estudantes, "Id", "NomeCompleto");
            ViewBag.FrequenciaId = new SelectList(db.Frequencias, "Id", "Nome");

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Nova nota";

            return View();
        }

        // POST: Notas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNota,Valor,EstudanteId,FrequenciaId,DisciplinaId")] Nota nota)
        {
            var disciplinasAluno = db.Database.SqlQuery<Disciplina>("SELECT Disciplinas.* FROM EstudanteDisciplinas INNER JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id = @p0", nota.EstudanteId).ToList();

            var disciplinasAlunoNotas = db.Database.SqlQuery<Nota>("SELECT Notas.* FROM Notas WHERE Notas.EstudanteId = @p0 AND Notas.DisciplinaId = @p1", nota.EstudanteId, nota.DisciplinaId).ToList();

            if (disciplinasAlunoNotas.Count() > 0)
            {
                ViewBag.ErrorMessage = "Esse estudante já tem uma nota nessa disciplina!";
                return View("Error");
            }

            if (disciplinasAluno.Count() > 0)
            {
                if (ModelState.IsValid)
                {
                    db.Notas.Add(nota);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else {
                 ViewBag.ErrorMessage = "Essa disciplina nao faz parte das disciplina esse aluno."; 
                    return View("Error");
            }

            ViewBag.DisciplinaId = new SelectList(db.Disciplinas, "Id", "Nome", nota.DisciplinaId);
            ViewBag.EstudanteId = new SelectList(db.Estudantes, "Id", "NomeCompleto", nota.EstudanteId);
            ViewBag.FrequenciaId = new SelectList(db.Frequencias, "Id", "Nome", nota.FrequenciaId);

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Nova nota";
            return View(nota);
        }

        // GET: Notas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisciplinaId = new SelectList(db.Disciplinas, "Id", "Nome", nota.DisciplinaId);
            ViewBag.EstudanteId = new SelectList(db.Estudantes, "Id", "NomeCompleto", nota.EstudanteId);
            ViewBag.FrequenciaId = new SelectList(db.Frequencias, "Id", "Nome", nota.FrequenciaId);

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Perfil da nota";
            return View(nota);
        }

        // POST: Notas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNota,Valor,EstudanteId,FrequenciaId,DisciplinaId")] Nota nota)
        {
            var disciplinasAluno = db.Database.SqlQuery<Disciplina>("SELECT Disciplinas.* FROM EstudanteDisciplinas INNER JOIN Disciplinas ON EstudanteDisciplinas.Disciplina_Id = Disciplinas.Id WHERE EstudanteDisciplinas.Estudante_Id = @p0", nota.EstudanteId).ToList();

            if (disciplinasAluno.Count() > 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(nota).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Essa disciplina nao faz parte das disciplina esse aluno.";
                return View("Error");
            }

            ViewBag.DisciplinaId = new SelectList(db.Disciplinas, "Id", "Nome", nota.DisciplinaId);
            ViewBag.EstudanteId = new SelectList(db.Estudantes, "Id", "NomeCompleto", nota.EstudanteId);
            ViewBag.FrequenciaId = new SelectList(db.Frequencias, "Id", "Nome", nota.FrequenciaId);

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Perfil da nota";
            return View(nota);
        }

        // GET: Notas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }

            ViewBag.page = "Notas";
            ViewBag.pageContex = "Perfil da nota";
            return View(nota);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nota nota = db.Notas.Find(id);
            db.Notas.Remove(nota);
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
