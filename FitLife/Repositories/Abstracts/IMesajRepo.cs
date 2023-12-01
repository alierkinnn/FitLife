using FitLife.Models;

namespace FitLife.Repositories.Abstracts
{
    public interface IMesajRepo
    {
        public bool MesajGonder(MesajModel model);
        public List<MesajModel> MesajlarimiGoruntule(MesajModel mesajModel);
    }
}
