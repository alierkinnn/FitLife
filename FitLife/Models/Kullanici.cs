namespace FitLife.Models
{
    public abstract class Kullanici
    {
        public string? Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string? Cinsiyet { get; set; }
        public string? Eposta { get; set; }
        public string? Sifre { get; set; }
        public string? Telefon { get; set; }
        public string? Rol { get; set; }
    }
}
