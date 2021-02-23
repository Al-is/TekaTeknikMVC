using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                //Toplam Çalışanlar
                var EmployeeAll = db.Employees.Count();
                ViewBag.EmployeeAll = EmployeeAll;
                //Makineler departmanında çalışanlar
                var departmentById = db.Employees.Count(x => x.DepartmentId == db.Departments.Where(z => z.DepartmentId == 2).Select(y => y.DepartmentId).FirstOrDefault());
                ViewBag.departmentById = departmentById;
                //Bugün izinli çalışanlar
                var permissionByDay = db.Permissions.Where(x => x.StartDay.Value.Day <= DateTime.Now.Day && x.EndDay.Value.Day >= DateTime.Now.Day).Select(y => y.PermissionId).Count();
                ViewBag.permissionByDay = permissionByDay;
                //İzin günü yaklaşan çalışanlar
                var permissionByDayEmp = db.Permissions.Where(x => x.StartDay.Value.Day >= DateTime.Now.Day).Select(y => y.PermissionId).Count();
                ViewBag.permissionByDayEmp = permissionByDayEmp;
                //İzin günü bitecek çalışanlar
                var permissionEndDayEmp = db.Permissions.Where(x => x.EndDay.Value.Day >= DateTime.Now.Day).Select(y => y.PermissionId).Count();
                ViewBag.permissionEndDayEmp = permissionEndDayEmp;
                //Günü yaklaşan ödemeler
                var paymentByDay = db.Payments.Where(x => x.Date.Value.Day >= DateTime.Now.Day && x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month).Select(y => y.PaymentId).Count();
                ViewBag.paymentByDay = paymentByDay;


            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}