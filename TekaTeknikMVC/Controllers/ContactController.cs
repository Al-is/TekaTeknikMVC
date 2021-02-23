using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var degerler = db.Contacts.Where(x => x.EmployeeId == id).ToList();
                var empName = db.Employees.Where(x => x.EmployeeId == id).Select(y => y.FirstName)
                    .FirstOrDefault();
                ViewBag.empName = empName;
                return View(degerler);
            }

        }
        [HttpGet]
        public ActionResult ContactAdd(int id)
        {


            return View(new Contacts { EmployeeId = id });
        }

        [HttpPost]
        public ActionResult ContactAdd(Contacts con)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Employees.Find(con.EmployeeId);
                db.Contacts.Add(con);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = con.EmployeeId });
            }
        }

        public ActionResult ContactDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {

                var con = db.Contacts.Find(id);
                string cont = con.EmployeeId.ToString();

                db.Contacts.Remove(con);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = cont });
            }
        }
        public ActionResult ContactGet(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var con = db.Contacts.Find(id);

                return View("ContactGet", con);
            }

        }

        public ActionResult ContactUpdate(Contacts con)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var e = db.Contacts.Find(con.ContactId);
                string id = e.EmployeeId.ToString();
                e.FirstName = con.FirstName;
                e.LastName = con.LastName;
                e.Phone = con.Phone;
                e.Relation = con.Relation;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = e.EmployeeId });

            }

        }
    }
}