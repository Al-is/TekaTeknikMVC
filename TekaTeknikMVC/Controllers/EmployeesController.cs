using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

// TODO: A) Ödeme ve izinlerde emp id ye göre yapılan detayın, filtrelemesini yap B)Emp id ye göre İzin detayın footer'ı C) İzinler Footer 2) Anasayfa Footer 3) Departman detay Footer 4) Görev detay footer


namespace TekaTeknikMVC.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees

        public ActionResult Index()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Employees.Include("Tasks").Include("Departments").Include("Districts").ToList();
                return View(degerler);

            }


        }
        [HttpGet]
        public ActionResult NewEmployeeAdd()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                List<SelectListItem> deger = (from x in db.Departments.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.DepartmentName,
                                                  Value = x.DepartmentId.ToString()
                                              }).ToList();
                ViewBag.deger = deger;
                List<SelectListItem> tasks = (from x in db.Tasks.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.TaskName,
                                                  Value = x.TaskId.ToString()
                                              }).ToList();
                ViewBag.tasks = tasks;
                List<SelectListItem> district = (from x in db.Districts.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.DistrictName,
                                                     Value = x.DistrictId.ToString()
                                                 }).ToList();
                ViewBag.district = district;

                return View();
            }

        }
        [HttpPost]
        public ActionResult NewEmployeeAdd(Employees emp)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult EmployeeDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var emp = db.Employees.Find(id);
                db.Employees.Remove(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public ActionResult EmployeeGet(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                List<SelectListItem> deger = (from x in db.Departments.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.DepartmentName,
                                                  Value = x.DepartmentId.ToString()
                                              }).ToList();
                ViewBag.deger = deger;
                List<SelectListItem> tasks = (from x in db.Tasks.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.TaskName,
                                                  Value = x.TaskId.ToString()
                                              }).ToList();
                ViewBag.tasks = tasks;

                List<SelectListItem> district = (from x in db.Districts.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.DistrictName,
                                                     Value = x.DistrictId.ToString()
                                                 }).ToList();
                ViewBag.district = district;

                List<SelectListItem> city = (from x in db.Cities.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.CityName,
                                                 Value = x.CityId.ToString()
                                             }).ToList();
                ViewBag.city = city;

                var emp = db.Employees.Find(id);

                return View("EmployeeGet", emp);
            }
        }

        public ActionResult EmployeeUpdate(Employees emp)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                Employees e = db.Employees.Find(emp.EmployeeId);
                e.FirstName = emp.FirstName;
                e.Lastname = emp.Lastname;
                e.DepartmentId = emp.DepartmentId;
                e.TaskId = emp.TaskId;
                e.DistrictId = emp.DistrictId;
                // e.Districts = emp.Districts.CityId;
                e.DateOfBirth = emp.DateOfBirth;
                e.StartDay = emp.StartDay;
                e.EndDay = emp.EndDay;
                e.PlaceOfBirth = emp.PlaceOfBirth;
                e.MobilePhone = emp.MobilePhone;
                e.Mail = emp.Mail;
                e.Address = emp.Address;
                e.Nationality = emp.Nationality;
                e.Salary = emp.Salary;
                e.EmployeeCode = emp.EmployeeCode;
                e.Gender = emp.Gender;
                e.IdentyNumber = emp.IdentyNumber;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

        }

        public ActionResult EmployeeDetail(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                //Bu haftada ki izinler
                var permissionByWeek = db.Permissions
                   .Where(x => x.EmployeeId == id && x.StartDay.Value.Year == DateTime.Now.Year && x.StartDay.Value.Month == DateTime.Now.Month && x.StartDay.Value.Day >= DateTime.Now.Day).Sum(x => x.Day).ToString();

                var apermissionByWeek = db.Permissions
                    .Where(x => x.EmployeeId == id && x.StartDay.Value.Day == DateTime.Now.Day).Sum(x => x.Day).ToString();
                //var permissionByWeek = db.Permissions.Where(x => x.EmployeeId == id && x.StartDay.Value.Day >= DateTime.Now.Day).Count().ToString();
                ViewBag.permissionByWeek = permissionByWeek;

                //Bu ay ki izinler
                var empPermissionByMonth = db.Permissions
                    .Where(x => x.EmployeeId == id && x.StartDay.Value.Year == DateTime.Now.Year && x.StartDay.Value.Month == DateTime.Now.Month).Sum(x => x.Day)
                    .ToString();
                ViewBag.empPermissionByMonth = empPermissionByMonth;

                //Bu yıl ki izinler
                var empPermissionByYear = db.Permissions
                    .Where(x => x.EmployeeId == id && x.StartDay.Value.Year == DateTime.Now.Year)
                    .Sum(x => x.Day).ToString();
                ViewBag.empPermissionByYear = empPermissionByYear;

                //Bu hafta yapılan ödeme
                //var empPaymentByWeek = db.Payments.Where(x => x.EmployeeId == id && x.Date.Value.DayOfWeek == DateTime.Now.DayOfWeek).Sum(x => x.Amount).ToString();
                //ViewBag.empPaymentByWeek = empPaymentByWeek;

                //Bu ay yapılan ödeme
                var empPaymentByMonth = db.Payments.Where(x => x.EmployeeId == id && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).Sum(x => x.Amount).ToString();
                ViewBag.empPaymentByMonth = empPaymentByMonth;
                //Bu yıl yapılan ödeme
                var empPaymentByYear = db.Payments.Where(x => x.EmployeeId == id && x.Date.Value.Year == DateTime.Now.Year).Sum(x => x.Amount).ToString();
                ViewBag.empPaymentByYear = empPaymentByYear;

                var degerler = db.Employees.Where(x => x.EmployeeId == id).Include("Permissions").Include("Agis").Include("Districts").ToList();

                return View(degerler);
            }

        }
    }
}