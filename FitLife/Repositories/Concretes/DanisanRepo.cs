using Firebase.Database.Query;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FitLife.Repositories.Concretes
{
    public class DanisanRepo : IDanisanRepo
    {
        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
            BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
        };


        public DanisanRepo()
        {
            client = new FireSharp.FirebaseClient(config);
        }
        IFirebaseClient client;

        public Danisan IdyeGoreDanisanGetir(string id)
        {
            FirebaseResponse response = client.Get("Danisanlar/" + id);
            Danisan danisan = JsonConvert.DeserializeObject<Danisan>(response.Body);
            return danisan;
        }

        public async Task<bool> DanisanProfilimiGuncelle(Danisan model)
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

        public IlerlemeKaydi IdyeGoreIlerlemeKaydiGetir(string id)
        {
            try
            {
                FirebaseResponse response = client.Get("IlerlemeKayitlari/" + id);

                if (response.Body != "null")
                {
                    IlerlemeKaydi ilerlemeKaydi = response.ResultAs<IlerlemeKaydi>();
                    return ilerlemeKaydi;
                }
                else
                {
                    // Belirtilen ID'ye sahip kayıt bulunamadı
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya uygun bir işlem yapabilirsiniz
                return null;
            }
        }

        private double? CalculateVucutKitleIndeksi(string strBoy, string strKilo)
        {
            if (double.TryParse(strBoy, out double boy) && double.TryParse(strKilo, out double kilo))
            {
                // Vücut Kitle İndeksini Hesaplama Formülü
                double vucutKitleIndeksi = kilo / Math.Pow(boy / 100, 2);
                return vucutKitleIndeksi;
            }

            return null; // Başarısız dönüşüm durumunda null dönebilirsiniz.
        }

        public List<IlerlemeKaydi> IdyeGoreIlerlemeKayitlariniGetir(string id)
        {
            FirebaseResponse response = client.Get("IlerlemeKayitlari/");

            if (response.Body == "null")
            {
                // No records found
                return new List<IlerlemeKaydi>();
            }

            dynamic data = response.ResultAs<dynamic>();

            List<IlerlemeKaydi> kayitlar = new List<IlerlemeKaydi>();

            foreach (var record in data)
            {
                if (record.Value.DanisanId == id)
                {
                    // Create an IlerlemeKaydi object and add it to the list
                    IlerlemeKaydi kayit = new IlerlemeKaydi
                    {
                        Boy = record.Value.Boy?.ToString(),
                        Kilo = record.Value.Kilo?.ToString(),
                        KasKutlesi = record.Value.KasKutlesi?.ToString(),
                        YagOrani = record.Value.YagOrani?.ToString(),
                        Tarih = record.Value.Tarih?.ToString(),
                        VucutKitleIndeksi = CalculateVucutKitleIndeksi(record.Value.Boy?.ToString(), record.Value.Kilo?.ToString())?.ToString("F4"),
                        Id = ((JProperty)record).Name
                    };

                    // Add the populated IlerlemeKaydi object to the list
                    kayitlar.Add(kayit);
                }
            }

            return kayitlar;
        }

        public bool IlerlemeKaydiSil(string id)
        {
            try
            {
                var silmeYolu = $"IlerlemeKayitlari/{id}";
                FirebaseResponse response = client.Delete(silmeYolu);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Hata oluştu
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }

        public bool IlerlemeKaydiEkle(IlerlemeKaydi model)
        {
            model.VucutKitleIndeksi = CalculateVucutKitleIndeksi(model.Boy?.ToString(), model.Kilo?.ToString())?.ToString("F4");
            try
            {
                PushResponse response = client.Push("IlerlemeKayitlari/", model);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Veri başarıyla eklendi
                    return true;
                }
                else
                {
                    // Veri eklenirken bir hata oluştu
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Hata oluştu
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }
    }

}
