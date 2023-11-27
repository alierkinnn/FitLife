using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;
using Newtonsoft.Json;

namespace FitLife.Repositories.Concretes
{
	public class HesapRepo : IHesapRepo
	{
		IFirebaseConfig config = new FirebaseConfig
		{
			AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
			BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
		};

		public HesapRepo()
		{
			client = new FireSharp.FirebaseClient(config);
		}

		IFirebaseClient client;

		public GirisYapModel GirisYap(GirisYapModel model)
		{
			// Firebase'den tüm kullanıcıları al
			FirebaseResponse response = client.Get("Kullanicilar/");

			if (response.Body != "null")
			{
				// Tüm kullanıcıları içeren bir sözlük elde et
				Dictionary<string, GirisYapModel> users = JsonConvert.DeserializeObject<Dictionary<string, GirisYapModel>>(response.Body);

				// E-posta ile eşleşen kullanıcıyı bul
				var userKeyValuePair = users.FirstOrDefault(u => u.Value.Eposta == model.Eposta);

				if (userKeyValuePair.Key != null)
				{
					// Eğer e-posta ile eşleşen bir kullanıcı bulunduysa
					GirisYapModel firebaseUser = userKeyValuePair.Value;

					// Şifre kontrolü
					if (firebaseUser.Sifre == model.Sifre)
					{
						// Giriş başarılı
						return firebaseUser;
					}
					else
					{
						// Şifre yanlış
						return null;
					}
				}
			}

			// Kullanıcı bulunamadı
			return null;
		}

	}
}
