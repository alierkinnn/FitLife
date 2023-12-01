using FitLife.Models;
using FitLife.Repositories.Concretes;
using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Controllers.Danisan
{
    public class DanisanController : Controller
	{
        private readonly IDanisanService _danisanService;
        private readonly IMesajService _mesajService;

        public DanisanController(IDanisanService danisanService, IMesajService mesajService)
        {
            this._danisanService = danisanService;
            this._mesajService = mesajService;
        }

        public IActionResult Index()
		{
			return View();
		}


        [HttpGet]
        public IActionResult DanisanProfilimiGuncelle()
        {
            var id = HttpContext.Session.GetString("Id");

            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            FitLife.Models.Danisan model = _danisanService.IdyeGoreDanisanGetir(id);

            model.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DanisanProfilimiGuncelle(FitLife.Models.Danisan model)
        {
            model.Id = HttpContext.Session.GetString("Id");
            try
            {
                if (await _danisanService.DanisanProfilimiGuncelle(model))
                {
                    // Danışan başarıyla eklendi
                    TempData["Uyari"] = "Danışan başarıyla güncellenmiştir";
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda gerekli loglama veya başka işlemler yapılabilir
                ModelState.AddModelError("Hata", $"Bir hata oluştu: {ex.Message}");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult IlerlemeKayitlarim()
        {
            var id = HttpContext.Session.GetString("Id");

            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<FitLife.Models.IlerlemeKaydi> model = _danisanService.IdyeGoreIlerlemeKayitlariniGetir(id);

            return View(model);
        }

        [HttpGet]
        public IActionResult IlerlemeKaydiEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IlerlemeKaydiEkle(IlerlemeKaydi model)
        {
            model.DanisanId = HttpContext.Session.GetString("Id");
            if (_danisanService.IlerlemeKaydiEkle(model))
            {

                return RedirectToAction("IlerlemeKayitlarim");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult IlerlemeKaydiSil(string id)
        {
            try
            {
                if (_danisanService.IlerlemeKaydiSil(id))
                {
                    // Danışan başarıyla eklendi
                    TempData["Uyari"] = "Kayıt başarıyla silinmiştir";
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda gerekli loglama veya başka işlemler yapılabilir
                ModelState.AddModelError("Hata", $"Bir hata oluştu: {ex.Message}");
            }
            return RedirectToAction("IlerlemeKayitlarim");
        }

        [HttpGet]
        public IActionResult MesajlarimiGoruntule()
        {
            //servisten idye gore danisani getir ordan antrenoridyi cek
            var danisanId = HttpContext.Session.GetString("Id");
            FitLife.Models.Danisan danisan = _danisanService.IdyeGoreDanisanGetir(danisanId);

            MesajModel mesajModel = new MesajModel()
            {
                DanisanId = danisanId,
                AntrenorId = danisan.AntrenorId   
            };

            List<MesajModel> model = _mesajService.MesajlarimiGoruntule(mesajModel);

            if (model == null)
            {
                model = new List<MesajModel>();
            }

            ViewBag.Mesajlar = model; // Mesajları ViewBag ile View'e gönder

            return View();
        }

        [HttpPost]
        public IActionResult MesajGonder(MesajModel model)
        {
            var danisanId = HttpContext.Session.GetString("Id");
            FitLife.Models.Danisan danisan = _danisanService.IdyeGoreDanisanGetir(danisanId);

            model.AntrenorId = danisan.AntrenorId;
            model.DanisanId = danisanId;
            model.Gonderen = "Danisan";

            // antrenorServicede olan MesajGonder metodunu çağır
            bool mesajGonderildi = _mesajService.MesajGonder(model);

            if (mesajGonderildi)
            {
                // Mesaj başarıyla gönderildiyse sayfayı MesajlarimiGoruntule action'ına yönlendir
                return RedirectToAction("MesajlarimiGoruntule");
            }
            else
            {
                // Mesaj gönderilemedi, kullanıcıya uyari ver
                ViewBag.UyariMesaji = "Mesaj gönderilemedi. Lütfen tekrar deneyin.";
                // veya uygun view'e yönlendirme yapabilirsiniz
                return RedirectToAction("MesajlarimiGoruntule");
            }
        }

    }
}
