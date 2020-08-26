using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.Profil.UyelikBilgileri
{
    [Activity(Label = "TestBang")]
    public class UyelikBilgileriBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        TextView AdSoyadText, PaketAdiText, FaturaBedeliText, OdemeMetoduText;
        EditText KuponEdittext, KartAdSoyad, KarNumara, KartSKT, KartCVC;
        Button SatinAlButton,IptalButton;
        string AlinacakPaketKodu = IyzicoHelperCla.TestbangStandartPaket;
        ResponseData<SubscriptionCreatedResource> response;
        List<ODEME_GECMISI> SonAbonelik = DataBase.ODEME_GECMISI_GETIR();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UyelikBilgileriBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            AdSoyadText = FindViewById<TextView>(Resource.Id.adsoyadtext);
            PaketAdiText = FindViewById<TextView>(Resource.Id.paketaditxt);
            FaturaBedeliText = FindViewById<TextView>(Resource.Id.paketfiyattxt);
            OdemeMetoduText = FindViewById<TextView>(Resource.Id.odemeyontemitxt);

            KuponEdittext = FindViewById<EditText>(Resource.Id.kuponedit);
            KartAdSoyad = FindViewById<EditText>(Resource.Id.adsoyadedit);
            KarNumara = FindViewById<EditText>(Resource.Id.kartnumarasiedit);
            KartSKT = FindViewById<EditText>(Resource.Id.sktedit);
            KartCVC = FindViewById<EditText>(Resource.Id.cvcedit);
            SatinAlButton = FindViewById<Button>(Resource.Id.satinalbutton);
            IptalButton = FindViewById<Button>(Resource.Id.iptaletbutton);
            IptalButton.Click += IptalButton_Click;
            SatinAlButton.Click += SatinAlButton_Click;
            KartSKT.TextChanged += KartSKT_TextChanged;

            if (SonAbonelik.Count > 0)
            {
                IptalButton.Visibility = ViewStates.Gone;
            }
        }

        private void IptalButton_Click(object sender, EventArgs e)
        {
            var cevap = new AlertDialog.Builder(this);
            cevap.SetCancelable(true);
            cevap.SetIcon(Resource.Mipmap.ic_launcher);
            cevap.SetTitle(Spannla(Color.Black, "TestBang"));
            cevap.SetMessage(Spannla(Color.DarkGray, "Aktif aboneliğinizi iptal etmek istediğinizden emin misiniz?"));
            cevap.SetPositiveButton(Spannla(Color.Black, "Evet"), delegate
            {
                Should_Cancel_Subscription();
            });
            cevap.SetNegativeButton(Spannla(Color.Black, "Hayır"), delegate
            {
            });
            cevap.Show();
        }
        SpannableStringBuilder Spannla(Color Renk, string textt)
        {
            ForegroundColorSpan foregroundColorSpan = new ForegroundColorSpan(Renk);

            string title = textt;
            SpannableStringBuilder ssBuilder = new SpannableStringBuilder(title);
            ssBuilder.SetSpan(
                    foregroundColorSpan,
                    0,
                    title.Length,
                    SpanTypes.ExclusiveExclusive
            );
            return ssBuilder;
        }

        public void Should_Cancel_Subscription()
        {
            if (SonAbonelik.Count > 0)
            {
                // Initializee();
                CancelSubscriptionRequest request = new CancelSubscriptionRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = "123456789",
                    SubscriptionReferenceCode = SonAbonelik[SonAbonelik.Count-1].iyzicoReferanceCode
                };

                IyzipayResourceV2 response = Subscription.Cancel(request, IyzicoHelperCla.options);
                if (response.Status == "success")
                {
                    Toast.MakeText(this, "Aboneliğiniz İptal Edildi.", ToastLength.Long).Show();
                    this.RunOnUiThread(delegate () {

                        IptalButton.Visibility = ViewStates.Gone;
                      //  OdemeMetoduText.Text = "";
                    });
                }
                else
                {
                    Toast.MakeText(this, "Bir sorun oluştu lütfen daha sonra tekrar deneyin", ToastLength.Long).Show();
                }
            }
        }

        private void SatinAlButton_Click(object sender, EventArgs e)
        {
            if (HepsiDolumu())
            {
                if (!string.IsNullOrEmpty(KuponEdittext.Text.Trim()))
                {
                    KuponKoduKullanilmismi();
                    Should_Initialize_Subscription();
                }
                else
                {
                    Should_Initialize_Subscription();
                }
            }
        }

        public void Should_Initialize_Subscription()
        {
            var meData = DataBase.MEMBER_DATA_GETIR()[0];
            var ad = "";
            var soyad = "";
            var ay = "";
            var yil = "";

            var ayyilbol = KartSKT.Text.Split("/");
            ay = ayyilbol[0];
            yil = "20" + ayyilbol[1];

            var Bol = KartAdSoyad.Text.Split(" ");
            for (int i = 0; i < Bol.Length; i++)
            {
                if (i == Bol.Length - 1)
                {
                    soyad = Bol[i];
                }
                else
                {
                    ad = ad + Bol[i];
                }
            }

            if (ad == "")
            {
                ad = soyad;
            }

            // string randomString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            SubscriptionInitializeRequest request = new SubscriptionInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                Customer = new CheckoutFormCustomer
                {
                    Email = meData.email,
                    Name = ad,
                    Surname = soyad,
                    BillingAddress = new Address
                    {
                        City = "İstanbul",
                        Country = "Türkiye",
                        Description = "billing-address-description",
                        ContactName = "billing-contact-name",
                        ZipCode = "010101"
                    },
                    ShippingAddress = new Address
                    {
                        City = "İstanbul",
                        Country = "Türkiye",
                        Description = "shipping-address-description",
                        ContactName = "shipping-contact-name",
                        ZipCode = "010102"
                    },

                    GsmNumber = "+905336961419",
                    IdentityNumber = meData.id.ToString()
                },
                PaymentCard = new CardInfo
                {
                    CardNumber = KarNumara.Text.Trim(),
                    CardHolderName = KartAdSoyad.Text.Trim(),
                    ExpireMonth = ay,
                    ExpireYear = yil,
                    Cvc = KartCVC.Text.Trim(),
                    RegisterConsumerCard = true
                },
                ConversationId = "123456789",
                PricingPlanReferenceCode = AlinacakPaketKodu
            };

            response = Subscription.Initialize(request, IyzicoHelperCla.options);
            var a = response;

            if (response.StatusCode != 200)
            {
                Toast.MakeText(this, response.ErrorMessage, ToastLength.Long).Show();
                //AlertHelper.AlertGoster(response.ErrorMessage, this);
            }
            else
            {
                ShowLoading.Show(this, "Lütfen Bekleyin");
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    OdemeGecmisiOlustur();
                })).Start();
            }
        }
        
        void OdemeGecmisiOlustur()
        {
            DataBase.ODEME_GECMISI_EKLE(new ODEME_GECMISI()
            {
                iyzicoReferanceCode = response.Data.ReferenceCode,
                UzakDB_ID = ""
            });
            this.RunOnUiThread(delegate () {
                ShowLoading.Hide();
                Toast.MakeText(this, "Aboneliğiniz Başlatıldı", ToastLength.Long).Show();
                this.Finish();
            });
        }
        bool KuponKoduKullanilmismi()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("coupons");
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KuponDTO>>(Donus.ToString());
                if (Icerik.Count > 0)
                {
                    var KuponKoduVarmi = Icerik.Find(item => item.name == KuponEdittext.Text.Trim());
                    if (KuponKoduVarmi != null)
                    {
                        if (DateTime.Now < KuponKoduVarmi.endDate)
                        {
                            AlinacakPaketKodu = KuponKoduVarmi.iyzicoProductCode;
                            AlertHelper.AlertGoster("Kupon kodu başarılı bir şekilde uygulandı.", this);
                            return true;
                        }
                        else
                        {
                            AlertHelper.AlertGoster("Kupon Kodu Kullanılamaz.", this);
                            return false;
                        }
                    }
                    else
                    {
                        AlertHelper.AlertGoster("Kupon Kodu Geçersiz.", this);
                        return false;
                    }
                }
                else
                {
                    AlertHelper.AlertGoster("Kupon Kodu Geçersiz.", this);
                    return false;
                }
            }
            else
            {
                AlertHelper.AlertGoster("Kupon Kodu Geçersiz.", this);
                return false;
            }
        }
        private void KartSKT_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (KartSKT.Text.Length == 2)
            {
                KartSKT.Text = KartSKT.Text + "/";
                KartSKT.SetSelection(KartSKT.Text.Length);
            }
        }
        bool HepsiDolumu()
        {
            if (KarNumara.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen kart numaranızı belirtin", ToastLength.Long).Show();
                return false;
            }
            else if (KartCVC.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen CVC alanını doldurun", ToastLength.Long).Show();
                return false;
            }
            else if (KartAdSoyad.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen kart üzerindeki ismi belirtin", ToastLength.Long).Show();
                return false;
            }
            else if (KartSKT.Text.Length < 5)
            {
                Toast.MakeText(this, "Lütfen kartın son kullanma tarihini belirtin", ToastLength.Long).Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        #region DTOS
        public class KuponDTO
        {
            public int? discountRate { get; set; }
            public DateTime? endDate { get; set; }
            public int? id { get; set; }
            public int? month { get; set; }
            public DateTime? startDate { get; set; }
            public string type { get; set; }
            public int? useCount { get; set; }
            //
            public string iyzicoProductCode { get; set; }
            public string name { get; set; }
        }



        public class OdemeGecmisiDTO
        {
            public string date { get; set; }
            public string fee { get; set; }
            public int userID { get; set; }
            public bool success { get; set; }
        }

        #endregion
    }
}