namespace _24083001_ceydasahin.Models
{
 // Soyut urun davranisi, tabloda gosterilen durum metnini uretir.
 public sealed class StandartUrun : Urun
 {
  public StandartUrun(string kod, string ad, string kategori, decimal fiyat, int stok, int kritik) : base(kod, ad, kategori, fiyat, stok, kritik) { }
  public override string DurumAciklamasi() { if (Stok == 0) return "Stok yok"; return KritikMi ? "Kritik stok" : "Yeterli"; }
 }
}