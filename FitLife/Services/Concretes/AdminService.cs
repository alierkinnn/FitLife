using FitLife.Models;
using FitLife.Repositories.Abstracts;
using FitLife.Repositories.Concretes;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo adminRepo;

        public AdminService(IAdminRepo adminRepo)
        {
            this.adminRepo = adminRepo;
        }

		public async Task<bool> DanisanEkle(Danisan model)
        {
            return await adminRepo.DanisanEkle(model);
        }

		public async Task<bool> DanisanGuncelle(Danisan model)
		{
            return await adminRepo.DanisanGuncelle(model);
		}

		public async Task<bool> DanisanSil(string id)
		{
            return await adminRepo.DanisanSil(id);
		}

		public Danisan IdyeGoreDanisanGetir(string id)
		{
            return adminRepo.IdyeGoreDanisanGetir(id);
		}

		public List<Danisan> TumDanisanlariListele()
        {
            return adminRepo.TumDanisanlariListele();
        }


		public async Task<bool> AntrenorEkle(Danisan model)
		{
			return await adminRepo.AntrenorEkle(model);
		}

		public async Task<bool> AntrenorGuncelle(Antrenor model)
		{
			return await adminRepo.AntrenorGuncelle(model);
		}

		public async Task<bool> AntrenorSil(string id)
		{
			return await adminRepo.AntrenorSil(id);
		}

		public Antrenor IdyeGoreAntrenorGetir(string id)
		{
			return adminRepo.IdyeGoreAntrenorGetir(id);
		}

		public List<Antrenor> TumAntrenorleriListele()
		{
			return adminRepo.TumAntrenorleriListele();
		}


	}
}
