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
    public class DisciplinasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Disciplinas
        public ActionResult Index()
        {
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = "Lista de Disciplinas";
            return View(db.Disciplinas.Include(e => e.Professor).ToList());
        }

        // GET: Disciplinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = disciplina.Nome;
            return View(disciplina);
        }

        // GET: Disciplinas/Create
        public ActionResult Create()
        {
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = "Nova Disciplina";
            ViewBag.ProfessorId = new SelectList(db.Professores, "IdProfessor", "Nome");
            return View();
        }

        // POST: Disciplinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,CodigoDisciplina,ProfessorId")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                db.Disciplinas.Add(disciplina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = "Nova Disciplina";
            ViewBag.ProfessorId = new SelectList(db.Professores, "IdProfessor", "Nome");
            return View(disciplina);
        }

        // GET: Disciplinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = disciplina.Nome;
            ViewBag.ProfessorId = new SelectList(db.Professores, "IdProfessor", "Nome");
            return View(disciplina);
        }

        // POST: Disciplinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,CodigoDisciplina,ProfessorId")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disciplina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Disciplinas";
            ViewBag.pageContex = disciplina.Nome;
            ViewBag.ProfessorId = new SelectList(db.Professores, "IdProfessor", "Nome");
            return View(disciplina);
        }

        // GET: Disciplinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Disciplina disciplina = db.Disciplinas.Find(id);
            db.Disciplinas.Remove(disciplina);
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
