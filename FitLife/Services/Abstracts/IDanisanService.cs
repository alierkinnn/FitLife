using FitLife.Models;

namespace FitLife.Services.Abstracts
{
    public interface IDanisanService
    {
        public Task<bool> DanisanProfilimiGuncelle(Danisan model);
        public Danisan IdyeGoreDanisanGetir(string id);
        public List<IlerlemeKaydi> IdyeGoreIlerlemeKayitlariniGetir(string id);
        public bool IlerlemeKaydiEkle(IlerlemeKaydi model);
        public bool IlerlemeKaydiSil(string id);
    }
}
