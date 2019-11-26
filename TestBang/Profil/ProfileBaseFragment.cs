using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestBang.Profil.ArkadasiniDavetEt;
using TestBang.Profil.DersProgrami;
using TestBang.Profil.ProfilDuzenle;
using TestBang.Profil.TestBangHakkinda;
using TestBang.Profil.Transkript;
using TestBang.Profil.UyelikBilgileri;

namespace TestBang.Profil
{
    public class ProfileBaseFragment : Android.Support.V4.App.Fragment
    {
        TextView ProfilDuzenleButton, UyelikBilgileriButton, DersProgramiButton,TranskriptButton,ArkadasiniDavetEtButton,TestBangHakkindaButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View Viewww = inflater.Inflate(Resource.Layout.ProfileBaseFragment, container, false);
            ProfilDuzenleButton = Viewww.FindViewById<TextView>(Resource.Id.profilim);
            UyelikBilgileriButton = Viewww.FindViewById<TextView>(Resource.Id.uyelik);
            DersProgramiButton = Viewww.FindViewById<TextView>(Resource.Id.dersprogrami);
            TranskriptButton = Viewww.FindViewById<TextView>(Resource.Id.transkriptt);
            ArkadasiniDavetEtButton = Viewww.FindViewById<TextView>(Resource.Id.arkadasinidavetet);
            TestBangHakkindaButton = Viewww.FindViewById<TextView>(Resource.Id.testbanghakkinda);



            TestBangHakkindaButton.Click += TestBangHakkindaButton_Click;
            ArkadasiniDavetEtButton.Click += ArkadasiniDavetEtButton_Click;
            TranskriptButton.Click += TranskriptButton_Click;
            UyelikBilgileriButton.Click += UyelikBilgileri_Click;
            ProfilDuzenleButton.Click += ProfilDuzenle_Click;
            DersProgramiButton.Click += DersProgramiButton_Click;
          
            return Viewww;
        }

        private void TestBangHakkindaButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(TestBangHakkindaBaseActivity));
        }

        private void ArkadasiniDavetEtButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(ArkadasiniDavetEtBaseActivity));
        }

        private void TranskriptButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(TranskriptListeBaseActivity));
        }

        private void DersProgramiButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(DersProgramiBaseActivity));
        }

        private void UyelikBilgileri_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(UyelikBilgileriBaseActivity));
        }

        private void ProfilDuzenle_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(ProfilDuzenlePart1BaseActivity));
        }
    }
}