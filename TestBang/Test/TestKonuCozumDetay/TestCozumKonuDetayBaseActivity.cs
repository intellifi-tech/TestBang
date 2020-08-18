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
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.Test.TestKonuCozumDetay
{
    [Activity(Label = "TestBang")]
    public class TestCozumKonuDetayBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        TestCozumKonuDetayRecyclerViewAdapter mViewAdapter;
        public List<TestCozumKonuDetayDTO> CozulenTestlerDTO1 = new List<TestCozumKonuDetayDTO>();
        TextView ToplamBosTxt, ToplamDogruTxt, ToplamYalnisTxt, DogruOranTxt, YanlisOranTxt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestCozumKonuDetayBaseActivity);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            ToplamBosTxt = FindViewById<TextView>(Resource.Id.textView4);
            ToplamDogruTxt = FindViewById<TextView>(Resource.Id.textView5);
            ToplamYalnisTxt = FindViewById<TextView>(Resource.Id.textView6);
            DogruOranTxt = FindViewById<TextView>(Resource.Id.textView7);
            YanlisOranTxt = FindViewById<TextView>(Resource.Id.textView8);
        }
        protected override void OnStart()
        {
            base.OnStart();
            ShowLoading.Show(this, "Lütfen Bekleyin");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                TestCozumDetalariniGetir();
                ShowLoading.Hide();
            })).Start();

        }
        void TestCozumDetalariniGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("user-tests/test-results/" + TestCozumKonuDetayBaseActivity_Helper.SecilenDersID,UsePoll:true);
            if (Donus!=null)
            {
                var CozulenTestlerDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestCozumKonuDetayDTO>>(Donus.ToString());
                if (CozulenTestlerDTO1.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        mRecyclerView.HasFixedSize = true;
                        mLayoutManager = new LinearLayoutManager(this);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        mViewAdapter = new TestCozumKonuDetayRecyclerViewAdapter(CozulenTestlerDTO1, this);
                        mRecyclerView.SetAdapter(mViewAdapter);
                        mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
                        mLayoutManager = new LinearLayoutManager(this);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        ToplamlariYasit(CozulenTestlerDTO1);
                    });
                }
            }
        }
        void ToplamlariYasit(List<TestCozumKonuDetayDTO> TestCozumKonuDetayDTO1)
        {
            int Bos = 0, Dogru = 0, Yalnis=0;
            for (int i = 0; i < TestCozumKonuDetayDTO1.Count; i++)
            {
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].emptyCount))
                {
                    Bos += Convert.ToInt32(TestCozumKonuDetayDTO1[i].emptyCount);
                }
                
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].correctCount))
                {
                    Dogru += Convert.ToInt32(TestCozumKonuDetayDTO1[i].correctCount);
                }
                
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].wrongCount))
                {
                    Yalnis += Convert.ToInt32(TestCozumKonuDetayDTO1[i].wrongCount);
                }
            }

            ToplamBosTxt.Text = Bos.ToString();
            ToplamDogruTxt.Text = Dogru.ToString();
            ToplamYalnisTxt.Text = Yalnis.ToString();

            var ToplamCozumSayisi = Dogru + Yalnis;
            if (Dogru>0)
            {
                DogruOranTxt.Text =  Math.Round((double)((100 * Dogru) / ToplamCozumSayisi), 0).ToString()+"%";

            }
            else
            {
                DogruOranTxt.Text = "0%";
            }
            if (Yalnis>0)
            {
                YanlisOranTxt.Text = Math.Round((double)((100 * Yalnis) / ToplamCozumSayisi), 0).ToString() + "%";
            }
            else
            {
                YanlisOranTxt.Text = "0%";
            }
              
        }
        private void MViewAdapter_ItemClick(object sender, int e)
        {

        }

        public class TestCozumKonuDetayDTO
        {
            public string topicId { get; set; }
            public string topicName { get; set; }
            public string correctCount { get; set; }
            public string emptyCount { get; set; }
            public string wrongCount { get; set; }
        }

        public static class TestCozumKonuDetayBaseActivity_Helper
        {
            public static string SecilenDersID { get; set; }
        }
    }
}