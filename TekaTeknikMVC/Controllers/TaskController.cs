using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Tasks.Include("Employees").ToList();
                return View(degerler);

            }

        }
        [HttpGet]
        public ActionResult NewTaskAdd()
        {
            return View();

        }
        [HttpPost]
        public ActionResult NewTaskAdd(Tasks task)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult TaskDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var task = db.Tasks.Find(id);
                db.Tasks.Remove(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult TaskGet(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var task = db.Tasks.Find(id);

                return View("TaskGet", task);
            }

        }

        public ActionResult TaskUpdate(Tasks task)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var e = db.Tasks.Find(task.TaskId);
                e.TaskName = task.TaskName;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult TaskDetail(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Employees.Where(x => x.TaskId == id).Include("Departments").ToList();
                var taskName = db.Tasks.Where(x => x.TaskId == id).Select(y => y.TaskName)
                    .FirstOrDefault();
                ViewBag.taskName = taskName;
                var totalTask = db.Employees.Count(x => x.TaskId == id).ToString();
                ViewBag.totalTask = totalTask;
                return View(degerler);
            }

        }
    }
}