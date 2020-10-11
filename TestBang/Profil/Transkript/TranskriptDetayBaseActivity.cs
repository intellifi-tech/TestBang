using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.Apache.Http.Impl.Client;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;
using static TestBang.Profil.Transkript.TranskriptListeBaseActivity;
using Color = iTextSharp.text.Color;
using Font = System.Drawing.Font;

namespace TestBang.Profil.Transkript
{
    [Activity(Label = "TestBang")]
    public class TranskriptDetayBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        FrameLayout PuanHazne;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TranskriptDetayRecyclerViewAdapter mViewAdapter;
        List<DenemeCozumKonuDetayDTO> DenemeCozumKonuDetayDTO1 = new List<DenemeCozumKonuDetayDTO>();
        TextView AdSoyadText, DenemeDetayText;
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];


        TextView DogruYuzde, DogruSayi, YanlisYuzde, YanlisSayi, BosYuzde, BosSayi;
        ProgressBar DogruProgres, YanlisProgres, BosProgres;
        Button TranskriptPaylas;
        RelativeLayout SiralamaHaznesi;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TranskriptDetayBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            AdSoyadText= FindViewById<TextView>(Resource.Id.adsoyadtext);
            DenemeDetayText = FindViewById<TextView>(Resource.Id.denemetarih);
            PuanHazne = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            PuanLayoutYerlestir();
            AdSoyadText.Text = Me.firstName.ToUpper() + " " + Me.lastName.ToUpper();
            DenemeDetayText.Text = ((DateTime)TranskriptDetayBaseActivity_Helper.SecilenDeneme.finishDate).ToString("dd MMMM yyyy");
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            SiralamaHaznesi = FindViewById<RelativeLayout>(Resource.Id.relativeLayout5);
            TranskriptPaylas = FindViewById<Button>(Resource.Id.button3);
            TranskriptPaylas.Click += TranskriptPaylas_Click;

            mRecyclerView.HasFixedSize = true;


            DogruYuzde = FindViewById<TextView>(Resource.Id.dogrucevapyuzde);
            DogruSayi = FindViewById<TextView>(Resource.Id.dogrucevapsayi);
            YanlisYuzde = FindViewById<TextView>(Resource.Id.yalniscevapyuzde);
            YanlisSayi = FindViewById<TextView>(Resource.Id.yalniscevapsayi);
            BosYuzde = FindViewById<TextView>(Resource.Id.boscevapyuzde);
            BosSayi = FindViewById<TextView>(Resource.Id.boscevapsayi);
            DogruProgres = FindViewById<ProgressBar>(Resource.Id.dogruprogress);
            YanlisProgres = FindViewById<ProgressBar>(Resource.Id.yanlisprogress);
            BosProgres = FindViewById<ProgressBar>(Resource.Id.bosprogress);

            DogruYuzde.Text = "%0";
            YanlisYuzde.Text = "%0";
            BosYuzde.Text = "%0";
            DogruSayi.Text = "0";
            YanlisSayi.Text = "0";
            BosSayi.Text = "0";
            DogruProgres.Progress = 0;
            YanlisProgres.Progress = 0;
            BosProgres.Progress = 0;


            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            builder.DetectFileUriExposure();
        }

        private void TranskriptPaylas_Click(object sender, EventArgs e)
        {
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) == Permission.Granted
               && ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted)
            {
                ShowLoading.Show(this,"Lütfen Bekleyin.");
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    PdfOlustur();
                })).Start();
            }
            else
            {
                RequestPermissions(new String[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage}, 111);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) == Permission.Granted
               && ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted)
            {

                ShowLoading.Show(this, "Lütfen Bekleyin.");
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    PdfOlustur();
                })).Start();
            }
            else
            {
                RequestPermissions(new String[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, 111);
            }
        }

        void PdfOlustur()
        {
            

            string path2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder

            if (Directory.Exists(path2 + "/" + "TestBang Dosyalar") == false)
            {
                Directory.CreateDirectory(path2 + "/" + "TestBang Dosyalar");
            }
            //string fontt = path2 + "/" + "Fizibilite Dosyalar/arial.ttf";

            //iTextSharp.text.Font Kirmizi = FontlariGetir1.KirmiziHeader();
            //iTextSharp.text.Font Siyahh = FontlariGetir1.SiyahAnaHeader();
            PdfPTable table1 = new PdfPTable(1);
            //PdfPTable table2 = new PdfPTable(1);
            PdfPTable table3 = new PdfPTable(2);
            PdfPTable table5 = new PdfPTable(1);
            PdfPTable table6 = new PdfPTable(1);
            PdfPTable table7 = new PdfPTable(1);
            PdfPTable table8 = new PdfPTable(7);
            PdfPTable table9 = new PdfPTable(1);
            PdfPTable table10 = new PdfPTable(1);
            PdfPTable table11 = new PdfPTable(1);
            PdfPTable table12 = new PdfPTable(1);


            table1.HorizontalAlignment= Element.ALIGN_CENTER;
            //table2.HorizontalAlignment = Element.ALIGN_CENTER;
            table3.HorizontalAlignment = Element.ALIGN_CENTER;
            table5.HorizontalAlignment = Element.ALIGN_CENTER;
            table6.HorizontalAlignment = Element.ALIGN_CENTER;
            table7.HorizontalAlignment = Element.ALIGN_CENTER;
            table8.HorizontalAlignment = Element.ALIGN_CENTER;
            table9.HorizontalAlignment = Element.ALIGN_CENTER;
            table10.HorizontalAlignment = Element.ALIGN_CENTER;
            table11.HorizontalAlignment = Element.ALIGN_CENTER;
            table12.HorizontalAlignment = Element.ALIGN_CENTER;



            SiralamaLogoEkle(createBitmap_Dinamik_View(SiralamaHaznesi), 300, table1);
           // TestBangLogoEkle(table2);
            
            PDFMetinEkle("Transkript No: " , TranskriptDetayBaseActivity_Helper.SecilenDeneme.TanskriptNo.ToString(), table3);
            PDFMetinEkle("Ad Soyad: " , Me.firstName + " "+Me.lastName, table3);
            PDFMetinEkle("Deneme Adı: " , TranskriptDetayBaseActivity_Helper.SecilenDeneme.name, table3);
            PDFMetinEkle("Deneme Açıklaması: " , TranskriptDetayBaseActivity_Helper.SecilenDeneme.description, table3);
            PDFMetinEkle("Sınav Tarihi: " , ((DateTime)TranskriptDetayBaseActivity_Helper.SecilenDeneme.finishDate).ToString("dd MMMM yyyy"), table3);
            //PDFMetinEkle("Deneme sınavı performanslarınızın genel değerlendirmesini aşağıda bulabilirsiniz.","", table3);
           // PDFBitmapEkle(createBitmap_Dinamik_View(SiralamaHaznesi), 300, table1);
            PDFBitmapEkle(createBitmap_Dinamik_View(PuanView), 500,300, table5);
            PDFBitmapEkle(createBitmap_Dinamik_View(FindViewById<LinearLayout>(Resource.Id.linearLayout10)), 500,500, table6);
            PDFMetinEkleTekil("KONULAR", table7, Element.ALIGN_LEFT);
            PDFMetinEkleTekil("DENEME DETAYLARI", table9, Element.ALIGN_LEFT);

            PDFMetinEkleTekil2("Kim demiş eğitim sadece okulda olur diye? ", table12);
            PDFMetinEkleTekil2("Test Bang! ile dilediğin yerde, dilediğin zaman, sana özel takvim ve programlarla kendini sınavlara hazırla,", table12);
            PDFMetinEkleTekil2("Türkiye çapında rekabete hemen şimdi başla.", table12);

            StoreLogoEkle(table10, Resource.Mipmap.storelink1, "http://test-bang.com/");
            StoreLogoEkle(table11, Resource.Mipmap.storelink2, "http://test-bang.com/");

            string[] Colonlar = { "KONU", "CEVAPSIZ", "DOĞRU", "YALNIŞ" };

            for (int i = 0; i < Colonlar.Length; i++)
            {
                var bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                PdfPCell HederCell = new PdfPCell(new Phrase(Colonlar[i], bold));
                HederCell.HorizontalAlignment = Element.ALIGN_CENTER;
                HederCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                HederCell.BorderWidth = 1;
                if (i==0)
                {
                    HederCell.Colspan = 4;
                }
                else
                {
                    HederCell.Colspan = 1;
                }
                table8.AddCell(HederCell);
            }


            for (int i = 0; i < DenemeCozumKonuDetayDTO1.Count; i++)
            {
                for (int i2 = 0; i2 < 4; i2++)
                {
                    
                    PdfPCell HederCell;
                    if (i2==0)
                    {
                        var regular = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                        HederCell = new PdfPCell(new Phrase(DenemeCozumKonuDetayDTO1[i].topicName, regular));
                        HederCell.Colspan = 4;
                        HederCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else if (i2 == 1)
                    {
                        var regular = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12,Color.BLACK);
                        HederCell = new PdfPCell(new Phrase(DenemeCozumKonuDetayDTO1[i].emptyCount.ToString()));
                        HederCell.Colspan = 1;
                        HederCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    else if (i2 == 2)
                    {
                        var regular = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Color.GREEN);
                        HederCell = new PdfPCell(new Phrase(DenemeCozumKonuDetayDTO1[i].correctCount.ToString(), regular));
                        HederCell.Colspan = 1;
                        HederCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    else 
                    {
                        var regular = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Color.RED);
                        HederCell = new PdfPCell(new Phrase(DenemeCozumKonuDetayDTO1[i].wrongCount.ToString(), regular));
                        HederCell.Colspan = 1;
                        HederCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    
                    HederCell.VerticalAlignment = Element.ALIGN_CENTER;
                    HederCell.BorderWidth = 1;
                    table8.AddCell(HederCell);
                }
            }


            var documents = Android.OS.Environment.ExternalStorageDirectory.Path;
            string fileLocation = documents + "/" + GetFileName() + ".pdf";
            Java.IO.File file = new Java.IO.File(fileLocation);
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }

            var fs = new FileStream(fileLocation, FileMode.Create);
            Document document = new Document(PageSize.A4, 0, 0, 100, 60);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new ITextEvents(this);
            document.Open();
            try
            {
                document.Add(BoslukEkle());
                document.Add(table1);
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(table5);
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(table6);
                document.Add(BoslukEkle());
                document.Add(table9);
                document.Add(BoslukEkle());
                document.Add(table3); 
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
                document.Add(table7);
                document.Add(BoslukEkle());
                document.Add(table8);
                document.Add(BoslukEkle());
                document.Add(BoslukEkle());
               
                document.Add(table12);
                document.Add(BoslukEkle());
                document.Add(table10);
                document.Add(table11);

            }
            catch (Exception ex)
            {
                var sss = ex.Message;
                this.RunOnUiThread(() =>
                {
                    ShowLoading.Hide();
                    AlertDialog.Builder SDAlert = new AlertDialog.Builder(this);
                    SDAlert.SetIcon(Resource.Mipmap.custom_alert_icon);
                    SDAlert.SetTitle("Yetersiz disk alanı.");
                    SDAlert.SetMessage("SD kart üzerinde boş alan yaratıp tekrar deneyin.");
                    SDAlert.SetPositiveButton("Tamam", delegate
                    {
                        SDAlert.Dispose();
                        //this.Finish();
                        return;
                    });
                    SDAlert.Show();
                });
                return;
            }
            document.Close();
            writer.Close();
            fs.Close();

            this.RunOnUiThread(() =>
            {
                ShowLoading.Hide();
                AlertDialog.Builder cevap2 = new AlertDialog.Builder(this);
                cevap2.SetIcon(Resource.Mipmap.custom_alert_icon);
                cevap2.SetTitle("İşlem Seçin");
                cevap2.SetMessage("Döküman Oluşturuldu.");
                cevap2.SetPositiveButton(Spannla(Android.Graphics.Color.Black,"Aç"), delegate
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(file), "application/pdf");
                    StartActivity(intent);
                    cevap2.Dispose();
                });
                cevap2.SetNegativeButton(Spannla(Android.Graphics.Color.Black, "Paylaş"), delegate
                {

                    var sharingIntent = new Intent();
                    sharingIntent.SetAction(Intent.ActionSend);
                    sharingIntent.SetType("application/pdf");
                    sharingIntent.PutExtra(Intent.ExtraText, "Paylas");
                    sharingIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(file));
                    sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    StartActivity(Intent.CreateChooser(sharingIntent, "Döküman Paylaş"));

                    cevap2.Dispose();
                });
                cevap2.Show();
            });
        
        }
        SpannableStringBuilder Spannla(Android.Graphics.Color Renk, string textt)
        {
            ForegroundColorSpan foregroundColorSpan = new ForegroundColorSpan(Renk);

            string title = textt;
            SpannableStringBuilder ssBuilder = new SpannableStringBuilder(title);
            ssBuilder.SetSpan(
                    foregroundColorSpan,
                    0,
                    title.Length,
                    SpanTypes.ExclusiveExclusive
            );
            return ssBuilder;
        }

        void SiralamaLogoEkle(Android.Graphics.Bitmap bitmap, int Yukseklik, PdfPTable table)
        {
            var logo = BitmapHelpers.LoadAndResizeBitmap2(bitmap, Yukseklik, Yukseklik);
            var ms1 = new MemoryStream();
            logo.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms1);
            var logobyte = ms1.ToArray();
            iTextSharp.text.Image LogoIcon = iTextSharp.text.Image.GetInstance(logobyte);
            LogoIcon.ScaleToFit(100, 100);
            PdfPCell logocell = new PdfPCell(LogoIcon);
            logocell.Colspan = 1;
            //  logocell.FixedHeight = Yukseklik+20;
            logocell.Border = 0;
            logocell.HorizontalAlignment = Element.ALIGN_CENTER;
            logocell.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(logocell);
        }
        

        void PDFBitmapEkle(Android.Graphics.Bitmap bitmap, int Yukseklik, int ScaleYukseklik, PdfPTable table)
        {
            var logo = BitmapHelpers.LoadAndResizeBitmap2(bitmap, Yukseklik, Yukseklik);
            var ms1 = new MemoryStream();
            logo.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms1);
            var logobyte = ms1.ToArray();
            iTextSharp.text.Image LogoIcon = iTextSharp.text.Image.GetInstance(logobyte);
            LogoIcon.ScaleToFit(ScaleYukseklik, ScaleYukseklik);
            PdfPCell logocell = new PdfPCell(LogoIcon);
            logocell.Colspan = 1;
          //  logocell.FixedHeight = Yukseklik+20;
            logocell.Border = 0;
            logocell.HorizontalAlignment = Element.ALIGN_CENTER;
            logocell.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(logocell);
        }


        void PDFMetinEkle(string Baslik, string Metin, PdfPTable table)
        {
            var regular = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,12);
            PdfPCell BaslikCell = new PdfPCell(new Phrase(Baslik, bold));
            BaslikCell.Colspan = 1;
            BaslikCell.FixedHeight = 20f;
            BaslikCell.HorizontalAlignment = Element.ALIGN_LEFT;
            BaslikCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            table.AddCell(BaslikCell);

            PdfPCell MetinCell = new PdfPCell(new Phrase(Metin,regular));
            MetinCell.Colspan = 2;
            MetinCell.FixedHeight = 20f;
            MetinCell.HorizontalAlignment = Element.ALIGN_LEFT;
            BaslikCell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
            table.AddCell(MetinCell);

        }
        void PDFMetinEkleTekil(string Metin, PdfPTable table,int yon)
        {
            var bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);
            PdfPCell BaslikCell = new PdfPCell(new Phrase(Metin, bold));
            BaslikCell.Colspan = 1;
            BaslikCell.FixedHeight = 20f;
            BaslikCell.HorizontalAlignment = yon;
            BaslikCell.Border = 0;
            table.AddCell(BaslikCell);
        }
        void PDFMetinEkleTekil2(string Metin, PdfPTable table)
        {
            var bold = FontFactory.GetFont(FontFactory.HELVETICA, 10f);
            PdfPCell BaslikCell = new PdfPCell(new Phrase(Metin, bold));
            BaslikCell.Colspan = 1;
            BaslikCell.HorizontalAlignment = Element.ALIGN_CENTER;
            BaslikCell.Border = 0;
            table.AddCell(BaslikCell);
        }

        PdfPTable BoslukEkle()
        {
            PdfPTable table = new PdfPTable(1);
            PdfPCell KullaniciAdSoyadHeader = new PdfPCell(new Phrase(""));
            KullaniciAdSoyadHeader.Colspan = 1;
            KullaniciAdSoyadHeader.FixedHeight = 20f;
            KullaniciAdSoyadHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            KullaniciAdSoyadHeader.Border = 0;
            KullaniciAdSoyadHeader.BorderColor = Color.WHITE;

            table.AddCell(KullaniciAdSoyadHeader);
            return table;
        }


        void StoreLogoEkle(PdfPTable table,int imgg,string urll)
        {
            var logo = BitmapHelpers.LoadAndResizeBitmap(imgg, 500, 500, this);
            var ms1 = new MemoryStream();
            logo.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms1);
            var logobyte = ms1.ToArray();
            iTextSharp.text.Image LogoIcon = iTextSharp.text.Image.GetInstance(logobyte);
            LogoIcon.ScaleToFit(220, 220);
            Chunk cImage = new Chunk(LogoIcon, 0, 0, false);

            Anchor anchor = new Anchor(cImage);
            anchor.Reference = urll;

            PdfPCell logocell = new PdfPCell(anchor);
            logocell.Colspan = 1;
            //logocell.FixedHeight = 100f;
            logocell.Border = 0;
            logocell.BorderWidthLeft = 0;           //İCON
            logocell.BorderWidthBottom = 0;
            logocell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(logocell);
        }



        string GetFileName()
        {
            return  Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }


        public Android.Graphics.Bitmap createBitmap_Dinamik_View(View GelenView)
        {
            try
            {
                GelenView.DrawingCacheEnabled = true;
                GelenView.BuildDrawingCache();
                Android.Graphics.Bitmap bm = GelenView.GetDrawingCache(true);
                return bm;
            }
            catch(System.Exception EX)
            {
                var aaa = EX.Message;
                return null;

            }
            
        }
        View PuanView;
        void PuanLayoutYerlestir()
        {
            ShowLoading.Show(this, "Lütfen Bekleyin");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Jsonn = webService.OkuGetir("user-trial-results/user/" + TranskriptDetayBaseActivity_Helper.SecilenDeneme.id.ToString(), UsePoll: true);
                if (Jsonn != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<DenemeSonuclariPaunDTO>(Jsonn.ToString());
                    if (Icerik != null)
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            if (TranskriptDetayBaseActivity_Helper.SecilenDeneme.type == "TYT")
                            {
                                LayoutInflater inflater = LayoutInflater.From(this);
                                PuanView = inflater.Inflate(Resource.Layout.TYTPuanTranskriptUI, PuanHazne, false);
                                PuanView.FindViewById<TextView>(Resource.Id.textView4).Text = Math.Round((double)Icerik.point + 100, 5).ToString();
                                PuanHazne.AddView(PuanView);
                            }
                            else
                            {
                                LayoutInflater inflater = LayoutInflater.From(this);
                                PuanView = inflater.Inflate(Resource.Layout.AYTPuanTranskriptUI, PuanHazne, false);
                                PuanView.FindViewById<TextView>(Resource.Id.saypuan).Text = Math.Round((double)Icerik.sayPoint + 100, 5).ToString();
                                PuanView.FindViewById<TextView>(Resource.Id.sozpuan).Text = Math.Round((double)Icerik.sozPoint + 100, 5).ToString();
                                PuanView.FindViewById<TextView>(Resource.Id.eapuan).Text = Math.Round((double)Icerik.eaPoint + 100, 5).ToString();
                                PuanHazne.AddView(PuanView);
                            }
                        });
                    }
                }
                ShowLoading.Hide();
            })).Start();
        }

        protected override void OnStart()
        {
            base.OnStart();
            ListeyiOlustur();
        }
        void ListeyiOlustur()
        {
            #region Genislik Alır
            int width = 0;
            int height = 0;

            mRecyclerView.Post(() =>
            {
                width = mRecyclerView.Width;
                height = mRecyclerView.Height;
                var Genislikk = width / 2;

                WebService webService = new WebService();
                var Donus2 = webService.OkuGetir("trial-informations/user/topic/" + TranskriptDetayBaseActivity_Helper.SecilenDeneme.id, UsePoll: true);
                if (Donus2 != null)
                {
                    DenemeCozumKonuDetayDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DenemeCozumKonuDetayDTO>>(Donus2.ToString());
                    if (DenemeCozumKonuDetayDTO1.Count > 0)
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            mViewAdapter = new TranskriptDetayRecyclerViewAdapter(DenemeCozumKonuDetayDTO1, this, Genislikk);
                            mRecyclerView.SetAdapter(mViewAdapter);
                            mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                            mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));
                            mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                            var layoutManager = new GridLayoutManager(this, 2);
                            mRecyclerView.SetLayoutManager(layoutManager);
                            GrafikleriOlustur();
                        });
                    }
                    else
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            DogruYuzde.Text = "%0";
                            YanlisYuzde.Text = "%0";
                            BosYuzde.Text = "%0";
                            DogruSayi.Text = "0";
                            YanlisSayi.Text = "0";
                            BosSayi.Text = "0";
                            DogruProgres.Progress = 0;
                            YanlisProgres.Progress = 0;
                            BosProgres.Progress = 0;
                        });
                    }
                }
                else
                {
                    this.RunOnUiThread(delegate ()
                    {
                        DogruYuzde.Text = "%0";
                        YanlisYuzde.Text = "%0";
                        BosYuzde.Text = "%0";
                        DogruSayi.Text = "0";
                        YanlisSayi.Text = "0";
                        BosSayi.Text = "0";
                        DogruProgres.Progress = 0;
                        YanlisProgres.Progress = 0;
                        BosProgres.Progress = 0;
                    });
                }
            });

            #endregion
        }
        void GrafikleriOlustur()
        {
            this.RunOnUiThread(delegate ()
            {
                var sumOfCorrect = DenemeCozumKonuDetayDTO1.Sum(item => item.correctCount);
                var sumOfWrong = DenemeCozumKonuDetayDTO1.Sum(item => item.wrongCount);
                var sumOfEmpty = DenemeCozumKonuDetayDTO1.Sum(item => item.emptyCount);

                var toplam = sumOfCorrect + sumOfWrong + sumOfEmpty;

                DogruSayi.Text = sumOfCorrect.ToString();
                YanlisSayi.Text = sumOfWrong.ToString();
                BosSayi.Text = sumOfEmpty.ToString();

                DogruYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * sumOfCorrect) / toplam)), 0);
                YanlisYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * sumOfWrong) / toplam)), 0);
                BosYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * sumOfEmpty) / toplam)), 0);

                DogruProgres.Progress = Convert.ToInt32(DogruYuzde.Text.Replace("%", ""));
                YanlisProgres.Progress = Convert.ToInt32(YanlisYuzde.Text.Replace("%", ""));
                BosProgres.Progress = Convert.ToInt32(BosYuzde.Text.Replace("%", ""));
            });
        }
        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            
        }

        public class DenemeCozumKonuDetayDTO
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string trialType { get; set; }
            public string trialId { get; set; }
            public int correctCount { get; set; }//
            public int wrongCount { get; set; }//
            public int emptyCount { get; set; }//
            public string net { get; set; }
            public string point { get; set; }
            public string lessonId { get; set; }//
            public string lessonName { get; set; }//
            public string topicId { get; set; }
            public string topicName { get; set; }
            public string trialPoint { get; set; }
        }

        public class DenemeSonuclariPaunDTO
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string trialType { get; set; }
            public string trialId { get; set; }
            public int? correctCount { get; set; }
            public int? wrongCount { get; set; }
            public int? emptyCount { get; set; }
            public double? net { get; set; }
            public double? point { get; set; }
            public double? sayPoint { get; set; }
            public double? sozPoint { get; set; }
            public double? eaPoint { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public string topicId { get; set; }
            public string topicName { get; set; }
            public string trialPoint { get; set; }
            public DateTime? trialDate { get; set; }
            public List<LessonResult> lessonResults { get; set; }
            public string order { get; set; }
        }

        public class LessonResult
        {
            public string lessonId { get; set; }
            public string lesonName { get; set; }
            public int? correctCount { get; set; }
            public int? wrongCount { get; set; }
            public int? emptyCount { get; set; }
            public double? net { get; set; }
        }


        public static class TranskriptDetayBaseActivity_Helper
        {
            public static TranskriptListDTO SecilenDeneme { get; set; }
        }

        public class ITextEvents : PdfPageEventHelper
        {
            Context context1;
            public ITextEvents(Context context2)
            {
                context1 = context2;
            }


            // This is the contentbyte object of the writer
            PdfContentByte cb;

            // we will put the final number of pages in a template
            PdfTemplate headerTemplate, footerTemplate;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;

            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {
                }
                catch (System.IO.IOException ioe)
                {
                }
            }

            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);
                var baseFontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var baseFontBig = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
              //  Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);

                //Create PdfTable object
                PdfPTable pdfTab = new PdfPTable(3);

                //We will have to create separate cells to include image logo and 2 separate strings
                //Row 1
                PdfPCell pdfCell1 = new PdfPCell();
                PdfPCell pdfCell2 = new PdfPCell(TestBangLogoEkle());
                PdfPCell pdfCell3 = new PdfPCell();
                String text = "Sayfa " + writer.PageNumber + " / ";

                //Add paging to header
                //{
                //    cb.BeginText();
                //    cb.SetFontAndSize(bf, 12);
                //    cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                //    cb.ShowText(text);
                //    cb.EndText();
                //    float len = bf.GetWidthPoint(text, 12);
                //    //Adds "12" in Page 1 of 12
                //    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
                //}
                //Add paging to footer
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 12);
                    cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 12);
                    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                }

                //Row 2
                PdfPCell pdfCell4 = new PdfPCell(new Phrase("TRANSKRIPT", baseFontNormal));

                //Row 3 
                PdfPCell pdfCell5 = new PdfPCell(new Phrase("TARİH:" + PrintTime.ToShortDateString(), baseFontBig));
                PdfPCell pdfCell6 = new PdfPCell();
                PdfPCell pdfCell7 = new PdfPCell(new Phrase("SAAT:" + string.Format("{0:t}", DateTime.Now), baseFontBig));

                //set the alignment of all three cells and set border to 0
                pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
               // pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

                pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                //pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
                pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

                //pdfCell4.Colspan = 3;

                pdfCell1.Border = 0;
                pdfCell2.Border = 0;
                pdfCell3.Border = 0;
                //pdfCell4.Border = 0;
                pdfCell5.Border = 0;
                pdfCell6.Border = 0;
                pdfCell7.Border = 0;

                //add all three cells into PdfTable
                pdfTab.AddCell(pdfCell1);
                pdfTab.AddCell(pdfCell2);
                pdfTab.AddCell(pdfCell3);
                //pdfTab.AddCell(pdfCell4);
                pdfTab.AddCell(pdfCell5);
                pdfTab.AddCell(pdfCell6);
                pdfTab.AddCell(pdfCell7);

                pdfTab.TotalWidth = document.PageSize.Width - 80f;
                pdfTab.WidthPercentage = 70;
                //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

                //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
                //first param is start row. -1 indicates there is no end row and all the rows to be included to write
                //Third and fourth param is x and y position to start writing
                pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
                //set pdfContent value

                //Move the pointer and draw line to separate header section from rest of page
                cb.MoveTo(40, document.PageSize.Height - 100);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, document.PageSize.GetBottom(50));
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
                cb.Stroke();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 12);
                headerTemplate.SetTextMatrix(0, 0);
                headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.EndText();

                footerTemplate.BeginText();
                footerTemplate.SetFontAndSize(bf, 12);
                footerTemplate.SetTextMatrix(0, 0);
                footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                footerTemplate.EndText();
            }


            PdfPCell TestBangLogoEkle()
            {
                var logo = BitmapHelpers.LoadAndResizeBitmap(Resource.Mipmap.testbang_logo_img1, 500, 500, context1);
                var ms1 = new MemoryStream();
                logo.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms1);
                var logobyte = ms1.ToArray();
                iTextSharp.text.Image LogoIcon = iTextSharp.text.Image.GetInstance(logobyte);
                LogoIcon.ScaleToFit(100, 100);
                PdfPCell logocell = new PdfPCell(LogoIcon);
                logocell.Colspan = 1;
                //logocell.FixedHeight = 100f;
                logocell.Border = 0;
                logocell.BorderWidthLeft = 0;           //İCON
                logocell.BorderWidthBottom = 0;
                logocell.HorizontalAlignment = Element.ALIGN_RIGHT;
                logocell.VerticalAlignment = Element.ALIGN_TOP;
                return logocell;
            }
        }
    }
}