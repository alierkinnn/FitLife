using FitLife.Models;

namespace FitLife.Repositories.Abstracts
{
    public interface IDanisanRepo
    {
        public Danisan IdyeGoreDanisanGetir(string id);
        public Task<bool> DanisanProfilimiGuncelle(Danisan model);
        public List<IlerlemeKaydi> IdyeGoreIlerlemeKayitlariniGetir(string id);
        public bool IlerlemeKaydiSil(string id);
        public bool IlerlemeKaydiEkle(IlerlemeKaydi model);
    }
}
