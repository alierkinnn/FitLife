using System.Net;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FitLife.Models;
using FitLife.Repositories.Abstracts;
using Newtonsoft.Json;

namespace FitLife.Repositories.Concretes
{
    public class AdminRepo : IAdminRepo
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "BaS9mh40xQcOoaaNbOU7A4HW5Z6hQXwSmiHos9JC",
            BasePath = "https://fitlife-b940d-default-rtdb.firebaseio.com/",
        };

        IFirebaseClient client;

        public AdminRepo()
        {
            client = new FireSharp.FirebaseClient(config);
        }

        public void DanisanEkle(Danisan model)
        {
            PushResponse response = client.Push("Danisanlar/", model);
        }

		public void DanisanGuncelle(Danisan model)
		{
            SetResponse response = client.Set("Danisanlar/" + model.Id, model);
		}

		public void DanisanSil(string id)
		{
            FirebaseResponse response = client.Delete("Danisanlar/" + id);
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

            var list = new List<Danisan>();
            foreach (var item in data)
            {
                Danisan danisan = item.Value;
                danisan.Id = item.Key;
				list.Add(danisan);
            }
            return list;
        }
    }
}
