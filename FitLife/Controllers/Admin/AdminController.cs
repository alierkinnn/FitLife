using FitLife.Models;
using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Controllers.Admin
{
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
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DanisanEkle(Danisan model)
        {
            model.Rol = "danisan";
            adminService.DanisanEkle(model);
			return RedirectToAction("TumDanisanlariListele");
		}

		[HttpGet]
		public IActionResult DanisanGuncelle(string id)
		{
			Danisan model = adminService.IdyeGoreDanisanGetir(id);
			model.Id = id;
			return View(model);
		}

        [HttpPost]
		public IActionResult DanisanGuncelle(Danisan model)
		{
            adminService.DanisanGuncelle(model);
			return RedirectToAction("TumDanisanlariListele");
		}

        public IActionResult DanisanSil(string id)
        {
            adminService.DanisanSil(id);
            return RedirectToAction("TumDanisanlariListele");
        }

	}
}
