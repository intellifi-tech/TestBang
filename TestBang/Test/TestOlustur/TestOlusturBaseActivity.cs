﻿using System;
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
using TestBang.Test.TestSinavAlani;

namespace TestBang.Test.TestOlustur
{
    [Activity(Label = "TestBang")]
    public class TestOlusturBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button TesteBasla;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestOlusturBaseActivity);
            TesteBasla = FindViewById<Button>(Resource.Id.button3);
            TesteBasla.Click += TesteBasla_Click;
        }

        private void TesteBasla_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(TestSinavAlaniBaseActivity));
            this.Finish();
        }
    }
}