using System.Data;
using System.Reflection;
using FitLife.Models;
using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Controllers.Admin
{
	[Authorize]
	public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DanisanEkle()
        {
            return View();
        }

        public IActionResult TumDanisanlariListele()
        {
			List<Danisan> model = adminService.TumDanisanlariListele();

			if (model == null)
			{
				model = new List<Danisan>(); // Eğer model null ise boş bir liste oluştur
			}

			return View(model);
		}

		public IActionResult TumAntrenorleriListele()
		{
			List<Antrenor> model = adminService.TumAntrenorleriListele();

			if (model == null)
			{
				model = new List<Antrenor>(); // Eğer model null ise boş bir liste oluştur
			}

			return View(model);
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DanisanEkleAsync(Danisan model)
        {
			model.Rol = "Danisan";

			try
			{
				if (await adminService.DanisanEkle(model))
				{
					// Danışan başarıyla eklendi
					TempData["Uyari"] = "Danışan başarıyla eklenmiştir.";
					return RedirectToAction("TumDanisanlariListele");
				}
				else
				{
					// E-posta daha önce alınmış
					ModelState.AddModelError("Hata", "Bu e-posta adresi zaten kullanımda.");
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda gerekli loglama veya başka işlemler yapılabilir
				ModelState.AddModelError("Hata", $"Bir hata oluştu: {ex.Message}");
			}

			return View(); // Aynı sayfaya geri dön
		}

		[HttpGet]
		public IActionResult DanisanGuncelle(string id)
		{
			Danisan model = adminService.IdyeGoreDanisanGetir(id);
			model.Id = id;
			return View(model);
		}

		[HttpGet]
		public IActionResult AntrenorGuncelle(string id)
		{
			Antrenor model = adminService.IdyeGoreAntrenorGetir(id);
			model.Id = id;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DanisanGuncelle(Danisan model)
		{
			try
			{
				if (await adminService.DanisanGuncelle(model))
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
			return RedirectToAction("TumDanisanlariListele");
		}

		[HttpPost]
		public async Task<IActionResult> AntrenorGuncelle(Antrenor model)
		{
			try
			{
				if (await adminService.AntrenorGuncelle(model))
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
			return RedirectToAction("TumAntrenorleriListele");
		}

		[HttpGet]
        public async Task<IActionResult> DanisanSil(string id)
		{
			try
			{
				if (await adminService.DanisanSil(id))
				{
					// Danışan başarıyla eklendi
					TempData["Uyari"] = "Danışan başarıyla silinmiştir";
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda gerekli loglama veya başka işlemler yapılabilir
				ModelState.AddModelError("Hata", $"Bir hata oluştu: {ex.Message}");
			}
			return RedirectToAction("TumDanisanlariListele");
		}

		[HttpGet]
		public async Task<IActionResult> AntrenorSil(string id)
		{
			try
			{
				if (await adminService.AntrenorSil(id))
				{
					// Danışan başarıyla eklendi
					TempData["Uyari"] = "Danışan başarıyla silinmiştir";
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda gerekli loglama veya başka işlemler yapılabilir
				ModelState.AddModelError("Hata", $"Bir hata oluştu: {ex.Message}");
			}
			return RedirectToAction("TumDanisanlariListele");
		}

	}
}
