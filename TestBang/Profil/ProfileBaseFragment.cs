﻿using System;
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
using TestBang.DataBasee;
using TestBang.Profil.ArkadasiniDavetEt;
using TestBang.Profil.Ayarlar;
using TestBang.Profil.DersProgrami;
using TestBang.Profil.ProfilDuzenle;
using TestBang.Profil.TestBangHakkinda;
using TestBang.Profil.Transkript;
using TestBang.Profil.UyelikBilgileri;
using TestBang.WebServices;

namespace TestBang.Profil
{
    public class ProfileBaseFragment : Android.Support.V4.App.Fragment
    {
        TextView ProfilDuzenleButton, UyelikBilgileriButton, DersProgramiButton,TranskriptButton,ArkadasiniDavetEtButton,TestBangHakkindaButton,Iletisimm;
        TextView AdSoyadText, IlIlceText;
        Button Ayarlarbutton;
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
            Iletisimm = Viewww.FindViewById<TextView>(Resource.Id.iletisimbutton);
            AdSoyadText = Viewww.FindViewById<TextView>(Resource.Id.adsoyadtext);
            IlIlceText = Viewww.FindViewById<TextView>(Resource.Id.ililcetext);
            Ayarlarbutton = Viewww.FindViewById<Button>(Resource.Id.ayarlarbutton);
            Ayarlarbutton.Click += Ayarlarbutton_Click;


            TestBangHakkindaButton.Click += TestBangHakkindaButton_Click;
            ArkadasiniDavetEtButton.Click += ArkadasiniDavetEtButton_Click;
            TranskriptButton.Click += TranskriptButton_Click;
            UyelikBilgileriButton.Click += UyelikBilgileri_Click;
            ProfilDuzenleButton.Click += ProfilDuzenle_Click;
            DersProgramiButton.Click += DersProgramiButton_Click;
            Iletisimm.Click += Iletisimm_Click;
            return Viewww;
        }

        private void Ayarlarbutton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(AyarlarBaseActivity));
        }

        private void Iletisimm_Click(object sender, EventArgs e)
        {
            Intent email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new String[] { "android@test-bang.com"});
            email.PutExtra(Intent.ExtraSubject, "Bilgi Almak İstiyorum.");
            email.PutExtra(Intent.ExtraText, "Merhaba,\n Aşağıdaki konu hakkında bilgi almak istiyorum.\n\n");

            //need this to prompts email client only  
            email.SetType("message/rfc822");

            this.StartActivity(Intent.CreateChooser(email, "Şununla Devam Et: "));
        }

        public override void OnStart()
        {
            base.OnStart();
            var UserInfo = DataBase.MEMBER_DATA_GETIR()[0];
            AdSoyadText.Text = (UserInfo.firstName + " " + UserInfo.lastName).ToUpper();
            IlIlceGetir(UserInfo.townId.ToString());
        }
        void IlIlceGetir(string TownID)
        {
            IlIlceText.Text = "";
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + TownID);
                if (Donus != null)
                {
                    var TownInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<TownDTO>(Donus.ToString());
                    if (TownInfo != null)
                    {
                        this.Activity.RunOnUiThread(delegate ()
                        {
                            IlIlceText.Text = TownInfo.name + " / " + TownInfo.cityName;
                        });
                    }
                }
            })).Start();
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



        public class TownDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}