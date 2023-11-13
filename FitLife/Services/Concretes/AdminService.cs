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

        public void DanisanEkle(Danisan model)
        {
            adminRepo.DanisanEkle(model);
        }

		public void DanisanGuncelle(Danisan model)
		{
            adminRepo.DanisanGuncelle(model);
		}

		public void DanisanSil(string id)
		{
            adminRepo.DanisanSil(id);
		}

		public Danisan IdyeGoreDanisanGetir(string id)
		{
            return adminRepo.IdyeGoreDanisanGetir(id);
		}

		public List<Danisan> TumDanisanlariListele()
        {
            return adminRepo.TumDanisanlariListele();
        }


    }
}
