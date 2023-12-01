namespace FitLife.Models
{
    public class EgzersizProgrami
    {
        public string Id { get; set; }
        public string EgzersizProgramiAdi { get; set; }
        public List<Egzersiz> Egzersizler { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string AntrenorId { get; set; }
    }
}
