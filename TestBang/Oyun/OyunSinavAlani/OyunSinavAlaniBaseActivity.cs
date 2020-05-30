using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using TestBang.GenericClass;
using TestBang.Oyun.KazandinKaybettin;

namespace TestBang.Oyun.OyunSinavAlani
{
    [Activity(Label = "TestBang")]
    public class OyunSinavAlaniBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewPager;
        Button SonrakiSoru, OncekiSoruButton;

        #region Sure Ile Ilgili Tanimlamalar
        TextView SureText;
        System.Timers.Timer Timer1 = new System.Timers.Timer();
        DateTime SifirBaslangic = new DateTime(0);
        bool SureIslemeyeDevamEt = true;
        int Dakika = 1;
        DateTime BitisZamani;

        LinearLayout Progress_Ben, Progress_O;

        List<LinearLayout> Progres_Ben_List = new List<LinearLayout>();
        List<LinearLayout> Progres_O_List = new List<LinearLayout>();

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            SetContentView(Resource.Layout.OyunSinavAlaniBaseActivity);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewPager.PageSelected += ViewPager_PageSelected;
            SonrakiSoru = FindViewById<Button>(Resource.Id.sonrakisorubutton);
            OncekiSoruButton = FindViewById<Button>(Resource.Id.oncekisorubutton);
            //AraVerButton = FindViewById<Button>(Resource.Id.araverbutton);
            SureText = FindViewById<TextView>(Resource.Id.suretext);
            Progress_Ben = FindViewById<LinearLayout>(Resource.Id.progress1);
            Progress_O = FindViewById<LinearLayout>(Resource.Id.progress2);



           // AraVerButton.Click += AraVerButton_Click;
            OncekiSoruButton.Click += OncekiSoruButton_Click;
            SonrakiSoru.Click += SonrakiSoru_Click;
            viewPager.OffscreenPageLimit = 10000;
            FnInitTabLayout();
            Dakika = 10;
            BitisZamani = new DateTime(0).AddMinutes(Dakika);
            Timer1.Interval = 1000;
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            Timer1.Start();
        }
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (SureIslemeyeDevamEt)
            {
                SifirBaslangic = SifirBaslangic.AddSeconds(1);
                if (SifirBaslangic == BitisZamani)
                {
                    SureIslemeyeDevamEt = false;
                }
                else
                {
                    this.RunOnUiThread(delegate () {
                        SureText.Text = SifirBaslangic.ToLongTimeString();
                    });
                }
            }
            else
            {
                Timer1.Stop();
            }
        }

        private void AraVerButton_Click(object sender, EventArgs e)
        {
            //TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1 = this;
            //TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
            Timer1.Stop();
            //new TestSoruKaydetGuncelle().KaydetGuncelle(SecilenTest.OlusanTest);
            //this.StartActivity(typeof(TesteAraVerildiBaseActivity));
        }

        public void TesteDevamEt()
        {
            Timer1.Start();
        }
        private void OncekiSoruButton_Click(object sender, EventArgs e)
        {
            viewPager.CurrentItem = viewPager.CurrentItem - 1;
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (viewPager.CurrentItem + 1 == 10)
            {
                SonrakiSoru.Text = "OYUNU BİTİR";
            }
            else
            {
                SonrakiSoru.Text = "SONRAKİ SORU";
            }
        }

        private void SonrakiSoru_Click(object sender, EventArgs e)
        {
            if (viewPager.CurrentItem + 1 == 10)
            {
                TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1 = this;
                TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
                this.StartActivity(typeof(KazandinKaybettinBaseActivity));
                this.Finish();
            }
            else
            {
                viewPager.CurrentItem = viewPager.CurrentItem + 1;
            }
        }

        Android.Support.V4.App.Fragment[] fragments;
        Java.Lang.ICharSequence[] titles;
        void FnInitTabLayout()
        {
            fragments = new Android.Support.V4.App.Fragment[10];
            titles = new Java.Lang.ICharSequence[10];
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new OyunSinavAlaniParcaFragment(i);
            }
            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);
        }

    }

    public static class TestSinavAlaniHelperClass
    {
        public static OyunSinavAlaniBaseActivity OyunSinavAlaniBaseActivity1 { get; set; }
        public static DateTime ToplamTestCozumSuresi { get; set; }
    }
}
