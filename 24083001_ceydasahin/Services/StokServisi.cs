using System;
using System.Collections.Generic;
using System.Linq;
using _24083001_ceydasahin.Models;
namespace _24083001_ceydasahin.Services
{
 // Bellekteki kayitlarin ve stok hareketlerinin tek islem noktasi.
 public sealed class StokServisi : IStokServisi
 {
  private readonly List<Urun> urunler = new List<Urun>();
  private readonly List<StokHareketi> hareketler = new List<StokHareketi>();
  public IReadOnlyList<Urun> Urunler { get { return urunler.AsReadOnly(); } }
  public IReadOnlyList<StokHareketi> Hareketler { get { return hareketler.AsReadOnly(); } }
  public void Ekle(string kod, string ad, string kategori, decimal fiyat, int stok, int kritik)
  {
   if (urunler.Any(u => string.Equals(u.Kod, (kod ?? "").Trim(), StringComparison.OrdinalIgnoreCase))) throw new ArgumentException("Bu ürün kodu zaten kayıtlı.");
   Urun urun = new StandartUrun(kod, ad, kategori, fiyat, stok, kritik); urunler.Add(urun);
   if (stok > 0) hareketler.Add(new StokHareketi(urun.Kod, HareketTuru.Baslangic, stok, stok));
  }
  private Urun Bul(string kod) { var urun = urunler.FirstOrDefault(u => string.Equals(u.Kod, kod, StringComparison.OrdinalIgnoreCase)); if (urun == null) throw new ArgumentException("Önce listeden bir ürün seçin."); return urun; }
  public void Guncelle(string kod, string ad, string kategori, decimal fiyat, int kritik) { Bul(kod).Guncelle(ad, kategori, fiyat, kritik); }
  public void Sil(string kod) { urunler.Remove(Bul(kod)); }
  public void StokIsle(string kod, int miktar, HareketTuru tur)
  {
   var urun = Bul(kod);
   switch(tur) { case HareketTuru.Giris: urun.StokGir(miktar); break; case HareketTuru.Cikis: urun.StokCik(miktar); break; default: throw new ArgumentException("Geçersiz stok işlemi."); }
   hareketler.Add(new StokHareketi(urun.Kod, tur, miktar, urun.Stok));
  }
 }
}