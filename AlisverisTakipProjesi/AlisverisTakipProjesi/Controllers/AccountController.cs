using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using AlisverisTakipProjesi.Models;

namespace AlisverisTakipProjesi.Controllers
{
    public class AccountController : Controller
    {
        OnlineStoreDBEntities1 db = new OnlineStoreDBEntities1();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanicilar p)
        {
            var bilgiler = db.Kullanicilar.FirstOrDefault(x => x.kullaniciAdi == p.kullaniciAdi && x.sifre == p.sifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.kullaniciAdi, false);
                Session["Kullanici Adi"] = bilgiler.kullaniciAdi.ToString();
                Session["Ad"] = bilgiler.ad.ToString();
                Session["soyad"] = bilgiler.soyad.ToString();
                Session["id"] = bilgiler.kullaniciID.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı veya Şifre Hatalı";
            }
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Kullanicilar data)
        {
            if (ModelState.IsValid)
            {
                data.rol = "U";
                db.Kullanicilar.Add(data);
                db.SaveChanges();
                ViewBag.Message = "Kayıt başarılı!";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Error = "Kayıt başarısız. Lütfen bilgileri kontrol edin.";
            return View(data);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}
