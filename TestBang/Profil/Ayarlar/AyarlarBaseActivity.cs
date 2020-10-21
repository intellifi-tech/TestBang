using System;
using System.Collections.Generic;
using System.IO;
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
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.Splashh;
using static TestBang.MainPage.MainPageBaseActivity;

namespace TestBang.Profil.Ayarlar
{
    [Activity(Label = "TestBang")]
    public class AyarlarBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        TextView BildirimButton, OturumKapatButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            SetContentView(Resource.Layout.AyarlarBaseActivity);
            BildirimButton = FindViewById<TextView>(Resource.Id.bildirimbutton);
            OturumKapatButton = FindViewById<TextView>(Resource.Id.oturumkapatbutton);
            BildirimButton.Click += BildirimButton_Click;
            OturumKapatButton.Click += OturumKapatButton_Click;
            BildirimDurum();
        }

        private void OturumKapatButton_Click(object sender, EventArgs e)
        {
            var cevap = new AlertDialog.Builder(this);
            cevap.SetCancelable(true);
            cevap.SetIcon(Resource.Mipmap.ic_launcher);
            cevap.SetTitle(Spannla(Color.Black, "TestBang!"));
            cevap.SetMessage(Spannla(Color.DarkGray, "Oturumunu kapatmak istediğine emin misin?"));
            cevap.SetPositiveButton(Spannla(Color.Black, "Evet"), delegate
            {
                string path;
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                File.Delete(System.IO.Path.Combine(path, "TestBang.db"));
                MainPageBaseActivity_Helperr.MainPageBaseActivity1.FinishAffinity();
                this.FinishAffinity();
                StartActivity(typeof(Splash));
                cevap.Dispose();
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

        void BildirimDurum()
        {
            var GuncelAyarlar = DataBase.AYARLAR_GETIR();
            if (GuncelAyarlar.Count > 0)
            {
                if (GuncelAyarlar[0].Notification==true)
                {
                    BildirimButton.Text = "Bildirimler (Açık)";
                }
                else
                {
                    BildirimButton.Text = "Bildirimler (Kapalı)";
                }
            }
            else
            {
                BildirimButton.Text = "Bildirimler (Kapalı)";
            }
        }

        private void BildirimButton_Click(object sender, EventArgs e)
        {
            var GuncelAyarlar = DataBase.AYARLAR_GETIR();
            if (GuncelAyarlar.Count > 0)
            {
                var DegisimOlacak = GuncelAyarlar[0];
                DegisimOlacak.Notification = !DegisimOlacak.Notification;
                DataBase.AYARLAR_Guncelle(DegisimOlacak);

            }
            BildirimDurum();
        }
    }
}