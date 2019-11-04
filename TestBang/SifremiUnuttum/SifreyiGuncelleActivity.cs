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

namespace TestBang.SifremiUnuttum
{
    [Activity(Label = "TestBang")]
    public class SifreyiGuncelleActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalr
        Button GuncelleButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SifreyiGuncelleActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            GuncelleButton = FindViewById<Button>(Resource.Id.button1);
            GuncelleButton.Click += GuncelleButton_Click;
        }

        private void GuncelleButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SifrenGuncellendi));
            this.Finish();
        }
    }
}