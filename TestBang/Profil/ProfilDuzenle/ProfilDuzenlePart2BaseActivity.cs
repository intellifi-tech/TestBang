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
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;
using static TestBang.GirisKayit.HesapOlusturActivity;

namespace TestBang.Profil.ProfilDuzenle
{
    [Activity(Label = "TestBang")]
    public class ProfilDuzenlePart2BaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        EditText AdText, SoyadText, EmailText, SifreText, SifreTekrarText;
        TextView AdSoyadTextView;
        Spinner SehirSpin, IlceSpin;
        List<SehirDTO> SehirDTO1 = new List<SehirDTO>();
        List<IlceDTO> IlceDTO1 = new List<IlceDTO>();
        MEMBER_DATA UserInfo = DataBase.MEMBER_DATA_GETIR()[0];
        Button GuncelleButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfilDuzenlePart2BaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            AdSoyadTextView = FindViewById<TextView>(Resource.Id.adsoyadtext);
            AdText = FindViewById<EditText>(Resource.Id.editText1);
            SoyadText = FindViewById<EditText>(Resource.Id.editText2);
            EmailText = FindViewById<EditText>(Resource.Id.editText3);
            SifreText = FindViewById<EditText>(Resource.Id.editText4);
            SifreTekrarText = FindViewById<EditText>(Resource.Id.editText5);
            SehirSpin = FindViewById<Spinner>(Resource.Id.sehirspin);
            IlceSpin = FindViewById<Spinner>(Resource.Id.ilcespin);
            GuncelleButton = FindViewById<Button>(Resource.Id.button2);
            GuncelleButton.Click += GuncelleButton_Click;
            GetUserInfo();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                SehirleriGetir();
                UserinkiniSectir();
            })).Start();

            //
        }

        private void GuncelleButton_Click(object sender, EventArgs e)
        {
            if (BosVarmi())
            {
                WebService webService = new WebService();
                var JSONData = webService.OkuGetir("account");
                if (JSONData != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(JSONData.ToString());
                    Icerik.firstName = AdText.Text.Trim();
                    Icerik.lastName = SoyadText.Text.Trim();
                    Icerik.email = EmailText.Text.Trim();
                    Icerik.password = SifreText.Text;
                    Icerik.townId = IlceDTO1[IlceSpin.SelectedItemPosition].id;
                    Icerik.gender = UserInfo.gender;

                    string jsonString = JsonConvert.SerializeObject(Icerik);
                    var Donus = webService.ServisIslem("users", jsonString, Method: "PUT");
                    if (Donus != "Hata")
                    {
                        UserInfo.firstName = AdText.Text.Trim();
                        UserInfo.lastName = SoyadText.Text.Trim();
                        UserInfo.email = EmailText.Text.Trim();
                        UserInfo.password = SifreText.Text;
                        UserInfo.townId = IlceDTO1[IlceSpin.SelectedItemPosition].id;
                        if (DataBase.MEMBER_DATA_Guncelle(UserInfo))
                        {
                            AlertHelper.AlertGoster("Bilgileriniz Güncellendi.", this);
                            return;
                        }
                        else
                        {
                            AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                            return;
                        }
                    }
                    else
                    {
                        AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                        return;
                    }
                }
                else
                {
                    AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                    return;
                }
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
            else
            {
                return true;
            }
        }
        void GetUserInfo()
        {
            AdSoyadTextView.Text = (UserInfo.firstName + " " + UserInfo.lastName).ToUpper();
            AdText.Text = UserInfo.firstName;
            SoyadText.Text = UserInfo.lastName;
            EmailText.Text = UserInfo.email;
            SifreText.Text = UserInfo.password;
            SifreTekrarText.Text = UserInfo.password;
        }
        bool IlkDegisim = false;
        private void SehirSpin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (!IlkDegisim)
            {
                IlkDegisim = true;
            }
            else
            {
                IlceleriGetir(SehirDTO1[e.Position].id);
            }
            
        }

        void SehirleriGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("cities", isLogin: true);
            if (Donus != null)
            {
                SehirDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SehirDTO>>(Donus.ToString());
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
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("towns/city/" + CityID, isLogin: true);
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
        void UserinkiniSectir()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + UserInfo.townId.ToString());
                if (Donus != null)
                {
                    var TownInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<IlceDTO>(Donus.ToString());
                    if (TownInfo != null)
                    {
                        IlceleriGetir(TownInfo.cityId);
                        this.RunOnUiThread(delegate ()
                        {
                            var CityIndex = SehirDTO1.FindIndex(item => item.id == TownInfo.cityId);
                            var TownIndex = IlceDTO1.FindIndex(item => item.id == TownInfo.id);
                            SehirSpin.SetSelection(CityIndex);
                            IlceSpin.SetSelection(TownIndex);
                            SehirSpin.ItemSelected += SehirSpin_ItemSelected;
                        });
                        
                    }
                }
            })).Start();
        }

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

    }
}