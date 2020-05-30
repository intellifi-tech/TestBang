using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using TestBang.DataBasee;
using TestBang.GenericUI;
using TestBang.Profil.DersProgrami;
using TestBang.Profil.ProfilDuzenle;
using TestBang.Profil.Transkript;
using TestBang.Test.TestOlustur;
using TestBang.WebServices;

namespace TestBang.AnaSayfa
{
    public class AnaSayfaBaseFragment : Android.Support.V4.App.Fragment
    {
        TextView AdText, CinsiyetText, YasText, IlceText, OkulText;
        Button ProfilGoruntuleButton, PerformansButton, TestCozButton;
        CircleImageView UserImage;
        MEMBER_DATA MeUser = DataBase.MEMBER_DATA_GETIR()[0];
        ImageView Cinsiyeticon;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View RootView = inflater.Inflate(Resource.Layout.AnaSayfaBaseFragment, container, false);
            AdText = RootView.FindViewById<TextView>(Resource.Id.adsoyadtext);
            CinsiyetText = RootView.FindViewById<TextView>(Resource.Id.cinsiyettext);
            YasText = RootView.FindViewById<TextView>(Resource.Id.yastext);
            IlceText = RootView.FindViewById<TextView>(Resource.Id.ilcetxt);
            OkulText = RootView.FindViewById<TextView>(Resource.Id.okultext);
            ProfilGoruntuleButton = RootView.FindViewById<Button>(Resource.Id.profilbutton);
            PerformansButton = RootView.FindViewById<Button>(Resource.Id.performansbutton);
            TestCozButton = RootView.FindViewById<Button>(Resource.Id.testcozbutton);
            UserImage = RootView.FindViewById<CircleImageView>(Resource.Id.profile_image);
            Cinsiyeticon = RootView.FindViewById<ImageView>(Resource.Id.cinsiyeticon);

            ProfilGoruntuleButton.Click += ProfilGoruntuleButton_Click;
            PerformansButton.Click += PerformansButton_Click;
            TestCozButton.Click += TestCozButton_Click;
            ShowUserInfo();
            return RootView;
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();
        private void TestCozButton_Click(object sender, EventArgs e)
        {
            Butonlarr = new List<Buttons_Image_DataModels>();
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Yeni Test Oluştur",
                Button_Image = Resource.Drawable.add
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Takvimde ki Testler",
                Button_Image = Resource.Drawable.calender
            });

            DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Yeni bir test oluşturabilir veya ders programındaki testleri çözebilirsin.", Buton_Click);
            DinamikActionSheet1.Show(this.Activity.SupportFragmentManager, "DinamikActionSheet1");
        }

        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {
                this.Activity.StartActivity(typeof(TestOlusturBaseActivity));
                
            }
            else if (Index == 1)
            {
                this.Activity.StartActivity(typeof(DersProgramiBaseActivity));
            }

            DinamikActionSheet1.Dismiss();
        }

        private void PerformansButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(TranskriptListeBaseActivity));
        }

        private void ProfilGoruntuleButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(ProfilDuzenlePart1BaseActivity));
        }

        void ShowUserInfo()
        {
            AdText.Text = MeUser.firstName.ToUpper() + " " + MeUser.lastName.ToUpper();
            CinsiyetText.Text = (bool)MeUser.gender ? "Erkek" : "Kadın";
            Cinsiyeticon.SetImageResource((bool)MeUser.gender ? Resource.Mipmap.maleimg1 : Resource.Mipmap.femaleimg1);
            YasText.Text = ((int)(DateTime.Now.Year - Convert.ToDateTime(MeUser.birthday).Year)).ToString();
            SetTownNameByID((int)MeUser.townId);
        }

        void SetTownNameByID(int TownID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + TownID);
                if (Donus != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<TownDTO>(Donus.ToString());
                    if (Icerik!=null)
                    {
                        this.Activity.RunOnUiThread(delegate () {
                            IlceText.Text = Icerik.name;
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