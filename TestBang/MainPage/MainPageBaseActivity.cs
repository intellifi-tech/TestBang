using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Newtonsoft.Json;
using TestBang.AnaSayfa;
using TestBang.Bildirim;
using TestBang.DataBasee;
using TestBang.Deneme;
using TestBang.GenericClass;
using TestBang.Oyun;
using TestBang.Profil;
using TestBang.Test;
using TestBang.WebServices;
using static TestBang.Deneme.DenemeBaseFragment;

namespace TestBang.MainPage
{
    [Activity(Label = "TestBang")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TabLayout tabLayout;
        ViewPager viewPager;
        LinearLayout Bildirimhaznebutton, ArkaPlan;
        DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
        ImageView Logoo;
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];
        TextView Bildirimcount;
        #endregion


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            dinamikStatusBarColor.Pembe(this);
            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            FindViewById<LinearLayout>(Resource.Id.bildirimhaznebutton).Click += MainPageBaseActivity_Click;
            viewPager.OffscreenPageLimit = 10;
            viewPager.PageSelected += ViewPager_PageSelected;
            ArkaPlan = FindViewById<LinearLayout>(Resource.Id.rootview);
            Logoo = FindViewById<ImageView>(Resource.Id.logoo);
            Bildirimcount = FindViewById<TextView>(Resource.Id.bildirimcount);
            Bildirimcount.Text = "0";
            FnInitTabLayout();
            BildirimleriKontrolEt();
            MainPageBaseActivity_Helperr.MainPageBaseActivity1 = this;
            DefaultAyarlariYukle();
        }
        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#F05070"));
                    dinamikStatusBarColor.Pembe(this);
                    Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
                    break;
                case 1:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#11122E"));
                    dinamikStatusBarColor.Lacivert(this);
                    Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img4);
                    break;
                case 2:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#1EB04B"));
                    dinamikStatusBarColor.Yesil(this);
                    Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
                    break;
                case 3:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#11122E"));
                    dinamikStatusBarColor.Lacivert(this);
                    Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img4);
                    break;
                case 4:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#F05171"));
                    dinamikStatusBarColor.Pembe(this);
                    Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
                    break;
                default:
                    break;
            }
        }
        private void MainPageBaseActivity_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(BildirimlerBaseActivity));
        }
        protected override void OnStart()
        {
            base.OnStart();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                UpdateFireBaseToken();
                //  new ContentoNotificationInit_Helper(this).Init();
            })).Start();
        }
        void FnInitTabLayout()
        {
            tabLayout.SetTabTextColors(Android.Graphics.Color.ParseColor("#11122E"), Android.Graphics.Color.ParseColor("#F05070"));
            Android.Support.V4.App.Fragment ss1, ss2, ss3, ss4, ss5;

            ss1 = new AnaSayfaBaseFragment();
            ss2 = new DenemeBaseFragment();
            ss3 = new TestCozBaseFragment();
            ss4 = new OyunBaseFragment();
            ss5 = new ProfileBaseFragment();

            //Fragment array
            var fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
                ss3,
                ss4,
                ss5,

            };

            var titles = CharSequence.ArrayFromStringArray(new[] {
               "ANA SAYFA",
               "DENEME SINAVI",
               "TEST ÇÖZ",
               "OYUN",
               "MENU",
            });

            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);

            tabLayout.SetupWithViewPager(viewPager);

            //((TextView)tabLayout.GetTabAt(0).CustomView).SetTextSize(Android.Util.ComplexUnitType.Dip, 8);
        }
        public static void setTextViewsCapsOff(View view)
        {
            if (!(view is ViewGroup))
            {
                return;
            }
            ViewGroup group = (ViewGroup)view;
            for (int i = 0; i < group.ChildCount; i++)
            {
                View child = group.GetChildAt(i);
                if (child is TextView)
                {
                    ((TextView)child).SetTextSize(Android.Util.ComplexUnitType.Dip, 20);

                }
            }
        }
        #region BildirimKontrol
        void BildirimleriKontrolEt()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("notifications");
                if (Donus != null)
                {
                    var notificationDTOS = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NotificationDTOS>>(Donus.ToString());
                    List<NotificationDTOS> BenimIcinFiltrelenmis = new List<NotificationDTOS>();
                    if (notificationDTOS.Count > 0)
                    {
                        var LocalBildirimler = DataBase.BILDIRIMLER_GETIR();
                        HashSet<int> sentIDs = new HashSet<int>(LocalBildirimler.Select(s => s.id));
                        var YeniBildirimler = (notificationDTOS.Where(m => !sentIDs.Contains(m.id))).ToList();

                        if (YeniBildirimler.Count > 0)
                        {
                            for (int i = 0; i < YeniBildirimler.Count; i++)
                            {
                                DataBase.BILDIRIMLER_EKLE(new BILDIRIMLER()
                                {
                                    id = YeniBildirimler[i].id,
                                    date = YeniBildirimler[i].date,
                                    Okundu = false,
                                    text = YeniBildirimler[i].text
                                });
                            }
                        }
                    }
                }

                var LocalBildirimler2 = DataBase.BILDIRIMLER_GETIR();
                var Countt = LocalBildirimler2.FindAll(item => item.Okundu == false);
                if (Countt.Count > 0)
                {
                    this.RunOnUiThread(delegate () {
                        if (Countt.Count >= 10)
                        {
                            Bildirimcount.Text = "9+";
                        }
                        else
                        {
                            Bildirimcount.Text = Countt.Count.ToString();
                        }

                        Bildirimcount.Visibility = ViewStates.Visible;
                    });
                }
                else
                {
                    this.RunOnUiThread(delegate () {
                        Bildirimcount.Text = "0";
                        Bildirimcount.Visibility = ViewStates.Invisible;
                    });
                }
            })).Start();
        }
        #endregion
        void UpdateFireBaseToken()
        {
            var MyToken = FirebaseInstanceId.Instance.Token;
            if (!string.IsNullOrEmpty(MyToken))
            {
                UpdateUserFireBaseToken(MyToken);
            }
        }
        void UpdateUserFireBaseToken(string tokenn)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var MeData = DataBase.MEMBER_DATA_GETIR()[0];
                WebService webService = new WebService();
                var JSONData = webService.OkuGetir("account");
                if (JSONData != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTOForUpdate>(JSONData.ToString());
                    Icerik.mobileFirebaseToken = tokenn;
                    string jsonString = JsonConvert.SerializeObject(Icerik);
                    var Donus4 = webService.ServisIslem("users", jsonString, Method: "PUT");
                    if (Donus4 != null)
                    {
                        MeData.mobileFirebaseToken = tokenn;
                        DataBase.MEMBER_DATA_Guncelle(MeData);
                    }
                }
            })).Start();
        }
        public override void OnBackPressed()
        {
            viewPager.CurrentItem = 0;

        }
        void DefaultAyarlariYukle()
        {
            var Ayarlarr = DataBase.AYARLAR_GETIR();
            if (Ayarlarr.Count <= 0)
            {
                DataBase.AYARLAR_EKLE(new AYARLAR() { 
                    Notification = true
                });
            }
        }

        #region DTOS
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class School
        {
            public string corpColor { get; set; }
            public int id { get; set; }
            public string logoPath { get; set; }
            public string name { get; set; }
            public string token { get; set; }
            public int? townId { get; set; }
            public List<MEMBER_DATA> users { get; set; }
        }

        public class NotificationDTOS
        {
            public DateTime? date { get; set; }
            public int id { get; set; }
            public List<School> schools { get; set; }
            public string text { get; set; }
        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class UserDTOForUpdate
        {
            public int id { get; set; }
            public string login { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string imageUrl { get; set; }
            public int? gender { get; set; }
            public bool activated { get; set; }
            public string langKey { get; set; }
            public string createdBy { get; set; }
            public DateTime? createdDate { get; set; }
            public string lastModifiedBy { get; set; }
            public DateTime? lastModifiedDate { get; set; }
            public List<string> authorities { get; set; }
            public DateTime? birthday { get; set; }
            public int townId { get; set; }
            public string webFirebaseToken { get; set; }
            public string mobileFirebaseToken { get; set; }
            public bool? userPaymentStatus { get; set; }
            public int? age { get; set; }
            public string townName { get; set; }
            public string cityName { get; set; }
            public string payRefCode { get; set; }
            public string alan { get; set; }
        }


        public static class MainPageBaseActivity_Helperr{
            public static MainPageBaseActivity MainPageBaseActivity1 { get; set; }
        }
        #endregion
    }
}