using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Test.TestTamamlandi;
using static TestBang.Test.TestOlustur.TestOlusturBaseActivity;

namespace TestBang.Test.TestSinavAlani
{
    [Activity(Label = "TestBang")]
    public class TestSinavAlaniBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewPager;
        Button SonrakiSoru,OncekiSoruButton,AraVerButton;
        
        #region Sure Ile Ilgili Tanimlamalar
        TextView SureText;
        System.Timers.Timer Timer1 = new System.Timers.Timer();
        DateTime SifirBaslangic = new DateTime(0);
        bool SureIslemeyeDevamEt = true;
        int Dakika = 1;
        DateTime BitisZamani;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestSinavAlaniBaseActivity);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewPager.PageSelected += ViewPager_PageSelected;
            SonrakiSoru = FindViewById<Button>(Resource.Id.sonrakisorubutton);
            OncekiSoruButton = FindViewById<Button>(Resource.Id.oncekisorubutton);
            AraVerButton = FindViewById<Button>(Resource.Id.araverbutton);
            SureText = FindViewById<TextView>(Resource.Id.suretext);
            AraVerButton.Click += AraVerButton_Click;
            OncekiSoruButton.Click += OncekiSoruButton_Click;
            SonrakiSoru.Click += SonrakiSoru_Click;
            viewPager.OffscreenPageLimit = 10000;
            FnInitTabLayout();
            Dakika = Convert.ToInt32(SecilenTest.OlusanTest.time);
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
                        if (SecilenTest.OlusanTest.time!= "100000")
                        {
                            SureText.Text = SifirBaslangic.ToLongTimeString();
                        }
                        else
                        {
                            SureText.Text = "SÜRESİZ";
                        }
                        
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
            TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1 = this;
            TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
            Timer1.Stop();
            new TestSoruKaydetGuncelle().KaydetGuncelle(SecilenTest.OlusanTest);
            this.StartActivity(typeof(TesteAraVerildiBaseActivity));
        }

        public void TesteDevamEt()
        {
            Timer1.Start();
        }
        private void OncekiSoruButton_Click(object sender, EventArgs e)
        {
            viewPager.CurrentItem = viewPager.CurrentItem -1;
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (viewPager.CurrentItem+1 == Convert.ToInt32(SecilenTest.OlusanTest.questionCount))
            {
                SonrakiSoru.Text = "TESTİ BİTİR";
            }
            else
            {
                SonrakiSoru.Text = "SONRAKİ SORU";
            }
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();
        int BosIndex;
        private void SonrakiSoru_Click(object sender, EventArgs e)
        {
            if (viewPager.CurrentItem+1== Convert.ToInt32(SecilenTest.OlusanTest.questionCount))
            {
                BosIndex = SecilenTest.OlusanTest.userTestQuestions.FindIndex(item => String.IsNullOrEmpty(item.userAnswer));
                if (BosIndex!=-1)
                {
                    Butonlarr = new List<Buttons_Image_DataModels>();
                    Butonlarr.Add(new Buttons_Image_DataModels()
                    {
                        Button_Text = "Evet, Boş Soruya Git.",
                        Button_Image = Resource.Drawable.add
                    });
                    Butonlarr.Add(new Buttons_Image_DataModels()
                    {
                        Button_Text = "Hayır, Testi Bitir.",
                        Button_Image = Resource.Drawable.calender
                    });

                    DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Boş bıraktığın sorular mevcut. Bu sorulara geri dönüp cevaplandırmak ister misin?", Buton_Click);
                    DinamikActionSheet1.Show(this.SupportFragmentManager, "DinamikActionSheet1");
                }
                else
                {
                    TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1 = this;
                    TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
                    this.StartActivity(typeof(TestTamamlandiBaseActivity));
                    this.Finish();
                }
            }
            else
            {
                viewPager.CurrentItem = viewPager.CurrentItem + 1;
            }
        }

        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {

                viewPager.CurrentItem = BosIndex;
            }
            else if (Index == 1)
            {
                TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1 = this;
                TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
                this.StartActivity(typeof(TestTamamlandiBaseActivity));
                this.Finish();
            }

            DinamikActionSheet1.Dismiss();
        }


        Android.Support.V4.App.Fragment[] fragments;
        Java.Lang.ICharSequence[] titles;
        void FnInitTabLayout()
        {
            fragments = new Android.Support.V4.App.Fragment[Convert.ToInt32(SecilenTest.OlusanTest.questionCount)];
            titles = new Java.Lang.ICharSequence[Convert.ToInt32(SecilenTest.OlusanTest.questionCount)];
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new TestSinavAlaniParcaFragment(i);
            }
            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);
        }
    }

    public static class TestSinavAlaniHelperClass
    {
        public static TestSinavAlaniBaseActivity TestSinavAlaniBaseActivity1 { get; set; }
        public static DateTime ToplamTestCozumSuresi { get; set; }
    }
}