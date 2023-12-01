using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;

namespace FitLife.Repositories.Concretes
{
    public class MesajRepo : IMesajRepo
    {

        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
            BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
        };



        public MesajRepo()
        {
            client = new FireSharp.FirebaseClient(config);
        }
        IFirebaseClient client;

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


    }
}
