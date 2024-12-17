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
    public class FrequenciasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Frequencias
        public ActionResult Index()
        {
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = "Lista de Frequencias";
            return View(db.Frequencias.ToList());
        }

        // GET: Frequencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequencia frequencia = db.Frequencias.Find(id);
            if (frequencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = frequencia.Nome;
            return View(frequencia);
        }

        // GET: Frequencias/Create
        public ActionResult Create()
        {
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = "Nova Frequência";
            return View();
        }

        // POST: Frequencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] Frequencia frequencia)
        {
            if (ModelState.IsValid)
            {
                db.Frequencias.Add(frequencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = "Nova Frequência";
            return View(frequencia);
        }

        // GET: Frequencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequencia frequencia = db.Frequencias.Find(id);
            if (frequencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = frequencia.Nome; 
            return View(frequencia);
        }

        // POST: Frequencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] Frequencia frequencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(frequencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = frequencia.Nome;
            return View(frequencia);
        }

        // GET: Frequencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frequencia frequencia = db.Frequencias.Find(id);
            if (frequencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = "Frequencias";
            ViewBag.pageContex = frequencia.Nome;
            return View(frequencia);
        }

        // POST: Frequencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Frequencia frequencia = db.Frequencias.Find(id);
            db.Frequencias.Remove(frequencia);
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
