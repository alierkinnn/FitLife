using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Microsoft.AspNetCore.Mvc;
using FitLife.Models;

namespace FitLife.Controllers.Antrenor
{
	public class AntrenorController : Controller
	{
		private readonly IAntrenorService _antrenorService;
		private readonly IMesajService _mesajService;

		public AntrenorController(IAntrenorService antrenorService, IMesajService mesajService)
		{
			this._antrenorService = antrenorService;
			this._mesajService = mesajService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AntrenorProfilimiGuncelle()
		{
			var id = HttpContext.Session.GetString("Id");

			if (id == null)
			{
				return RedirectToAction("Login", "Account");
			}

			FitLife.Models.Antrenor model = _antrenorService.IdyeGoreAntrenorGetir(id);

			model.Id = id;

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AntrenorProfilimiGuncelle(FitLife.Models.Antrenor model)
		{
			model.Id = HttpContext.Session.GetString("Id");
			try
			{
				if (await _antrenorService.AntrenorProfilimiGuncelle(model))
				{
					// Danışan başarıyla eklendi
					TempData["Uyari"] = "Antrenör başarıyla güncellenmiştir";
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
		public IActionResult DanisanlarimiListele()
		{
			var id = HttpContext.Session.GetString("Id");
			List<FitLife.Models.Danisan> model = _antrenorService.DanisanlarimiListele(id);

			if (model == null)
			{
				model = new List<FitLife.Models.Danisan>(); // Eğer model null ise boş bir liste oluştur
			}

			return View(model);
		}

		[HttpGet]
        public IActionResult DanisanDetaylariniGoster()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MesajlarimiGoruntule(string danisanId)
        {
            MesajModel mesajModel = new MesajModel()
            {
                AntrenorId = HttpContext.Session.GetString("Id"),
                DanisanId = danisanId
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
            var antrenorId = HttpContext.Session.GetString("Id");
            model.AntrenorId = antrenorId;
			model.Gonderen = "Antrenor";

            // antrenorServicede olan MesajGonder metodunu çağır
            bool mesajGonderildi = _mesajService.MesajGonder(model);

            if (mesajGonderildi)
            {
                // Mesaj başarıyla gönderildiyse sayfayı MesajlarimiGoruntule action'ına yönlendir
                return RedirectToAction("MesajlarimiGoruntule", new { danisanId = model.DanisanId });
            }
            else
            {
                // Mesaj gönderilemedi, kullanıcıya uyari ver
                ViewBag.UyariMesaji = "Mesaj gönderilemedi. Lütfen tekrar deneyin.";
                // veya uygun view'e yönlendirme yapabilirsiniz
                return RedirectToAction("MesajlarimiGoruntule", new { danisanId = model.DanisanId });
            }
        }

        [HttpGet]
        public IActionResult EgzersizProgramlariniListele()
        {
            var id = HttpContext.Session.GetString("Id");
            List<EgzersizProgrami> model = _antrenorService.EgzersizProgramlariniListele(id);

            if (model == null)
            {
                model = new List<EgzersizProgrami>(); 
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EgzersizProgramiEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EgzersizProgramiEkle(EgzersizProgrami model)
        {
            var antrenorId = HttpContext.Session.GetString("Id");
			model.AntrenorId = antrenorId;

            if (_antrenorService.EgzersizProgramiEkle(model))
            {
                return RedirectToAction("EgzersizProgramlariniListele");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult BeslenmeProgramlariniListele()
        {
            var id = HttpContext.Session.GetString("Id");
            List<BeslenmeProgrami> model = _antrenorService.BeslenmeProgramlariniListele(id);

            if (model == null)
            {
                model = new List<BeslenmeProgrami>();
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult BeslenmeProgramiEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BeslenmeProgramiEkle(BeslenmeProgrami model)
        {
            var antrenorId = HttpContext.Session.GetString("Id");
            model.AntrenorId = antrenorId;

            if (_antrenorService.BeslenmeProgramiEkle(model))
            {
                return RedirectToAction("BeslenmeProgramlariniListele");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
		public IActionResult BeslenmeProgramiSil(string id)
		{
            if (_antrenorService.BeslenmeProgramiSil(id))
            {
                return RedirectToAction("BeslenmeProgramlariniListele");
            }
            else
            {
				ViewBag.SilmeHataMesaji = "Silme işlemi başarısız oldu";
				return View("BeslenmeProgramlariniListele");
			}
			
		}

        [HttpGet]
        public IActionResult BeslenmeProgramiGuncelle(string id)
        {
            BeslenmeProgrami beslenmeProgrami = _antrenorService.IdyeGoreBeslenmeProgramiGetir(id);

            return View(beslenmeProgrami);
		}

		[HttpPost]
		public IActionResult BeslenmeProgramiGuncelle(BeslenmeProgrami model)
		{
            model.AntrenorId = HttpContext.Session.GetString("Id");

			if (_antrenorService.BeslenmeProgramiGuncelle(model))
            {
                return RedirectToAction("BeslenmeProgramlariniListele");
            }
            else
            {
                return View();
            }

		}
	}
}
