# Tamamlama ve kontrol rehberi

## Önce çalıştır
Visual Studio'da C:\ceydasahin\24083001_ceydasahin\24083001_ceydasahin.sln dosyasını aç ve F5'e bas. Açılmazsa Visual Studio'nun gösterdiği hata mesajını paylaş. Uygulama Denetimi engeli varsa güvenlik ayarlarını değiştirmeden okulun/cihaz yöneticisinin desteklediği çalıştırma yolunu kullan.

## Elle çalıştırılacak testler
Aşağıdaki testler henüz yürütülmedi. Gerçekleşen sonucu denedikten sonra kaydet.

| No | İşlem / girdi | Beklenen sonuç | Gerçekleşen sonuç |
|---|---|---|---|
| 1 | P001, Kalem, Kırtasiye, 15 TL, stok 10, kritik 5 ekle | 1 ürün, 10 toplam stok, 0 kritik ürün | Bekliyor |
| 2 | p001 koduyla ikinci ürün ekle | Yinelenen kod uyarısı; sayı ve stok değişmez | Bekliyor |
| 3 | Ürün adı boş ekle | Zorunlu alan uyarısı; kayıt oluşmaz | Bekliyor |
| 4 | P001'i seç, fiyatı 20 ve adı Mavi Kalem yap, güncelle | Aynı kod ve stok korunur; ad/fiyat değişir | Bekliyor |
| 5 | P001'e 5 stok girişi yap | Stok 15; hareket kaydı eklenir | Bekliyor |
| 6 | P001'den 12 stok çıkışı yap | Stok 3; kritik ürün sayısı 1 | Bekliyor |
| 7 | Stok 3 iken 4 çıkış yap | Yetersiz stok uyarısı; stok ve hareketler değişmez | Bekliyor |
| 8 | Miktar 0 ile giriş ve çıkış yap | Sıfırdan büyük miktar uyarısı | Bekliyor |
| 9 | Kod P001 ve ad Mavi ile ara | İlgili ürün görünür; alakasız aramada liste boşalır | Bekliyor |
| 10 | İkinci kategoride ürün ekle, kategori filtresini değiştir | Yalnız seçili kategorideki ürünler görünür | Bekliyor |
| 11 | Kritik seviye 5 için stok 4 ve sonra 5 durumunu kontrol et | 4 kritiktir, 5 kritik değildir | Bekliyor |
| 12 | Filtreler açıkken özetleri kontrol et | Özetler tüm güncel ürünleri sayar | Bekliyor |
| 13 | Sil'e bas, önce Hayır sonra Evet seç | Hayır kaydı korur; Evet kaydı siler ve özetleri günceller | Bekliyor |
| 14 | Seçim yokken güncelle / stok işlemi yap | Önce ürün seçilmesi uyarısı | Bekliyor |
| 15 | Uygulamayı kapatıp yeniden aç | Bellekteki kayıtlar silinmiştir | Bekliyor |

## Visual Studio debugging kanıtı
Bu işlem henüz yapılmadı; raporda yapılmış gibi yazılmamalıdır.
1. Models/Urun.cs içinde StokCik metodundaki `if (miktar > Stok)` satırına breakpoint koy.
2. F5 ile çalıştır; 3 stoklu ürün ekle ve 4 çıkış dene.
3. Locals/Watch penceresinde miktar=4, Stok=3 değerlerini gözle.
4. F10 ile koşulu ilerlet; InvalidOperationException oluştuğunu ve Form1.Calistir içindeki catch ile uyarı gösterildiğini kontrol et.
5. Stokun 3 kaldığını kontrol et. Gerçek gözlemini ve breakpoint görüntüsünü rapora ekle.

## Gerçek uygulamadan çekilecek görüntüler
1. Ürün ekleme/listeleme ve özetler.
2. Seçili ürünün güncellenmiş bilgileri.
3. Stok girişi sonrası ürün ve bilgi mesajı.
4. Stok çıkışı sonrası ürün ve bilgi mesajı.
5. Yetersiz stok uyarısı.
6. Arama/kategori/kritik stok filtreleri.
7. Stok hareketleri sekmesi.
Her görselin altına numara ve işlem açıklaması yaz. Tasarım ekranını çalışan uygulama görüntüsü yerine kullanma.

## Raporun zorunlu 13 başlığı
1. Kapak: BGT331, .NET Programlama I, Stok Yönetim Sistemi, Ceyda Şahin, 24083001, gerçek teslim tarihi.
2. Amaç ve kapsam.
3. Visual Studio, C# ve .NET Framework 4.8.
4. Genel yapı ve temel işlevler.
5. Sınıf ve nesne tasarımı.
6. OOP ilkelerinin kaynak koddan somut örnekleri (README'ye bak).
7. WinForms kontrolleri ve Click/SelectionChanged/TextChanged olayları.
8. List<Urun>, List<StokHareketi> ve bellekte veri yönetimi.
9. Gerçekte yürütülen debugging örneği.
10. Numaralı ve açıklamalı çalışma ekran görüntüleri.
11. En az 5 test: girdi, beklenen ve gerçekleşen sonuç.
12. Sorunlar ve çözümler: .NET 10 başlangıç projesinin Framework 4.8'e dönüştürülmesi; eski obj dosyalarının ayrılması; varsa çalışma testlerinde bulunan sorunlar.
13. Sonuç ve değerlendirme.

## GitHub ve son teslim
- Private repository adı: 24083001_CeydaSahin_InventoryManagement.
- En az 3 gerçek aşamada anlamlı commit. Geçmişi geriye dönük uydurma; örneğin mevcut uygulama, doğrulanan hata düzeltmeleri, rapor/test belgelendirmesi aşamalarını kaydet.
- Şartnamedeki tohid.yousefi collaborator davetini kullanıcı GitHub hesabından ayrıca göndermelidir. Bu rehber davetin gönderildiği anlamına gelmez.
- Repository URL'sini README ve 04_GitHub içine ekle.
- Son ZIP: 01_SourceCode (.sln ve kaynaklar), 02_Application (EXE ve config), 03_Report (tamamlanmış PDF/DOCX ve gerçek görseller), 04_GitHub (README ve bağlantı), kökte README.
- Şartnameye göre son teslim: 20.11.2026 23:59, ALMS. ZIP'i yükledikten sonra yüklemenin tamamlandığını kontrol et.
- Mevcut kaynak ZIP'i rapor/GitHub/çalışma testleri tamamlanmadan nihai teslim olarak kullanma.
