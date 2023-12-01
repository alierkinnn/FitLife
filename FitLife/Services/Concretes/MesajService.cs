using FitLife.Models;
using FitLife.Repositories.Abstracts;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
    public class MesajService : IMesajService
    {
        private readonly IMesajRepo _mesajRepo;

        public MesajService(IMesajRepo mesajRepo)
        {
            this._mesajRepo = mesajRepo;
        }

        public bool MesajGonder(MesajModel model)
        {
            return _mesajRepo.MesajGonder(model);
        }

        public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel)
        {
            return _mesajRepo.MesajlarimiGoruntule(mesajModel);
        }
    }
}
