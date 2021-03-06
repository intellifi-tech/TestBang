﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;
using TestBang.DataBasee;
using TestBang.Deneme.DenemeCozumKonuDetay;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Profil.DersProgrami;
using TestBang.Profil.Transkript;
using TestBang.WebServices;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Deneme
{
    public class DenemeBaseFragment : Android.Support.V4.App.Fragment
    {
        TabLayout tabLayout;
        ViewPager viewPager;
        LinearLayout ButtonsHazne;
        TextView MevcutSiralamaText, EnYuksekSiralamaText;

        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        DenemeBaseFragmentRecyclerViewAdapter mViewAdapter;
        public List<DenemeDersAnalizDTO> DenemeDersAnalizDTO1 = new List<DenemeDersAnalizDTO>();
        List<List<DenemeDersAnalizDTO>> DenemeDersAnalizDTO1_Gruplar = new List<List<DenemeDersAnalizDTO>>();



        Button GecmisDenemeSinavlariButton, GelecekDenemeSinavlariButton, DenemeSinavinaKatilButton;

        TextView AdText, IlIlceText;
        List<UzakSunucuTakvimDTO> UzakSunucuTakvimDTO1 = new List<UzakSunucuTakvimDTO>();
        List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO1 = new List<KullanicininGirdigiDenemelerDTO>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vieww = inflater.Inflate(Resource.Layout.DenemeBaseFragment, container, false);
            tabLayout = Vieww.FindViewById<TabLayout>(Resource.Id.tabLayoutchart);
            viewPager = Vieww.FindViewById<ViewPager>(Resource.Id.viewPager1chart);
            ButtonsHazne = Vieww.FindViewById<LinearLayout>(Resource.Id.buttonsHazne);
            mRecyclerView = Vieww.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            GecmisDenemeSinavlariButton = Vieww.FindViewById<Button>(Resource.Id.oncekidenemesinavlaributton);
            GelecekDenemeSinavlariButton = Vieww.FindViewById<Button>(Resource.Id.gelecekdenemesinavlaributton);
            DenemeSinavinaKatilButton = Vieww.FindViewById<Button>(Resource.Id.button3);
            AdText = Vieww.FindViewById<TextView>(Resource.Id.adsoyadtext);
            IlIlceText = Vieww.FindViewById<TextView>(Resource.Id.ililcetext);
            MevcutSiralamaText = Vieww.FindViewById<TextView>(Resource.Id.textView1);
            EnYuksekSiralamaText = Vieww.FindViewById<TextView>(Resource.Id.textView2);
            GecmisDenemeSinavlariButton.Click += GecmisDenemeSinavlariButton_Click;
            GelecekDenemeSinavlariButton.Click += GelecekDenemeSinavlariButton_Click;
            DenemeSinavinaKatilButton.Click += DenemeSinavinaKatilButton_Click;

            ButtonsHazne.ClipToOutline = true;





            return Vieww;
        }





        private void DenemeSinavinaKatilButton_Click(object sender, EventArgs e)
        {
            var TumDenemeler = UzakSunucuTakvimDTO1.FindAll(item => !string.IsNullOrEmpty(item.trialId));
            if (TumDenemeler.Count > 0)
            {
                var GelecekDenemeler = TumDenemeler.FindAll(item => item.date > DateTime.Now);
                if (GelecekDenemeler.Count > 0)
                {
                    DersProgramiBaseActivityHelper.SecilenTarih = (DateTime)GelecekDenemeler[0].date;
                    var EnYakinDeneme = DenemeGetir(GelecekDenemeler[0].trialId);
                    if (EnYakinDeneme != null)
                    {
                        DersProgramiBaseActivityHelper.SecilenDeneme = EnYakinDeneme;
                        var TakvimeKayitlimi = UzakSunucuTakvimDTO1.Find(item => item.trialId == DersProgramiBaseActivityHelper.SecilenDeneme.id);
                        var PaylasimSayisiDialogFragment1 = new DenemeSayacDialogFragment(DersProgramiBaseActivityHelper.SecilenDeneme, TakvimeKayitlimi);
                        PaylasimSayisiDialogFragment1.Show(this.Activity.SupportFragmentManager, "PaylasimSayisiDialogFragment1");
                    }
                    else
                    {
                        AlertHelper.AlertGoster("Bir sorun oluştu lütfen tekrar deneyin.", this.Activity);
                    }
                }
                else
                {
                    AlertHelper.AlertGoster("Takviminizde ileri tarihli bir deneme bulunmuyor.", this.Activity);
                }
            }
            else
            {
                AlertHelper.AlertGoster("Takviminizde ileri tarihli bir deneme bulunmuyor.", this.Activity);
            }
        }

        UzakSunucuDenemeDTO DenemeGetir(string trialid)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("trials/" + trialid, UsePoll: true);
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<UzakSunucuDenemeDTO>(Donus.ToString());
                if (Icerik != null)
                {
                    return Icerik;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void GelecekDenemeSinavlariButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(DersProgramiBaseActivity));

        }

        private void GecmisDenemeSinavlariButton_Click(object sender, EventArgs e)
        {
            this.Activity.StartActivity(typeof(TranskriptListeBaseActivity));
        }

        public override void OnStart()
        {
            base.OnStart();
            var MeData = DataBase.MEMBER_DATA_GETIR()[0];
            AdText.Text = MeData.firstName.ToUpper() + " " + MeData.lastName.ToUpper();
            SetTownNameByID((int)MeData.townId);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                KullanicininDenemeleriniGetir();

            })).Start();
        }


        void KullanicininDenemeleriniGetir()
        {
            DenemeDersAnalizDTO1_Gruplar = new List<List<DenemeDersAnalizDTO>>();
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("trials/user", UsePoll: true);

            if (Donus != null)
            {
                KullanicininGirdigiDenemelerDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KullanicininGirdigiDenemelerDTO>>(Donus.ToString());
                if (KullanicininGirdigiDenemelerDTO1 != null)
                {
                    if (KullanicininGirdigiDenemelerDTO1.Count > 0)
                    {
                        KullanicininGirdigiDenemelerDTO1.Reverse();

                        for (int i = 0; i < KullanicininGirdigiDenemelerDTO1.Count; i++)
                        {
                            var Donus2 = webService.OkuGetir("trial-informations/user/lesson/" + KullanicininGirdigiDenemelerDTO1[i].id, UsePoll: true);
                            if (Donus2 != null)
                            {
                                var Icerikk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DenemeDersAnalizDTO>>(Donus2.ToString());
                                if (Icerikk.Count > 0)
                                {
                                    DenemeDersAnalizDTO1_Gruplar.Add(Icerikk);

                                }
                            }
                        }
                        if (DenemeDersAnalizDTO1_Gruplar.Count > 0)
                        {
                            ListeyiToparlaBirlestir();
                            if (DenemeDersAnalizDTO1.Count > 0)
                            {
                                FillDataModel();
                                FnInitTabLayout();
                            }
                        }
                    }
                }
            }
        }

        void ListeyiToparlaBirlestir()
        {
            DenemeDersAnalizDTO1 = new List<DenemeDersAnalizDTO>();
            for (int i = 0; i < DenemeDersAnalizDTO1_Gruplar.Count; i++)
            {
                for (int i2 = 0; i2 < DenemeDersAnalizDTO1_Gruplar[i].Count; i2++)
                {
                    var SearchIndex = DenemeDersAnalizDTO1.FindIndex(item => item.lessonId == DenemeDersAnalizDTO1_Gruplar[i][i2].lessonId);
                    if (SearchIndex == -1)//Henüz bu ders listeye eklenmemis
                    {
                        DenemeDersAnalizDTO1.Add(DenemeDersAnalizDTO1_Gruplar[i][i2]);
                    }
                    else//Daha önce eklenmis ise
                    {
                        DenemeDersAnalizDTO1[SearchIndex].correctCount += DenemeDersAnalizDTO1_Gruplar[i][i2].correctCount;
                        DenemeDersAnalizDTO1[SearchIndex].wrongCount += DenemeDersAnalizDTO1_Gruplar[i][i2].wrongCount;
                        DenemeDersAnalizDTO1[SearchIndex].emptyCount += DenemeDersAnalizDTO1_Gruplar[i][i2].emptyCount;
                    }
                }
            }

        }

        void FillDataModel()
        {
            this.Activity.RunOnUiThread(delegate ()
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this.Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mViewAdapter = new DenemeBaseFragmentRecyclerViewAdapter(DenemeDersAnalizDTO1, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
                mLayoutManager = new LinearLayoutManager(this.Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
            });
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            DenemeCozumKonuDetay.DenemeCozumKonuDetayBaseActivity.DenemeCozumKonuDetayBaseActivity_Helper.SecilenDersID = DenemeDersAnalizDTO1[e].lessonId;
            DenemeCozumKonuDetay.DenemeCozumKonuDetayBaseActivity.DenemeCozumKonuDetayBaseActivity_Helper.KullanicininGirdigiDenemelerDTO1 = KullanicininGirdigiDenemelerDTO1;
            DenemeCozumKonuDetay.DenemeCozumKonuDetayBaseActivity.DenemeCozumKonuDetayBaseActivity_Helper.SecilenDersAdi = DenemeDersAnalizDTO1[e].lessonName;
            this.Activity.StartActivity(typeof(DenemeCozumKonuDetayBaseActivity));
        }
        void FnInitTabLayout()
        {

            Android.Support.V4.App.Fragment ss1, ss2;
            List<DenemeSonuclariPaunDTO> IcerikTYT, IcerikAYT;
            IcerikTYT = new List<DenemeSonuclariPaunDTO>();
            IcerikAYT = new List<DenemeSonuclariPaunDTO>();
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("user-trial-results/user", UsePoll: true);
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DenemeSonuclariPaunDTO>>(Donus.ToString());
                IcerikTYT = Icerik.FindAll(item => item.trialType == "TYT");
                IcerikAYT = Icerik.FindAll(item => item.trialType == "AYT");

                if (Icerik.Count > 0)
                {
                    if (Icerik[Icerik.Count - 1].order != null)
                    {
                        MevcutSiralamaText.Text = Icerik[Icerik.Count - 1].order.ToString();
                    }

                    try
                    {
                        var EnYusegiBul = Icerik;
                        var NullOlmayanlar = EnYusegiBul.FindAll(item => item.order != null).ToList();
                        if (NullOlmayanlar.Count > 0)
                        {
                            List<int> Siralamalar = new List<int>();
                            for (int i = 0; i < NullOlmayanlar.Count; i++)
                            {
                                Siralamalar.Add(Convert.ToInt32(NullOlmayanlar[i].order));
                            }

                            Siralamalar.Sort();
                            EnYuksekSiralamaText.Text = Siralamalar.Last().ToString();
                        }
                    }
                    catch (Exception exx)
                    {
                        string aaa = exx.Message;
                    }

                }
            }


            ss1 = new DenemeChartFragment_TYT(KullanicininGirdigiDenemelerDTO1.FindAll(item => item.type == "TYT"), IcerikTYT);
            ss2 = new DenemeChartFragment_AYT(KullanicininGirdigiDenemelerDTO1.FindAll(item => item.type == "AYT"), IcerikAYT);

            //Fragment array
            var fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
            };

            var titles = CharSequence.ArrayFromStringArray(new[] {
               "TYT",
               "AYT",
            });

            this.Activity.RunOnUiThread(delegate ()
            {
                tabLayout.SetTabTextColors(Android.Graphics.Color.ParseColor("#000000"), Android.Graphics.Color.ParseColor("#000000"));
                viewPager.Adapter = new TabPagerAdaptor(this.Activity.SupportFragmentManager, fragments, titles, true);

                tabLayout.SetupWithViewPager(viewPager);
            });

            //((TextView)tabLayout.GetTabAt(0).CustomView).SetTextSize(Android.Util.ComplexUnitType.Dip, 8);
        }
        void SetTownNameByID(int TownID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + TownID);
                if (Donus != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<TownDTO>(Donus.ToString());
                    if (Icerik != null)
                    {
                        this.Activity.RunOnUiThread(delegate ()
                        {
                            IlIlceText.Text = Icerik.name + ", " + Icerik.cityName;
                        });
                    }
                }
            })).Start();
        }
        public class KullanicininGirdigiDenemelerDTO
        {
            public string id { get; set; }
            public string name { get; set; }
            public string schoolId { get; set; }
            public DateTime? startDate { get; set; }
            public bool? resultExplained { get; set; }
            public bool? finish { get; set; }
            public DateTime? finishDate { get; set; }
            public string description { get; set; }
            public int? questionCount { get; set; }
            public string type { get; set; }
            public string userAlan { get; set; }
        }

        public class TownDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }

        public class DenemeDersAnalizDTO
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


        public class DenemeSonuclariPaunDTO
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string trialType { get; set; }
            public string trialId { get; set; }
            public int? correctCount { get; set; }
            public int? wrongCount { get; set; }
            public int? emptyCount { get; set; }
            public double? net { get; set; }
            public double? point { get; set; }
            public double? sayPoint { get; set; }
            public double? sozPoint { get; set; }
            public double? eaPoint { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public string topicId { get; set; }
            public string topicName { get; set; }
            public string trialPoint { get; set; }
            public DateTime? trialDate { get; set; }
            public List<LessonResult> lessonResults { get; set; }
            public string order { get; set; }
        }

        public class LessonResult
        {
            public string lessonId { get; set; }
            public string lesonName { get; set; }
            public int? correctCount { get; set; }
            public int? wrongCount { get; set; }
            public int? emptyCount { get; set; }
            public double? net { get; set; }
        }

        public class DenemeChartFragment_TYT : Android.Support.V4.App.Fragment
        {
            ChartView ChartView1;
            List<DenemeSonuclariPaunDTO> DenemeSonuclariDTO1;
            List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO1;
            public DenemeChartFragment_TYT(List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO2, List<DenemeSonuclariPaunDTO> DenemeSonuclariDTO2)
            {
                KullanicininGirdigiDenemelerDTO1 = KullanicininGirdigiDenemelerDTO2;
                DenemeSonuclariDTO1 = DenemeSonuclariDTO2;
            }

            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View Vieww = inflater.Inflate(Resource.Layout.DenemeChartFragment, container, false);
                ChartView1 = Vieww.FindViewById<ChartView>(Resource.Id.chartView3);
                return Vieww;
            }

            public override void OnStart()
            {
                base.OnStart();
                GetUserTrialResult();
                CreateChart();
            }
            void GetUserTrialResult()
            {
                if (DenemeSonuclariDTO1.Count > 0)
                {
                    DenemeSonuclariDTO1 = DenemeSonuclariDTO1.TakeLast(10).ToList();
                    CreateChart();
                }

            }
            void CreateChart()
            {
                List<Entry> entries = new List<Entry>();
                for (int i = 0; i < DenemeSonuclariDTO1.Count; i++)
                {
                    var DenemeDtosu = KullanicininGirdigiDenemelerDTO1.Find(item => item.id == DenemeSonuclariDTO1[i].trialId);
                    string DenemeAdi = "";
                    if (DenemeDtosu != null)
                    {
                        DenemeAdi = DenemeDtosu.name;
                    }
                    entries.Add(new Entry(Convert.ToSingle(Math.Round((double)DenemeSonuclariDTO1[i].point, 5) + 100))
                    {

                        Label = DenemeAdi,
                        ValueLabel = (DenemeSonuclariDTO1[i].point + 100).ToString(),
                        Color = SKColor.Parse(getRandomColor()),

                    });
                }

                var lineChart = new LineChart() { Entries = entries.ToArray() };
                lineChart.Margin = 15f;
                //chart.PointAreaAlpha = 0;
                lineChart.LineAreaAlpha = 0;
                lineChart.LineSize = 3f;

                ChartView1.Chart = lineChart;
            }

            string getRandomColor()
            {
                string[] colors = new string[] { "#1EB04B", "#F05070", "#8F5CE8", "#1EB04B" };
                Random _random = new Random();
                return colors[_random.Next(0, 4)].ToString();
            }


        }

        public class DenemeChartFragment_AYT : Android.Support.V4.App.Fragment
        {
            ChartView ChartView1;
            List<DenemeSonuclariPaunDTO> DenemeSonuclariDTO1;
            List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO1;

            public DenemeChartFragment_AYT(List<KullanicininGirdigiDenemelerDTO> KullanicininGirdigiDenemelerDTO2, List<DenemeSonuclariPaunDTO> DenemeSonuclariDTO2)
            {
                KullanicininGirdigiDenemelerDTO1 = KullanicininGirdigiDenemelerDTO2;
                DenemeSonuclariDTO1 = DenemeSonuclariDTO2;
            }


            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View Vieww = inflater.Inflate(Resource.Layout.DenemeChartFragment_AYT, container, false);
                ChartView1 = Vieww.FindViewById<ChartView>(Resource.Id.chartView3);
                return Vieww;
            }

            public override void OnStart()
            {
                base.OnStart();
                GetUserTrialResult();
                CreateChart();
            }

            void GetUserTrialResult()
            {
                if (DenemeSonuclariDTO1.Count > 0)
                {
                    DenemeSonuclariDTO1 = DenemeSonuclariDTO1.TakeLast(10).ToList();
                    CreateChart();
                }

            }

            void CreateChart()
            {
                var SAY = GetChartEntries("#F05070", 0);
                var SOZ = GetChartEntries("#1EB04B", 1);
                var EA = GetChartEntries("#8F5CE8", 2);

                var chart = new MultiLineChart() { multiline_entries = new List<List<Entry>> { SAY, SOZ, EA } };
                chart.Margin = 15f;
                //chart.PointAreaAlpha = 0;
                chart.LineAreaAlpha = 0;
                chart.LineSize = 3f;

                ChartView1.Chart = chart;
            }

            List<Entry> GetChartEntries(string Colorr, int index)
            {
                List<Entry> entries = new List<Entry>();
                for (int i = 0; i < DenemeSonuclariDTO1.Count; i++)
                {
                    float Puann = 0;
                    switch (index)
                    {
                        case 0:
                            Puann = Convert.ToSingle(Math.Round((double)DenemeSonuclariDTO1[i].sayPoint, 5));
                            break;
                        case 1:
                            Puann = Convert.ToSingle(Math.Round((double)DenemeSonuclariDTO1[i].sozPoint, 5));
                            break;
                        case 2:
                            Puann = Convert.ToSingle(Math.Round((double)DenemeSonuclariDTO1[i].eaPoint, 5));
                            break;
                        default:
                            break;
                    }
                    var DenemeDtosu = KullanicininGirdigiDenemelerDTO1.Find(item => item.id == DenemeSonuclariDTO1[i].trialId);
                    string DenemeAdi = "";
                    if (DenemeDtosu != null)
                    {
                        DenemeAdi = DenemeDtosu.name;
                    }
                    Puann = Puann + 100;
                    entries.Add(
                    new Entry(Puann)
                    {
                        Label = DenemeAdi,
                        ValueLabel = "",
                        Color = SKColor.Parse(Colorr)
                    });
                }
                return entries;
            }
        }
    }
}
