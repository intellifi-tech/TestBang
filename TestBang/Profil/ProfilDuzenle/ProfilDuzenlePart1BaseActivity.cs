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

namespace TestBang.Profil.ProfilDuzenle
{
    [Activity(Label = "TestBang")]
    public class ProfilDuzenlePart1BaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button ProfilDuzenleButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfilDuzenlePart1);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            ProfilDuzenleButton = FindViewById<Button>(Resource.Id.button2);
            ProfilDuzenleButton.Click += ProfilDuzenleButton_Click;
        }

        private void ProfilDuzenleButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ProfilDuzenlePart2BaseActivity));
            this.Finish();
        }
    }
}