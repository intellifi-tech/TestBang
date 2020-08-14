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
using TestBang.GenericClass;

namespace TestBang.Oyun.KazandinKaybettin
{
    [Activity(Label = "TestBang")]
    public class KazandinKaybettinBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        TextView Baslik,OyunText, KazanmaDurum;
        Button YeniOyun;
        ImageView KazanmaDurumIMG;
        AndroidX.CardView.Widget.CardView CardVieww;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.KazandinKaybettinBaseActivty);
            Baslik = FindViewById<TextView>(Resource.Id.textView1);
            KazanmaDurum = FindViewById<TextView>(Resource.Id.textView3);
            OyunText = FindViewById<TextView>(Resource.Id.textView2);
            YeniOyun = FindViewById<Button>(Resource.Id.testcozbutton);
            CardVieww = FindViewById<AndroidX.CardView.Widget.CardView>(Resource.Id.cardView1);
            KazanmaDurumIMG = FindViewById<ImageView>(Resource.Id.ımageView2);
            YeniOyun.Click += YeniOyun_Click;
            if (!KazandinKaybettinBaseActivity_Helper.Kazandinmi)
            {
                CardVieww.SetCardBackgroundColor(Android.Graphics.Color.White);
                Baslik.Text = "ÜZGÜNÜZ";
                OyunText.SetTextColor(Android.Graphics.Color.ParseColor("#11122E"));
                KazanmaDurumIMG.SetImageResource(Resource.Mipmap.oyunukaybettinimg);
                KazanmaDurum.Text = "KAYBETTİNİZ";
            }
        }

        private void YeniOyun_Click(object sender, EventArgs e)
        {
            OyunSocketHelper_Helper.WebSocket1.Close();
            this.Finish();
        }
    }

    public static class KazandinKaybettinBaseActivity_Helper
    {
        public static bool Kazandinmi { get; set; }
    }
}