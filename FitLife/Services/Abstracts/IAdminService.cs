using FitLife.Models;
using FitLife.Repositories.Concretes;

namespace FitLife.Services.Abstracts
{
    public interface IAdminService
    {
        public Task<bool> DanisanEkle(Danisan model);
		public Task<bool> DanisanGuncelle(Danisan model);
		public Task<bool> DanisanSil(string id);
		Danisan IdyeGoreDanisanGetir(string id);
		public List<Danisan> TumDanisanlariListele();
		public Task<bool> AntrenorEkle(Antrenor model);
		public Task<bool> AntrenorGuncelle(Antrenor model);
		public Task<bool> AntrenorSil(string id);
		Antrenor IdyeGoreAntrenorGetir(string id);
		public List<Antrenor> TumAntrenorleriListele();

	}
}
