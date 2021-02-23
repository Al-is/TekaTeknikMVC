using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        public ActionResult Index(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Permissions.Where(x => x.EmployeeId == id).ToList();
                var perName = db.Employees.Where(x => x.EmployeeId == id).Select(y => y.FirstName)
                    .FirstOrDefault();
                ViewBag.perName = perName;
                var totalPerm = db.Permissions.Count(x => x.EmployeeId == id).ToString();
                ViewBag.totalPerm ="İzin aldığı toplam gün sayısı  "+ totalPerm;
                return View(degerler);
            }
        }

        public ActionResult GetAll()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Permissions.Include("Employees").ToList();
                return View(degerler);

            }
        }

        [HttpPost]
        public ActionResult GetAll(Permissions per)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var getir = db.Permissions.Include("Employees").Where(y => y.StartDay.Value.Month >= per.StartDay.Value.Month && y.StartDay.Value.Day >= per.StartDay.Value.Day || y.EndDay.Value.Day <= per.EndDay.Value.Day && y.EndDay.Value.Month <= per.EndDay.Value.Month).ToList();
                //Seçilmiş tarihler arasında ki izinler
                var permissionByDate = db.Permissions.Where(y => y.StartDay.Value.Month == per.StartDay.Value.Month && y.StartDay.Value.Year == per.StartDay.Value.Year).Sum(x => x.Day)
                    .ToString();
                ViewBag.permissionByDate = "Seçilen tarihler arasında ki toplam izinler   " + permissionByDate;

                //Seçilen ayın izinleri
                var permissionByMonth = db.Permissions.Where(y => y.StartDay.Value.Month == per.StartDay.Value.Month&& y.StartDay.Value.Year == per.StartDay.Value.Year).Sum(x => x.Day)
                    .ToString();
                ViewBag.permissionByMonth ="Seçilen ayın izinleri   "+ permissionByMonth;
                //Seçilen Yılın izinleri
                var permissionByYear = db.Permissions.Where(y => y.StartDay.Value.Year == per.StartDay.Value.Year).Sum(x => x.Day)
                    .ToString();
                ViewBag.permissionByYear = "Seçilen yılın izinleri   "+ permissionByYear;
                return View(getir);

            }
        }
        [HttpGet]
        public ActionResult PermissionAdd(int id)
        {


            return View(new Permissions { EmployeeId = id });
        }

        [HttpPost]
        public ActionResult PermissionAdd(Permissions per)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Employees.Find(per.EmployeeId);
                db.Permissions.Add(per);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = per.EmployeeId });
            }
        }
        public ActionResult PermissionDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var per = db.Permissions.Find(id);
                string perm = per.EmployeeId.ToString();
                db.Permissions.Remove(per);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = perm });
            }
        }
        public ActionResult PermissionGet(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var per = db.Permissions.Find(id);

                return View("PermissionGet", per);
            }

        }

        public ActionResult PermissionUpdate(Permissions per)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var e = db.Permissions.Find(per.PermissionId);
                string id = per.EmployeeId.ToString();
                e.StartDay = per.StartDay;
                e.Day = per.Day;
                e.EndDay = per.EndDay;
                e.PermissionType = per.PermissionType;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = e.EmployeeId });
            }

        }
    }
}