using System;
namespace _24083001_ceydasahin.Models
{
 public enum HareketTuru { Baslangic, Giris, Cikis }
 public sealed class StokHareketi
 {
  public DateTime Tarih { get; private set; }
  private readonly Urun urun;
  public string UrunKodu { get { return urun.Kod; } }
  public HareketTuru Tur { get; private set; }
  public int Miktar { get; private set; }
  public int SonStok { get; private set; }
  public StokHareketi(Urun urun, HareketTuru tur, int miktar, int sonStok) { Tarih = DateTime.Now; this.urun = urun; Tur = tur; Miktar = miktar; SonStok = sonStok; }
 }
}