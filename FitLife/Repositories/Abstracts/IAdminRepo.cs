using FitLife.Models;

namespace FitLife.Repositories.Abstracts
{
    public interface IAdminRepo
    {
		Task<bool> AntrenorEkle(Danisan model);
		Task<bool> AntrenorGuncelle(Antrenor model);
		Task<bool> AntrenorSil(string id);
		public Task<bool> DanisanEkle(Danisan model);
		public Task<bool> DanisanGuncelle(Danisan model);
		public Task<bool> DanisanSil(string id);
		public Antrenor IdyeGoreAntrenorGetir(string id);
		public Danisan IdyeGoreDanisanGetir(string id);
		public List<Antrenor> TumAntrenorleriListele();
		public List<Danisan> TumDanisanlariListele();
    }
}
