using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TestBang.DataBasee;
using TestBang.Deneme;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Test.TestSinavAlani;
using TestBang.WebServices;

namespace TestBang.Profil.DersProgrami
{
    [Activity(Label = "TestBang")]
    public class DersProgramiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        RecyclerView mRecyclerView;
        Android.Support.V7.Widget.LinearLayoutManager mLayoutManager;
        DersProgramiRecyclerViewAdapter mViewAdapter;
        //public List<DersProgramiDTO> DersProgramiList = new List<DersProgramiDTO>();
        Button YeniEkle;

        List<TakvimTarihlerDTO> TakvimTarihlerDTO1 = new List<TakvimTarihlerDTO>();
        TakvimGridAdapter TakvimGridAdapter1;
        GridView TakvimGrid;
        DateTime SonTakvimTarihi;
        ImageButton TakvimIleriButton, TakvimGeriButton;
        TextView TakvimAyAdiText, TakvimYilText;
        MEMBER_DATA UserData = DataBase.MEMBER_DATA_GETIR()[0];
        List<UzakSunucuTakvimDTO> UzakSunucuTakvimDTO1 = new List<UzakSunucuTakvimDTO>();
        List<UzakSunucuDenemeDTO> UzakSunucuDenemeDTO1 = new List<UzakSunucuDenemeDTO>();
        List<DersProgramiDTO> DersProgramiDTO1 = new List<DersProgramiDTO>();
        MEMBER_DATA MeUser = DataBase.MEMBER_DATA_GETIR()[0];
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DersProgramiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            YeniEkle = FindViewById<Button>(Resource.Id.button2);
            TakvimGrid = FindViewById<GridView>(Resource.Id.gridView1);
            TakvimIleriButton = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            TakvimGeriButton = FindViewById<ImageButton>(Resource.Id.ımageButton2);
            TakvimAyAdiText = FindViewById<TextView>(Resource.Id.takvimayaditext);
            TakvimYilText = FindViewById<TextView>(Resource.Id.takvimyiltext);
            FindViewById<TextView>(Resource.Id.adsoyadtext).Text=MeUser.firstName.ToUpper() + " "+ MeUser.lastName.ToUpper();
            TakvimGrid.ItemClick += TakvimGrid_ItemClick;
            YeniEkle.Visibility = ViewStates.Gone;
            YeniEkle.Click += YeniEkle_Click;
            TakvimIleriButton.Click += TakvimIleriButton_Click;
            TakvimGeriButton.Click += TakvimGeriButton_Click;
            DersProgramiBaseActivityHelper.DersProgramiBaseActivity1 = this;
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();
        private void TakvimGrid_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Butonlarr = new List<Buttons_Image_DataModels>();
            if (TakvimTarihlerDTO1[e.Position].DenemeSinaviVami)
            {
                if (TakvimTarihlerDTO1[e.Position].Tarih > DateTime.Now)
                {
                    DersProgramiBaseActivityHelper.SecilenTarih = (DateTime)TakvimTarihlerDTO1[e.Position].Tarih;
                    var HangiDenemeOnuBul = UzakSunucuDenemeDTO1.Find(item => item.id == TakvimTarihlerDTO1[e.Position].DenemeID);
                    if (HangiDenemeOnuBul!=null)
                    {
                        DersProgramiBaseActivityHelper.SecilenDeneme = HangiDenemeOnuBul;
                        Butonlarr.Add(new Buttons_Image_DataModels()
                        {
                            Button_Text = "Denemeyi Gör/Katıl",
                            Button_Image = Resource.Drawable.eye
                        });
                        Butonlarr.Add(new Buttons_Image_DataModels()
                        {
                            Button_Text = "Takvime Test Ekle",
                            Button_Image = Resource.Drawable.calender_add
                        });

                        DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Deneme sınavı detaylarını görebilir veya bu güne özel bir test ekleyebilirsiniz.", Buton_Click);
                        DinamikActionSheet1.Show(this.SupportFragmentManager, "DinamikActionSheet1");
                    }
                }
                else
                {
                    AlertHelper.AlertGoster("Bu deneme sınavı sonlandı. Yeni test oluşturabilirsiniz.", this);
                    DersProgramiBaseActivityHelper.SecilenTarih = (DateTime)TakvimTarihlerDTO1[e.Position].Tarih;
                    StartActivity(typeof(DersProgramiEkleBaseActivity));
                }
            }
            else
            {
                DersProgramiBaseActivityHelper.SecilenTarih = (DateTime)TakvimTarihlerDTO1[e.Position].Tarih;
                StartActivity(typeof(DersProgramiEkleBaseActivity));
            }
            
        }
        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {
                var TakvimeKayitlimi = UzakSunucuTakvimDTO1.Find(item => item.trialId == DersProgramiBaseActivityHelper.SecilenDeneme.id);
                var PaylasimSayisiDialogFragment1 = new DenemeSayacDialogFragment(DersProgramiBaseActivityHelper.SecilenDeneme, TakvimeKayitlimi);
                PaylasimSayisiDialogFragment1.Show(this.SupportFragmentManager, "PaylasimSayisiDialogFragment1");
            }
            else if (Index == 1)
            {
                StartActivity(typeof(DersProgramiEkleBaseActivity));
            }
        
            DinamikActionSheet1.Dismiss();
        }
        protected override void OnStart()
        {
            base.OnStart();
            IcerikleriOlustur();
        }
        public void IcerikleriOlustur()
        {
            ShowLoading.Show(this, "Takvim Alınıyor..");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                TakvimIcerikleriniCek();
                CreateTakvimDayList(DateTime.Now);
                ShowLoading.Hide();
            })).Start();
        }
        private void TakvimGeriButton_Click(object sender, EventArgs e)
        {
            CreateTakvimDayList(SonTakvimTarihi.AddMonths(1));
        }

        private void TakvimIleriButton_Click(object sender, EventArgs e)
        {
            CreateTakvimDayList(SonTakvimTarihi.AddMonths(-1));
        }
        void TakvimIcerikleriniCek()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("calendars/user");
            if (Donus != null)
            {
                UzakSunucuTakvimDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UzakSunucuTakvimDTO>>(Donus.ToString());
            }
            DenemeleriCek();
        }

        void DenemeleriCek()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("http://185.184.210.20:8082/api/trials", DontUseHostURL:true);
            if (Donus != null)
            {
                UzakSunucuDenemeDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UzakSunucuDenemeDTO>>(Donus.ToString());
            }
        }
        
        #region Takim Icin

        void CreateTakvimDayList(DateTime GelenAy)
        {
            TakvimTarihlerDTO1 = new List<TakvimTarihlerDTO>();
            var BuAykiTotalGunSayisi = DateTime.DaysInMonth(GelenAy.Year, GelenAy.Month);
            var OtelemeSayisii = OtelemeSayisi(GelenAy);
            var TakvimBaslanginc = new DateTime(GelenAy.Year, GelenAy.Month, 1).AddDays(-1 * OtelemeSayisii);
            for (int i = 0; i < 35; i++)
            {
                if (i < OtelemeSayisii)
                {
                    TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                    {
                        GunGoster = false,
                        Tarih = TakvimBaslanginc.AddDays(-1 * i)
                    });
                }
                else
                {
                    if (i < BuAykiTotalGunSayisi + OtelemeSayisii)
                    {
                        TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                        {
                            GunGoster = true,
                            Tarih = TakvimBaslanginc.AddDays(i)
                        });
                    }
                    else
                    {
                        TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                        {
                            GunGoster = false,
                            Tarih = TakvimBaslanginc.AddDays(i)
                        });
                    }
                }
            }

            TakvimdekiEventleriAyarla();


            //var aaa = TakvimTarihlerDTO1;

            //TakvimTarihlerDTO1[25].DenemeSinaviVami = true;
            //TakvimTarihlerDTO1[10].KisiselTestVarmi = true;
            //TakvimTarihlerDTO1[22].KisiselTestVarmi = true;
            //TakvimTarihlerDTO1[22].DenemeSinaviVami = true;

            this.RunOnUiThread(() => {
                TakvimGridAdapter1 = new TakvimGridAdapter(this, Resource.Layout.TakvimCustomGridCell, TakvimTarihlerDTO1);
                TakvimGrid.Adapter = TakvimGridAdapter1;

                TakvimAyAdiText.Text = GelenAy.ToString("MMMM");
                TakvimYilText.Text = GelenAy.Year.ToString();
                SonTakvimTarihi = GelenAy;
            });

            
        }


        void TakvimdekiEventleriAyarla()
        {
            DersProgramiDTO1 = new List<DersProgramiDTO>();
            for (int i = 0; i < TakvimTarihlerDTO1.Count; i++)
            {
                #region Takvim DTO Search
                var TestVarmi = UzakSunucuTakvimDTO1.Find(item => !string.IsNullOrEmpty(item.testId) && ((DateTime)item.date).ToShortDateString() == ((DateTime)TakvimTarihlerDTO1[i].Tarih).ToShortDateString());
                if (TestVarmi != null)
                {
                    if (TakvimTarihlerDTO1[i].GunGoster)
                    {
                        TakvimTarihlerDTO1[i].KisiselTestVarmi = true;
                        DersProgramiDTO1.Add(new DersProgramiDTO()
                        {
                            Tarih = (DateTime)TakvimTarihlerDTO1[i].Tarih,
                            TestMiDenemeMi = true,
                            TestID = TestVarmi.testId.ToString()
                        });

                    }
                }

                var DenemeVarmi = UzakSunucuTakvimDTO1.Find(item => !string.IsNullOrEmpty(item.trialId) && ((DateTime)item.date).ToShortDateString() == ((DateTime)TakvimTarihlerDTO1[i].Tarih).ToShortDateString());
                if (DenemeVarmi != null)
                {
                    if (TakvimTarihlerDTO1[i].GunGoster)
                    {
                        TakvimTarihlerDTO1[i].DenemeSinaviVami = true;
                        TakvimTarihlerDTO1[i].DenemeID = DenemeVarmi.trialId;
                        DersProgramiDTO1.Add(new DersProgramiDTO()
                        {
                            Tarih = (DateTime)TakvimTarihlerDTO1[i].Tarih,
                            TestMiDenemeMi = false,
                            DenemeID = DenemeVarmi.trialId.ToString()
                        });
                    }
                }

                #endregion

                #region Deneme List DTO Search
                var HenuzKatilmadigiVarmi = UzakSunucuDenemeDTO1.Find(item => ((DateTime)item.startDate).ToShortDateString() == ((DateTime)TakvimTarihlerDTO1[i].Tarih).ToShortDateString());
                if (HenuzKatilmadigiVarmi != null)
                {
                    if (TakvimTarihlerDTO1[i].GunGoster)
                    {
                        TakvimTarihlerDTO1[i].DenemeSinaviVami = true;
                        TakvimTarihlerDTO1[i].DenemeID = HenuzKatilmadigiVarmi.id;
                        DersProgramiDTO1.Add(new DersProgramiDTO()
                        {
                            Tarih = (DateTime)TakvimTarihlerDTO1[i].Tarih,
                            TestMiDenemeMi = false,
                            DenemeID = HenuzKatilmadigiVarmi.id
                        });
                    }
                }
                #endregion
            }
            FillDataModel();
        }


        int OtelemeSayisi(DateTime GelenGun)
        {
            var dtIlkGunu = new DateTime(GelenGun.Year, GelenGun.Month, 1);
            switch (dtIlkGunu.DayOfWeek)
            {
                case DayOfWeek.Monday://Pazartesi
                    return 0;
                case DayOfWeek.Tuesday://Salı
                    return 1;
                case DayOfWeek.Wednesday://Çarşamba
                    return 2;
                case DayOfWeek.Thursday://Perşembe
                    return 3;
                case DayOfWeek.Friday://Cuma
                    return 4;
                case DayOfWeek.Saturday://Cumartesi
                    return 5;
                case DayOfWeek.Sunday://Pazar
                    return 6;
                default:
                    return -1;
            }
        }

        public class TakvimTarihlerDTO
        {
            public DateTime? Tarih { get; set; }
            public bool GunGoster { get; set; }
            public bool DenemeSinaviVami { get; set; }
            public bool KisiselTestVarmi { get; set; }
            public string DenemeID { get; set; }
        }

        public class UzakSunucuTakvimDTO
        {
            public DateTime? date { get; set; }
            public string description { get; set; }
            public string firebaseToken { get; set; }
            public int? id { get; set; }
            public string testId { get; set; }
            public string trialId { get; set; }
            public int? userId { get; set; }
        }

        public class UzakSunucuDenemeDTO
        {
            public string description { get; set; }
            public DateTime finishDate { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string schoolId { get; set; }
            public DateTime startDate { get; set; }
            public string type { get; set; }
            public int? questionCount { get; set; } /*= 10;*/
        }

        class TakvimGridAdapter : BaseAdapter<TakvimTarihlerDTO>
        {
            private Context mContext;
            private int mRowLayout;
            private List<TakvimTarihlerDTO> mUrunler;
            public TakvimGridAdapter(Context context, int rowLayout, List<TakvimTarihlerDTO> friends)
            {
                mContext = context;
                mRowLayout = rowLayout;
                mUrunler = friends;
            }
            public override int Count
            {
                get { return mUrunler.Count; }
            }

            public override TakvimTarihlerDTO this[int position]
            {
                get { return mUrunler[position]; }
            }

            public override long GetItemId(int position)
            {
                return position;
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                MyViewHolder3 holder3;
                View row = convertView;
                var GelenDTO = mUrunler[position];
                if (row != null)
                {

                    holder3 = row.Tag as MyViewHolder3;
                }
                else //(row == null)
                {
                    holder3 = new MyViewHolder3();
                    row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false); //**
                    holder3.TarihLabel = row.FindViewById<TextView>(Resource.Id.textView1);
                    
                    row.Tag = holder3;
                }


                if (GelenDTO.Tarih != null)
                {
                    holder3.TarihLabel.Text = ((DateTime)GelenDTO.Tarih).Day.ToString();
                    if (!GelenDTO.GunGoster)
                    {
                        holder3.TarihLabel.SetTextColor(Color.Rgb(235, 235, 235));
                        holder3.TarihLabel.Text = "";
                    }
                    else
                    {
                        holder3.TarihLabel.SetTextColor(Color.Rgb(17, 18, 46));
                        if (((DateTime)GelenDTO.Tarih).DayOfWeek == DayOfWeek.Saturday || ((DateTime)GelenDTO.Tarih).DayOfWeek == DayOfWeek.Sunday)
                        {
                            holder3.TarihLabel.SetTextColor(Color.Rgb(240, 81, 113));
                        }
                        if (((DateTime)GelenDTO.Tarih).Date == DateTime.Today)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_gun_gosterge);
                        }
                        if (GelenDTO.DenemeSinaviVami)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_deneme_sinavi_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                        if (GelenDTO.KisiselTestVarmi)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_kisisel_test_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                        if (GelenDTO.KisiselTestVarmi == true && GelenDTO.DenemeSinaviVami == true)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_deneme_ve_test_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                    }
                }

                holder3.TarihLabel.Tag = position;
                
                return row;
            }


            private class MyViewHolder3 : Java.Lang.Object
            {
                public TextView TarihLabel { get; set; }
            }
        }

        #endregion


        private void YeniEkle_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DersProgramiEkleBaseActivity));
        }

        void FillDataModel()
        {
            this.RunOnUiThread(delegate ()
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mViewAdapter = new DersProgramiRecyclerViewAdapter(DersProgramiDTO1, this);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                try
                {
                    SnapHelper snapHelper = new LinearSnapHelper();
                    snapHelper.AttachToRecyclerView(mRecyclerView);
                }
                catch
                {
                }
            });
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            var item = DersProgramiDTO1[e];

            if (item.TestMiDenemeMi)//Test
            {
                if (GetTestInfos(item.TestID))
                {
                    
                    this.StartActivity(typeof(TestSinavAlaniBaseActivity));
                    this.Finish();
                }
            }
            else//Deneme
            {
                if (item.Tarih > DateTime.Now)
                {
                    DersProgramiBaseActivityHelper.SecilenTarih = item.Tarih;
                    var HangiDenemeOnuBul = UzakSunucuDenemeDTO1.Find(item2 => item2.id == item.DenemeID);
                    if (HangiDenemeOnuBul != null)
                    {
                        DersProgramiBaseActivityHelper.SecilenDeneme = HangiDenemeOnuBul;
                        var TakvimeKayitlimi = UzakSunucuTakvimDTO1.Find(itemm => itemm.trialId == DersProgramiBaseActivityHelper.SecilenDeneme.id);
                        var PaylasimSayisiDialogFragment1 = new DenemeSayacDialogFragment(DersProgramiBaseActivityHelper.SecilenDeneme, TakvimeKayitlimi);
                        PaylasimSayisiDialogFragment1.Show(this.SupportFragmentManager, "PaylasimSayisiDialogFragment1");
                    }
                }
                else
                {
                    AlertHelper.AlertGoster("Bu deneme sınavı sonlandı.", this);
                }
            }
        }

        bool GetTestInfos(string testid)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir ("user-tests/"+ testid, UsePoll: true);
            if (Donus != "Hata")
            {
                var aa = Donus.ToString();
                var LokaldekiKayit = DataBase.OLUSTURULAN_TESTLER_GETIR_TestID(testid);
                var Icerik2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TestSoruKaydetGuncelle.TestDTO>(Donus.ToString());
                if (Icerik2.questionCount == null)
                {
                    Icerik2.questionCount = LokaldekiKayit[0].questionCount.ToString();
                    Icerik2.lessonName = LokaldekiKayit[0].lessonName;
                    Icerik2.topicName = LokaldekiKayit[0].topicName;
                }
                Test.TestOlustur.TestOlusturBaseActivity.SecilenTest.OlusanTest = Icerik2;
                return true;
            }
            else
            {
                return false;
            }
        }

        public class DersProgramiDTO
        {
            public DateTime Tarih { get; set; }
            public string Baslik { get; set; }
            public string Aciklama { get; set; }
            public string TestSoruSayisi { get; set; }
            public string TestKonuVeyaSinavAlani { get; set; }
            public bool TestMiDenemeMi { get; set; }//Test true / Deneme false
            public string TestID { get; set; }
            public string DenemeID { get; set; }
            public bool UIUygulandimi { get; set; }
        }

        public static class DersProgramiBaseActivityHelper
        {
            public static DersProgramiBaseActivity DersProgramiBaseActivity1 { get; set; }
            public static DateTime SecilenTarih { get; set; }
            public static UzakSunucuDenemeDTO SecilenDeneme { get; set; }
        }
    }
}