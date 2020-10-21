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
using Android.Icu.Text;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Java.IO;
using Newtonsoft.Json;
using Refractored.Controls;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.GenericUI
{
    class CizimYapDialogFragment : Android.Support.V4.App.Fragment
    {
        #region Tanimlamlar
        ImageButton KapatButton,TemizleButton;
        FingerPaintCanvasView fingerPaintCanvasView;
        EventHandler KapatButtonEvent;
        List<ImageButton> KalinlikButtonList = new List<ImageButton>();
        List<ImageButton> RenkButtonList = new List<ImageButton>();
        #endregion
        public CizimYapDialogFragment(EventHandler KapatButtonEvent1)
        {
            KapatButtonEvent = KapatButtonEvent1;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CizimEkrani, container, false);
            fingerPaintCanvasView = view.FindViewById<FingerPaintCanvasView>(Resource.Id.fingerPaintCanvasView1);
            fingerPaintCanvasView.SetBackgroundColor(Color.Transparent);
            KapatButton = view.FindViewById<ImageButton>(Resource.Id.ımageButton10);
            TemizleButton = view.FindViewById<ImageButton>(Resource.Id.ımageButton9);
            TemizleButton.Click += TemizleButton_Click;
            KapatButton.Click += KapatButtonEvent;
            fingerPaintCanvasView.StrokeWidth = 10;
            fingerPaintCanvasView.StrokeColor = Color.ParseColor("#11122E");

            KalinlikButtonList = new List<ImageButton>() {
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton1),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton2),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton3),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton4),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton5),
            };


            RenkButtonList = new List<ImageButton>() {
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton6),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton7),
                 view.FindViewById<ImageButton>(Resource.Id.ımageButton8),
            };

            for (int i = 0; i < KalinlikButtonList.Count; i++)
            {
                KalinlikButtonList[i].Tag = i;
                KalinlikButtonList[i].Click += KalinlikButton_Click;
            }  
            
            
            for (int i = 0; i < RenkButtonList.Count; i++)
            {
                RenkButtonList[i].Tag = i;
                RenkButtonList[i].Click += RenkButtonList_Click;
            }
            KalinlikButtonList[2].SetBackgroundColor(Color.ParseColor("#d3d3d3"));
            return view;
        }

        private void TemizleButton_Click(object sender, EventArgs e)
        {
            var cevap = new AlertDialog.Builder(this.Activity);
            cevap.SetCancelable(true);
            cevap.SetIcon(Resource.Mipmap.ic_launcher);
            cevap.SetTitle(Spannla(Color.Black, "TestBang!"));
            cevap.SetMessage(Spannla(Color.DarkGray, "TestPad ekranını temizlemek istediğine emin misin?"));
            cevap.SetPositiveButton(Spannla(Color.Black, "Evet"), delegate
            {
                fingerPaintCanvasView.ClearAll();
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

        public void SormadanTemizle()
        {
            fingerPaintCanvasView.ClearAll();
        }
        private void KalinlikButton_Click(object sender, EventArgs e)
        {
            KalinlikButtonList.ForEach(item => { item.SetBackgroundColor(Color.Transparent); });
            KalinlikButtonList[(int)((ImageButton)sender).Tag].SetBackgroundColor( Color.ParseColor("#d3d3d3"));
            switch ((int)((ImageButton)sender).Tag)
            {
                //2, 5, 10, 20, 50
                case 0:
                    fingerPaintCanvasView.StrokeWidth = 2;
                    break;
                case 1:
                    fingerPaintCanvasView.StrokeWidth = 5;
                    break;
                case 2:
                    fingerPaintCanvasView.StrokeWidth = 10;
                    break;
                case 3:
                    fingerPaintCanvasView.StrokeWidth = 20;
                    break;
                case 4:
                    fingerPaintCanvasView.StrokeWidth = 50;
                    break;
                default:
                    fingerPaintCanvasView.StrokeWidth = 10;
                    break;
            }
        }
        private void RenkButtonList_Click(object sender, EventArgs e)
        {
            switch ((int)((ImageButton)sender).Tag)
            {
                //2, 5, 10, 20, 50
                case 0:
                    //fingerPaintCanvasView.StrokeColor = Color.ParseColor("#F05070");
                    fingerPaintCanvasView.StrokeColor = Color.ParseColor("#F05070");
                    break;
                case 1:
                    fingerPaintCanvasView.StrokeColor = Color.ParseColor("#1EB04B");
                    break;
                case 2:
                    fingerPaintCanvasView.StrokeColor = Color.ParseColor("#11122E");
                    break;
                default:
                    fingerPaintCanvasView.StrokeColor = Color.ParseColor("#11122E");
                    break;
            }
        }
        

        bool Actinmi = false;
        public override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                Actinmi = true;
            }
        }
    }
}