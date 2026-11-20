using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using _24083001_ceydasahin.Models;
using _24083001_ceydasahin.Services;
namespace _24083001_ceydasahin
{
 public partial class Form1 : Form
 {
  private readonly IStokServisi servis = new StokServisi();
  private readonly TextBox kod = new TextBox(), ad = new TextBox(), arama = new TextBox();
  private readonly ComboBox kategori = new ComboBox(), filtre = new ComboBox();
  private readonly NumericUpDown fiyat = Sayi(100000000, 2), stok = Sayi(1000000);
  private readonly DataGridView tablo = Grid(), hareket = Grid();
  private readonly CheckBox sadeceKritik = new CheckBox { Text = "Yalnızca kritik stok", AutoSize = true };
  private readonly Label ozet = new Label { AutoSize = true, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(22,75,104), Margin = new Padding(12) };
  private string seciliKod;
  private bool yenileniyor;
  public Form1() { InitializeComponent(); ArayuzKur(); Temizle(); Yenile(); }
  private static NumericUpDown Sayi(decimal max, int basamak = 0) { return new NumericUpDown { Minimum = 0, Maximum = max, DecimalPlaces = basamak, ThousandsSeparator = true, Width = 130 }; }
  private static DataGridView Grid() { return new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, AutoGenerateColumns = true }; }
  private static void Alan(FlowLayoutPanel panel, string baslik, Control kontrol) { var p = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Width = 155, Height = 65 }; p.Controls.Add(new Label { Text = baslik, AutoSize = true }); kontrol.Width = 145; p.Controls.Add(kontrol); panel.Controls.Add(p); }
  private Button Buton(FlowLayoutPanel panel, string yazi, Action islem) { var b = new Button { Text = yazi, AutoSize = true, Height = 34, Padding = new Padding(8,0,8,0), BackColor = Color.FromArgb(229,239,245), FlatStyle = FlatStyle.Flat }; b.Click += (s,e) => islem(); panel.Controls.Add(b); return b; }
  private void ArayuzKur()
  {
   Font = new Font("Segoe UI", 10); MinimumSize = new Size(1080,780); StartPosition = FormStartPosition.CenterScreen; BackColor = Color.FromArgb(244,247,250);
   var ana = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, Padding = new Padding(16) };
   foreach (int yukseklik in new[] {45,48,80,45,50}) ana.RowStyles.Add(new RowStyle(SizeType.Absolute,yukseklik));
   ana.RowStyles.Add(new RowStyle(SizeType.Percent,100)); ana.RowStyles.Add(new RowStyle(SizeType.Absolute,28)); Controls.Add(ana);
   ana.Controls.Add(new Label { Text = "STOK YÖNETİM SİSTEMİ", AutoSize = true, Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.FromArgb(22,75,104) },0,0);
   ana.Controls.Add(ozet,0,1);
   kategori.Items.AddRange(new object[] { "Elektronik", "Kırtasiye", "Gıda", "Temizlik", "Diğer" }); kategori.DropDownStyle = ComboBoxStyle.DropDown; kategori.MaxLength = 60; kod.MaxLength = 40; ad.MaxLength = 100;
   var edit = new FlowLayoutPanel { Dock = DockStyle.Fill }; Alan(edit,"Ürün kodu",kod); Alan(edit,"Ürün adı",ad); Alan(edit,"Kategori",kategori); Alan(edit,"Birim fiyat (₺)",fiyat); Alan(edit,"Başlangıç stoku",stok); ana.Controls.Add(edit,0,2);
   var islemler = new FlowLayoutPanel { Dock = DockStyle.Fill };
   Buton(islemler,"Ürün ekle",() => Calistir(() => { servis.Ekle(kod.Text,ad.Text,kategori.Text,fiyat.Value,(int)stok.Value); Temizle(); },"Ürün eklendi."));
   Buton(islemler,"Güncelle",UrunDuzenle);
   Buton(islemler,"Sil",() => { if (seciliKod == null) { Uyari("Önce listeden bir ürün seçin."); return; } if (MessageBox.Show(this,seciliKod + " kodlu ürün silinsin mi?", "Silme onayı",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes) Calistir(() => { servis.Sil(seciliKod); Temizle(); },"Ürün silindi."); });

   Buton(islemler,"Stok girişi",() => StokMiktariSor(HareketTuru.Giris));
   Buton(islemler,"Stok çıkışı",() => StokMiktariSor(HareketTuru.Cikis)); ana.Controls.Add(islemler,0,3);
   var ara = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(0,8,0,0) }; ara.Controls.Add(new Label { Text = "Kod / ad ara:", AutoSize = true }); arama.Width = 220; ara.Controls.Add(arama); filtre.DropDownStyle = ComboBoxStyle.DropDownList; filtre.Width = 180; ara.Controls.Add(filtre); ara.Controls.Add(sadeceKritik); Buton(ara,"Filtreleri temizle",FiltreleriTemizle); ana.Controls.Add(ara,0,4);
   var sekmeler = new TabControl { Dock = DockStyle.Fill }; var urunSayfa = new TabPage("Ürünler"); var hareketSayfa = new TabPage("Stok hareketleri"); urunSayfa.Controls.Add(tablo); hareketSayfa.Controls.Add(hareket); sekmeler.TabPages.Add(urunSayfa); sekmeler.TabPages.Add(hareketSayfa); ana.Controls.Add(sekmeler,0,5);
   ana.Controls.Add(new Label { Text = "Ceyda Şahin • 24083001 • BGT331 | Veriler yalnızca bellekte tutulur.", AutoSize = true, ForeColor = Color.DimGray },0,6);
   arama.TextChanged += (s,e) => Listele(); filtre.SelectedIndexChanged += (s,e) => Listele(); sadeceKritik.CheckedChanged += (s,e) => Listele(); tablo.SelectionChanged += (s,e) => SecimYukle();
   FormClosing += (s,e) => { if(servis.Urunler.Count > 0 && MessageBox.Show(this,"Uygulama kapanınca bellekteki veriler silinir. Çıkılsın mı?","Çıkış",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true; };
  }
  // Giris ve cikis ayni miktar penceresini kullanir; iptal stogu degistirmez.
  private void StokMiktariSor(HareketTuru tur)
  {
   bool giris = tur == HareketTuru.Giris;
   var urun = servis.Urunler.FirstOrDefault(u => u.Kod == seciliKod);
   if (urun == null) { Uyari("Önce işlem yapmak istediğiniz ürünü listeden seçin."); return; }
   using (var pencere = new Form())
   {
    pencere.Text = (giris ? "Stok girişi | " : "Stok çıkışı | ") + urun.Kod;
    pencere.ClientSize = new Size(440, 215);
    pencere.Font = Font; pencere.FormBorderStyle = FormBorderStyle.FixedDialog;
    pencere.MaximizeBox = false; pencere.MinimizeBox = false;
    pencere.StartPosition = FormStartPosition.CenterParent; pencere.ShowInTaskbar = false;
    var bilgi = new Label { Text = urun.Ad + " — Mevcut stok: " + urun.Stok, AutoEllipsis = true, Location = new Point(18,18), Size = new Size(404,28) };
    var soru = new Label { Text = giris ? "Kaç adet eklenecek?" : "Kaç adet silinecek?", AutoSize = true, Location = new Point(18,58) };
    var adet = new NumericUpDown { Minimum = 1, Maximum = 1000000, Value = 1, ThousandsSeparator = true, Location = new Point(18,88), Size = new Size(200,28) };
    var ekle = new Button { Text = giris ? "Ekle" : "Stoktan düş", Location = new Point(220,158), Size = new Size(95,32) };
    var iptal = new Button { Text = "İptal", DialogResult = DialogResult.Cancel, Location = new Point(325,158), Size = new Size(95,32) };
    ekle.Click += (sender,e) =>
    {
     pencere.ValidateChildren();
     try
     {
      servis.StokIsle(urun.Kod,(int)adet.Value,tur);
      pencere.DialogResult = DialogResult.OK; pencere.Close();
     }
     catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is OverflowException)
     {
      MessageBox.Show(pencere,ex is OverflowException ? "Stok miktarı izin verilen sınırı aşıyor." : ex.Message,"İşlem yapılamadı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
     }
    };
    pencere.Controls.AddRange(new Control[] { bilgi,soru,adet,ekle,iptal });
    pencere.AcceptButton = ekle; pencere.CancelButton = iptal;
    pencere.Shown += (sender,e) => { adet.Focus(); adet.Select(0,adet.Text.Length); };
    if (pencere.ShowDialog(this) == DialogResult.OK)
    {
     Yenile();
     string sonuc = giris ? "Stok girişi tamamlandı." : "Stok çıkışı tamamlandı.";
     string islem = giris ? "Eklenen miktar: " : "Çıkarılan miktar: ";
     MessageBox.Show(this, sonuc + Environment.NewLine +
      "Ürün: " + urun.Ad + " (" + urun.Kod + ")" + Environment.NewLine +
      islem + (int)adet.Value + " adet" + Environment.NewLine +
      "Güncel stok: " + urun.Stok + " adet",
      "İşlem başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
   }
  }
  private void UrunDuzenle()
  {
   var urun = servis.Urunler.FirstOrDefault(u => u.Kod == seciliKod);
   if (urun == null) { Uyari("Önce listeden güncellemek istediğiniz ürünü seçin."); return; }
   using (var pencere = new UrunDuzenleForm(urun, servis.Urunler.Select(u => u.Kategori).Concat(kategori.Items.Cast<string>()),
    (yeniKod, yeniAd, yeniKategori, yeniFiyat) => servis.Guncelle(urun.Kod, yeniKod, yeniAd, yeniKategori, yeniFiyat)))
   {
    if (pencere.ShowDialog(this) != DialogResult.OK) return;
    seciliKod = urun.Kod;
    Yenile();
    MessageBox.Show(this, "Ürün güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }
  }
  private void Calistir(Action islem,string mesaj) { try { ValidateChildren(); islem(); Yenile(); if (!string.IsNullOrEmpty(mesaj)) MessageBox.Show(this,mesaj,"Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information); } catch(Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is OverflowException) { Uyari(ex is OverflowException ? "Stok miktarı izin verilen sınırı aşıyor." : ex.Message); } }
  private void Uyari(string mesaj) { MessageBox.Show(this,mesaj,"İşlem yapılamadı",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
  private void Yenile()
  {
   string onceki = filtre.SelectedItem as string; yenileniyor = true; filtre.Items.Clear(); filtre.Items.Add("Tüm kategoriler"); foreach(var k in servis.Urunler.Select(u=>u.Kategori).Distinct().OrderBy(x=>x)) filtre.Items.Add(k); filtre.SelectedItem = onceki ?? "Tüm kategoriler"; if(filtre.SelectedIndex < 0) filtre.SelectedIndex=0; yenileniyor=false;
   ozet.Text = string.Format("{0} ürün     |     {1} toplam stok     |     {2} kritik ürün",servis.Urunler.Count,servis.Urunler.Sum(u=>(long)u.Stok),servis.Urunler.Count(u=>u.KritikMi));
   hareket.DataSource = servis.Hareketler.Reverse().Select(h => new { Tarih = h.Tarih.ToString("dd.MM.yyyy HH:mm:ss"), Kod=h.UrunKodu, İşlem=h.Tur == HareketTuru.Giris ? "Giriş" : h.Tur == HareketTuru.Cikis ? "Çıkış" : "Başlangıç", h.Miktar, h.SonStok }).ToList(); Listele();
  }
  // Filtreleri birlikte sifirla, ardindan tum urunleri bir kez listele.
  private void FiltreleriTemizle()
  {
   yenileniyor = true;
   try
   {
    arama.Clear();
    if (filtre.Items.Count > 0) filtre.SelectedIndex = 0;
    sadeceKritik.Checked = false;
   }
   finally { yenileniyor = false; }
   Listele();
   MessageBox.Show(this, "Arama ve filtreler temizlendi. Tüm ürünler listeleniyor.",
    "Filtreler temizlendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
   arama.Focus();
  }
  private void Listele()
  {
   if(yenileniyor) return; yenileniyor=true; string onceki=seciliKod; string metin=arama.Text.Trim(); string kat=filtre.SelectedIndex > 0 ? filtre.SelectedItem as string : null;
   tablo.DataSource = servis.Urunler.Where(u => (CultureInfo.GetCultureInfo("tr-TR").CompareInfo.IndexOf(u.Ad,metin,CompareOptions.IgnoreCase)>=0 || u.Kod.IndexOf(metin,StringComparison.OrdinalIgnoreCase)>=0) && (kat==null || u.Kategori==kat) && (!sadeceKritik.Checked || u.KritikMi)).Select(u=>new { u.Kod, u.Ad, u.Kategori, u.BirimFiyat, u.Stok, u.KritikSeviye, Durum=u.DurumAciklamasi(), u.StokDegeri }).ToList();
   tablo.Columns["BirimFiyat"].HeaderText="Birim fiyat"; tablo.Columns["BirimFiyat"].DefaultCellStyle.Format="N2"; tablo.Columns["KritikSeviye"].HeaderText="Otomatik kritik seviye"; tablo.Columns["StokDegeri"].HeaderText="Stok değeri"; tablo.Columns["StokDegeri"].DefaultCellStyle.Format="N2";
   tablo.ClearSelection(); foreach(DataGridViewRow row in tablo.Rows) { var urun=servis.Urunler.First(u=>u.Kod==(string)row.Cells["Kod"].Value); if(urun.KritikMi) { row.DefaultCellStyle.BackColor=Color.MistyRose; row.DefaultCellStyle.ForeColor=Color.DarkRed; row.DefaultCellStyle.SelectionBackColor=Color.Firebrick; row.DefaultCellStyle.SelectionForeColor=Color.White; row.Cells["Durum"].ToolTipText="Otomatik kritik değer 0–10 arasındadır: en yüksek stokun %20'si yukarı yuvarlanır ve en fazla 10 alınır. Sıfır stok her zaman uyarılır."; } if(urun.Kod==onceki) row.Selected=true; }
   yenileniyor=false; SecimYukle();
  }
  // Yeni urun alanlari satir seciminden bagimsizdir; duzenleme ayri pencerededir.
  private void SecimYukle()
  {
   if (yenileniyor) return;
   seciliKod = tablo.SelectedRows.Count == 0 ? null : (string)tablo.SelectedRows[0].Cells["Kod"].Value;
  }
  private void Temizle() { seciliKod=null; kod.ReadOnly=false; kod.Clear(); ad.Clear(); kategori.SelectedIndex=-1; kategori.Text=""; fiyat.Value=0; stok.Value=0; stok.Enabled=true; if(!yenileniyor) { yenileniyor=true; tablo.ClearSelection(); tablo.CurrentCell=null; yenileniyor=false; } }
 }
}


