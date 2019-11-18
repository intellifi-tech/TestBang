using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using TestBang.AnaSayfa;
using TestBang.GenericClass;

namespace TestBang.MainPage
{
    [Activity(Label = "TestBang")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TabLayout tabLayout;
        ViewPager viewPager;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            FnInitTabLayout();
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
            ss2 = new AnaSayfaBaseFragment();
            ss3 = new AnaSayfaBaseFragment();
            ss4 = new AnaSayfaBaseFragment();
            ss5 = new AnaSayfaBaseFragment();

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
        }
    }
}