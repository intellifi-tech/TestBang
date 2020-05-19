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
using TestBang.Deneme.DenemeSinavAlani;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Deneme
{
    class DenemeSayacDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        Button DenemeyeKatilButton;
        ImageButton KapatButton;
        TextView DenemeAdiText, DenemeTuruText, DenemeAciklamaText, KalanSureText;
        UzakSunucuDenemeDTO UzakSunucuDenemeDTO1;
        UzakSunucuTakvimDTO UzakSunucuTakvimDTO1;
        

        System.Timers.Timer Timer1 = new System.Timers.Timer();
        #endregion
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation3;
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, DPX.dpToPx(this.Activity, 460));
            Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.FillVertical | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
        }
        public DenemeSayacDialogFragment(UzakSunucuDenemeDTO UzakSunucuDenemeDTO2, UzakSunucuTakvimDTO UzakSunucuTakvimDTO2)
        {
            UzakSunucuDenemeDTO1 = UzakSunucuDenemeDTO2;
            UzakSunucuTakvimDTO1 = UzakSunucuTakvimDTO2;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            return dialog;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DenemeSayacDialogFragment, container, false);
            //view.FindViewById<RelativeLayout>(Resource.Id.rootView).ClipToOutline = true;
            DenemeyeKatilButton = view.FindViewById<Button>(Resource.Id.button2);
            DenemeyeKatilButton.Click += DenemeyeKatilButton_Click;
            KapatButton = view.FindViewById<ImageButton>(Resource.Id.ımageButton1);

            DenemeAdiText = view.FindViewById<TextView>(Resource.Id.denemeaditext);
            DenemeTuruText = view.FindViewById<TextView>(Resource.Id.denemeturutext);
            DenemeAciklamaText = view.FindViewById<TextView>(Resource.Id.denemeaciklamasitext);
            KalanSureText = view.FindViewById<TextView>(Resource.Id.denemekalansuretext);

            KapatButton.Click += KapatButton_Click;

            DenemeAdiText.Text = UzakSunucuDenemeDTO1.name;
            DenemeAciklamaText.Text = UzakSunucuDenemeDTO1.description;
            DenemeTuruText.Text = UzakSunucuDenemeDTO1.type;
            Timer1.Interval = 1000;
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            Timer1.Start();

            //if (UzakSunucuTakvimDTO1!=null)
            //{
            //    DenemeyeKatilButton.Text = "KATILIMINIZ ONAYLI";
            //    DenemeyeKatilButton.Enabled = false;
            //}

            return view;
        }
        void CreateCalander()
        {
            //DersProgramiBaseActivityHelper.SecilenTarih = DateTime.Now;
            WebService webService = new WebService();

            DERS_PROGRAMI dERS_PROGRAMI = new DERS_PROGRAMI()
            {
                date = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                description = "",
                trialId = UzakSunucuDenemeDTO1.id,
                userId = DataBase.MEMBER_DATA_GETIR()[0].id,
            };
            string jsonString = JsonConvert.SerializeObject(dERS_PROGRAMI);
            var Donus = webService.ServisIslem("calendars", jsonString);
            if (Donus != "Hata")
            {
                UzakSunucuTakvimDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<UzakSunucuTakvimDTO>(Donus.ToString());
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<DERS_PROGRAMI>(Donus.ToString());
                if (Icerik != null)
                {
                    DataBase.DERS_PROGRAMI_EKLE(Icerik);
                    AlertHelper.AlertGoster("Ders Programı Oluşturuldu", this.Activity);
                    DenemeyeKatilButton.Text = "KATILIMINIZ ONAYLI";
                    DenemeyeKatilButton.Enabled = false;
                    return;
                }
            }
        }
        void SinavaGir()
        {
            DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1 = this.UzakSunucuDenemeDTO1;
            this.Activity.StartActivity(typeof(DenemeSinavAlaniBaseActivity));
            this.Dismiss();
        }
        private void KapatButton_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void DenemeyeKatilButton_Click(object sender, EventArgs e)
        {
            SinavaGir();
            return;
            CreateCalander();
            
            
        }
        bool Actinmi = false;
        public override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                
                
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
                
                


                Actinmi = true;
            }
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var KalanZaman = UzakSunucuDenemeDTO1.startDate - DateTime.Now;
            KalanSureText.Text = (int)KalanZaman.Days + ":" + (int)KalanZaman.Hours + ":" + (int)KalanZaman.Minutes + ":" + (int)KalanZaman.Seconds;
        }
    }
}