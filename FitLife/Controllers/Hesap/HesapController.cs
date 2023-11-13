using FitLife.Firebase;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Firebase.Auth.Providers;

namespace FitLife.Controllers.Hesap
{
	public class HesapController : Controller
	{
		private readonly FirebaseAuthService _firebaseAuthService;

		FirebaseAuthProvider auth;

		public HesapController(FirebaseAuthService firebaseAuthService)
		{
			_firebaseAuthService = firebaseAuthService;
		}

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

	}
}
