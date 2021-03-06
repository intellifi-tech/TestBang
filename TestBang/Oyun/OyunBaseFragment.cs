﻿using System;
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
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Oyun.ArkadaslarindanSec;
using TestBang.Oyun.OyunKur;
using TestBang.Oyun.OyunSinavAlani;

namespace TestBang.Oyun
{
    public class OyunBaseFragment : Android.Support.V4.App.Fragment
    {
        Spinner AlanSpinner;
        string[] AlanlarDizi = new string[] { "Alan Seç", "SAY", "SÖZ","EA" };
        Button ArkadaslarindanSec, RasgeleAra;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vieww = inflater.Inflate(Resource.Layout.OyunBaseFragment, container, false);
            AlanSpinner = Vieww.FindViewById<Spinner>(Resource.Id.spinner1);
            AlanSpinner.Adapter = new ArrayAdapter(this.Activity, Android.Resource.Layout.SimpleListItem1, AlanlarDizi);
            ArkadaslarindanSec = Vieww.FindViewById<Button>(Resource.Id.button3);
            RasgeleAra = Vieww.FindViewById<Button>(Resource.Id.button2);
            ArkadaslarindanSec.Click += ArkadaslarindanSec_Click;
            RasgeleAra.Click += RasgeleAra_Click;
            return Vieww;
        }
        
        private void RasgeleAra_Click(object sender, EventArgs e)
        {
            if (AlanSpinner.SelectedItemPosition!=0)
            {
                OyunSocketHelper_Helper.SecilenAlan = AlanlarDizi[AlanSpinner.SelectedItemPosition].ToString();
                var RasgeleRakipAraDialogFragment1 = new RasgeleRakipAraDialogFragment();
                RasgeleRakipAraDialogFragment1.Cancelable = false;
                RasgeleRakipAraDialogFragment1.Show(this.Activity.SupportFragmentManager, "RasgeleRakipAraDialogFragment1");
            }
            else
            {
                AlertHelper.AlertGoster("Lütfen bir oyun alanı seç", this.Activity);
                return;
            }

            // this.Activity.StartActivity(typeof(OyunSinavAlaniBaseActivity));
        }

        private void ArkadaslarindanSec_Click(object sender, EventArgs e)
        {
            if (AlanSpinner.SelectedItemPosition != 0)
            {
                OyunSocketHelper_Helper.SecilenAlan = AlanlarDizi[AlanSpinner.SelectedItemPosition].ToString();
                var ArkadaslarindanSecDialogFragment1 = new ArkadaslarindanSecDialogFragment();
                ArkadaslarindanSecDialogFragment1.Show(this.Activity.SupportFragmentManager, "ArkadaslarindanSecDialogFragment1");
                //this.Activity.StartActivity(typeof(OyunSinavAlaniBaseActivity));
            }
            else
            {
                AlertHelper.AlertGoster("Lütfen bir oyun alanı seç", this.Activity);
                return;
            }
            
        }


        


    }
}