using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestBang.GenericClass;
using TestBang.MainPage;

namespace TestBang.GirisKayit
{
    [Activity(Label = "TestBang")]
    public class HosgeldinActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button TesteBasla;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HosgeldinActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Beyaz(this);
            TesteBasla = FindViewById<Button>(Resource.Id.button1);
            TesteBasla.Click += TesteBasla_Click;
        }

        private void TesteBasla_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(MainPageBaseActivity));
            this.Finish();
        }
    }
}