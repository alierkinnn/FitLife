using FitLife.Models;
using FitLife.Repositories.Abstracts;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
    public class DanisanService : IDanisanService
    {
        private readonly IDanisanRepo _danisanRepo;

        public DanisanService(IDanisanRepo danisanRepo)
        {
            this._danisanRepo = danisanRepo;
        }

        public Task<bool> DanisanProfilimiGuncelle(Danisan model)
        {
            return _danisanRepo.DanisanProfilimiGuncelle(model);
        }

        public Danisan IdyeGoreDanisanGetir(string id)
        {
            return _danisanRepo.IdyeGoreDanisanGetir(id);
        }

        public List<IlerlemeKaydi> IdyeGoreIlerlemeKayitlariniGetir(string id)
        {
            return _danisanRepo.IdyeGoreIlerlemeKayitlariniGetir(id);
        }

        public bool IlerlemeKaydiEkle(IlerlemeKaydi model)
        {
            model.Tarih = DateTime.Now.ToString("dd-MM-yyyy");
            return _danisanRepo.IlerlemeKaydiEkle(model);
        }

        public bool IlerlemeKaydiSil(string id)
        {
            return _danisanRepo.IlerlemeKaydiSil(id);
        }
    }
}
