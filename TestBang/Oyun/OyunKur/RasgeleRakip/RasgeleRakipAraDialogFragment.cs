using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.IO;
using Newtonsoft.Json;
using Refractored.Controls;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericClass.StompHelper;
using TestBang.GenericUI;
using TestBang.WebServices;
using WebSocketSharp;

namespace TestBang.Oyun.OyunKur
{
    class RasgeleRakipAraDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        ImageButton KapatButton;
        MEMBER_DATA MeId = DataBase.MEMBER_DATA_GETIR()[0];

        #region Soket
        public WebSocket ws = new WebSocket("ws://185.184.210.20:8080/ws");//ws://94.101.87.230:10000/ws
        //ws://192.168.1.38:8080/ws/app/register
        public StompMessageSerializer serializer = new StompMessageSerializer();
        #endregion


        #endregion
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation3;
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, DPX.dpToPx(this.Activity, 400));
            Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.FillVertical | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
        }
    
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            return dialog;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.RasgeleRakipAraDialogFragment, container, false);

            KapatButton = view.FindViewById<ImageButton>(Resource.Id.ımageButton1);
            KapatButton.Click += KapatButton_Click;

            return view;
        }
        private void KapatButton_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
        
        bool Actinmi = false;
        public override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, DPX.dpToPx(this.Activity, 600));
                Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
                SoketeGirisYap();
                Actinmi = true;
            }
        }

        void SoketeGirisYap()
        {

            CreateSocketEvents();


            //SoketJoin SoketJoin1 = new SoketJoin()
            //{
            //    userName = DataBase.MEMBER_DATA_GETIR()[0].email
            //};
            //WebService webService = new WebService();
            //var jsonstring = JsonConvert.SerializeObject(SoketJoin1);
            //var Donus = webService.ServisIslem("join", jsonstring, localip:true);
            //if (Donus != "Hata")
            //{
            //    var aaa = Donus.ToString();
            //    var SoketJoinResponseDTO1 = JsonConvert.DeserializeObject<SoketJoinResponseDTO>(Donus.ToString());
                
            //}
        }


        #region Soket Metod
        void CreateSocketEvents()
        {
            
            var cookiee = new WebSocketSharp.Net.Cookie("username", MeId.login); //poiu@poiuy.com qwer1234


            var cookiee2 = new WebSocketSharp.Net.Cookie("Bearer", MeId.API_TOKEN);

            ws.Log.Level = LogLevel.Trace;
            //  ws.SetCredentials("Bearer", "MeId.API_TOKEN", false);
            // ws.SetCookie(cookiee);
            ws.SetCookie(cookiee2);
          //  ws.WaitTime = TimeSpan.FromSeconds(5);
            ws.OnMessage -= ws_OnMessage;
            ws.OnClose -= ws_OnClose;
            ws.OnOpen -= ws_OnOpen;
            ws.OnError -= ws_OnError;
            //ws.EmitOnPing = true;
            ws.OnMessage += ws_OnMessage;
            ws.OnClose += ws_OnClose;
            ws.OnOpen += ws_OnOpen;
            ws.OnError += ws_OnError;
            ws.Connect();
        }


        //connect olduktan sonra register'a send yapıyoruz


        #region SocketEvents
        void UpdateListBox(string data)
        {
            System.Console.WriteLine("MessageListenerrrrr : " + data);
            // Toast.MakeText(this, data, ToastLength.Short).Show();
        }
        void ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            UpdateListBox(DateTime.Now.ToString() + " ws_OnError says: " + e.ToString());
        }
        void ws_OnOpen(object sender, EventArgs e)
        {
            UpdateListBox(" ws_OnOpen says: " + e.ToString());
            System.Console.WriteLine("AÇILDII : " + DateTime.Now.ToString());
            ConnectStomp();
        }
        void ws_OnClose(object sender, CloseEventArgs e)
        {
            UpdateListBox(" ws_OnClose says: " + e.ToString());
            System.Console.WriteLine("AÇILDII : " + DateTime.Now.ToString());
            ws.Connect();
        }
        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            System.Console.WriteLine("-----------------------------");
            System.Console.WriteLine(" ws_OnMessage says: " + e.Data);
            StompMessage msg = serializer.Deserialize(e.Data);
            if (msg.Command == StompFrame.CONNECTED)
            {
                UpdateListBox(e.Data);
                SubscribeStomp();
            }
            else if (msg.Command == StompFrame.MESSAGE)
            {
                //Content Mesajj = null;
                //try
                //{
                //    Mesajj = Newtonsoft.Json.JsonConvert.DeserializeObject<Content>(msg.Body.ToString());
                //    UpdateLastMessage(Mesajj);
                //    //YeniMesajIslemi(Mesajj);
                //}
                //catch
                //{
                //}
            }
        }
       
        
        private void ConnectStomp()
        {
            var connect = new StompMessage(StompFrame.CONNECT);
            connect["accept-version"] = "1.2";
            connect["host"] = "";
            // first number Zero mean client not able to send Heartbeat, 
            //Second number mean Server will sending heartbeat to client instead
            connect["heart-beat"] = "0,10000";

            var sss = serializer.Serialize(connect);
            ws.Send(serializer.Serialize(connect));
        }
        private void SubscribeStomp()
        {
            var sub1 = new StompMessage(StompFrame.SUBSCRIBE);
            sub1["destination"] = "/user/"+MeId.login+"/start"; //Oyun başlatıyoruz soruları alıyoruz
            ws.Send(serializer.Serialize(sub1));

            var sub2 = new StompMessage(StompFrame.SUBSCRIBE);
            sub2["destination"] = "/user/" + MeId.login + "/left"; //Bağlantın koptuğu anda buraya istek atıyorsun karşı oyuncuyu bilgilendirmek için
            ws.Send(serializer.Serialize(sub2));

            var sub3 = new StompMessage(StompFrame.SUBSCRIBE);
            sub3["destination"] = "/user/" + MeId.login + "/index"; //Oyun esnasında rakibin son durumunu alacağız
            ws.Send(serializer.Serialize(sub3));

        }
        #endregion
        #endregion




        #region DTOS
        public class SoketSendDTO
        {
            public string userName;
            public string category;
            public string userQuestionIndex;
        }
        #endregion
    }
}