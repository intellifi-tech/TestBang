using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public WebSocket ws = new WebSocket("ws://185.184.210.20:8080/ws/websocket");//ws://185.184.210.20:8080/ws/websocket
        //ws://192.168.1.38:8080/ws/app/register
        public StompMessageSerializer serializer = new StompMessageSerializer();
        OyunSocketHelper OyunSocketHelper1 = new OyunSocketHelper();
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


            OyunSocketHelper1.Init((Android.Support.V7.App.AppCompatActivity)this.Activity,this);

           // CreateSocketEvents();


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
        #region DTOS
         public class SoketSendRegisterDTO //ChatUser
        {
            public string userName;
            public string category;
            public string userQuestionIndex;
        }


        public class Content
        {
            public string type { get; set; }
            public string userName { get; set; }
            public string toUserName { get; set; }
            public string message { get; set; }
        }
        #endregion
    }
}