using FitLife.Models;
using FitLife.Repositories.Abstracts;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
	public class AntrenorService : IAntrenorService
	{
		private readonly IAntrenorRepo _antrenorRepo;

		public AntrenorService(IAntrenorRepo antrenorRepo)
		{
			this._antrenorRepo = antrenorRepo;
		}

		public Task<bool> AntrenorProfilimiGuncelle(Antrenor model)
		{
			return _antrenorRepo.AntrenorProfilimiGuncelle(model);
		}

        public bool BeslenmeProgramiEkle(BeslenmeProgrami model)
        {
			return _antrenorRepo.BeslenmeProgramiEkle(model);
        }

		public bool BeslenmeProgramiGuncelle(BeslenmeProgrami model)
		{
			return _antrenorRepo.BeslenmeProgramiGuncelle(model);
		}

		public bool BeslenmeProgramiSil(string id)
		{
			return _antrenorRepo.BeslenmeProgramiSil(id);
		}

		public List<BeslenmeProgrami> BeslenmeProgramlariniListele(string id)
        {
			return _antrenorRepo.BeslenmeProgramlariniListele(id);
        }

        public List<Danisan> DanisanlarimiListele(string id)
		{
			return _antrenorRepo.DanisanlarimiListele(id);
		}

        public bool EgzersizProgramiEkle(EgzersizProgrami model)
        {
			return _antrenorRepo.EgzersizProgramiEkle(model);
        }

        public List<EgzersizProgrami> EgzersizProgramlariniListele(string id)
        {
			return _antrenorRepo.EgzersizProgramlariniListele(id);
        }

        public Antrenor IdyeGoreAntrenorGetir(string id)
		{
			return _antrenorRepo.IdyeGoreAntrenorGetir(id);
		}

		public BeslenmeProgrami IdyeGoreBeslenmeProgramiGetir(string id)
		{
			return _antrenorRepo.IdyeGoreBeslenmeProgramiGetir(id);
		}

		public bool MesajGonder(MesajModel model)
        {
			return _antrenorRepo.MesajGonder(model);
        }

        public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel)
		{
			return _antrenorRepo.MesajlarimiGoruntule(mesajModel);
		}
	}
}
