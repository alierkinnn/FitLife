using Firebase.Database.Query;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FitLife.Repositories.Concretes
{
	public class AntrenorRepo : IAntrenorRepo
	{
		FirebaseConfig config = new FirebaseConfig
		{
			AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
			BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
		};



		public AntrenorRepo()
		{
			client = new FireSharp.FirebaseClient(config);
		}
		IFirebaseClient client;
		public Antrenor IdyeGoreAntrenorGetir(string id)
		{
			FirebaseResponse response = client.Get("Antrenorler/" + id);
			Antrenor antrenor = JsonConvert.DeserializeObject<Antrenor>(response.Body);
			return antrenor;
		}

		public async Task<bool> AntrenorProfilimiGuncelle(Antrenor model)
		{
			try
			{
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

		public List<Danisan> DanisanlarimiListele(string antrenorId)
		{
			try
			{
				// Antrenör bilgisini çekme
				FirebaseResponse antrenorResponse = client.Get($"Antrenorler/{antrenorId}");
				Antrenor antrenor = antrenorResponse.ResultAs<Antrenor>();

				if (antrenor != null)
				{
					// Antrenörün danışan Id'lerini içeren düğümü çekme
					FirebaseResponse danisanIdListResponse = client.Get($"Antrenorler/{antrenorId}/DanisanIdListesi/Danisanlar");
					List<string> danisanIdListesi = danisanIdListResponse.ResultAs<List<string>>();

					if (danisanIdListesi != null)
					{
						// Her bir danışanın bilgisini çekme
						List<Danisan> danisanlar = new List<Danisan>();
						foreach (var danisanId in danisanIdListesi)
						{
							FirebaseResponse danisanResponse = client.Get($"Danisanlar/{danisanId}");
							Danisan danisan = danisanResponse.ResultAs<Danisan>();

							if (danisan != null)
							{
								danisanlar.Add(danisan);
							}
						}

						return danisanlar;
					}
				}

				// Eğer belirtilen Id'ye sahip antrenör veya danışan bulunamazsa veya başka bir hata oluşursa, null veya boş liste dönebilirsiniz.
				return null;
			}
			catch (Exception ex)
			{
				// Hata durumunda yapılacak işlemler
				Console.WriteLine($"Hata: {ex.Message}");
				return null;
			}
		}

		public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel)
		{
			FirebaseResponse response = client.Get("Mesajlar/");

			if (response.Body != "null") // Veritabanında mesajlar varsa
			{
				Dictionary<string, MesajModel> mesajlar = response.ResultAs<Dictionary<string, MesajModel>>();

				// Mesajları antrenor ve danışan ID'lerine göre filtreleme
				var filtrelenmisMesajlar = mesajlar
					.Where(m => m.Value.AntrenorId == mesajModel.AntrenorId && m.Value.DanisanId == mesajModel.DanisanId)
					.Select(m => m.Value)
					.ToList();

				return filtrelenmisMesajlar;
			}

			return new List<MesajModel>(); // Veritabanında mesajlar yoksa boş liste döndür
		}

        public bool MesajGonder(MesajModel model)
        {
            try
            {
                // Firebase Realtime Database'e yeni bir kayıt ekle
                PushResponse response = client.Push("Mesajlar/", model);

                if (response.Result != null && !string.IsNullOrEmpty(response.Result.name))
                {
                    // Kayıt başarıyla eklendi
                    return true;
                }
                else
                {
                    // Kayıt eklenemedi
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya uygun bir işlem yapabilirsiniz
                return false;
            }
        }

        public bool EgzersizProgramiEkle(EgzersizProgrami model)
        {
            try
            {
                PushResponse response = client.Push("EgzersizProgramlari/", model);

                if (response.Result != null && !string.IsNullOrEmpty(response.Result.name))
                {
                    // Kayıt başarıyla eklendi
                    return true;
                }
                else
                {
                    // Kayıt eklenemedi
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya uygun bir işlem yapabilirsiniz
                return false;
            }
        }

        public List<EgzersizProgrami> EgzersizProgramlariniListele(string id)
        {
			return null;
        }

		public List<BeslenmeProgrami> BeslenmeProgramlariniListele(string id)
		{
			try
			{
				FirebaseResponse response = client.Get("BeslenmeProgramlari/");

				if (response != null && response.Body != null)
				{
					JObject data = JObject.Parse(response.Body);

					List<BeslenmeProgrami> beslenmeProgramlari = new List<BeslenmeProgrami>();

					foreach (var programNode in data)
					{
						// "AntrenorId" özelliğini al
						string programAntrenorId = programNode.Value["AntrenorId"].ToString();

						// AntrenorId kontrolü
						if (programAntrenorId == id)
						{
							// BeslenmeProgrami nesnesini oluştur ve listeye ekle
							BeslenmeProgrami beslenmeProgrami = new BeslenmeProgrami
							{
								Id = programNode.Key, // Düğümün ID'sini BeslenmeProgrami nesnesinin ID özelliğine eşitle
								AntrenorId = programAntrenorId,
								BeslenmeProgramiAdi = programNode.Value["BeslenmeProgramiAdi"].ToString(),
								Hedef = programNode.Value["Hedef"].ToString(),
								KaloriHedefi = programNode.Value["KaloriHedefi"].ToString(),
								GunlukOgunler = programNode.Value["GunlukOgunler"].ToString(),
							};

							beslenmeProgramlari.Add(beslenmeProgrami);
						}
					}

					return beslenmeProgramlari;
				}
				else
				{
					// response null ise veya body null ise boş bir dizi döndür
					return new List<BeslenmeProgrami>();
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda loglama yapabilirsiniz
				Console.WriteLine("Veri çekme işlemi başarısız oldu. Hata: " + ex.Message);

				// Hata durumunda boş bir dizi döndür
				return new List<BeslenmeProgrami>();
			}
		}



		public bool BeslenmeProgramiEkle(BeslenmeProgrami model)
        {
            try
            {
                PushResponse response = client.Push("BeslenmeProgramlari/", model);

                if (response.Result != null && !string.IsNullOrEmpty(response.Result.name))
                {
                    // Kayıt başarıyla eklendi
                    return true;
                }
                else
                {
                    // Kayıt eklenemedi
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya uygun bir işlem yapabilirsiniz
                return false;
            }
        }

		public bool BeslenmeProgramiSil(string id)
		{
			try
			{
				FirebaseResponse response = client.Delete("BeslenmeProgramlari/" + id);

				// Silme işlemi başarılı ise true döndür
				return response.StatusCode == System.Net.HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				// Hata durumunda loglama yapabilirsiniz
				Console.WriteLine("Silme işlemi başarısız oldu. Hata: " + ex.Message);

				return false;
			}
		}

		public BeslenmeProgrami IdyeGoreBeslenmeProgramiGetir(string id)
		{
			try
			{
				FirebaseResponse response = client.Get($"BeslenmeProgramlari/{id}");

				if (response != null && response.Body != null)
				{
					JObject beslenmeProgramiNode = JObject.Parse(response.Body);

					// "AntrenorId" özelliğini al
					string programAntrenorId = beslenmeProgramiNode["AntrenorId"].ToString();

					// BeslenmeProgrami nesnesini oluştur ve geri döndür
					BeslenmeProgrami beslenmeProgrami = new BeslenmeProgrami
					{
						Id = id, // Veritabanındaki ID
						AntrenorId = programAntrenorId,
						BeslenmeProgramiAdi = beslenmeProgramiNode["BeslenmeProgramiAdi"].ToString(),
						Hedef = beslenmeProgramiNode["Hedef"].ToString(),
						KaloriHedefi = beslenmeProgramiNode["KaloriHedefi"].ToString(),
						GunlukOgunler = beslenmeProgramiNode["GunlukOgunler"].ToString(),
					};

					return beslenmeProgrami;
				}
				else
				{
					// response null ise veya body null ise null döndür
					return null;
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda loglama yapabilirsiniz
				Console.WriteLine("Veri çekme işlemi başarısız oldu. Hata: " + ex.Message);

				// Hata durumunda null döndür
				return null;
			}
		}

		public bool BeslenmeProgramiGuncelle(BeslenmeProgrami model)
		{
			try
			{
				FirebaseResponse response = client.Get($"BeslenmeProgramlari/{model.Id}");

				if (response != null && response.Body != null)
				{
					JObject beslenmeProgramiNode = JObject.Parse(response.Body);

					// Veritabanındaki düğümü güncelle
					beslenmeProgramiNode["AntrenorId"] = model.AntrenorId;
					beslenmeProgramiNode["BeslenmeProgramiAdi"] = model.BeslenmeProgramiAdi;
					beslenmeProgramiNode["Hedef"] = model.Hedef;
					beslenmeProgramiNode["KaloriHedefi"] = model.KaloriHedefi;
					beslenmeProgramiNode["GunlukOgunler"] = model.GunlukOgunler;

					FirebaseResponse updateResponse = client.Update($"BeslenmeProgramlari/{model.Id}", beslenmeProgramiNode);

					// Güncelleme işlemi başarılı ise true döndür
					return updateResponse.StatusCode == System.Net.HttpStatusCode.OK;
				}
				else
				{
					// response null ise veya body null ise güncelleme işlemi başarısız
					return false;
				}
			}
			catch (Exception ex)
			{
				// Hata durumunda loglama yapabilirsiniz
				Console.WriteLine("Güncelleme işlemi başarısız oldu. Hata: " + ex.Message);

				// Güncelleme işlemi başarısız ise false döndür
				return false;
			}
		}

	}

}
