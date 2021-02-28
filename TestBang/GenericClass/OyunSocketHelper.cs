using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass.StompHelper;
using TestBang.GenericUI;
using TestBang.Oyun.OyunKur.ArkadaslarindanSec;
using TestBang.Oyun.OyunSinavAlani;
using TestBang.WebServices;
using WebSocketSharp;
using static TestBang.GenericClass.OyunSocketHelper;
using static TestBang.Oyun.OyunKur.ArkadaslarindanSec.ArkadasOyunSec_Gonderen;

namespace TestBang.GenericClass
{
    public class OyunSocketHelper
    {
        #region Soket
        public WebSocket ws = new WebSocket("ws://185.184.210.20:8080/ws/websocket");//ws://185.184.210.20:8080/ws/websocket
                                                                                     //ws://192.168.1.38:8080/ws/app/register
        public StompMessageSerializer serializer = new StompMessageSerializer();

        MEMBER_DATA MeId;

        Android.Support.V7.App.AppCompatActivity GelenBase;
        Android.Support.V7.App.AppCompatDialogFragment GelenDialog;

        bool RegisterFriend1;
        #endregion

        public void Init(Android.Support.V7.App.AppCompatActivity GelenBase2, Android.Support.V7.App.AppCompatDialogFragment GelenDialog2,bool RegisterFriend=false)
        {
            MeId = DataBase.MEMBER_DATA_GETIR()[0];
            GelenBase = GelenBase2;
            GelenDialog = GelenDialog2;
            OyunSocketHelper_Helper.OyunSocketHelper1 = this;
            RegisterFriend1 = RegisterFriend;
            CreateSocketEvents();
        }
        void CreateSocketEvents()
        {

            // var cookiee = new WebSocketSharp.Net.Cookie("username", MeId.login); //poiu@poiuy.com qwer1234
            var cookie1 = new WebSocketSharp.Net.Cookie("clientType", "socket"); //poiu@poiuy.com qwer1234
            var cookiee2 = new WebSocketSharp.Net.Cookie("Bearer", MeId.API_TOKEN);

            ws.Log.Level = LogLevel.Trace;
            //  ws.SetCredentials("Bearer", "MeId.API_TOKEN", false);
            // ws.SetCookie(cookiee);
            ws.SetCookie(cookiee2);
            ws.SetCookie(cookie1);
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
            TestConnection();
        }
        void ws_OnClose(object sender, CloseEventArgs e)
        {
            UpdateListBox(" ws_OnClose says: " + e.ToString());
            System.Console.WriteLine("AÇILDII : " + DateTime.Now.ToString());
            //ws.Connect();
        }
        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            System.Console.WriteLine("-----------------------------");
            System.Console.WriteLine(" ws_OnMessage says: " + e.Data);
            StompMessage msg = serializer.Deserialize(e.Data);
            if (msg.Command == StompFrame.CONNECTED)
            {
                // UpdateListBox(e.Data);
                OyunSocketHelper_Helper.WebSocket1 = ws;
                OyunSocketHelper_Helper.OyunSocketHelper1 = this;
                SubscribeStomp();
                if (GelenDialog!=null)
                {
                    SendRegister();
                }
                else
                {
                    if (!RegisterFriend1)
                    {
                        SendRegisterWithFriend();
                    }
                }
            }
            else if (msg.Command == StompFrame.MESSAGE)
            {
                //RoomQuestionsDTO
                var MesajKanali = msg["destination"];
                if (MesajKanali == "/user/" + MeId.login + "/start")//Soruları aldığımız yer
                {
                    OyunSocketHelper_Helper.RoomQuestionsDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomQuestionsDTO>(msg.Body.ToString());
                    if (OyunSocketHelper_Helper.RoomQuestionsDTO1!=null)
                    {
                        //Oyunu Aç
                        GelenBase.StartActivity(typeof(OyunSinavAlaniBaseActivity));
                        //Rasgele Oyunu İse
                        if (GelenDialog!=null)
                        {
                            GelenDialog.Dismiss();
                        }
                        //Arkadaş Oyunu İse
                        if (GelenBase is ArkadasOyunSec_Gonderen || GelenBase is ArkadasOyunSec_Gelen)
                        {
                            try
                            {
                                ShowLoading.Hide();
                            }
                            catch 
                            {
                            }
                            
                            GelenBase.Finish();
                        }
                    }
                }
                else if (MesajKanali == "/user/" + MeId.login + "/index")//Kullanıcı çözülen soru son durum
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<SoketSendRegisterDTO>(msg.Body.ToString());
                    TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1.OGuncelle(Convert.ToInt32(Icerik.userQuestionIndex));
                }
                else if (MesajKanali == "/user/" + MeId.login + "/left")//Karsi kullanici oyunu terk etti
                {
                    TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1.KarsiKullaniciOyunuTerkEtti();
                }
                else if (MesajKanali == "/user/" + MeId.login + "/ended")//Kari kullanici oyunu bitirdi
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatRoom>(msg.Body.ToString());
                    if (Icerik != null)
                    {
                        if (Icerik.gameResult!=null)
                        {
                            TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1.KarsiKullaniciOyuınuBitirdi(Icerik);

                        }
                    }
                }
                else if(MesajKanali == "/user/" + MeId.login + "/denieded") // Arkadaş oyununda karı taraf isteği reddettiyse
                {
                    if (GelenBase is ArkadasOyunSec_Gonderen || GelenBase is ArkadasOyunSec_Gelen)
                    {
                        AlertHelper.AlertGoster("Meydan Okuma Reddedildi!", GelenBase);
                        GelenBase.Finish();
                    }
                }

