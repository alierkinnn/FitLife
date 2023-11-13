using FitLife.Models;
using FitLife.Repositories.Concretes;

namespace FitLife.Services.Abstracts
{
    public interface IAdminService
    {
        public void DanisanEkle(Danisan model);
		public void DanisanGuncelle(Danisan model);
		public void DanisanSil(string id);
		Danisan IdyeGoreDanisanGetir(string id);
		public List<Danisan> TumDanisanlariListele();
    }
}
