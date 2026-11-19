# Stok Yönetim Sistemi
Ceyda Şahin | 24083001 | BGT331 - .NET Programlama I

## Amaç ve özellikler
C# / Windows Forms ile bellekte ürün ve stok yönetimi. Ürün ekleme, listeleme, güncelleme, onaylı silme; stok girişi/çıkışı; kod/ad araması, kategori filtresi, kritik stok filtresi ve güncel özetler içerir. Stok hareketleri ayrı sekmede izlenir. Veritabanı veya kalıcı kayıt kullanılmaz.

## Teknoloji
.NET Framework 4.8, C# 7.3, Windows Forms. Visual Studio 2026 Community MSBuild ile Release derlemesi başarılıdır. Ek NuGet paketi gerekmez. Visual Studio'da .NET masaüstü geliştirme iş yükü ve .NET Framework 4.8 hedefleme paketi bulunmalıdır.

## Açma ve çalıştırma
1. Visual Studio'da `24083001_ceydasahin.sln` dosyasını açın. Eski `.slnx` dosyası yerine `.sln` dosyasını kullanın.
2. Başlangıç projesini `24083001_ceydasahin` seçin; Release veya Debug / Any CPU yapılandırmasını kullanın.
3. Çözümü derleyin ve F5 ile çalıştırın.
4. Yeni/Temizle ile alanları hazırlayın. Örneğin P001, Kalem, Kırtasiye, 15, 10, 5 girip Ürün ekle'ye basın.
5. Güncelleme için tablodan satır seçip Güncelle butonuna basın. Açılan pencerede ürün adı, kategori, birim fiyat ve kritik seviyeyi düzenleyip Kaydet seçin. İptal değişiklikleri kaydetmez; değişiklik yapılmadan Kaydet seçilirse bilgilendirme gösterilir. Stok miktarı stok giriş/çıkış butonlarıyla değiştirilir.
6. Uygulama kapanınca kayıtlar silinir.

## Kaynak düzeni
- Program.cs: başlangıç noktası.
- UrunDuzenleForm.cs: seçili ürünün bilgilerini Kaydet/İptal ile düzenleme penceresi.
- Form1.cs / Form1.Designer.cs: arayüz, olaylar ve kullanıcı mesajları.
- Models/Urun.cs: soyut ürün, kontrollü property ve stok metotları.
- Models/StandartUrun.cs: Urun'den türeyen somut ürün, override edilen durum metodu.
- Models/StokHareketi.cs: zaman, ürün kodu, hareket türü, miktar ve son stok.
- Services/IStokServisi.cs: arayüzün kullandığı servis sözleşmesi.
- Services/StokServisi.cs: benzersiz kod denetimi ve List<T> üzerinden ürün/stok yönetimi.

## OOP örnekleri
Kapsülleme: stok private setter ile korunur; StokGir/StokCik doğrulama yapar.
Kalıtım: StandartUrun : Urun.
Çok biçimlilik: Urun koleksiyonundaki nesnelerin DurumAciklamasi metodu tabloda çağrılır; somut sınıfın override metodu çalışır.
Soyutlama: abstract Urun ve IStokServisi uygulama akışında kullanılır. Form, IStokServisi türündeki alan üzerinden çalışır.
Constructor: Urun, StandartUrun, StokHareketi ve Form1.

## Doğrulama durumu
17.11.2026 tarihinde .NET Framework 4.8 Release derlemesi başarılıdır. Windows Uygulama Denetimi ilkesi EXE başlatılmasını engellediği için bu ortamda çalışma testleri ve ekran görüntüleri tamamlanamamıştır. Test listesindeki sonuçlar kullanıcı tarafından doğrulanmalıdır.

## Görseller ve rapor
Gerçek çalışma ekran görüntüleri henüz alınmamıştır. `TAMAMLAMA_REHBERI.md` dosyası çekilecek görselleri, testleri ve rapor başlıklarını içerir. Bu kaynak paketi nihai ALMS teslimi değildir.

## GitHub
Repository: https://github.com/ceydasahin0/24083001_CeydaSahin_StokYonetimSistemi

Şartnamedeki ad: `24083001_CeydaSahin_InventoryManagement`. Depo adı, gizlilik, öğretim elemanı collaborator daveti ve geliştirme geçmişi teslim öncesinde kontrol edilmelidir.

## Stok girişi kullanımı
Tablodan ürünü seçip Stok girişi düğmesine basın. Açılan pencerede Kaç adet eklenecek? alanına miktarı yazıp Ekle seçin. Yalnızca seçili ürünün stoku yazılan miktar kadar artar. İptal veya pencereyi kapatma stok değiştirmez. Stok çıkışı düğmesi de Kaç adet silinecek? penceresini açar. Girilen miktar Stoktan düş ile stoktan çıkarılır. Mevcut stoktan fazlası reddedilir; İptal stok değiştirmez.