                //Start => ChatRoom
                //Level => ChatUser  Send yapılır
                //Index => ChatUser 

            }
        }
        void TestConnection()
        {
            Timer _timerr;
            Handler h2;
            h2 = new Handler();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                _timerr = new System.Threading.Timer((o) =>
                {
                    if (ws.IsAlive)
                    {
                        var alivee = ws.IsAlive;
                        var pibngg = ws.Ping();
                        if (!alivee && !pibngg)
                        {
                            ws.Connect();
                        }
                    }
                    else
                    {
                        try
                        {
                            ws.Connect();
                        }
                        catch
                        {
                            Thread.Sleep(10000);
                        }

                    }
                }, null, 0, 5000);

            })).Start();
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
            sub1["id"] = MeId.login;
            sub1["destination"] = "/user/" + MeId.login + "/start"; //Oyun başlatıyoruz soruları alıyoruz
            ws.Send(serializer.Serialize(sub1));

            var sub2 = new StompMessage(StompFrame.SUBSCRIBE);
            sub2["id"] = MeId.login;
            sub2["destination"] = "/user/" + MeId.login + "/left"; //Bağlantın koptuğu anda buraya istek atıyorsun karşı oyuncuyu bilgilendirmek için
            ws.Send(serializer.Serialize(sub2));

            var sub3 = new StompMessage(StompFrame.SUBSCRIBE);
            sub3["id"] = MeId.login;
            sub3["destination"] = "/user/" + MeId.login + "/index"; //Oyun esnasında rakibin son durumunu alacağız
            ws.Send(serializer.Serialize(sub3));

            var sub4 = new StompMessage(StompFrame.SUBSCRIBE);
            sub4["id"] = MeId.login;
            sub4["destination"] = "/user/" + MeId.login + "/ended"; //Oyun bittiyse buradan düşer
            ws.Send(serializer.Serialize(sub4));


            //Arkadas oyunu ise ekstra olarak ;
            if (GelenBase is ArkadasOyunSec_Gonderen || GelenBase is ArkadasOyunSec_Gelen)
            {
                var sub5 = new StompMessage(StompFrame.SUBSCRIBE);
                sub5["id"] = MeId.login;
                sub5["destination"] = "/user/" + MeId.login + "/denieded"; //Arkadaş oyununda istek red edilme durumunu dinle
                ws.Send(serializer.Serialize(sub5));
            }

            // /app/level -> chatuser question index dolu
        }

        //Rasgele
        public void SendRegister()
        {
            var content = new SoketSendRegisterDTO()
            {
                category = OyunSocketHelper_Helper.SecilenAlan,//SAY SOZ EA
                userName = MeId.login,
                userQuestionIndex = "0",
                userToken = MeId.API_TOKEN,
                filters = GetLessonsForAlan(),
                isFriend = false
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/register";
            var aaa = serializer.Serialize(broad);
            if (ws.IsAlive)
            {
                ws.Send(aaa);
            }
        }

        //Arkadaşla Oynamak İçin
        public void SendRegisterWithFriend()
        {
            var content = new SoketSendRegisterDTO()
            {
                category = OyunSocketHelper_Helper.SecilenAlan,//SAY SOZ EA
                userName = MeId.login,
                userQuestionIndex = "0",
                userToken = MeId.API_TOKEN,
                filters = GetLessonsForAlan(),
                friendsUser = ArkadasOyunSec_Gonderen_Helper.SecilenKisi.login,
                isFriend = true,
                startTime = DateTime.Now.ToString()
                
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/requestFriend";
            var aaa = serializer.Serialize(broad);
            if (ws.IsAlive)
            {
                ws.Send(aaa);
            }
        }


        public List<string> GetLessonsForAlan()
        {
            WebService webService = new WebService();
            var jSONstring = webService.OkuGetir("lessons");
            if (jSONstring != null)
            {
                var Lessons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lessonss>>(jSONstring.ToString());
                if (Lessons.Count>0)
                {
                    switch (OyunSocketHelper_Helper.SecilenAlan)
                    {
                        case "SAY":
                            return Lessons.FindAll(item => item.say == true).Select(item2 => item2.id).ToList();
                        case "SOZ":
                            return Lessons.FindAll(item => item.soz == true).Select(item2 => item2.id).ToList();
                        case "EA":
                            return Lessons.FindAll(item => item.ea == true).Select(item2 => item2.id).ToList();
                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        /*
            private String userName;
            private String friendsUser;
            private String userToken;
            private String category;
            private String userQuestionIndex;
            private List<String> filters;
            private int  questionCount;
            private int  correctCount;
         
         */

        public class SoketSendRegisterDTO //ChatUserDTO
        {
            public string userName;
            public string userToken;
            public string category;
            public string userQuestionIndex;
            public List<string> filters;
            public int questionCount;
            public int correctCount;
            //ForFriendGame
            public string friendsUser;//Frined Mail Adress
            public bool isFriend { get; set; }
            public string startTime;
        }


        #region Lessons
        public class Lessonss
        {
            public string id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
            public string icon { get; set; }
            public bool say { get; set; }
            public bool soz { get; set; }
            public bool ea { get; set; }
        }


        #endregion

        #region QuestionsDTO




        public class ChatRoom
        {
            public string roomName { get; set; }
            public List<User> users { get; set; }
            public string roomCaetory { get; set; }
            public GameResult gameResult;
            public bool left;
            public bool right;
        }

        public class User
        {
            public string userName { get; set; }
            public string userToken { get; set; }
            public string category { get; set; }
            public string userQuestionIndex { get; set; }
            public List<string> filters { get; set; }
        }
       
        public class GameResult
        {
            public string userName;
            public int correctCount;
            public int questionCount;
            public bool isEnd;
            public bool isEquel;
        }

        public class Answer
        {
            public string id { get; set; }
            public string index { get; set; }
            public string text { get; set; }
            public string imagePath { get; set; }
        }

        public class QuestionList
        {
            public string id { get; set; }
            public string text { get; set; }
            public string imagePath { get; set; }
            public string topicId { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public string correctAnswer { get; set; }
            public string description { get; set; }
            public List<Answer> answers { get; set; }
            
            //----
            public string userAnswer { get; set; }
        
        }

        public class RoomQuestionsDTO
        {
            public ChatRoom chatRoom { get; set; }
            public List<QuestionList> questionList { get; set; }
        }

        #endregion
    }
    public static class OyunSocketHelper_Helper
    {
        public static WebSocket WebSocket1 { get; set; }
        public static OyunSocketHelper OyunSocketHelper1 { get; set; }
        public static RoomQuestionsDTO RoomQuestionsDTO1 { get; set; }
        public static string SecilenAlan { get; set; }
    }
}