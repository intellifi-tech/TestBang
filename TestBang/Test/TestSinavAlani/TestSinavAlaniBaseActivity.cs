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
using TestBang.Test.TestTamamlandi;

namespace TestBang.Test.TestSinavAlani
{
    [Activity(Label = "TestBang")]
    public class TestSinavAlaniBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewPager;
        Button SonrakiSoru,OncekiSoruButton,AraVerButton;
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
            AraVerButton.Click += AraVerButton_Click;
            OncekiSoruButton.Click += OncekiSoruButton_Click;
            SonrakiSoru.Click += SonrakiSoru_Click;
            viewPager.OffscreenPageLimit = 10000;
            FnInitTabLayout();
        }

        private void AraVerButton_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(TesteAraVerildiBaseActivity));
        }

        private void OncekiSoruButton_Click(object sender, EventArgs e)
        {
            viewPager.CurrentItem = viewPager.CurrentItem -1;
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (viewPager.CurrentItem == 4)
            {
                SonrakiSoru.Text = "TESTİ BİTİR";
            }
            else
            {
                SonrakiSoru.Text = "SONRAKİ SORU";
            }
        }

        private void SonrakiSoru_Click(object sender, EventArgs e)
        {
            if (viewPager.CurrentItem==4)
            {
                this.StartActivity(typeof(TestTamamlandiBaseActivity));
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
            fragments = new Android.Support.V4.App.Fragment[5];
            titles = new Java.Lang.ICharSequence[5];
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new TestSinavAlaniParcaFragment();
            }
            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);
        }
    }
}