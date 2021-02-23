using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department

        public ActionResult Index()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var departments = db.Departments.Include("Employees").ToList();
                return View(departments);
            }


        }
        [HttpGet]
        public ActionResult NewDepartmentAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewDepartmentAdd(Departments dep)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Departments.Add(dep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult DepartmentDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var dep = db.Departments.Find(id);
                db.Departments.Remove(dep);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

        }

        public ActionResult DepartmentGet(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var dep = db.Departments.Find(id);

                return View("DepartmentGet", dep);
            }

        }

        public ActionResult DepartmentUpdate(Departments dep)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var e = db.Departments.Find(dep.DepartmentId);
                e.DepartmentName = dep.DepartmentName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult DepartmentDetail(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Employees.Where(x => x.DepartmentId == id).ToList();
                var totalEmp = db.Employees.Count(x => x.DepartmentId == id).ToString();
                ViewBag.totalEmp = totalEmp;
                var departmentName = db.Departments.Where(x => x.DepartmentId == id).Select(y => y.DepartmentName)
                    .FirstOrDefault();
                ViewBag.departmentName = departmentName;
                return View(degerler);
            }

        }
    }
}