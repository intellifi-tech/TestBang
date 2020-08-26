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
using Iyzipay;

namespace TestBang.GenericClass
{
    public static class IyzicoHelperCla
    {
        public static string TestbangStandartPaket = "c29d4927-7c06-4bbb-bc50-56a28b631e5f";
        
        public static Options options = new Options()
        {
            ApiKey = "sandbox-ssckX11QqSy9rAkcW7UIlLNDYynG0m0M",
            SecretKey = "sandbox-D3fhj5tbRRXO4QXudDnZsTveZwX2rZaR",
            BaseUrl = "https://sandbox-api.iyzipay.com"   //https://api.iyzipay.com
        };
    }
}