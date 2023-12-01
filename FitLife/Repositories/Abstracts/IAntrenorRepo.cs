using FitLife.Models;

namespace FitLife.Repositories.Abstracts
{
	public interface IAntrenorRepo
	{
		public Task<bool> AntrenorProfilimiGuncelle(Antrenor model);
        public bool BeslenmeProgramiEkle(BeslenmeProgrami model);
		public bool BeslenmeProgramiGuncelle(BeslenmeProgrami model);
		public bool BeslenmeProgramiSil(string id);
		public List<BeslenmeProgrami> BeslenmeProgramlariniListele(string id);
        public List<Danisan>DanisanlarimiListele(string id);
        public bool EgzersizProgramiEkle(EgzersizProgrami model);
        public List<EgzersizProgrami> EgzersizProgramlariniListele(string id);
        public Antrenor IdyeGoreAntrenorGetir(string id);
		public BeslenmeProgrami IdyeGoreBeslenmeProgramiGetir(string id);
		public bool MesajGonder(MesajModel model);
        public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel);
	}
}
