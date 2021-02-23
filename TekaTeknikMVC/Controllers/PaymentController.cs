using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TekaTeknikMVC.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult GetAll()
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {

                var degerler = db.Payments.Include("Employees").ToList();
                ////Bu ay toplam ödeme
                var PaymentByMonth =
                    db.Payments.Where(y => y.Date.Value.Month == DateTime.Now.Month && y.Date.Value.Year == DateTime.Now.Year).Sum(x => x.Amount).ToString();
                ViewBag.PaymentByMonth = " Bu ay yapılan ödeme: "+ PaymentByMonth;
                ////Bu yıl toplam ödeme
                var PaymentByYear =
                    db.Payments.Where(y => y.Date.Value.Year == DateTime.Now.Year).Sum(x => x.Amount).ToString();
                ViewBag.PaymentByYear = " Bu yıl yapılan ödeme: " + PaymentByYear;
                return View(degerler);
            }

        }
        [HttpPost]
        public ActionResult GetAll(Payments pay)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var getir = db.Payments.Include("Employees").Where(y => y.Date.Value.Month == pay.Month && y.Date.Value.Year == pay.Year).ToList();
                //Seçilen ayın ödemeleri
                var paymentByMonth = db.Payments.Where(y => y.Date.Value.Month == pay.Month && y.Date.Value.Year == pay.Year).Sum(x => x.Amount)
                    .ToString();
                ViewBag.paymentByMonth = "Seçilen ayın ödemeleri: " + paymentByMonth;
                //Seçilen Yılın ödemeleri
                var PaymentByYear = db.Payments.Where(y => y.Date.Value.Year == pay.Year).Sum(x => x.Amount)
                    .ToString();
                ViewBag.PaymentByYear = "Seçilen Yılın ödemeleri: " + PaymentByYear;
                ViewBag.date = "Seçilen Tarih : " + pay.Month.ToString() + " / " + pay.Year.ToString();
                return View(getir);
            }
        }

        public ActionResult Index(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                //Bu ay ki yapılan ödeme-personel ödeme detayı
                var totalPaymentByMonth = db.Payments.Where(x => x.EmployeeId == id && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year)
                    .Sum(p => p.Amount).ToString();
                ViewBag.totalPaymentByMonth = "Bu ay yapılan ödeme-personel ödeme detayı: " + totalPaymentByMonth;
                //Bu yıl yapılan odeme -personel ödeme detayı
                var totalPaymentByYear = db.Payments.Where(x => x.EmployeeId == id && x.Date.Value.Year == DateTime.Now.Year)
                    .Sum(p => p.Amount).ToString();
                ViewBag.totalPaymentByYear = "Bu yıl yapılan odeme -personel ödeme detayı: " + totalPaymentByYear;

                var degerler = db.Payments.Where(x => x.EmployeeId == id).ToList();

                var empName = db.Employees.Where(x => x.EmployeeId == id).Select(y => y.FirstName)
                    .FirstOrDefault();
                ViewBag.empName = empName;
                return View(degerler);

            }
        }
        [HttpGet]
        public ActionResult PaymentAdd(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
            }

            return View(new Payments { EmployeeId = id });
        }
        [HttpPost]
        public ActionResult PaymentAdd(Payments pay)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                db.Employees.Find(pay.EmployeeId);
                db.Payments.Add(pay);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = pay.EmployeeId });
            }
        }
        public ActionResult PaymentDelete(int id)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var pay = db.Payments.Find(id);
                string paym = pay.EmployeeId.ToString();
                db.Payments.Remove(pay);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = paym });
            }
        }
        public ActionResult PaymentGet(int id)
        {

            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var pay = db.Payments.Find(id);

                return View("PaymentGet", pay);
            }
        }
        public ActionResult PaymentUpdate(Payments pay)
        {
            using (TekaTeknikEntities db = new TekaTeknikEntities())
            {
                var e = db.Payments.Find(pay.PaymentId);
                string id = pay.EmployeeId.ToString();
                e.Amount = pay.Amount;
                e.PaymentType = pay.PaymentType;
                e.Date = pay.Date;
                e.Year = pay.Year;
                e.Month = pay.Month;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = e.EmployeeId });
            }
        }
    }
}