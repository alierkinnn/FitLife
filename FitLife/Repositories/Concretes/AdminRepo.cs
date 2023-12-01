using System.Net;
using FirebaseAdmin.Auth;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;
using Newtonsoft.Json;
using Firebase.Database.Query;
using FitLife.Services.Abstracts;

namespace FitLife.Repositories.Concretes
{
    public class AdminRepo : IAdminRepo
    {

        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
            BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
        };

        

        public AdminRepo(IHesapService hesapService)
        {
            client = new FireSharp.FirebaseClient(config);
			this.hesapService = hesapService;
		}
		IFirebaseClient client;
		private readonly IHesapService hesapService;

		public async Task<bool> DanisanEkle(Danisan model)
		{
			try
			{
				model.Sifre = hesapService.HashPassword(model.Sifre);
				// Firebase Authentication'ta kullanıcıyı oluştur
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				var user = await auth.CreateUserAsync(new UserRecordArgs()
				{
					Email = model.Eposta,
					Password = model.Sifre,
				});

				// Kullanıcı Firebase Authentication'a başarıyla eklendiyse devam et
				if (user != null)
				{
					try
					{
						// Firebase Authentication'tan alınan UID ile kaydı Realtime Database'e ekleyin
						var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);

						await firebaseClient
							.Child("Danisanlar")
							.Child(user.Uid) // Kullanıcının UID'sini kullanarak kaydı ekleyin
							.PutAsync(model);

						// Her iki aşama da başarılı oldu, işlem tamamlandı
						return true;
					}
					catch (Exception ex)
					{
						// Realtime Database'e kayıt eklerken bir hata olursa Firebase Authentication'tan kullanıcıyı sil
						await auth.DeleteUserAsync(user.Uid);
						return false;
					}
				}

				// Firebase Authentication'a kullanıcı eklenemediyse
				return false;
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
		}



		public async Task<bool> DanisanGuncelle(Danisan model)
		{
			try
			{
				model.Sifre = hesapService.HashPassword(model.Sifre);

				// Firebase Authentication'ta kullanıcıyı güncelle
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				var user = await auth.GetUserAsync(model.Id);

				if (user != null)
				{
					// Firebase Authentication kullanıcısını güncelle
					var updateUserArgs = new UserRecordArgs
					{
						Uid = user.Uid,
						Email = model.Eposta,
						// Diğer güncellenecek alanları ekleyin
					};
					await auth.UpdateUserAsync(updateUserArgs);

					// Realtime Database'de güncelleme yap
					var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);

					await firebaseClient
						.Child("Danisanlar")
						.Child(user.Uid) // Kullanıcının UID'sini kullanarak kaydı güncelle
						.PutAsync(model);

					return true; // Her iki işlem de başarılı oldu
				}

				return false; // Kullanıcı bulunamadı
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
		}

		public async Task<bool> DanisanSil(string danisanId)
		{
			try
			{
				// Firebase Authentication'tan kullanıcıyı sil
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				await auth.DeleteUserAsync(danisanId);

				// Realtime Database'den kaydı sil
				var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);
				await firebaseClient.Child("Danisanlar").Child(danisanId).DeleteAsync();

				return true;
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
			catch (Firebase.Database.FirebaseException ex)
			{
				// Realtime Database işleminde bir hata olursa
				return false;
			}
			catch (Exception ex)
			{
				// Diğer olası hataları kontrol etmek için genel bir Exception catch bloğu
				return false;
			}
		}


		public Danisan IdyeGoreDanisanGetir(string id)
		{
            FirebaseResponse response = client.Get("Danisanlar/" + id);
            Danisan danisan = JsonConvert.DeserializeObject<Danisan>(response.Body);
            return danisan;
		}

		public List<Danisan> TumDanisanlariListele()
        {
            FirebaseResponse response = client.Get("Danisanlar/");
            var result = response.Body;
            var data = JsonConvert.DeserializeObject<Dictionary<string, Danisan>>(result);

			if(data != null)
			{
				var list = new List<Danisan>();
				foreach (var item in data)
				{
					Danisan danisan = item.Value;
					danisan.Id = item.Key;
					list.Add(danisan);
				}
				return list;
			}
			else
			{
				return null;
			}
        }

