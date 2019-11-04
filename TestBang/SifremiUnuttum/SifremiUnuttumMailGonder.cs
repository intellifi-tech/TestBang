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
    public class SifremiUnuttumMailGonder : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalr
        Button MailGonderButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SifremiUnuttumMailGonder);
             DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            MailGonderButton = FindViewById<Button>(Resource.Id.button1);
            MailGonderButton.Click += MailGonderButton_Click;
        }

        private void MailGonderButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SifreyiGuncelleActivity));
            this.Finish();
        }
    }
}