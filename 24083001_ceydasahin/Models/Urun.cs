using System;
namespace _24083001_ceydasahin.Models
{
 // Stok miktari yalnizca kontrollu metotlarla degisir.
 public abstract class Urun
 {
  public string Kod { get; private set; }
  public string Ad { get; private set; }
  public string Kategori { get; private set; }
  public decimal BirimFiyat { get; private set; }
  public int Stok { get; private set; }
  public int KritikSeviye { get; private set; }
  public bool KritikMi { get { return Stok < KritikSeviye; } }
  public decimal StokDegeri { get { return BirimFiyat * Stok; } }
  protected Urun(string kod, string ad, string kategori, decimal fiyat, int stok, int kritik)
  {
   if (string.IsNullOrWhiteSpace(kod)) throw new ArgumentException("Ürün kodu zorunludur.");
   if (stok < 0) throw new ArgumentException("Başlangıç stoku negatif olamaz.");
   Kod = kod.Trim().ToUpperInvariant(); Guncelle(ad, kategori, fiyat, kritik); Stok = stok;
  }
  public void Guncelle(string ad, string kategori, decimal fiyat, int kritik)
  {
   if (string.IsNullOrWhiteSpace(ad)) throw new ArgumentException("Ürün adı zorunludur.");
   if (string.IsNullOrWhiteSpace(kategori)) throw new ArgumentException("Kategori zorunludur.");
   if (fiyat < 0 || kritik < 0) throw new ArgumentException("Fiyat ve kritik seviye negatif olamaz.");
   Ad = ad.Trim(); Kategori = kategori.Trim(); BirimFiyat = fiyat; KritikSeviye = kritik;
  }
  public void StokGir(int miktar) { MiktarKontrol(miktar); Stok = checked(Stok + miktar); }
  public void StokCik(int miktar) { MiktarKontrol(miktar); if (miktar > Stok) throw new InvalidOperationException("Yetersiz stok. Mevcut miktar: " + Stok); Stok -= miktar; }
  private static void MiktarKontrol(int miktar) { if (miktar <= 0) throw new ArgumentException("Miktar sıfırdan büyük olmalıdır."); }
  public abstract string DurumAciklamasi();
 }
}