		public async Task<bool> AntrenorEkle(Antrenor model)
		{
			try
			{
				model.Sifre = hesapService.HashPassword(model.Sifre);
				// Firebase Authentication'ta kullanıcıyı oluştur
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				var user = await auth.CreateUserAsync(new UserRecordArgs()
				{
					Email = model.Eposta,
					Password = model.Sifre,
				});

				// Kullanıcı Firebase Authentication'a başarıyla eklendiyse devam et
				if (user != null)
				{
					try
					{
						// Firebase Authentication'tan alınan UID ile kaydı Realtime Database'e ekleyin
						var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);

						await firebaseClient
							.Child("Antrenorler")
							.Child(user.Uid) // Kullanıcının UID'sini kullanarak kaydı ekleyin
							.PutAsync(model);

						// Her iki aşama da başarılı oldu, işlem tamamlandı
						return true;
					}
					catch (Exception ex)
					{
						// Realtime Database'e kayıt eklerken bir hata olursa Firebase Authentication'tan kullanıcıyı sil
						await auth.DeleteUserAsync(user.Uid);
						return false;
					}
				}

				// Firebase Authentication'a kullanıcı eklenemediyse
				return false;
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
		}

		public async Task<bool> AntrenorGuncelle(Antrenor model)
		{
			try
			{
				model.Sifre = hesapService.HashPassword(model.Sifre);
				// Firebase Authentication'ta kullanıcıyı güncelle
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				var user = await auth.GetUserAsync(model.Id);

				if (user != null)
				{
					// Firebase Authentication kullanıcısını güncelle
					var updateUserArgs = new UserRecordArgs
					{
						Uid = user.Uid,
						Email = model.Eposta,
						// Diğer güncellenecek alanları ekleyin
					};
					await auth.UpdateUserAsync(updateUserArgs);

					// Realtime Database'de güncelleme yap
					var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);

					await firebaseClient
						.Child("Antrenorler")
						.Child(user.Uid) // Kullanıcının UID'sini kullanarak kaydı güncelle
						.PutAsync(model);

					return true; // Her iki işlem de başarılı oldu
				}

				return false; // Kullanıcı bulunamadı
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
		}

		public async Task<bool> AntrenorSil(string id)
		{
			try
			{
				// Firebase Authentication'tan kullanıcıyı sil
				var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
				await auth.DeleteUserAsync(id);

				// Realtime Database'den kaydı sil
				var firebaseClient = new Firebase.Database.FirebaseClient(config.BasePath);
				await firebaseClient.Child("Antrenorler").Child(id).DeleteAsync();

				return true;
			}
			catch (FirebaseAuthException ex)
			{
				// Firebase Authentication işleminde bir hata olursa
				return false;
			}
			catch (Firebase.Database.FirebaseException ex)
			{
				// Realtime Database işleminde bir hata olursa
				return false;
			}
			catch (Exception ex)
			{
				// Diğer olası hataları kontrol etmek için genel bir Exception catch bloğu
				return false;
			}
		}

		public Antrenor IdyeGoreAntrenorGetir(string id)
		{
			FirebaseResponse response = client.Get("Antrenorler/" + id);
			Antrenor antrenor = JsonConvert.DeserializeObject<Antrenor>(response.Body);
			return antrenor;
		}

		public List<Antrenor> TumAntrenorleriListele()
		{
			FirebaseResponse response = client.Get("Antrenorler/");
			var result = response.Body;
			var data = JsonConvert.DeserializeObject<Dictionary<string, Antrenor>>(result);

			if (data != null)
			{
				var list = new List<Antrenor>();
				foreach (var item in data)
				{
					Antrenor antrenor = item.Value;
					antrenor.Id = item.Key;
					list.Add(antrenor);
				}
				return list;
			}
			else
			{
				return null;
			}
		}
	}
}
