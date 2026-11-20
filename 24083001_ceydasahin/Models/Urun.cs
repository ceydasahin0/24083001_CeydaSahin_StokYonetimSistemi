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
  private int enYuksekStok;
  // En yuksek stogun yuzde 20'si, kritik deger icin 0-10 araligina sinirlanir.
  public int KritikSeviye { get { return Math.Min(10, Math.Max(0, (int)Math.Ceiling(enYuksekStok * 0.20m))); } }
  public bool KritikMi { get { return Stok == 0 || Stok < KritikSeviye; } }
  public decimal StokDegeri { get { return BirimFiyat * Stok; } }
  protected Urun(string kod, string ad, string kategori, decimal fiyat, int stok)
  {
   if (string.IsNullOrWhiteSpace(kod)) throw new ArgumentException("Ürün kodu zorunludur.");
   if (stok < 0) throw new ArgumentException("Başlangıç stoku negatif olamaz.");
   Kod = kod.Trim().ToUpperInvariant(); Guncelle(ad, kategori, fiyat); Stok = stok; enYuksekStok = stok;
  }
  public void Guncelle(string ad, string kategori, decimal fiyat)
  {
   if (string.IsNullOrWhiteSpace(ad)) throw new ArgumentException("Ürün adı zorunludur.");
   if (string.IsNullOrWhiteSpace(kategori)) throw new ArgumentException("Kategori zorunludur.");
   if (fiyat < 0) throw new ArgumentException("Fiyat negatif olamaz.");
   Ad = ad.Trim(); Kategori = kategori.Trim(); BirimFiyat = fiyat;
  }
  internal void KodDegistir(string yeniKod)
  {
   if (string.IsNullOrWhiteSpace(yeniKod)) throw new ArgumentException("Ürün kodu zorunludur.");
   Kod = yeniKod.Trim().ToUpperInvariant();
  }
  public void StokGir(int miktar) { MiktarKontrol(miktar); Stok = checked(Stok + miktar); enYuksekStok = Math.Max(enYuksekStok, Stok); }
  public void StokCik(int miktar) { MiktarKontrol(miktar); if (miktar > Stok) throw new InvalidOperationException("Yetersiz stok. Mevcut miktar: " + Stok); Stok -= miktar; }
  private static void MiktarKontrol(int miktar) { if (miktar <= 0) throw new ArgumentException("Miktar sıfırdan büyük olmalıdır."); }
  public abstract string DurumAciklamasi();
 }
}