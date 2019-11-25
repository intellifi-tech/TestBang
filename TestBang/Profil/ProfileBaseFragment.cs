using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestBang.Profil.ProfilDuzenle;
using TestBang.Profil.UyelikBilgileri;

namespace TestBang.Profil
{
    public class ProfileBaseFragment : Android.Support.V4.App.Fragment
    {
        TextView ProfilDuzenle,UyelikBilgileri;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View Viewww = inflater.Inflate(Resource.Layout.ProfileBaseFragment, container, false);
            ProfilDuzenle = Viewww.FindViewById<TextView>(Resource.Id.profilim);
            UyelikBilgileri = Viewww.FindViewById<TextView>(Resource.Id.uyelik);
            UyelikBilgileri.Click += UyelikBilgileri_Click;
            ProfilDuzenle.Click += ProfilDuzenle_Click;
            return Viewww;
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