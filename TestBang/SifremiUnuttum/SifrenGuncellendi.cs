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
    public class SifrenGuncellendi : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalr
        Button TestCozmeyeBaslaButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SifrenGuncellendi);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            TestCozmeyeBaslaButton = FindViewById<Button>(Resource.Id.button1);
            TestCozmeyeBaslaButton.Click += TestCozmeyeBaslaButton_Click;
        }

        private void TestCozmeyeBaslaButton_Click(object sender, EventArgs e)
        {
            this.Finish();
        }
    }
}