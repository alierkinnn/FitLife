using FitLife.Models;

namespace FitLife.Repositories.Abstracts
{
    public interface IAdminRepo
    {
        public void DanisanEkle(Danisan model);
		public void DanisanGuncelle(Danisan model);
		public void DanisanSil(string id);
		Danisan IdyeGoreDanisanGetir(string id);
		public List<Danisan> TumDanisanlariListele();
    }
}
