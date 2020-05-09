using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestBang.GenericUI;
using TestBang.Test.TestKonuCozumDetay;
using TestBang.Test.TestOlustur;
using TestBang.WebServices;

namespace TestBang.Test
{
    public class TestCozBaseFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        TestCozListRecyclerViewAdapter mViewAdapter;
        public List<CozulenTestlerDTO> CozulenTestlerDTO1 = new List<CozulenTestlerDTO>();
        Button TestOlusturButton;

        GenelTestSonuclariDTO GenelTestSonuclariDTO1 = new GenelTestSonuclariDTO();


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vieww = inflater.Inflate(Resource.Layout.TestCozBaseFragment, container, false);
            mRecyclerView = Vieww.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            TestOlusturButton = Vieww.FindViewById<Button>(Resource.Id.button3);
            TestOlusturButton.Click += TestOlusturButton_Click;
            return Vieww;
        }

        private void TestOlusturButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(TestOlusturBaseActivity));
        }

        public override void OnStart()
        {
            base.OnStart();
            GenelTestSonuclariniGetir();
            FillDataModel();
        }

        void GenelTestSonuclariniGetir()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                 WebService webService = new WebService();
                var Donus = webService.OkuGetir("user-tests/test-results", UsePoll: true);
                if (Donus!=null)
                {
                    var aa = Donus.ToString();
                    GenelTestSonuclariDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<GenelTestSonuclariDTO>(Donus.ToString());
                }
                //ShowLoading.Hide();
            })).Start();
        }

        void FillDataModel()
        {

            for (int i = 0; i < 20; i++)
            {
                CozulenTestlerDTO1.Add(new CozulenTestlerDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new TestCozListRecyclerViewAdapter(CozulenTestlerDTO1, (Android.Support.V7.App.AppCompatActivity)this.Activity);
            mRecyclerView.SetAdapter(mViewAdapter);
            mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            this.Activity.StartActivity(typeof(TestCozumKonuDetayBaseActivity));
        }
        public class CozulenTestlerDTO
        {

        }


        public class UserLessonInfoDTO
        {
            public int? correctCount { get; set; }
            public int? emptyCount { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public int? questionCount { get; set; }
            public int? wrongCount { get; set; }
        }

        public class GenelTestSonuclariDTO
        {
            public int? sumOfCorrect { get; set; }
            public int? sumOfEmpty { get; set; }
            public int? sumOfQuestions { get; set; }
            public int? sumOfWrong { get; set; }
            public int? totalTime { get; set; }
            public List<UserLessonInfoDTO> userLessonInfoDTOS { get; set; }
        }
    }
}