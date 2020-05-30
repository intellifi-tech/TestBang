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
using TestBang.DataBasee;
using TestBang.GenericClass;

namespace TestBang.Profil.ArkadasiniDavetEt
{
    [Activity(Label = "TestBang")]
    public class ArkadasiniDavetEtBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button DavetGonderButton;

        MEMBER_DATA MeUser = DataBase.MEMBER_DATA_GETIR()[0];
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ArkadasiniDavetEtBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            DavetGonderButton = FindViewById<Button>(Resource.Id.button2);
            FindViewById<TextView>(Resource.Id.adsoyadtext).Text = MeUser.firstName + " " + MeUser.lastName;
            DavetGonderButton.Click += DavetGonderButton_Click;
        }

        private void DavetGonderButton_Click(object sender, EventArgs e)
        {
            Intent intentsend = new Intent();
            intentsend.SetAction(Intent.ActionSend);
            intentsend.PutExtra(Intent.ExtraText, "Kim demiş eğitim sadece okulda olur diye?\n\nTest Bang! ile dilediğin yerde, dilediğin zaman, sana özel takvim ve programlarla kendini sınavlara hazırla, Türkiye çapında rekabete hemen şimdi başla.\n\nhttps://play.google.com/store/apps/details?id=com.testbang.android");
            intentsend.SetType("text/plain");
            StartActivity(intentsend);
        }
    }
}