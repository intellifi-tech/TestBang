using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Test.TestOlustur;
using TestBang.Test.TestSinavAlani;
using TestBang.WebServices;
using static TestBang.Test.TestOlustur.TestOlusturBaseActivity;

namespace TestBang.Test.TestTamamlandi
{
    [Activity(Label = "TestBang")]
    public class TestTamamlandiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        TestTamamlandiRecyclerViewAdapter mViewAdapter;
        public List<TestTamamlandiDTO> TestTamamlandiDTO1 = new List<TestTamamlandiDTO>();
        TextView ToplamSureText, DogruYuzde, DogruSayi, YanlisYuzde, YanlisSayi, BosYuzde, BosSayi;
        ProgressBar DogruProgres, YanlisProgres, BosProgres;
        Button YeniTestButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestTamamlandiBaseActivity);
            ToplamSureText = FindViewById<TextView>(Resource.Id.toplamsuretext);
            DogruYuzde = FindViewById<TextView>(Resource.Id.dogrucevapyuzde);
            DogruSayi = FindViewById<TextView>(Resource.Id.dogrucevapsayi);
            YanlisYuzde = FindViewById<TextView>(Resource.Id.yalniscevapyuzde);
            YanlisSayi = FindViewById<TextView>(Resource.Id.yalniscevapsayi);
            BosYuzde = FindViewById<TextView>(Resource.Id.boscevapyuzde);
            BosSayi = FindViewById<TextView>(Resource.Id.boscevapsayi);
            DogruProgres = FindViewById<ProgressBar>(Resource.Id.dogruprogress);
            YanlisProgres = FindViewById<ProgressBar>(Resource.Id.yanlisprogress);
            BosProgres = FindViewById<ProgressBar>(Resource.Id.bosprogress);
            YeniTestButton = FindViewById<Button>(Resource.Id.button2);
            ToplamSureText.Text = TestSinavAlaniHelperClass.ToplamTestCozumSuresi.ToLongTimeString();


            DogruYuzde.Text = "%0";
            YanlisYuzde.Text = "%0";
            BosYuzde.Text = "%0";
            DogruSayi.Text = "0";
            YanlisSayi.Text = "0";
            BosSayi.Text = "0";
            DogruProgres.Progress = 0;
            YanlisProgres.Progress = 0;
            BosProgres.Progress = 0;
            YeniTestButton.Click += YeniTestButton_Click;
            TestiKaydet();
        }

        private void YeniTestButton_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(TestOlusturBaseActivity));
            TestSinavAlaniHelperClass.TestSinavAlaniBaseActivity1.Finish();
            this.Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        void TestiKaydet()
        {
            ShowLoading.Show(this, "Sonuçlar Alınıyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                SecilenTest.OlusanTest.finish = true;
                SecilenTest.OlusanTest.finishDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                SecilenTest.OlusanTest.testTime = ((int)((TestSinavAlaniHelperClass.ToplamTestCozumSuresi - new DateTime(0)).TotalMinutes)).ToString();
                string jsonString = JsonConvert.SerializeObject(SecilenTest.OlusanTest);
                var Donus = webService.ServisIslem("user-tests", jsonString, Method: "PUT", UsePoll: true);
                if (Donus != "Hata")
                {
                    var sonuc = Donus.ToString();
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<OLUSTURULAN_TESTLER>(Donus.ToString());
                    if (Icerik!=null)
                    {
                        var TestinLokalKaydi = DataBase.OLUSTURULAN_TESTLER_GETIR_TestID(SecilenTest.OlusanTest.id);
                        if (TestinLokalKaydi.Count > 0)
                        {
                            Icerik.SorularJsonPath = TestinLokalKaydi[0].SorularJsonPath;
                            Icerik.localid = TestinLokalKaydi[0].localid;
                            Icerik.questionCount = TestinLokalKaydi[0].questionCount;
                            if (DataBase.OLUSTURULAN_TESTLER_Guncelle(Icerik))
                            {
                                this.RunOnUiThread(delegate
                                {
                                    DogruSayi.Text = Icerik.correctCount.ToString();
                                    YanlisSayi.Text = Icerik.wrongCount.ToString();
                                    BosSayi.Text = Icerik.emptyCount.ToString();

                                    DogruYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * Icerik.correctCount) / Icerik.questionCount)), 0);
                                    YanlisYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * Icerik.wrongCount) / Icerik.questionCount)), 0);
                                    BosYuzde.Text = "%" + Math.Round(Convert.ToDouble(((100 * Icerik.emptyCount) / Icerik.questionCount)), 0);

                                    DogruProgres.Progress = Convert.ToInt32(DogruYuzde.Text.Replace("%", ""));
                                    YanlisProgres.Progress = Convert.ToInt32(YanlisYuzde.Text.Replace("%", ""));
                                    BosProgres.Progress = Convert.ToInt32(BosYuzde.Text.Replace("%", ""));
                                    ShowLoading.Hide();
                                });
                            }
                            else
                            {
                                ShowLoading.Hide();
                                AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                                this.Finish();
                            }
                        }
                        else
                        {
                            ShowLoading.Hide();
                            AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                            this.Finish();
                        }
                    }
                    else
                    {
                        ShowLoading.Hide();
                        AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                        this.Finish();
                    }
                }
                else
                {
                    ShowLoading.Hide();
                    AlertHelper.AlertGoster("Bir Sorun Oluştu.", this);
                    this.Finish();
                }
                
            })).Start();
        }

        void FillDataModel()
        {
            for (int i = 0; i < 20; i++)
            {
                TestTamamlandiDTO1.Add(new TestTamamlandiDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new TestTamamlandiRecyclerViewAdapter(TestTamamlandiDTO1, this);
            mRecyclerView.SetAdapter(mViewAdapter);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }
        public class TestTamamlandiDTO
        {

        }
    }
}