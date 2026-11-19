using System;
namespace _24083001_ceydasahin.Models
{
 public enum HareketTuru { Baslangic, Giris, Cikis }
 public sealed class StokHareketi
 {
  public DateTime Tarih { get; private set; }
  public string UrunKodu { get; private set; }
  public HareketTuru Tur { get; private set; }
  public int Miktar { get; private set; }
  public int SonStok { get; private set; }
  public StokHareketi(string kod, HareketTuru tur, int miktar, int sonStok) { Tarih = DateTime.Now; UrunKodu = kod; Tur = tur; Miktar = miktar; SonStok = sonStok; }
 }
}