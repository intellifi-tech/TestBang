using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        Spinner SehirSpin, IlceSpin, CinsiyetSpin, OkulSpin, AlanSpin;
        List<SehirDTO> SehirDTO1 = new List<SehirDTO>();
        List<IlceDTO> IlceDTO1 = new List<IlceDTO>();
        MEMBER_DATA UserInfo = DataBase.MEMBER_DATA_GETIR()[0];
        Button GuncelleButton;
        TextView DogumText;
        ImageButton Resimeklebutton;
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
            DogumText = FindViewById<TextView>(Resource.Id.dogumtxt);
            DogumText.Click += DogumText_Click;
            SehirSpin = FindViewById<Spinner>(Resource.Id.sehirspin);
            IlceSpin = FindViewById<Spinner>(Resource.Id.ilcespin);
            CinsiyetSpin = FindViewById<Spinner>(Resource.Id.cinsiyetspin);
            OkulSpin = FindViewById<Spinner>(Resource.Id.okulspin);
            AlanSpin = FindViewById<Spinner>(Resource.Id.alanspin);
            GuncelleButton = FindViewById<Button>(Resource.Id.button2);
            GuncelleButton.Click += GuncelleButton_Click;
            Resimeklebutton = FindViewById<ImageButton>(Resource.Id.resimeklebutton);
            CinsiyetSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, new string[] { "Cinsiyet", "Kadın", "Erkek" });
            AlanSpin.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, new string[] { "Alan", "SAY", "SÖZ", "EA" });
            Resimeklebutton.Click += Resimeklebutton_Click;
            GetUserInfo();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                SehirleriGetir();
                UserinkiniSectir();
            })).Start();

            //
        }

        private void Resimeklebutton_Click(object sender, EventArgs e)
        {
            var Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Fotoğraf Seç"), 444);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == 444) && (resultCode == Android.App.Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                using (var inputStream = this.ContentResolver.OpenInputStream(uri))
                {
                    using (var streamReader = new System.IO.StreamReader(inputStream))
                    {
                        var bytes = default(byte[]);
                        using (var memstream = new System.IO.MemoryStream())
                        {
                            streamReader.BaseStream.CopyTo(memstream);
                            bytes = memstream.ToArray();
                            OnceLogoyuYule(bytes);
                            //SecilenGoruntuByte = bytes;
                            //circleImageView.SetImageURI(uri);
                            //LogoYukleButton.Visibility = ViewStates.Gone;
                            //LogoPath = uri;
                        }
                    }
                }
            }
        }

        string OnceLogoyuYule(byte[] mediabyte)
        {
            var MeID = DataBase.MEMBER_DATA_GETIR()[0];
            var client = new RestSharp.RestClient("http://185.184.210.20:8080/api/users/profile-photos");
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", "Bearer " + MeID.API_TOKEN);
            request.AddFile("photo", mediabyte, "testbang_user_img.jpg");
            RestSharp.IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.InternalServerError &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.Forbidden &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotAcceptable &&
                response.StatusCode != HttpStatusCode.RequestTimeout &&
                response.StatusCode != HttpStatusCode.NotFound)
            {
                var jsonobj = response.Content;
                if (!string.IsNullOrEmpty(jsonobj))
                {
                    MeID.imageUrl = jsonobj;
                    DataBase.MEMBER_DATA_Guncelle(MeID);
                    return "";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        private void DogumText_Click(object sender, EventArgs e)
        {
            Tarih_Cek frag = Tarih_Cek.NewInstance(delegate (DateTime time)
            {
                DogumText.Text = time.ToShortDateString();
            }, DateTime.Now);
            frag.Show(this.SupportFragmentManager, Tarih_Cek.TAG);
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
            else if (DogumText.Text.Trim() == "Doğum Tarihiniz")
            {
                AlertHelper.AlertGoster("Lütfen doğum tarihinizi belirtiniz.", this);
                return false;
            }
            else if (CinsiyetSpin.SelectedItemPosition == 0)
            {
                AlertHelper.AlertGoster("Lütfen cinsiyetinizi belirtin.", this);
                return false;
            }
            else if (AlanSpin.SelectedItemPosition == 0)
            {
                AlertHelper.AlertGoster("Lütfen Alanınızı belirtin.", this);
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
            DogumText.Text = Convert.ToDateTime(UserInfo.birthday).ToShortDateString();
            CinsiyetSpin.SetSelection((bool)UserInfo.gender ? 2 : 1);
            switch (UserInfo.alan)
            {
                case "SAY":
                    AlanSpin.SetSelection(1);
                    break;
                case "SÖZ":
                    AlanSpin.SetSelection(2);
                    break;
                case "EA":
                    AlanSpin.SetSelection(3);
                    break;
                default:
                    AlanSpin.SetSelection(0);
                    break;
            }

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