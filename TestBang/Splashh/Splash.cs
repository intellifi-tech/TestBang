using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using Firebase;
using Firebase.Iid;
using TestBang.AppIntro;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.MainPage;
using TestBang.Oyun.OyunKur.ArkadaslarindanSec;

namespace TestBang.Splashh
{
    [Activity(Label = "TestBang",MainLauncher =true)]
    public class Splash : Android.Support.V7.App.AppCompatActivity, Animator.IAnimatorListener
    {
        LottieAnimationView animationView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            SetContentView(Resource.Layout.Splash);
            animationView = FindViewById<LottieAnimationView>(Resource.Id.follow_icon2);
            new DataBase();
            FirebaseApp.InitializeApp(this);
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        async void SimulateStartup()
        {
            await Task.Delay(1000);
            this.RunOnUiThread(delegate
            {
                animationView.SetAnimation("testBang_01_Splash.json");
                animationView.AddAnimatorListener(this);
                animationView.PlayAnimation();
            });
            
        }
        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
            this.RunOnUiThread(delegate
            {
                var Kullanici = DataBase.MEMBER_DATA_GETIR();
                if (Kullanici.Count > 0)
                {
                    StartActivity(typeof(MainPageBaseActivity));
                }
                else
                {
                    StartActivity(typeof(AppIntroBaseActivity));
                }

                this.Finish();
            });
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }
    }
}