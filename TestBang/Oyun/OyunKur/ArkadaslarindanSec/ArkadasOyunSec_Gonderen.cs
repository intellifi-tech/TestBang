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
using Org.Apache.Http.Impl.Client;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericClass.StompHelper;
using TestBang.WebServices;
using WebSocketSharp.Server;
using static TestBang.GenericClass.OyunSocketHelper;

namespace TestBang.Oyun.OyunKur.ArkadaslarindanSec
{
    [Activity(Label = "TestBang")]
    public class ArkadasOyunSec_Gonderen : Android.Support.V7.App.AppCompatActivity
    {
        ImageView BenImage, OImage;
        TextView SayacTexy,BenIsim,OIsim;
        MEMBER_DATA Ben = DataBase.MEMBER_DATA_GETIR()[0];
        OyunSocketHelper OyunSocketHelper1 = new OyunSocketHelper();
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ArkadasOyunSayac_Gonderen);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            BenImage = FindViewById<ImageView>(Resource.Id.profile_image1);
            OImage = FindViewById<ImageView>(Resource.Id.profile_image2);
            SayacTexy = FindViewById<TextView>(Resource.Id.textView2);
            BenIsim = FindViewById<TextView>(Resource.Id.textView4);
            OIsim = FindViewById<TextView>(Resource.Id.textView5);
            BenIsim.Text = Ben.firstName;
            OIsim.Text = ArkadasOyunSec_Gonderen_Helper.SecilenKisi.firstName;
            BenIsim.Selected = true;
            OIsim.Selected = true;
        }

        protected override void OnStart()
        {
            base.OnStart();
            SayacBaslat();
            SoketeGirisYap();
           // SendNotificationRequest();
        }
        void SoketeGirisYap()
        {
            if (OyunSocketHelper_Helper.WebSocket1 == null || !OyunSocketHelper_Helper.WebSocket1.IsAlive || !OyunSocketHelper_Helper.WebSocket1.Ping())
            {
                OyunSocketHelper1.Init(this, null);
            }
            else
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.SendRegisterWithFriend();
            }
        }

        void SendNotificationRequest()
        {
            WebService webService = new WebService();
            SoketSendRegisterDTO soketSendRegisterDTO = new SoketSendRegisterDTO() { 
            
              friendsUser = ArkadasOyunSec_Gonderen_Helper.SecilenKisi.login,
              userName = Ben.login,
              category = OyunSocketHelper_Helper.SecilenAlan,//SAY SOZ EA
              userQuestionIndex = "0",
              userToken = Ben.API_TOKEN,
              filters = OyunSocketHelper_Helper.OyunSocketHelper1.GetLessonsForAlan(),
              
            };
            string jsonString = JsonConvert.SerializeObject(soketSendRegisterDTO);
            var Donus = webService.ServisIslem("requestFriend", jsonString);
            if (Donus!="Hata")
            {
                var aa = Donus.ToString();
            }
        }
        bool TaskRun = true;
        void SayacBaslat()
        {
            var IleriTarih = DateTime.Now.AddMinutes(1);
            var GeriTarih = DateTime.Now;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                Task.Run(async delegate () {
                   
                    
                    Atla:
                    var Suan = DateTime.Now;
                    if (Suan >= IleriTarih)
                    {
                        this.RunOnUiThread(delegate () {
                           // OyundanCikisiIlet();
                            this.Finish();
                        });
                    }
                    else
                    {
                        if (TaskRun)
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
            broad["destination"] = "/app/deniedFriend";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }

        public static class ArkadasOyunSec_Gonderen_Helper
        {
            public static MEMBER_DATA SecilenKisi { get; set; }
        }
    }
}