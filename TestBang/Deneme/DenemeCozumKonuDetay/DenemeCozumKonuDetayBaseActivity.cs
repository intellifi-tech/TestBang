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
using static TestBang.Deneme.DenemeBaseFragment;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Deneme.DenemeCozumKonuDetay
{
    [Activity(Label = "TestBang")]
    public class DenemeCozumKonuDetayBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        DenemeCozumKonuDetayRecyclerViewAdapter mViewAdapter;
        public List<DenemeCozumKonuDetayDTO> DenemeCozumKonuDetayDTO1 = new List<DenemeCozumKonuDetayDTO>();
        List<List<DenemeCozumKonuDetayDTO>> DenemeCozumKonuDetayDTO_Gruplar = new List<List<DenemeCozumKonuDetayDTO>>();
        List<TopicDTO> TopicDTO1 = new List<TopicDTO>();

        TextView ToplamBosTxt, ToplamDogruTxt, ToplamYalnisTxt, DogruOranTxt, YanlisOranTxt, DersAdiTxt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.DenemeCozumKonuDetayBaseActivity);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            DersAdiTxt = FindViewById<TextView>(Resource.Id.textView1);
            ToplamBosTxt = FindViewById<TextView>(Resource.Id.textView4);
            ToplamDogruTxt = FindViewById<TextView>(Resource.Id.textView5);
            ToplamYalnisTxt = FindViewById<TextView>(Resource.Id.textView6);
            DogruOranTxt = FindViewById<TextView>(Resource.Id.textView7);
            YanlisOranTxt = FindViewById<TextView>(Resource.Id.textView8);
            DersAdiTxt.Text = DenemeCozumKonuDetayBaseActivity_Helper.SecilenDersAdi + " Soru Dağılımı";
        }
        protected override void OnStart()
        {
            base.OnStart();
            ShowLoading.Show(this, "Lütfen Bekleyin");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                TopicleriGetir();
                Duzenle();
                ShowLoading.Hide();
            })).Start();

        }

        void Duzenle()
        {
            WebService webService = new WebService();
            for (int i = 0; i < DenemeCozumKonuDetayBaseActivity_Helper.KullanicininGirdigiDenemelerDTO1.Count; i++)
            {
                var Donus2 = webService.OkuGetir("trial-informations/user/topic/" + DenemeCozumKonuDetayBaseActivity_Helper.KullanicininGirdigiDenemelerDTO1[i].id, UsePoll: true);
                if (Donus2 != null)
                {
                    var Icerikk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DenemeCozumKonuDetayDTO>>(Donus2.ToString());
                    if (Icerikk.Count > 0)
                    {
                        //Icerikk.ForEach(item => {
                        //    item.topicId = item.lessonId;
                        //});
                        List<DenemeCozumKonuDetayDTO> bosList = new List<DenemeCozumKonuDetayDTO>();
                        for (int i2 = 0; i2 < TopicDTO1.Count; i2++)
                        {
                            var IcerikVarmi = Icerikk.Find(item => item.topicId == TopicDTO1[i2].id.ToString());
                            if (IcerikVarmi!=null)
                            {
                                bosList.Add(IcerikVarmi);
                            }
                        }

                        DenemeCozumKonuDetayDTO_Gruplar.Add(bosList);
                    }
                }
            }
            if (DenemeCozumKonuDetayDTO_Gruplar.Count > 0)
            {
                ListeyiToparlaBirlestir();
                if (DenemeCozumKonuDetayDTO1.Count > 0)
                {
                    FillDataModel();
                }
            }
        }

        void ListeyiToparlaBirlestir()
        {
            DenemeCozumKonuDetayDTO1 = new List<DenemeCozumKonuDetayDTO>();
            for (int i = 0; i < DenemeCozumKonuDetayDTO_Gruplar.Count; i++)
            {
                for (int i2 = 0; i2 < DenemeCozumKonuDetayDTO_Gruplar[i].Count; i2++)
                {
                    var SearchIndex = DenemeCozumKonuDetayDTO1.FindIndex(item => item.topicId == DenemeCozumKonuDetayDTO_Gruplar[i][i2].topicId);
                    if (SearchIndex == -1)//Henüz bu ders listeye eklenmemis
                    {
                        DenemeCozumKonuDetayDTO1.Add(DenemeCozumKonuDetayDTO_Gruplar[i][i2]);
                    }
                    else//Daha önce eklenmis ise
                    {
                        DenemeCozumKonuDetayDTO1[SearchIndex].correctCount += DenemeCozumKonuDetayDTO_Gruplar[i][i2].correctCount;
                        DenemeCozumKonuDetayDTO1[SearchIndex].wrongCount += DenemeCozumKonuDetayDTO_Gruplar[i][i2].wrongCount;
                        DenemeCozumKonuDetayDTO1[SearchIndex].emptyCount += DenemeCozumKonuDetayDTO_Gruplar[i][i2].emptyCount;
                    }
                }
            }
        }
        private void FillDataModel()
        {
            this.RunOnUiThread(delegate ()
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mViewAdapter = new DenemeCozumKonuDetayRecyclerViewAdapter(DenemeCozumKonuDetayDTO1, this);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
                mLayoutManager = new LinearLayoutManager(this);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                ToplamlariYasit(DenemeCozumKonuDetayDTO1);
            });
        }


        void ToplamlariYasit(List<DenemeCozumKonuDetayDTO> TestCozumKonuDetayDTO1)
        {
            int Bos = 0, Dogru = 0, Yalnis=0;
            for (int i = 0; i < TestCozumKonuDetayDTO1.Count; i++)
            {
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].emptyCount.ToString()))
                {
                    Bos += Convert.ToInt32(TestCozumKonuDetayDTO1[i].emptyCount);
                }
                
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].correctCount.ToString()))
                {
                    Dogru += Convert.ToInt32(TestCozumKonuDetayDTO1[i].correctCount);
                }
                
                if (!string.IsNullOrEmpty(TestCozumKonuDetayDTO1[i].wrongCount.ToString()))
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

        void TopicleriGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("topics");
            if (Donus!=null)
            {
                TopicDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TopicDTO>>(Donus.ToString());
                if (TopicDTO1.Count>0)
                {
                    TopicDTO1 = TopicDTO1.FindAll(item => item.lessonId.ToString() == DenemeCozumKonuDetayBaseActivity_Helper.SecilenDersID);
                    if (TopicDTO1.Count<=0)
                    {
                        this.Finish();
                    }
                }
            }
            else
            {
                this.Finish();
            }
        }

        public class DenemeCozumKonuDetayDTO
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string trialType { get; set; }
            public string trialId { get; set; }
            public int correctCount { get; set; }//
            public int wrongCount { get; set; }//
            public int emptyCount { get; set; }//
            public string net { get; set; }
            public string point { get; set; }
            public string lessonId { get; set; }//
            public string lessonName { get; set; }//
            public string topicId { get; set; }
            public string topicName { get; set; }
            public string trialPoint { get; set; }
        }

        public class TopicDTO
        {
            public bool? ayt { get; set; }
            public string icon { get; set; }
            public int id { get; set; }
            public int lessonId { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }


        public static class DenemeCozumKonuDetayBaseActivity_Helper
        {
            public static string SecilenDersID { get; set; }
            public static string SecilenDersAdi { get; set; }
            public static List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO1 { get; set; }


        }
    }
}