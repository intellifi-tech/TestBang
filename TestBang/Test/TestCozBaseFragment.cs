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
        Button TestOlusturButton;
        GenelTestSonuclariDTO GenelTestSonuclariDTO1 = new GenelTestSonuclariDTO();

        TextView EnCokDersText, EnCokDers_Bos, EnCokDers_Dogru, EnCokDers_Yalnis;
        TextView ToplamSoruSayisi, Toplam_Bos, Toplam_Dogru, Toplam_Yalnis;
        TextView ToplamSureText;
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


            EnCokDersText = Vieww.FindViewById<TextView>(Resource.Id.encokders_adi);
            EnCokDers_Bos = Vieww.FindViewById<TextView>(Resource.Id.encokders_bos);
            EnCokDers_Dogru = Vieww.FindViewById<TextView>(Resource.Id.encokders_dogru);
            EnCokDers_Yalnis = Vieww.FindViewById<TextView>(Resource.Id.encokders_yalnis);

            ToplamSoruSayisi = Vieww.FindViewById<TextView>(Resource.Id.toplam_sorusayisi);
            Toplam_Bos = Vieww.FindViewById<TextView>(Resource.Id.toplam_bos);
            Toplam_Dogru = Vieww.FindViewById<TextView>(Resource.Id.toplam_dogru);
            Toplam_Yalnis = Vieww.FindViewById<TextView>(Resource.Id.toplam_yalnis);

            ToplamSureText = Vieww.FindViewById<TextView>(Resource.Id.toplam_sure);

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
                    if (GenelTestSonuclariDTO1!=null)
                    {
                        if (GenelTestSonuclariDTO1.sumOfQuestions != null)
                        {
                            EnCoklariGetir();
                            ToplamCozumDetaylariniGetir();
                            ToplamSureYansit();
                            FillDataModel();
                        }
                        else
                        {
                            this.Activity.RunOnUiThread(delegate ()
                            {
                                EnCokDersText.Text = "";
                                EnCokDers_Bos.Text = "";
                                EnCokDers_Dogru.Text = "";
                                EnCokDers_Yalnis.Text = "";
                                ToplamSoruSayisi.Text = "";
                                Toplam_Bos.Text = "";
                                Toplam_Dogru.Text = "";
                                Toplam_Yalnis.Text = "";
                                ToplamSureText.Text = "";
                            });
                        }
                    }
                }
                //ShowLoading.Hide();
            })).Start();
        }

        void EnCoklariGetir()
        {
            var SortedListt = GenelTestSonuclariDTO1.userLessonInfoDTOS.OrderBy(o => o.questionCount).ToList();
            SortedListt.Reverse();
            this.Activity.RunOnUiThread(delegate () {
                //EnCokDersText, EnCokDers_Bos, EnCokDers_Dogru, EnCokDers_Yalnis;
                EnCokDersText.Text = SortedListt[0].lessonName;
                EnCokDers_Bos.Text = SortedListt[0].emptyCount.ToString();
                EnCokDers_Dogru.Text = SortedListt[0].correctCount.ToString();
                EnCokDers_Yalnis.Text = SortedListt[0].wrongCount.ToString();
            });
        }
        void ToplamCozumDetaylariniGetir()
        {
            //ToplamSoruSayisi, Toplam_Bos, Toplam_Dogru, Toplam_Yalnis;
            this.Activity.RunOnUiThread(delegate () {

                ToplamSoruSayisi.Text = GenelTestSonuclariDTO1.sumOfQuestions.ToString();
                Toplam_Bos.Text = GenelTestSonuclariDTO1.sumOfEmpty.ToString();
                Toplam_Dogru.Text = GenelTestSonuclariDTO1.sumOfCorrect.ToString();
                Toplam_Yalnis.Text = GenelTestSonuclariDTO1.sumOfWrong.ToString();
            });
        }
        void ToplamSureYansit()
        {
            this.Activity.RunOnUiThread(delegate () {

                ToplamSureText.Text =new DateTime(0).AddMinutes((int)GenelTestSonuclariDTO1.totalTime).ToLongTimeString();

             });
        }


        void FillDataModel()
        {
            this.Activity.RunOnUiThread(delegate () {
                if (GenelTestSonuclariDTO1.userLessonInfoDTOS.Count > 0)
                {
                    mRecyclerView.HasFixedSize = true;
                    mLayoutManager = new LinearLayoutManager(this.Activity);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mViewAdapter = new TestCozListRecyclerViewAdapter(GenelTestSonuclariDTO1.userLessonInfoDTOS, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
                    mLayoutManager = new LinearLayoutManager(this.Activity);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                }
            });
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            this.Activity.StartActivity(typeof(TestCozumKonuDetayBaseActivity));
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