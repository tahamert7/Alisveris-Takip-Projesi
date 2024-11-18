using AlisverisTakipProjesi.Models;
using AlisverisTakipProjesi.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace AlisverisTakipProjesi.Controllers
{
    [Authorize]
    public class SepetController : Controller
    {
        OnlineStoreDBEntities1 db = new OnlineStoreDBEntities1();

        // GET: Sepet
        public ActionResult Index()
        {
            int kullaniciID = Convert.ToInt32(Session["id"]);

            var sepet = db.Sepet.Where(x => x.KullaniciID == kullaniciID).ToList();
            var urunler = db.Urunler.ToList();

            var urunSepet = sepet.Select(x => new SepetViewModel
            {
                sepet = new Sepet
                {
                    Adet = x.Adet,
                    KullaniciID = kullaniciID,
                    TotalFiyat = x.TotalFiyat,
                    UrunID = x.UrunID,
                },
                urun = urunler.Where(u => u.urunID == x.UrunID).Select(u => new Urunler
                {
                    urunID = u.urunID,
                    aciklama = u.aciklama,
                    fiyat = u.fiyat,
                    resim = u.resim,
                    urunAdi = u.urunAdi,
                }).ToList()
            }).ToList();

            return View(urunSepet);
        }

        [HttpPost]
        public JsonResult AdetArttir(int urunId)
        {
            var sepetItem = db.Sepet.FirstOrDefault(x => x.UrunID == urunId);
            var urun = db.Urunler.FirstOrDefault(x=>x.urunID == urunId);
            if (sepetItem != null)
            {
                sepetItem.Adet++;
                sepetItem.TotalFiyat = urun.fiyat * sepetItem.Adet;
            }

            db.SaveChanges();

            return Json(new { success = true, totalFiyat = db.Sepet.Sum(x => x.TotalFiyat) });
        }

        [HttpPost]
        public JsonResult AdetDusur(int urunId)
        {
            var urun = db.Urunler.FirstOrDefault(x => x.urunID == urunId);

            var sepetItem = db.Sepet.FirstOrDefault(x => x.UrunID == urunId);
            if (sepetItem != null && sepetItem.Adet > 1)
            {
                sepetItem.Adet--;
                sepetItem.TotalFiyat = urun.fiyat * sepetItem.Adet;
            }
            db.SaveChanges();

            return Json(new { success = true, totalFiyat = db.Sepet.Sum(x => x.TotalFiyat) });
        }

        public ActionResult Sil(int id)
        {
            var urun = db.Sepet.FirstOrDefault(x=>x.UrunID == id);
            db.Sepet.Remove(urun);
            db.SaveChanges();

            return RedirectToAction("Index","Sepet");
        }

        public ActionResult Onayla()
        {
            int kullaniciID = Convert.ToInt32(Session["id"]);

            var sepet  = db.Sepet.Where(x=>x.KullaniciID == kullaniciID).ToList();

            var sepetUrun = sepet.Select(x => new UrunSatislari
            {
                miktar = x.Adet,
                musteriID = kullaniciID,
                toplamFiyat = x.TotalFiyat,
                urunID = (int)x.UrunID,
            }).ToList();

            foreach (var item in sepetUrun)
            {
                db.UrunSatislari.Add(item);
            }
            foreach (var item in sepet)
            {
                db.Sepet.Remove(item);
            }

            db.SaveChanges();


            return RedirectToAction("Index", "Sepet");
        }
    }
}