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
using TestBang.Deneme.DenemeSinavAlani;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.Deneme.DenemeTamamlandi
{
    [Activity(Label = "TestBang")]
    public class DenemeTamamlandiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button Kapat;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DenemeTamamlandiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            Kapat = FindViewById<Button>(Resource.Id.button3);
            Kapat.Click += Kapat_Click;
        }

        private void Kapat_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        bool Acildi = false;
        protected override void OnStart()
        {
            base.OnStart();
            if (!Acildi)
            {
                DenemeSonucunuKaydet();
                Acildi = true;
            }
        }
        private void DenemeSonucunuKaydet()
        {
            ShowLoading.Show(this, "Sonuçlar Gönderiliyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var jsonstring = JsonConvert.SerializeObject(DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1);
                var Donus = webService.ServisIslem("trial-informations/save", jsonstring, UsePoll: true);
                if (Donus != "Hata")
                {
                    var aaa = Donus.ToString();
                }
                ShowLoading.Hide();
            })).Start();
            
        }
    }
}