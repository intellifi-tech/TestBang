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

namespace TestBang.Bildirim
{
    [Activity(Label = "TestBang")]
    public class BildirimlerBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.BildirimlerBaseActivity);
        }
    }
}