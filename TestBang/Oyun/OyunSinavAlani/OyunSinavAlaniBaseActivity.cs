using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericClass.StompHelper;
using TestBang.GenericUI;
using TestBang.Oyun.KazandinKaybettin;
using TestBang.WebServices;
using static TestBang.GenericClass.OyunSocketHelper;

namespace TestBang.Oyun.OyunSinavAlani
{
    [Activity(Label = "TestBang")]
    public class OyunSinavAlaniBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewPager;
        Button SonrakiSoru, OncekiSoruButton;

        #region Sure Ile Ilgili Tanimlamalar
        TextView SureText,OIsim,BenIsim;
        System.Timers.Timer Timer1 = new System.Timers.Timer();
        DateTime SifirBaslangic = new DateTime(0);
        bool SureIslemeyeDevamEt = true;
        int Dakika = 1;
        DateTime BitisZamani;
        List<LinearLayout> BenRatingList = new List<LinearLayout>();
        List<LinearLayout> ORatingList = new List<LinearLayout>();
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];
        ChatRoom KarsiKullaniciSonDurum1 = null;
        bool BenBitirdimmi = false;

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Lacivert(this);
            SetContentView(Resource.Layout.OyunSinavAlaniBaseActivity);
            OIsim = FindViewById<TextView>(Resource.Id.oIsim);
            BenIsim = FindViewById<TextView>(Resource.Id.benIsim);
            OIsim.Text = "";
            BenIsim.Text = "";
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewPager.PageSelected += ViewPager_PageSelected;
            SonrakiSoru = FindViewById<Button>(Resource.Id.sonrakisorubutton);
            OncekiSoruButton = FindViewById<Button>(Resource.Id.oncekisorubutton);
            //AraVerButton = FindViewById<Button>(Resource.Id.araverbutton);
            SureText = FindViewById<TextView>(Resource.Id.suretext);


           // AraVerButton.Click += AraVerButton_Click;
            OncekiSoruButton.Click += OncekiSoruButton_Click;
            SonrakiSoru.Click += SonrakiSoru_Click;
            viewPager.OffscreenPageLimit = 10000;
            FnInitTabLayout();
            Dakika = 10;
            BitisZamani = new DateTime(0).AddMinutes(Dakika);
            Timer1.Interval = 1000;
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            Timer1.Start();


            #region Ratingleri Ekle
            BenRatingList = new List<LinearLayout>() {
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben1),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben2),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben3),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben4),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben5),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben6),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben7),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben8),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben9),
                FindViewById<LinearLayout>(Resource.Id.linearLayoutben10),
            };

            ORatingList = new List<LinearLayout>() {
                FindViewById<LinearLayout>(Resource.Id.linearLayouto1),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto2),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto3),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto4),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto5),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto6),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto7),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto8),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto9),
                FindViewById<LinearLayout>(Resource.Id.linearLayouto10),
            };
            #endregion
            TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1 = this;
            AdSoyadYansit();
        }


        void AdSoyadYansit()
        {
            BenIsim.Text = Me.firstName;
            var BenOlmayan = OyunSocketHelper_Helper.RoomQuestionsDTO1.chatRoom.users.Find(item => item.userName != Me.login);
            if (BenOlmayan!=null)
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    WebService webService = new WebService();
                    var Donus = webService.OkuGetir("users/" + BenOlmayan.userName);
                    if (Donus!=null)
                    {
                        var Icerik= Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                        if (Icerik!=null)
                        {
                            this.RunOnUiThread(delegate ()
                            {
                                OIsim.Text = Icerik.firstName;
                            });
                        }
                    }
                })).Start();
            }
        }


        public void BenGuncelle(int CozumSayisi)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i + 1 <= CozumSayisi)
                {
                    BenRatingList[i].SetBackgroundResource(Resource.Drawable.oyunyesil);
                }
                else
                {
                    BenRatingList[i].SetBackgroundResource(Resource.Drawable.oyungri);
                }

            }
        }
        int KarsiTarafCozumSayisi;
        public void OGuncelle(int CozumSayisi)
        {
            KarsiTarafCozumSayisi = CozumSayisi;
            for (int i = 0; i < 10; i++)
            {
                if (i + 1 <= CozumSayisi)
                {
                    ORatingList[i].SetBackgroundResource(Resource.Drawable.oyunyesil);
                }
                else
                {
                    ORatingList[i].SetBackgroundResource(Resource.Drawable.oyungri);
                }

            }
        }
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
            Timer1.Stop();
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
            if (viewPager.CurrentItem + 1 == 10)
            {
                SonrakiSoru.Text = "OYUNU BİTİR";
            }
            else
            {
                SonrakiSoru.Text = "SONRAKİ SORU";
            }
        }

        private void SonrakiSoru_Click(object sender, EventArgs e)
        {
            if (viewPager.CurrentItem + 1 == 10)
            {
                var cevap = new AlertDialog.Builder(this);
                cevap.SetCancelable(true);
                cevap.SetIcon(Resource.Mipmap.ic_launcher);
                cevap.SetTitle(Spannla(Color.Black, "TestBang!"));
                cevap.SetMessage(Spannla(Color.DarkGray, "Oyunu bitirmek istediğine emin misin?"));
                cevap.SetPositiveButton("Evet", delegate
                {
                    OyunuBitir();
                    ShowLoading.Show(this, "Rakibin oyunu bitirmesi bekleniyor..");
                });
                cevap.SetNegativeButton("Hayır", delegate
                {
                });
                cevap.Show();
            }
            else
            {
                viewPager.CurrentItem = viewPager.CurrentItem + 1;
            }
        }

        SpannableStringBuilder Spannla(Color Renk, string textt)
        {
            ForegroundColorSpan foregroundColorSpan = new ForegroundColorSpan(Renk);

            string title = textt;
            SpannableStringBuilder ssBuilder = new SpannableStringBuilder(title);
            ssBuilder.SetSpan(
                    foregroundColorSpan,
                    0,
                    title.Length,
                    SpanTypes.ExclusiveExclusive
            );
            return ssBuilder;
        }

        void OyunuBitir()
        {
            
            var ToplamCozumSayisi = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.FindAll(item => !string.IsNullOrEmpty(item.userAnswer)).Count;
            
            var content = new SoketSendRegisterDTO()
            {
                category = "",
                userName = Me.login,
                userQuestionIndex = "0",
                userToken = Me.API_TOKEN,
                filters = new List<string>(),
                correctCount = ToplamDogruSayisiDon(),
                questionCount = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.Count
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/end";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }

        int ToplamDogruSayisiDon()
        {
            int ToplamDogruSayisi = 0;
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.ForEach(item => {
                if (item.correctAnswer == item.userAnswer)
                {
                    ToplamDogruSayisi += 1;
                }
            });
            return ToplamDogruSayisi;
        }
        public void KarsiKullaniciOyuınuBitirdi(ChatRoom KarsiKullaniciSonDurum)
        {
            if (!KarsiKullaniciSonDurum.gameResult.isEquel)
            {
                if (KarsiKullaniciSonDurum.gameResult.userName != Me.login) //KarsiTaraf Kazandi
                {
                    KazandinKaybettinBaseActivity_Helper.Kazandinmi = false;
                }
                else//Ben Kazandim
                {
                    KazandinKaybettinBaseActivity_Helper.Kazandinmi = true;
                }
                ShowLoading.Hide();
                TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1 = this;
                TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
                this.StartActivity(typeof(KazandinKaybettinBaseActivity));
                this.Finish();
            }
            else
            {
                //Beraberlik Durumu
                KazandinKaybettinBaseActivity_Helper.Kazandinmi = true;
                ShowLoading.Hide();
                TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1 = this;
                TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
                this.StartActivity(typeof(KazandinKaybettinBaseActivity));
                this.Finish();
            }
            
        }

        Android.Support.V4.App.Fragment[] fragments;
        Java.Lang.ICharSequence[] titles;
        void FnInitTabLayout()
        {
            fragments = new Android.Support.V4.App.Fragment[OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.Count];
            titles = new Java.Lang.ICharSequence[OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.Count];
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new OyunSinavAlaniParcaFragment(i);
            }
            viewPager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles, true);
        }

        public void KarsiKullaniciOyunuTerkEtti()
        {
            KazandinKaybettinBaseActivity_Helper.Kazandinmi = true;
            TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1 = this;
            TestSinavAlaniHelperClass.ToplamTestCozumSuresi = SifirBaslangic;
            this.StartActivity(typeof(KazandinKaybettinBaseActivity));
            this.Finish();
        }


        public override void Finish()
        {
            OyundanCikisiIlet();
            base.Finish();
        }
        protected override void OnDestroy()
        {
            OyundanCikisiIlet();
            base.OnDestroy();
        }
        protected override void OnStop()
        {
            OyundanCikisiIlet();
            base.OnStop();

        }
        public override void OnBackPressed()
        {
            OyundanCikisiIlet();
            base.OnBackPressed();
        }

        void OyundanCikisiIlet()
        {
            var content = new SoketSendRegisterDTO()
            {
                category = "",
                userName = Me.login,
                userQuestionIndex = "0",
                userToken = Me.API_TOKEN,
                filters = new List<string>()
            };
            var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
            broad["content-type"] = "application/json";
            // broad["username"] = MeId.login;
            broad["destination"] = "/app/leave";
            var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
            if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
            {
                OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
            }
        }
    }

    public static class TestSinavAlaniHelperClass
    {
        public static OyunSinavAlaniBaseActivity OyunSinavAlaniBaseActivity1 { get; set; }
        public static DateTime ToplamTestCozumSuresi { get; set; }
    }
}
