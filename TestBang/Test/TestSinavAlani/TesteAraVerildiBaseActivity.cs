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
using Newtonsoft.Json;
using TestBang.GenericClass;
using TestBang.WebServices;
using static TestBang.Test.TestOlustur.TestOlusturBaseActivity;

namespace TestBang.Test.TestSinavAlani
{
    [Activity(Label = "TestBang")]
    public class TesteAraVerildiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button TesteDevamEtButton, YeniTestButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TesteAraVerildiBaseActivity);
            TesteDevamEtButton = FindViewById<Button>(Resource.Id.button3);
            YeniTestButton = FindViewById<Button>(Resource.Id.button4);
            TesteDevamEtButton.Click += TesteDevamEtButton_Click;
            YeniTestButton.Click += YeniTestButton_Click;
            TestDurumunuGuncelle(false);
        }

        private void YeniTestButton_Click(object sender, EventArgs e)
        {
            TestDurumunuGuncelle(true);
            this.StartActivity(typeof(TestOlusturBaseActivity));
            TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1.Finish();
            this.Finish();
        }

        private void TesteDevamEtButton_Click(object sender, EventArgs e)
        {
            TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1.TesteDevamEt();
            this.Finish();
        }
        void TestDurumunuGuncelle(bool BitirmeDurumu)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                SecilenTest.OlusanTest.finish = BitirmeDurumu;
                SecilenTest.OlusanTest.finishDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                string jsonString = JsonConvert.SerializeObject(SecilenTest.OlusanTest);
                var Donus = webService.ServisIslem("user-tests", jsonString, Method: "PUT", UsePoll: true);
            })).Start();
            
        }
        public override void OnBackPressed()
        {
            
        }
    }
}