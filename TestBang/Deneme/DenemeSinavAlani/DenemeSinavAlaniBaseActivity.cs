using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestBang.Deneme.DenemeTamamlandi;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Oyun.ArkadaslarindanSec;
using TestBang.WebServices;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Deneme.DenemeSinavAlani
{
    [Activity(Label = "TestBang")]
    public class DenemeSinavAlaniBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewPager;
        Button SonrakiSoru, OncekiSoruButton, AraVerButton;

        #region Sure Ile Ilgili Tanimlamalar
        TextView SureText;
        System.Timers.Timer Timer1 = new System.Timers.Timer();
        DateTime SifirBaslangic = new DateTime(0);
        bool SureIslemeyeDevamEt = true;
        int Dakika = 1;
        DateTime BitisZamani;


        Android.Support.V4.App.FragmentTransaction ft;
        ImageButton CizimYapButton;
        FrameLayout CizimHaznesi;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DenemeSinavAlaniBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestSinavAlaniBaseActivity);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewPager.PageSelected += ViewPager_PageSelected;
            SonrakiSoru = FindViewById<Button>(Resource.Id.sonrakisorubutton);
            OncekiSoruButton = FindViewById<Button>(Resource.Id.oncekisorubutton);
            AraVerButton = FindViewById<Button>(Resource.Id.araverbutton);
            SureText = FindViewById<TextView>(Resource.Id.suretext);
            CizimYapButton = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            CizimHaznesi = FindViewById<FrameLayout>(Resource.Id.cizimhazne);
            CizimYapButton.Click += CizimYapButton_Click;
            AcKapat();
            AraVerButton.Click += AraVerButton_Click;
            OncekiSoruButton.Click += OncekiSoruButton_Click;
            SonrakiSoru.Click += SonrakiSoru_Click;
            viewPager.OffscreenPageLimit = 10000;
            DenemeSinavAlaniHelperClass.DenemeSorulariDTO1 = new List<DenemeSorulariDTO>();
            DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1 = new List<KullaniciCevaplariDTO>();
            for (int i = 0; i < (int)DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount; i++)
            {
                DenemeSinavAlaniHelperClass.KullaniciCevaplariDTO1.Add(new KullaniciCevaplariDTO());
            }
            FnInitTabLayout();
            Dakika = Convert.ToInt32((DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.finishDate- DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.startDate).TotalMinutes);
            BitisZamani = new DateTime(0).AddMinutes(Dakika);
            Timer1.Interval = 1000;
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            Timer1.Start();
        }

        bool Actinmi = false;
        protected override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                CizimYapDialogFragment CizimYapDialogFragment1 = new CizimYapDialogFragment(CizimYapButton_Click);
                ft = this.SupportFragmentManager.BeginTransaction();
                ft.AddToBackStack(null);
                ft.Replace(Resource.Id.cizimhazne, CizimYapDialogFragment1);//
                ft.Commit();
                Actinmi = true;
            }
        }
        public void CizimYapButton_Click(object sender, EventArgs e)
        {
            AcKapat();
        }

        #region Liste Aç Kapat Animation

        bool durum = true;
        int boyut;
        public void AcKapat()
        {
            int sayac1 = CizimHaznesi.Height;
            if (durum == false)
            {
                CizimHaznesi.Visibility = ViewStates.Visible;
                int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                CizimHaznesi.Measure(widthSpec, heightSpec);

                DisplayMetrics displayMetrics = new DisplayMetrics();
                WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
                int screenheight = displayMetrics.HeightPixels;
                ValueAnimator mAnimator = slideAnimator(0, screenheight);
                mAnimator.Start();
                durum = true;
            }
            else if (durum == true)
            {
                int finalHeight = CizimHaznesi.Height;

                ValueAnimator mAnimator = slideAnimator(finalHeight, 0);
                mAnimator.Start();
                mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                {
                    CizimHaznesi.Visibility = ViewStates.Gone;
                };
                durum = false;
            }

        }
        private ValueAnimator slideAnimator(int start, int end)
        {

            ValueAnimator animator = ValueAnimator.OfInt(start, end);
            //ValueAnimator animator2 = ValueAnimator.OfInt(start, end);
            //  animator.AddUpdateListener (new ValueAnimator.IAnimatorUpdateListener{
            animator.Update +=
                (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    //  int newValue = (int)
                    //e.Animation.AnimatedValue; // Apply this new value to the object being animated.
                    //  myObj.SomeIntegerValue = newValue; 
                    var value = (int)animator.AnimatedValue;
                    ViewGroup.LayoutParams layoutParams = CizimHaznesi.LayoutParameters;
                    layoutParams.Height = value;
                    CizimHaznesi.LayoutParameters = layoutParams;
                };
            //      });
            return animator;
        }

        #endregion

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (SureIslemeyeDevamEt)
            {
                SifirBaslangic = SifirBaslangic.AddSeconds(1);
                if (SifirBaslangic == BitisZamani)
                {
                    SureIslemeyeDevamEt = false;
                }
                else
                {
                    this.RunOnUiThread(delegate () {
                        SureText.Text = SifirBaslangic.ToLongTimeString();
                    });
                }
            }
            else
            {
                Timer1.Stop();
            }
        }

        private void AraVerButton_Click(object sender, EventArgs e)
        {
            //TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1 = this;
            //TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
            //Timer1.Stop();
            //new TestSoruKaydetGuncelle().KaydetGuncelle(SecilenTest.OlusanTest);
            //this.StartActivity(typeof(TesteAraVerildiBaseActivity));
        }

        public void TesteDevamEt()
        {
            Timer1.Start();
        }
        private void OncekiSoruButton_Click(object sender, EventArgs e)
        {
            viewPager.CurrentItem = viewPager.CurrentItem - 1;
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (viewPager.CurrentItem + 1 == Convert.ToInt32(DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount))
            {
                SonrakiSoru.Text = "SINAVI BİTİR";
            }
            else
            {
                if (viewPager.CurrentItem % 5 == 0)
                {
                    DenemeSorulariniGetir((Convert.ToInt32(viewPager.CurrentItem / 5) + 1).ToString().ToString(), "5");
                    for (int i = 0; i < 4; i++)
                    {
                        ((DenemeSinavAlaniParcaFragment)fragments[viewPager.CurrentItem + i]).UIGuncelle();
                    }
                }
                SonrakiSoru.Text = "SONRAKİ SORU";
            }
        }

        private void SonrakiSoru_Click(object sender, EventArgs e)
        {
            if (viewPager.CurrentItem + 1 == Convert.ToInt32(DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount))
            {

                this.StartActivity(typeof(DenemeTamamlandiBaseActivity));
                this.Finish();
            }
            else
            {
                if (viewPager.CurrentItem%5==0)
                {
                    DenemeSorulariniGetir((Convert.ToInt32(viewPager.CurrentItem / 5)+1).ToString(), "5");
                    for (int i = 0; i < 4; i++)
                    {
                        ((DenemeSinavAlaniParcaFragment)fragments[viewPager.CurrentItem + i]).UIGuncelle();
                    }
                }
                viewPager.CurrentItem = viewPager.CurrentItem + 1;
            }
        }

        Android.Support.V4.App.Fragment[] fragments;
        Java.Lang.ICharSequence[] titles;
        void FnInitTabLayout()
        {
            DenemeSorulariniGetir("1", "5");
            fragments = new Android.Support.V4.App.Fragment[Convert.ToInt32(DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount)];
            titles = new Java.Lang.ICharSequence[Convert.ToInt32(DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount)];
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new DenemeSinavAlaniParcaFragment(i);
            }
            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);

            ((DenemeSinavAlaniParcaFragment)fragments[0]).UIGuncelle();
            ((DenemeSinavAlaniParcaFragment)fragments[1]).UIGuncelle();
            ((DenemeSinavAlaniParcaFragment)fragments[2]).UIGuncelle();
            ((DenemeSinavAlaniParcaFragment)fragments[3]).UIGuncelle();
            ((DenemeSinavAlaniParcaFragment)fragments[4]).UIGuncelle();
        }

        void DenemeSorulariniGetir(string page,string size)
        {
            var datavarmi = Convert.ToInt32(page) * 5;
            if (DenemeSinavAlaniHelperClass.DenemeSorulariDTO1 == null || DenemeSinavAlaniHelperClass.DenemeSorulariDTO1.Count-1 < datavarmi)
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("trials/questions/" + DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.id.ToString() + "?page=" + page + "&size=" + size + "",UsePoll:true,DontUseSize:true);
                if (Donus != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DenemeSorulariDTO>>(Donus.ToString());
                    DenemeSinavAlaniHelperClass.DenemeSorulariDTO1.AddRange(Icerik);
                }
            }
        }
    }

    public class Answer
    {
        public string id { get; set; }
        public string index { get; set; }
        public string text { get; set; }
        public string imagePath { get; set; }
        public string questionId { get; set; }
    }
    public class DenemeSorulariDTO
    {
        public string id { get; set; }
        public string text { get; set; }
        public string imagePath { get; set; }
        public string topicId { get; set; }
        public string lessonId { get; set; }
        public string lessonName { get; set; }
        public string correctAnswer { get; set; }
        public string description { get; set; }
        public List<Answer> answers { get; set; }
        public string trialId { get; set; }
    }

    public class KullaniciCevaplariDTO
    {
        public bool correct { get; set; }
        public bool empty { get; set; }
        public string id { get; set; }
        public string lessonId { get; set; }
        public string lessonName { get; set; }
        public string questionId { get; set; }
        public string topicId { get; set; }
        public string topicName { get; set; }
        public string trialId { get; set; }
        public string userAnswer { get; set; }
        public string userId { get; set; }
        public bool wrong { get; set; }
    }

    public static class DenemeSinavAlaniHelperClass
    {
        public static DenemeSinavAlaniBaseActivity DenemeSinavAlaniBaseActivity1 { get; set; }
        public static DateTime ToplamTestCozumSuresi { get; set; }
        public static UzakSunucuDenemeDTO UzakSunucuDenemeDTO1 { get; set; }
        public static List<DenemeSorulariDTO> DenemeSorulariDTO1 { get; set; }
        public static List<KullaniciCevaplariDTO> KullaniciCevaplariDTO1 { get; set; }
    }
}
