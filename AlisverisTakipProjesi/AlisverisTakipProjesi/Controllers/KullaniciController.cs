using AlisverisTakipProjesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlisverisTakipProjesi.Controllers
{
    [Authorize]
    public class KullaniciController : Controller
    {
        OnlineStoreDBEntities1 db = new OnlineStoreDBEntities1();

        // GET: Kullanici
        public ActionResult Index()
        {
            var kullanicilar = db.Kullanicilar.Where(x=>x.rol == "U").ToList();

            var kullanici = kullanicilar.Select(x => new Kullanicilar
            {
                rol = x.rol,
                ad = x.ad,
                kullaniciAdi = x.kullaniciAdi,
                kullaniciID = x.kullaniciID,
                sifre = x.sifre,
                soyad = x.soyad,
            }).ToList();

            return View(kullanici);
        }

        public ActionResult Sil(int id)
        {
            var kullanici = db.Kullanicilar.FirstOrDefault(x=>x.kullaniciID == id);

            db.Kullanicilar.Remove(kullanici);
            db.SaveChanges();

            return RedirectToAction("Index", "Kullanici");
        }
        
        public ActionResult Guncelle(int id)
        {
            var kullanici = db.Kullanicilar.FirstOrDefault(x=>x.kullaniciID==id);
            return View(kullanici);
        }

        [HttpPost]
        public ActionResult Guncelle(Kullanicilar kullanici, int id)
        {
            var yeniKullanici = db.Kullanicilar.FirstOrDefault(x=>x.kullaniciID == id);

            yeniKullanici.kullaniciAdi = kullanici.kullaniciAdi;
            yeniKullanici.ad = kullanici.ad;
            yeniKullanici.soyad = kullanici.soyad;
            yeniKullanici.rol = kullanici.rol;
            yeniKullanici.sifre = kullanici.sifre;

            db.SaveChanges();

            return RedirectToAction("Guncelle", "Kullanici");
        }
    }
}