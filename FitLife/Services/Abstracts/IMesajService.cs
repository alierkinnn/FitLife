using FitLife.Models;

namespace FitLife.Services.Abstracts
{
    public interface IMesajService
    {
        public bool MesajGonder(MesajModel model);
        public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel);
    }
}
