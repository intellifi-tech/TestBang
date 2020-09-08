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
using TestBang.DataBasee;
using TestBang.Deneme.DenemeSinavAlani;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.Deneme.DenemeTamamlandi
{
    [Activity(Label = "TestBang")]
    public class DenemeTamamlandiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];
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
                KonuveAlanEkle();
                WebService webService = new WebService();
                var jsonstring = JsonConvert.SerializeObject(DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1);
                var Donus = webService.ServisIslem("http://185.184.210.20:8082/api/trial-informations/save", jsonstring, UsePoll: true,DontUseHostURL:true);
                if (Donus != "Hata")
                {
                    var aaa = Donus.ToString();
                }
                ShowLoading.Hide();
            })).Start();
            
        }
        void KonuveAlanEkle()
        {
            DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1 = DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1.Select(c => { c.userAlan = Me.alan; return c; }).ToList();
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("topics");
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Topics>>(Donus.ToString());
                if (Icerik.Count>0)
                {
                    DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1.ForEach(item => {
                        var bulunan = Icerik.Find(item2 => item2.id == item.topicId);
                        if (bulunan!=null)
                        {
                            item.topicName = bulunan.name;
                        }
                    });
                }
            }
        }



        public class Topics
        {
            public bool ayt { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public int lessonId { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}