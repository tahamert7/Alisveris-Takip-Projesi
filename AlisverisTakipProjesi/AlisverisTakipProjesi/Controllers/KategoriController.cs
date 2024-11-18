using AlisverisTakipProjesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlisverisTakipProjesi.Controllers
{
    [Authorize]
    public class KategoriController : Controller
    {
        OnlineStoreDBEntities1 db = new OnlineStoreDBEntities1();

        // GET: Kategori
        public ActionResult Index()
        {
            var kategoriler = db.Kategoriler.ToList();

            var kat = kategoriler.Select(x=> new Kategoriler
            {
               aciklama = x.aciklama,
               kategoriAdi = x.kategoriAdi,
               kategoriID = x.kategoriID,
            }).ToList();
            
            return View(kat);
        }

        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Kategoriler kategori)
        {
            var kat = new Kategoriler
            {
                aciklama = kategori.aciklama,
                kategoriAdi = kategori.kategoriAdi,
            };

            db.Kategoriler.Add(kat);
            db.SaveChanges();

            return RedirectToAction("Index", "Kategori");
        }

        public ActionResult Sil(int id)
        {
            var silinecek = db.Kategoriler.FirstOrDefault(x=>x.kategoriID == id);

            db.Kategoriler.Remove(silinecek);
            db.SaveChanges();

            return RedirectToAction("Index", "Kategori");
        }

        public ActionResult Guncelle(int id) 
        {
            var kategori = db.Kategoriler.FirstOrDefault(x=>x.kategoriID==id);

            return View(kategori); 
        }

        [HttpPost]
        public ActionResult Guncelle(Kategoriler kategori, int id)
        {
            var kat = db.Kategoriler.FirstOrDefault(x=>x.kategoriID == id);

            kat.aciklama = kategori.aciklama;
            kat.kategoriAdi = kategori.kategoriAdi;

            db.SaveChanges();

            return RedirectToAction("Index", "Kategori");
        }
    }
}