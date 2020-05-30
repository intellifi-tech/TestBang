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
using Org.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;
using static TestBang.GirisKayit.GirisBaseActivity;

namespace TestBang.GirisKayit
{
    [Activity(Label = "TestBang")]
    public class HesapOlusturActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        Button KayitOlButton;
        EditText AdText, SoyadText, EmailText, SifreText, SifreTekrarText;
        TextView DogumText;
        Spinner SehirSpin, IlceSpin,CinsiyetSpin;

        List<SehirDTO> SehirDTO1 = new List<SehirDTO>();
        List<IlceDTO> IlceDTO1 = new List<IlceDTO>();
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HesapOlusturActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Trans(this,true);
            KayitOlButton = FindViewById<Button>(Resource.Id.button1);
            AdText = FindViewById<EditText>(Resource.Id.editText1);
            SoyadText = FindViewById<EditText>(Resource.Id.editText2);
            EmailText = FindViewById<EditText>(Resource.Id.editText3);
            SifreText = FindViewById<EditText>(Resource.Id.editText4);
            SifreTekrarText = FindViewById<EditText>(Resource.Id.editText5);
            DogumText = FindViewById<TextView>(Resource.Id.dogumtxt);
            DogumText.Click += DogumText_Click;
            SehirSpin = FindViewById<Spinner>(Resource.Id.sehirspin);
            IlceSpin = FindViewById<Spinner>(Resource.Id.ilcespin);
            CinsiyetSpin = FindViewById<Spinner>(Resource.Id.cinsiyetspin);
            CinsiyetSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, new string[] { "Cinsiyet", "Kadın", "Erkek" });
            // OkulSpin = FindViewById<Spinner>(Resource.Id.okulspin);


            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            { 
                SehirleriGetir();

            })).Start();


            //GetSchoolByIlceID(null);
            SehirSpin.ItemSelected += SehirSpin_ItemSelected;
            IlceSpin.ItemSelected += IlceSpin_ItemSelected;
            KayitOlButton.Click += KayitOlButton_Click;


            //AdText.Text = "Mesut";
            //SoyadText.Text = "Polat";
            //EmailText.Text = "mesut4@intellifi.tech";
            //SifreText.Text = "qwer1234";
            //SifreTekrarText.Text = "qwer1234";
            //DogumText.Text = new DateTime(1994, 12, 12).ToShortDateString();
        }

        private void DogumText_Click(object sender, EventArgs e)
        {
            Tarih_Cek frag = Tarih_Cek.NewInstance(delegate (DateTime time)
            {
                DogumText.Text = time.ToShortDateString();
            },DateTime.Now);
            frag.Show(this.SupportFragmentManager, Tarih_Cek.TAG);
        }

        private void IlceSpin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
           // GetSchoolByIlceID(IlceDTO1[e.Position].id);
        }

        private void SehirSpin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            IlceleriGetir(SehirDTO1[e.Position].id);
        }

        private void KayitOlButton_Click(object sender, EventArgs e)
        {
            if (BosVarmi())
            {
                ShowLoading.Show(this,"Lütfen Bekleyin...");
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    WebService webService = new WebService();
                    KayitIcinRoot kayitIcinRoot = new KayitIcinRoot()
                    {
                        firstName = AdText.Text.Trim(),
                        lastName = SoyadText.Text.Trim(),
                        password = SifreText.Text,
                        login = EmailText.Text,
                        email = EmailText.Text,
                        gender = CinsiyetSpin.SelectedItemPosition == 1 ? false:true,
                        birthday = Convert.ToDateTime(DogumText.Text).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                        townId = IlceDTO1[IlceSpin.SelectedItemPosition].id
                    };
                    string jsonString = JsonConvert.SerializeObject(kayitIcinRoot);
                    var Responsee = webService.ServisIslem("register", jsonString, true);
                    if (Responsee != "Hata")
                    {
                        TokenAlDevamEt();
                        ShowLoading.Hide();
                    }
                    else
                    {
                        ShowLoading.Hide();
                        AlertHelper.AlertGoster("Bir sorunla karşılaşıldı!", this);
                        return;
                    }
                })).Start();
            }

            //StartActivity(typeof(HosgeldinActivity));
            //this.Finish();
        }
        void TokenAlDevamEt()
        {
            LoginRoot loginRoot = new LoginRoot()
            {
                password = SifreText.Text,
                rememberMe = true,
                username = EmailText.Text,

            };
            string jsonString = JsonConvert.SerializeObject(loginRoot);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("authenticate", jsonString, true);
            if (Donus == "Hata")
            {
                ShowLoading.Hide();
                AlertHelper.AlertGoster("Giriş Yapılamadı!", this);
                return;
            }
            else
            {
                JSONObject js = new JSONObject(Donus);
                var Token = js.GetString("id_token");
                if (Token != null && Token != "")
                {
                    APITOKEN.TOKEN = Token;
                    if (GetMemberData())
                    {
                        ShowLoading.Hide();
                        GirisBaseActivityHelper.GirisBaseActivity1.Finish();
                        StartActivity(typeof(HosgeldinActivity));
                        this.Finish();
                    }
                    else
                    {
                        ShowLoading.Hide();
                        AlertHelper.AlertGoster("Bir sorun oluştu lütfen daha sonra tekrar deneyin.", this);
                        return;
                    }
                }
            }
        }
        bool GetMemberData()
        {
            WebService webService = new WebService();
            var JSONData = webService.OkuGetir("account");
            if (JSONData != null)
            {
                var JsonSting = JSONData.ToString();

                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(JSONData.ToString());
                Icerik.API_TOKEN = APITOKEN.TOKEN;
                Icerik.password = SifreText.Text;
                Icerik.townId = IlceDTO1[IlceSpin.SelectedItemPosition].id;
                Icerik.gender = CinsiyetSpin.SelectedItemPosition == 1 ? false : true;
                DataBase.MEMBER_DATA_EKLE(Icerik);
                return true;
            }
            else
            {
                return false;
            }

        }
        bool BosVarmi()
        {
            if (AdText.Text.Trim() == "")
            {
                AlertHelper.AlertGoster("Lütfen adınızı giriniz.", this);
                return false;
            }
            else if (SoyadText.Text.Trim() == "")
            {
                AlertHelper.AlertGoster("Lütfen soyadınızı giriniz.", this);
                return false;
            }
            else if (EmailText.Text.Trim() == "")
            {
                AlertHelper.AlertGoster("Lütfen emalinizi giriniz.", this);
                return false;
            }
            else if (SifreText.Text.Trim() == "")
            {
                AlertHelper.AlertGoster("Lütfen şifrenizi giriniz.", this);
                return false;
            }
            else if (SifreTekrarText.Text.Trim() == "")
            {
                AlertHelper.AlertGoster("Lütfen şifre tekrarını giriniz.", this);
                return false;
            }
            else if (SifreText.Text.Trim() != SifreTekrarText.Text.Trim())
            {
                AlertHelper.AlertGoster("Şifreleriniz uyuşmuyor lütfen tekrar kontrol ediniz.", this);
                return false;
            }
            else if(DogumText.Text.Trim() == "Doğum Tarihiniz")
            {
                AlertHelper.AlertGoster("Lütfen doğum tarihinizi belirtiniz.", this);
                return false;
            }
            else if (CinsiyetSpin.SelectedItemPosition == 0)
            {
                AlertHelper.AlertGoster("Lütfen cinsiyetinizi belirtin.", this);
                return false;
            }
            else
            {
                return true;
            }
        }

        void SehirleriGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("cities", isLogin : true);
            if (Donus != null)
            {
                SehirDTO1 =  Newtonsoft.Json.JsonConvert.DeserializeObject<List<SehirDTO>>(Donus.ToString());
                if (SehirDTO1.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        SehirSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, SehirDTO1.Select(item => item.name).ToArray());
                    });
                }
            }
        }

        void IlceleriGetir(int CityID)
        {
            ///api/
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("towns/city/" + CityID , isLogin: true);
            if (Donus != null)
            {
                IlceDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IlceDTO>>(Donus.ToString());
                if (IlceDTO1.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        IlceSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, IlceDTO1.Select(item => item.name).ToArray());
                    });
                }
            }
        }



        //void GetSchoolByIlceID(int? IlceID)
        //{
        //    if (IlceID==null)
        //    {
        //        WebService webService = new WebService();
        //        var Donus = webService.OkuGetir("schools", isLogin: true);
        //        if (Donus != null)
        //        {
        //            SchoolDTO1_Full = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolDTO>>(Donus.ToString());
        //        }
        //    }
        //    else
        //    {
        //        if (SchoolDTO1_Full.Count > 0)
        //        {
        //            SchoolDTO1_Part = SchoolDTO1_Full.FindAll(item => item.townId == IlceID);
        //            if (SchoolDTO1_Part.Count>0)
        //            {
        //                OkulSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, SchoolDTO1_Part.Select(item => item.name).ToArray());
        //            }
        //        }
        //    }
        //}

        public class SehirDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }

        public class IlceDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }

        public class SchoolDTO
        {
            public string corpColor { get; set; }
            public int id { get; set; }
            public string logoPath { get; set; }
            public string name { get; set; }
            public string token { get; set; }
            public int townId { get; set; }
        }


        public class KayitIcinRoot
        {
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string login { get; set; }
            public string password { get; set; }
            public bool gender { get; set; }
            public string birthday { get; set; }
            public int townId { get; set; }
        }
    }
}