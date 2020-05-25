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
using TestBang.Oyun.ArkadaslarindanSec;

namespace TestBang.Oyun.OyunKur
{
    [Activity(Label = "TestBang")]
    public class OyunKurBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Spinner AlanSpinner;
        string[] AlanlarDizi = new string[] { "Alan Seç", "TYT", "AYT" };
        Button ArkadaslarindanSec, RasgeleAra;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            SetContentView(Resource.Layout.OyunKurBaseActivity);
            AlanSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            AlanSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, AlanlarDizi);
            ArkadaslarindanSec = FindViewById<Button>(Resource.Id.button3);
            RasgeleAra = FindViewById<Button>(Resource.Id.button2);
            ArkadaslarindanSec.Click += ArkadaslarindanSec_Click;
            RasgeleAra.Click += RasgeleAra_Click;
        }

        private void RasgeleAra_Click(object sender, EventArgs e)
        {
            var RasgeleRakipAraDialogFragment1 = new RasgeleRakipAraDialogFragment();
            RasgeleRakipAraDialogFragment1.Show(this.SupportFragmentManager, "RasgeleRakipAraDialogFragment1");
        }

        private void ArkadaslarindanSec_Click(object sender, EventArgs e)
        {
            var ArkadaslarindanSecDialogFragment1 = new ArkadaslarindanSecDialogFragment();
            ArkadaslarindanSecDialogFragment1.Show(this.SupportFragmentManager, "RasgeleRakipAraDialogFragment1");
        }
    }
}