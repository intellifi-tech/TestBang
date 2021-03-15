﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using TestBang.GenericClass;
using TestBang.GirisKayit;

namespace TestBang.AppIntro
{
    [Activity(Label = "TestBang")]
    public class AppIntroBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        ViewPager viewpager;
        protected IPageIndicator _indicator;
        RelativeLayout Transformiew;
       
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AppIntro);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            Transformiew = FindViewById<RelativeLayout>(Resource.Id.rootview);
            viewpager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viepageratama();
            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.circlePageIndicator1);
            _indicator.SetViewPager(viewpager);
            ((CirclePageIndicator)_indicator).Snap = true;
            var density = Resources.DisplayMetrics.Density;
            //((CirclePageIndicator)_indicator).SetBackgroundColor(Color.Argb(255, 204, 204, 204));
            ((CirclePageIndicator)_indicator).Radius = 5 * density;
            ((CirclePageIndicator)_indicator).PageColor = Color.Argb(255, 255, 255, 255);
            ((CirclePageIndicator)_indicator).FillColor = Color.Transparent;
            ((CirclePageIndicator)_indicator).StrokeColor = Color.White;
            ((CirclePageIndicator)_indicator).StrokeWidth = 2f;

            //BaslangicIslemleri();
        }


        Android.Support.V4.App.Fragment[] fragments;
        void viepageratama()
        {
            var ss1 = new IntroFragment("Artık Testleri Tam Kalbinden", "VURMA ZAMANI", "Testbang’de eğitimde Hedefi yükseltme zamanı!\nİstediğin zaman, istediğin yerde\nistediğin konu ve seviyede soru çöz.", "testBang_02_Splash.json", false);
            var ss2 = new IntroFragment("Sınava Hazırlıkta Rekabet", "YENİ SEVİYEYE ULAŞTI", "Senin de dünyan artık dijitalse, eğitimde\nrekabeti artık internette yaşa.Seviyeni okuluna,\niline, ya da tüm Türkiye’ye karşı gör.", "testBang_03_Splash.json", false);
            var ss3 = new IntroFragment("Sınavların Sosyal Olma", "VAKTİ GELDİ", "İstersen arkadaşlarınla kapış, bilgini konuştur.\nİstersen Türkiye’nin dört bir yanındaki\nTestbang kullanıcılarıyla tanış ve yarış.", "testBang_04_Splash.json", true);

            //Fragment array
            fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
                ss3,
         
            };
            //Tab title array
            var titles = CharSequence.ArrayFromStringArray(new[] {
               "s1",
               "s2",
               "s3",
            });
            try
            {
                viewpager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles);
            }
            catch
            {
            }
            viewpager.SetPageTransformer(true, new IntroTransformer(Transformiew));
        }
        public class IntroFragment : Android.Support.V4.App.Fragment
        {
            string Metin1, Metin2, Metin3;
            string imageid;
            bool buttondurum;
            TextView MetinText1,MetinText2,MetinText3;
            Button devamet;
            LottieAnimationView animationView;
            public IntroFragment(string metin1,string metin2,string metin3, string gelenimageid, bool buttonolsunmu)
            {
                Metin1 = metin1;
                Metin2 = metin2;
                Metin3 = metin3;
                imageid = gelenimageid;
                buttondurum = buttonolsunmu;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.AppIntroParcaFragment, container, false);
                devamet = view.FindViewById<Button>(Resource.Id.button1);
                MetinText1 = view.FindViewById<TextView>(Resource.Id.textView1);
                MetinText2 = view.FindViewById<TextView>(Resource.Id.textView2);
                MetinText3 = view.FindViewById<TextView>(Resource.Id.textView3);
                animationView = view.FindViewById<LottieAnimationView>(Resource.Id.follow_icon2);


                MetinText1.Text = Metin1;
                MetinText2.Text = Metin2;
                MetinText3.Text = Metin3;
               

                if (buttondurum == false)
                    devamet.Visibility = ViewStates.Invisible;
                else
                    devamet.Click += Devamet_Click;


                animationView.SetAnimation(imageid);
                animationView.PlayAnimation();

                return view;

            }

            private void Devamet_Click(object sender, EventArgs e)
            {
                this.Activity.StartActivity(typeof(GirisBaseActivity));
                this.Activity.Finish();

            }
        }
    }
}