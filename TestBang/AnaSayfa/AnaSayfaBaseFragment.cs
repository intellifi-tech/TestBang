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

        TextView DogruYuzde, DogruSayi, YanlisYuzde, YanlisSayi, BosYuzde, BosSayi;
        ProgressBar DogruProgres, YanlisProgres, BosProgres;

        GenelTestSonuclariDTO GenelTestSonuclariDTO1 = new GenelTestSonuclariDTO();

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
            DogruYuzde = RootView.FindViewById<TextView>(Resource.Id.dogrucevapyuzde);
            DogruSayi = RootView.FindViewById<TextView>(Resource.Id.dogrucevapsayi);
            YanlisYuzde = RootView.FindViewById<TextView>(Resource.Id.yalniscevapyuzde);
            YanlisSayi = RootView.FindViewById<TextView>(Resource.Id.yalniscevapsayi);
            BosYuzde = RootView.FindViewById<TextView>(Resource.Id.boscevapyuzde);
            BosSayi = RootView.FindViewById<TextView>(Resource.Id.boscevapsayi);
            DogruProgres = RootView.FindViewById<ProgressBar>(Resource.Id.dogruprogress);
            YanlisProgres = RootView.FindViewById<ProgressBar>(Resource.Id.yanlisprogress);
            BosProgres = RootView.FindViewById<ProgressBar>(Resource.Id.bosprogress);

            DogruYuzde.Text = "%0";
            YanlisYuzde.Text = "%0";
            BosYuzde.Text = "%0";
            DogruSayi.Text = "0";
            YanlisSayi.Text = "0";
            BosSayi.Text = "0";
            DogruProgres.Progress = 0;
            YanlisProgres.Progress = 0;
            BosProgres.Progress = 0;


            ProfilGoruntuleButton.Click += ProfilGoruntuleButton_Click;
            PerformansButton.Click += PerformansButton_Click;
            TestCozButton.Click += TestCozButton_Click;
            ShowUserInfo();
            GenelTestSonuclariniGetir();
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
            
            YasText.Text = ((int)(DateTime.Now.Year - Convert.ToDateTime(MeUser.birthday).Year)).ToString();
            try
            {
                CinsiyetText.Text = (bool)MeUser.gender ? "Erkek" : "Kadın";
                Cinsiyeticon.SetImageResource((bool)MeUser.gender ? Resource.Mipmap.maleimg1 : Resource.Mipmap.femaleimg1);
            }
            catch 
            {
            }
            try
            {
                SetTownNameByID((int)MeUser.townId);
            }
            catch 
            {
            }
            
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


        #region Genel Test Sonuclari
        void GenelTestSonuclariniGetir()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("user-tests/test-results", UsePoll: true);
                if (Donus != null)
                {
                    var aa = Donus.ToString();
                    GenelTestSonuclariDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<GenelTestSonuclariDTO>(Donus.ToString());
                    if (GenelTestSonuclariDTO1 != null)
                    {
                        if (GenelTestSonuclariDTO1.sumOfQuestions != null)
                        {
                            this.Activity.RunOnUiThread(delegate ()
                            {

                                DogruSayi.Text = GenelTestSonuclariDTO1.sumOfCorrect.ToString();
                                YanlisSayi.Text = GenelTestSonuclariDTO1.sumOfWrong.ToString();
                                BosSayi.Text = GenelTestSonuclariDTO1.sumOfEmpty.ToString();

                                DogruYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * GenelTestSonuclariDTO1.sumOfCorrect) / GenelTestSonuclariDTO1.sumOfQuestions)), 0);
                                YanlisYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * GenelTestSonuclariDTO1.sumOfWrong) / GenelTestSonuclariDTO1.sumOfQuestions)), 0);
                                BosYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * GenelTestSonuclariDTO1.sumOfEmpty) / GenelTestSonuclariDTO1.sumOfQuestions)), 0);

                                DogruProgres.Progress = Convert.ToInt32(DogruYuzde.Text.Replace("%", ""));
                                YanlisProgres.Progress = Convert.ToInt32(YanlisYuzde.Text.Replace("%", ""));
                                BosProgres.Progress = Convert.ToInt32(BosYuzde.Text.Replace("%", ""));
                            });
                        }
                        else
                        {
                            this.Activity.RunOnUiThread(delegate ()
                            {
                                DogruYuzde.Text = "%0";
                                YanlisYuzde.Text = "%0";
                                BosYuzde.Text = "%0";
                                DogruSayi.Text = "0";
                                YanlisSayi.Text = "0";
                                BosSayi.Text = "0";
                                DogruProgres.Progress = 0;
                                YanlisProgres.Progress = 0;
                                BosProgres.Progress = 0;
                            });
                        }
                    }
                }
            })).Start();
        }
        #endregion
        public class UserLessonInfoDTO
        {
            public int? correctCount { get; set; }
            public int? emptyCount { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public int? questionCount { get; set; }
            public int? wrongCount { get; set; }
        }

        public class GenelTestSonuclariDTO
        {
            public int? sumOfCorrect { get; set; }
            public int? sumOfEmpty { get; set; }
            public int? sumOfQuestions { get; set; }
            public int? sumOfWrong { get; set; }
            public int? totalTime { get; set; }
            public List<UserLessonInfoDTO> userLessonInfoDTOS { get; set; }
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