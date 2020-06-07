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
using TestBang.AnaSayfa;
using TestBang.Bildirim;
using TestBang.Deneme;
using TestBang.GenericClass;
using TestBang.Oyun;
using TestBang.Profil;
using TestBang.Test;
using static TestBang.Deneme.DenemeBaseFragment;

namespace TestBang.MainPage
{
    [Activity(Label = "TestBang")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TabLayout tabLayout;
        ViewPager viewPager;
        LinearLayout Bildirimhaznebutton,ArkaPlan;
        DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
        ImageView Logoo;
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
            FnInitTabLayout();
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#F05070"));
                    dinamikStatusBarColor.Pembe(this);
                    //Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
                    break;
                case 1:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#11122E"));
                    dinamikStatusBarColor.Lacivert(this);
                    //Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img3);
                    break;
                case 2:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#1EB04B"));
                    dinamikStatusBarColor.Yesil(this);
                    //Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
                    break;
                case 3:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#11122E"));
                    dinamikStatusBarColor.Lacivert(this);
                   // Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img3);
                    break;
                case 4:
                    ArkaPlan.SetBackgroundColor(Color.ParseColor("#F05171"));
                    dinamikStatusBarColor.Pembe(this);
                    //Logoo.SetImageResource(Resource.Mipmap.testbang_logo_img2);
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
    }
}