using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericClass.StompHelper;
using TestBang.GenericUI;
using TestBang.MainPage;
using TestBang.WebServices;
using static TestBang.GenericClass.OyunSocketHelper;

namespace TestBang.Oyun.OyunKur.ArkadaslarindanSec
{
    [Activity(Label = "TestBang",Exported =true)]
    public class ArkadasOyunSec_Gelen : Android.Support.V7.App.AppCompatActivity
    {
        ImageView BenImage, OImage;
        TextView SayacTexy, BenIsim, OIsim;
        MEMBER_DATA Ben = DataBase.MEMBER_DATA_GETIR()[0];
        Button KabulEt, Reddet;
        OyunSocketHelper OyunSocketHelper1 = new OyunSocketHelper();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ArkadasOyunSec_Gelen);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            BenImage = FindViewById<ImageView>(Resource.Id.profile_image1);
            OImage = FindViewById<ImageView>(Resource.Id.profile_image2);
            SayacTexy = FindViewById<TextView>(Resource.Id.textView2);
            BenIsim = FindViewById<TextView>(Resource.Id.textView4);
            OIsim = FindViewById<TextView>(Resource.Id.textView5);
            KabulEt = FindViewById<Button>(Resource.Id.button3);
            Reddet = FindViewById<Button>(Resource.Id.button2);
            Reddet.Click += Reddet_Click;
            KabulEt.Click += KabulEt_Click;
            BenIsim.Text = Ben.firstName;

            try
            {
                string GetMessageDTOJSON1 = Intent.GetStringExtra("GetMessageDTOJSON");
                ArkadasOyunSec_Gelen_Helper.FCMData = Newtonsoft.Json.JsonConvert.DeserializeObject<MyFirebaseMessagingService.GelenMesajDTO>(GetMessageDTOJSON1.ToString());
            }
            catch 
            {
            }
            OIsim.Text = "";
            BenIsim.Selected = true;
            OIsim.Selected = true;
            SecilenKisiDoldur();
        }

        private void KabulEt_Click(object sender, EventArgs e)
        {
            TasRun = false;
            OyunuKabulEt();
            //kULLANICIYI bEKLET UI
        }

        void SoketeGirisYap()
        {
            if (OyunSocketHelper_Helper.WebSocket1 == null || !OyunSocketHelper_Helper.WebSocket1.IsAlive || !OyunSocketHelper_Helper.WebSocket1.Ping())
            {
                OyunSocketHelper1.Init(this, null,true);
            }
            //else
            //{
            //    OyunSocketHelper_Helper.OyunSocketHelper1.SendRegisterWithFriend();
            //}
        }


        private void Reddet_Click(object sender, EventArgs e)
        {
            ReddedildiGonder();
            this.StartActivity(typeof(MainPageBaseActivity));
            this.Finish();
        }

        bool Acildimi = false;
        protected override void OnStart()
        {
            base.OnStart();
            if (!Acildimi)
            {
                if (ArkadasOyunSec_Gelen_Helper.FCMData != null)
                {
                    if (!string.IsNullOrEmpty(ArkadasOyunSec_Gelen_Helper.FCMData.data.startTime))
                    {
                        var convertDT = Convert.ToDateTime(ArkadasOyunSec_Gelen_Helper.FCMData.data.startTime);
                        if (convertDT.AddMinutes(1).AddSeconds(5) > DateTime.Now)//Bildirim 1 dk 5 saniden daha fazla süre önce geldi
                        {
                            SoketeGirisYap();
                            SayacBaslat();
                        }
                        else
                        {
                            this.StartActivity(typeof(MainPageBaseActivity));
                            this.Finish();
                        }
                    }
                    else
                    {
                        this.StartActivity(typeof(MainPageBaseActivity));
                        this.Finish();
                    }

                }
                else
                {
                    this.StartActivity(typeof(MainPageBaseActivity));
                    this.Finish();
                }
                Acildimi = true;
            }
            
        }
        bool TasRun = true;
        void SayacBaslat()
        {
            var IleriTarih = DateTime.Now.AddMinutes(1);
            var GeriTarih = DateTime.Now;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                Task t;
                 t = Task.Run(async delegate () {
                    Atla:
                    var Suan = DateTime.Now;
                    if (Suan >= IleriTarih)
                    {
                        this.RunOnUiThread(delegate () {

                            TasRun = false;
                            ReddedildiGonder();
                            this.StartActivity(typeof(MainPageBaseActivity));
                            this.Finish();
                            
                        });
                    }
                    else
                    {
                         if (TasRun)
                         {
                             this.RunOnUiThread(delegate () {
                                 if ((IleriTarih - Suan).TotalSeconds > 10)
                                 {
                                     SayacTexy.Text = "00:" + Math.Round((IleriTarih - Suan).TotalSeconds, 0).ToString();
                                 }
                                 else
                                 {
                                     SayacTexy.Text = "00:0" + Math.Round((IleriTarih - Suan).TotalSeconds, 0).ToString();
                                 }

                             });
                             await Task.Delay(1000);
                             goto Atla;
                         }
                    }
                });
            })).Start();

        }

        void OyunuKabulEt()
        {
            ShowLoading.Show(this, "Lütfen Bekleyin...");
            var content = new SoketSendRegisterDTO()
            {
                category = ArkadasOyunSec_Gelen_Helper.FCMData.data.category,
                userName = Ben.login,
                userQuestionIndex = "0",
                userToken = Ben.API_TOKEN,
                filters = ArkadasOyunSec_Gelen_Helper.FCMData.data.filters,
                friendsUser = ArkadasOyunSec_Gelen_Helper.FCMData.data.userName
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/acceptFriend";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }
        //leave
        //en
        //level

        void SecilenKisiDoldur()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var userrr = ArkadasOyunSec_Gelen_Helper.FCMData.data.userName;
                var Jsonn = webService.OkuGetir("users/" + userrr);
                if (Jsonn != null)
                {
                    ArkadasOyunSec_Gelen_Helper.SecilenKisi = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Jsonn.ToString());
                    if (ArkadasOyunSec_Gelen_Helper.SecilenKisi!=null)
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            OIsim.Text = ArkadasOyunSec_Gelen_Helper.SecilenKisi.firstName;
                        });
                    }
                }
            })).Start();
        }
        void ReddedildiGonder()
        {
            var content = new SoketSendRegisterDTO()
            {
                category = "",
                userName = Ben.login,
                userQuestionIndex = "0",
                userToken = Ben.API_TOKEN,
                filters = new List<string>(),
                friendsUser = ArkadasOyunSec_Gelen_Helper.FCMData.data.userName
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/deniedFriend";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }


        public void OyundanCikisiIlet()
        {
            var content = new SoketSendRegisterDTO()
            {
                category = "",
                userName = Ben.login,
                userQuestionIndex = "0",
                userToken = Ben.API_TOKEN,
                filters = new List<string>()
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/leave";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }

        public static class ArkadasOyunSec_Gelen_Helper
        {
            public static MEMBER_DATA SecilenKisi { get; set; }
            public static MyFirebaseMessagingService.GelenMesajDTO FCMData { get; set; }
        }
    }
}