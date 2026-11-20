using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _24083001_ceydasahin.Models;
namespace _24083001_ceydasahin
{
 // Veriler Kaydet'e kadar yalnizca form kontrollerinde tutulur.
 public sealed class UrunDuzenleForm : Form
 {
  private readonly TextBox kod = new TextBox { MaxLength = 40, Dock = DockStyle.Fill };
  private readonly TextBox ad = new TextBox { MaxLength = 100, Dock = DockStyle.Fill };
  private readonly ComboBox kategori = new ComboBox { MaxLength = 60, Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown };
  private readonly NumericUpDown fiyat = new NumericUpDown { Maximum = 100000000, DecimalPlaces = 2, ThousandsSeparator = true, Dock = DockStyle.Fill };
  private readonly Urun urun;
  private readonly Action<string, string, string, decimal> kaydet;
  public UrunDuzenleForm(Urun urun, IEnumerable<string> kategoriler, Action<string, string, string, decimal> kaydet)
  {
   this.urun = urun; this.kaydet = kaydet;
   Text = "Ürün bilgilerini güncelle | " + urun.Kod;
   Font = new Font("Segoe UI", 10); ClientSize = new Size(510, 350);
   FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false;
   StartPosition = FormStartPosition.CenterParent; ShowInTaskbar = false;
   var alanlar = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), ColumnCount = 2, RowCount = 8 };
   alanlar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); alanlar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
   for (int i = 0; i < 8; i++) alanlar.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 0 ? 44 : i == 6 ? 48 : 35));
   var bilgi = new Label { Text = "Hangi bilgileri güncellemek istiyorsunuz? Alanları düzenleyip Kaydet'e basın.", Dock = DockStyle.Fill };
   alanlar.Controls.Add(bilgi,0,0); alanlar.SetColumnSpan(bilgi,2);
   Satir(alanlar,1,"Ürün kodu",kod);
   Satir(alanlar,2,"Ürün adı",ad); Satir(alanlar,3,"Kategori",kategori);
   Satir(alanlar,4,"Birim fiyat (₺)",fiyat); Satir(alanlar,5,"Otomatik kritik seviye",new Label { Text = urun.KritikSeviye.ToString() + " adet (min 0, maks 10)", Dock = DockStyle.Fill });
   var stokBilgi = new Label { Text = "Mevcut stok: " + urun.Stok + ". Stok miktarını ana ekrandaki stok giriş/çıkış işlemleriyle değiştirin.", Dock = DockStyle.Fill, ForeColor = Color.DimGray };
   alanlar.Controls.Add(stokBilgi,0,6); alanlar.SetColumnSpan(stokBilgi,2);
   var butonlar = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
   var iptal = new Button { Text = "İptal", DialogResult = DialogResult.Cancel, AutoSize = true };
   var onay = new Button { Text = "Kaydet", AutoSize = true };
   onay.Click += Kaydet; butonlar.Controls.Add(iptal); butonlar.Controls.Add(onay);
   alanlar.Controls.Add(butonlar,0,7); alanlar.SetColumnSpan(butonlar,2); Controls.Add(alanlar);
   AcceptButton = onay; CancelButton = iptal;
   kategori.Items.AddRange(kategoriler.Distinct().OrderBy(k => k).Cast<object>().ToArray());
   kod.Text = urun.Kod; ad.Text = urun.Ad; kategori.Text = urun.Kategori; fiyat.Value = urun.BirimFiyat;
  }
  private static void Satir(TableLayoutPanel panel,int satir,string baslik,Control kontrol)
  {
   panel.Controls.Add(new Label { Text = baslik, AutoSize = true, Anchor = AnchorStyles.Left },0,satir);
   panel.Controls.Add(kontrol,1,satir);
  }
  private void Kaydet(object sender,EventArgs e)
  {
   ValidateChildren();
   if (kod.Text.Trim().ToUpperInvariant() == urun.Kod && ad.Text.Trim() == urun.Ad && kategori.Text.Trim() == urun.Kategori && fiyat.Value == urun.BirimFiyat)
   {
    MessageBox.Show(this,"Henüz bir bilgi değiştirmediniz. Güncellemek istediğiniz alanı düzenleyin veya İptal'e basın.","Değişiklik yok",MessageBoxButtons.OK,MessageBoxIcon.Information); return;
   }
   try
   {
    kaydet(kod.Text,ad.Text,kategori.Text,fiyat.Value);
    DialogResult = DialogResult.OK; Close();
   }
   catch (ArgumentException ex)
   {
    MessageBox.Show(this,ex.Message,"Bilgileri kontrol edin",MessageBoxButtons.OK,MessageBoxIcon.Warning);
   }
  }
 }
}
