using Microsoft.AspNetCore.Mvc;
using FitLife.Models;
using FitLife.Services.Abstracts;
using FirebaseAdmin.Auth;
using System.Text;
using System.Security.Cryptography;
using FireSharp.Config;
using FireSharp.Interfaces;
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using FitLife.Services.Concretes;

namespace FitLife.Controllers.Hesap
{
	public class HesapController : Controller
	{

		IFirebaseConfig config = new FirebaseConfig
		{
			AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
			BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
		};


		private readonly IAdminService _adminService;
		private readonly IHesapService _hesapService;

		public HesapController(IHesapService hesapService)
		{
			client = new FireSharp.FirebaseClient(config);
			this._hesapService = hesapService;
		}
		IFirebaseClient client;

		//public HesapController(IHesapService hesapService, IAdminService adminService)
		//{
		//	this._hesapService = hesapService;
		//	this._adminService = adminService;
		//}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GirisYap()
		{
			return View();
		}

		public IActionResult KayitOl()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> KayitOl(KayitOlModel model)
		{
			try
			{
				model.Sifre = _hesapService.HashPassword(model.Sifre);

				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;

				var user = await auth.CreateUserAsync(new UserRecordArgs()
				{
					Email = model.Eposta,
					Password = model.Sifre,
				});


				var firebaseClient = new FirebaseClient(config.BasePath);
				if(model.Rol == "danisan")
				{
					await firebaseClient
				   .Child("Danisanlar") // Ekleme yapmak istediğiniz düğüm
				   .Child(user.Uid) // Belirli bir ID kullanarak veriyi ekleyin
				   .PutAsync(model);
				}
				else
				{
					await firebaseClient
				   .Child("Antrenorler") // Ekleme yapmak istediğiniz düğüm
				   .Child(user.Uid) // Belirli bir ID kullanarak veriyi ekleyin
				   .PutAsync(model);
				}


				return RedirectToAction("GirisYap", "Hesap");
			}
			catch (FirebaseAuthException ex)
			{
				return BadRequest(new { Message = $"Kayıt olma hatası: {ex.Message}" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> GirisYap(GirisYapModel model)
		{

			try
			{
				model.Sifre = _hesapService.HashPassword(model.Sifre);

				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				
				var user = await auth.GetUserByEmailAsync(model.Eposta);

				if (user != null)
				{
					string uid = user.Uid;

					// Danisanlar tablosundan veriyi al
					FirebaseResponse danisanResponse = client.Get("Danisanlar/" + uid);
					FitLife.Models.Danisan danisan = null;
					if (danisanResponse != null && danisanResponse.Body != "null")
					{
						danisan = JsonConvert.DeserializeObject<FitLife.Models.Danisan>(danisanResponse.Body);
					}

					// Eğer danisan tablosunda kayıt bulunmazsa, Adminler tablosundan veriyi al
					if (danisan == null)
					{
						FirebaseResponse adminResponse = client.Get("Adminler/" + uid);
						Yonetici yonetici = null;
						if (adminResponse != null && adminResponse.Body != "null")
						{
							yonetici = JsonConvert.DeserializeObject<Yonetici>(adminResponse.Body);
						}

						// Eğer admin tablosunda kayıt bulunmazsa, Antrenorler tablosundan veriyi al
						if (yonetici == null)
						{
							FirebaseResponse antrenorResponse = client.Get("Antrenorler/" + uid);
							FitLife.Models.Antrenor antrenor = null;
							if (antrenorResponse != null && antrenorResponse.Body != "null")
							{
								antrenor = JsonConvert.DeserializeObject<FitLife.Models.Antrenor>(antrenorResponse.Body);
							}

							// Şimdi danisan, admin ve antrenor değişkenlerinde ilgili kayıtlar bulunmaktadır.
							// İhtiyacınıza göre bu kayıtları kullanabilirsiniz.
							if(antrenor.Sifre == model.Sifre)
							{
								var claims = new List<Claim>{
									new Claim(ClaimTypes.Email, model.Eposta)
								};
								var userIdentity = new ClaimsIdentity(claims, "Login");
								ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
								HttpContext.SignInAsync(principal).GetAwaiter().GetResult();

								HttpContext.Session.SetString("Id", uid);

								return RedirectToAction("Index", "Antrenor");
							}
							else
							{
								ModelState.AddModelError(string.Empty, "Şifre hatalı");
								return View();
							}
						}
						else
						{
							if(yonetici.Sifre == model.Sifre)
							{
								var claims = new List<Claim>{
									new Claim(ClaimTypes.Email, model.Eposta)
								};
								var userIdentity = new ClaimsIdentity(claims, "Login");
								ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
								HttpContext.SignInAsync(principal).GetAwaiter().GetResult();

								HttpContext.Session.SetString("Id", uid);

								return RedirectToAction("Index", "Admin");
							}
							else
							{
								ModelState.AddModelError(string.Empty, "Şifre hatalı");
								return View();
							}
						}
					}
					else
					{
						if(danisan.Sifre == model.Sifre)
						{
							var claims = new List<Claim>{
									new Claim(ClaimTypes.Email, model.Eposta)
								};
							var userIdentity = new ClaimsIdentity(claims, "Login");
							ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
							HttpContext.SignInAsync(principal).GetAwaiter().GetResult();

							HttpContext.Session.SetString("Id", uid);

							return RedirectToAction("Index", "Danisan");
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Şifre hatalı");
							return View();
						}
					}
				}

				else
				{
					ModelState.AddModelError(string.Empty, "Eposta hatalı");
					return View();
				}

			}
			catch (FirebaseAuthException ex)
			{
				return BadRequest(new { Message = $"Giriş yapma hatası: {ex.Message}" });
			}
		}



		//[HttpPost]
		//public IActionResult GirisYap(GirisYapModel model)
		//{
		//	GirisYapModel model1 = _hesapService.GirisYap(model);
		//	if(model1 != null)
		//	{
		//		if (model1.Rol == "Danisan")
		//		{
		//			return RedirectToAction("TumDanisanlariListele", "Admin");
		//		}

		//		else if (model1.Rol == "Antrenor")
		//		{
		//			return RedirectToAction("TumDanisanlariListele", "Admin");
		//		}

		//		else
		//		{
		//			return RedirectToAction("TumDanisanlariListele", "Admin");
		//		}
		//	}

		//	else
		//	{
		//		ModelState.AddModelError("Hata", "Belirtilen e-posta ve şifre ile bir kullanıcı bulunamadı.");
		//		return View(); 
		//	}

		//}

	}
}
