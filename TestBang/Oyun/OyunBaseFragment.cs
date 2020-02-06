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
using TestBang.Oyun.OyunKur;

namespace TestBang.Oyun
{
    public class OyunBaseFragment : Android.Support.V4.App.Fragment
    {
        Button RakipAraButton;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vieww = inflater.Inflate(Resource.Layout.OyunBaseFragment, container, false);
            RakipAraButton = Vieww.FindViewById<Button>(Resource.Id.button3);
            RakipAraButton.Click += RakipAraButton_Click;
            return Vieww;
        }
        private void RakipAraButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(OyunKurBaseActivity));
        }
    }
}