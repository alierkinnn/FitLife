using FitLife.Models;
using FitLife.Repositories.Abstracts;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
	public class HesapService : IHesapService
	{
		private readonly IHesapRepo _hesapRepo;

		public HesapService(IHesapRepo hesapRepo)
		{
			this._hesapRepo = hesapRepo;
		}

		public GirisYapModel GirisYap(GirisYapModel model)
		{
			return _hesapRepo.GirisYap(model);
		}
	}
}
