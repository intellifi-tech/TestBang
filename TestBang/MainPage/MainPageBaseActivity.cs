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
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewPager.OffscreenPageLimit = 10;
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