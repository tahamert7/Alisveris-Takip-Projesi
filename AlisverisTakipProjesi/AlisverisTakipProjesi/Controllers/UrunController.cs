using AlisverisTakipProjesi.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlisverisTakipProjesi.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
        OnlineStoreDBEntities1 db = new OnlineStoreDBEntities1();
        [Authorize]
        public ActionResult Index(string ara)
        {
            var list = db.Urunler.ToList();

            var urun = list.Select(x => new Urunler
            {
                aciklama = x.aciklama,
                durum = x.durum,
                fiyat = x.fiyat,
                kategoriID = x.kategoriID,
                Kategoriler = x.Kategoriler,
                populer = x.populer,
                resim = x.resim,
                stok = x.stok,
                urunAdi = x.urunAdi,
                urunID = x.urunID,
                UrunSatislari = x.UrunSatislari
            }).ToList();

            if (!string.IsNullOrEmpty(ara))
            {
                list = list.Where(x => x.urunAdi.Contains(ara) || x.aciklama.Contains(ara)).ToList();
            }
            return View(urun);
        }

        public ActionResult SepetEkle(int id)
        {
            var urun = db.Urunler.FirstOrDefault(x=>x.urunID == id);

            int kullaniciID = Convert.ToInt32(Session["id"]);

            var sepetUrun = new Sepet
            {
                Adet = 1,
                UrunID = id,
                TotalFiyat = urun.fiyat,
                KullaniciID = kullaniciID,
            };
            db.Sepet.Add(sepetUrun);
            db.SaveChanges();

            return RedirectToAction("Index", "Urun");
        }

        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.Kategoriler.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.kategoriAdi,
                                               Value = x.kategoriID.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(Urunler data, HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/Image" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            data.resim = File.FileName.ToString();
            db.Urunler.Add(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            var urun = db.Urunler.Where(x => x.urunID == id).FirstOrDefault();
            db.Urunler.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Guncelle(int id)
        {
                var guncelle= db.Urunler.Where(x => x.urunID == id).FirstOrDefault();
                List<SelectListItem> deger1 = (from x in db.Kategoriler.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.kategoriAdi,
                                               Value = x.kategoriID.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View(guncelle);
        }
        [HttpPost]
        //public ActionResult Guncelle(Urunler Model, HttpPostedFileBase File)
        //{
        //    var urun = db.Urunler.Find(Model.urunID);
        //    if (File==null)
        //    {
        //        urun.urunAdi = Model.urunAdi;
        //        urun.aciklama = Model.aciklama;
        //        urun.fiyat = Model.fiyat;
        //        urun.stok = Model.stok;
        //        urun.populer = Model.populer;
        //        urun.Kategoriler = Model.Kategoriler;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");

        //    }
        //    else
        //    {
        //        urun.resim = File.FileName.ToString();
        //        urun.urunAdi = Model.urunAdi;
        //        urun.aciklama = Model.aciklama;
        //        urun.fiyat = Model.fiyat;
        //        urun.stok = Model.stok;
        //        urun.populer = Model.populer;
        //        urun.Kategoriler = Model.Kategoriler;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //}
        public ActionResult Guncelle(int id, Urunler model, HttpPostedFileBase file)
        {
            var urun = db.Urunler.Find(id);
            if (urun == null)
            {
                return HttpNotFound();
            }

            if (file == null)
            {
                urun.urunAdi = model.urunAdi;
                urun.aciklama = model.aciklama;
                urun.fiyat = model.fiyat;
                urun.stok = model.stok;
                urun.populer = model.populer;
                urun.Kategoriler = model.Kategoriler;
            }
            else
            {
                urun.resim = file.FileName.ToString();
                urun.urunAdi = model.urunAdi;
                urun.aciklama = model.aciklama;
                urun.fiyat = model.fiyat;
                urun.stok = model.stok;
                urun.populer = model.populer;
                urun.Kategoriler = model.Kategoriler;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}