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
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.WebServices;

namespace TestBang.Profil.ProfilDuzenle
{
    [Activity(Label = "TestBang")]
    public class ProfilDuzenlePart1BaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button ProfilDuzenleButton;
        TextView AdSoyadText, CinsiyetText, DogumTarihiYasText, IlIlceText, OkulText, MailAdresiText;
        ImageView CinsiyetIcon;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfilDuzenlePart1);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            ProfilDuzenleButton = FindViewById<Button>(Resource.Id.button2);
            AdSoyadText = FindViewById<TextView>(Resource.Id.adsoyadtext);
            CinsiyetText = FindViewById<TextView>(Resource.Id.cinsiyettext);
            DogumTarihiYasText = FindViewById<TextView>(Resource.Id.dogumtarihitext);
            IlIlceText = FindViewById<TextView>(Resource.Id.ililcetext);
            OkulText = FindViewById<TextView>(Resource.Id.okultext);
            MailAdresiText = FindViewById<TextView>(Resource.Id.mailtext);
            CinsiyetIcon = FindViewById<ImageView>(Resource.Id.cinsiyeticon);
            ProfilDuzenleButton.Click += ProfilDuzenleButton_Click;
            IlIlceText.Text = "";
        }
        protected override void OnStart()
        {
            base.OnStart();
            var UserInfo = DataBase.MEMBER_DATA_GETIR()[0];
            AdSoyadText.Text = (UserInfo.firstName + " " + UserInfo.lastName).ToUpper();
            CinsiyetText.Text = (bool)UserInfo.gender ? "Erkek" : "Kadın";
            DogumTarihiYasText.Text = (DateTime.Now.Year - Convert.ToDateTime(UserInfo.birthday).Year).ToString() + " | " + Convert.ToDateTime(UserInfo.birthday).ToShortDateString();
            IlIlceGetir(UserInfo.townId.ToString());
            OkulText.Text = "-";
            MailAdresiText.Text = UserInfo.email;
            CinsiyetIcon.SetImageResource((bool)UserInfo.gender ? Resource.Mipmap.maleimg1 : Resource.Mipmap.femaleimg1);
        }
        private void ProfilDuzenleButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ProfilDuzenlePart2BaseActivity));
            this.Finish();
        }
        void IlIlceGetir(string TownID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + TownID);
                if (Donus != null)
                {
                    var TownInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<TownDTO>(Donus.ToString());
                    if (TownInfo != null)
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            IlIlceText.Text = TownInfo.name + " / " + TownInfo.cityName;
                        });
                    }
                }
            })).Start();
        }

        public class TownDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}