using System.Collections.Generic;
using _24083001_ceydasahin.Models;
namespace _24083001_ceydasahin.Services
{
 public interface IStokServisi
 {
  IReadOnlyList<Urun> Urunler { get; }
  IReadOnlyList<StokHareketi> Hareketler { get; }
  void Ekle(string kod, string ad, string kategori, decimal fiyat, int stok);
  void Guncelle(string kod, string yeniKod, string ad, string kategori, decimal fiyat);
  void Sil(string kod);
  void StokIsle(string kod, int miktar, HareketTuru tur);
 }
}