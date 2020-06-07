using System;
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

        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        DenemeBaseFragmentRecyclerViewAdapter mViewAdapter;
        public List<DenemeBaseFragmentDTO> DenemeBaseFragmentDTO1 = new List<DenemeBaseFragmentDTO>();

        Button GecmisDenemeSinavlariButton, GelecekDenemeSinavlariButton,DenemeSinavinaKatilButton;

        TextView AdText,IlIlceText;
        List<UzakSunucuTakvimDTO> UzakSunucuTakvimDTO1 = new List<UzakSunucuTakvimDTO>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vieww= inflater.Inflate(Resource.Layout.DenemeBaseFragment, container, false);
            tabLayout = Vieww.FindViewById<TabLayout>(Resource.Id.tabLayoutchart);
            viewPager = Vieww.FindViewById<ViewPager>(Resource.Id.viewPager1chart);
            ButtonsHazne = Vieww.FindViewById<LinearLayout>(Resource.Id.buttonsHazne);
            mRecyclerView = Vieww.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            GecmisDenemeSinavlariButton = Vieww.FindViewById<Button>(Resource.Id.oncekidenemesinavlaributton);
            GelecekDenemeSinavlariButton = Vieww.FindViewById<Button>(Resource.Id.gelecekdenemesinavlaributton);
            DenemeSinavinaKatilButton = Vieww.FindViewById<Button>(Resource.Id.button3);
            AdText = Vieww.FindViewById<TextView>(Resource.Id.adsoyadtext);
            IlIlceText = Vieww.FindViewById<TextView>(Resource.Id.ililcetext);
            GecmisDenemeSinavlariButton.Click += GecmisDenemeSinavlariButton_Click;
            GelecekDenemeSinavlariButton.Click += GelecekDenemeSinavlariButton_Click;
            DenemeSinavinaKatilButton.Click += DenemeSinavinaKatilButton_Click;

            ButtonsHazne.ClipToOutline = true;
            FnInitTabLayout();
            return Vieww;
        }

        private void DenemeSinavinaKatilButton_Click(object sender, EventArgs e)
        {
            var TumDenemeler= UzakSunucuTakvimDTO1.FindAll(item => !string.IsNullOrEmpty(item.trialId));
            if (TumDenemeler.Count>0)
            {
                var GelecekDenemeler = TumDenemeler.FindAll(item => item.date > DateTime.Now);
                if (GelecekDenemeler.Count>0)
                {
                    DersProgramiBaseActivityHelper.SecilenTarih = (DateTime)GelecekDenemeler[0].date;
                    var EnYakinDeneme = DenemeGetir(GelecekDenemeler[0].trialId);
                    if (EnYakinDeneme!=null)
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

        void TakvimIcerikleriniCek()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("calendars/user");
            if (Donus != null)
            {
                UzakSunucuTakvimDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UzakSunucuTakvimDTO>>(Donus.ToString());
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
            FillDataModel();
            var MeData = DataBase.MEMBER_DATA_GETIR()[0];
            AdText.Text = MeData.firstName.ToUpper() + " " + MeData.lastName.ToUpper();
            SetTownNameByID((int)MeData.townId);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                TakvimIcerikleriniCek();
            })).Start();
        }

        void FillDataModel()
        {
            for (int i = 0; i < 20; i++)
            {
                DenemeBaseFragmentDTO1.Add(new DenemeBaseFragmentDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new DenemeBaseFragmentRecyclerViewAdapter(DenemeBaseFragmentDTO1, (Android.Support.V7.App.AppCompatActivity)this.Activity);
            mRecyclerView.SetAdapter(mViewAdapter);
            mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
            mLayoutManager = new LinearLayoutManager(this.Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {

        }
        void FnInitTabLayout()
        {
            tabLayout.SetTabTextColors(Android.Graphics.Color.ParseColor("#000000"), Android.Graphics.Color.ParseColor("#000000"));
            Android.Support.V4.App.Fragment ss1, ss2, ss3, ss4, ss5;

            ss1 = new DenemeChartFragment_TYT();
            ss2 = new DenemeChartFragment_AYT();

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

            viewPager.Adapter = new TabPagerAdaptor(this.Activity.SupportFragmentManager, fragments, titles, true);

            tabLayout.SetupWithViewPager(viewPager);

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
                    if (Icerik!=null)
                    {
                        this.Activity.RunOnUiThread(delegate () {
                            IlIlceText.Text = Icerik.name + ", " + Icerik.cityName;
                        });
                    }
                }
            })).Start();
        }

        public class TownDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }


        public class DenemeBaseFragmentDTO
        {

        }

        public class DenemeChartFragment_TYT : Android.Support.V4.App.Fragment
        {
            ChartView ChartView1;
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
                CreateChart();
            }
            void CreateChart()
            {
                var entries = new[]
                {
                        new Entry(100)
                        {
                            Label = "Deneme 1",
                            ValueLabel = "100",
                            Color = SKColor.Parse("#1EB04B"),
                        },
                        new Entry(200)
                        {
                            Label = "Deneme 2",
                            ValueLabel = "200",
                            Color = SKColor.Parse("#F05070"),
                        },
                        new Entry(150)
                        {
                            Label = "Deneme 3",
                            ValueLabel = "150",
                            Color = SKColor.Parse("#8F5CE8"),
                        },
                        new Entry(300)
                        {
                            Label = "Deneme 4",
                            ValueLabel = "300",
                            Color = SKColor.Parse("#1EB04B"),
                        },
                        new Entry(200)
                        {
                            Label = "Deneme 5",
                            ValueLabel = "200",
                            Color = SKColor.Parse("#F05070"),
                        },
                    };

                var lineChart = new LineChart() { Entries = entries };
                lineChart.Margin = 15f;
                //chart.PointAreaAlpha = 0;
                lineChart.LineAreaAlpha = 0;
                lineChart.LineSize = 3f;

                ChartView1.Chart = lineChart;
            }
        }


        public class DenemeChartFragment_AYT : Android.Support.V4.App.Fragment
        {
            ChartView ChartView1;
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
                CreateChart();
            }
            void CreateChart()
            {

                var SAY = GetChartEntries("#F05070", "Deneme");
                var SOZ = GetChartEntries("#1EB04B", "Deneme");
                var EA = GetChartEntries("#8F5CE8", "Deneme");

                var chart = new MultiLineChart() { multiline_entries = new List<List<Entry>> { SAY, SOZ, EA } };
                chart.Margin = 15f;
                //chart.PointAreaAlpha = 0;
                chart.LineAreaAlpha = 0;
                chart.LineSize = 3f;
                
                ChartView1.Chart = chart;
            }

            List<Entry> GetChartEntries(string Colorr, string labell)
            {
                List<Entry> entries = new List<Entry>();
                for (int i = 0; i < 6; i++)
                {
                    entries.Add(
                    new Entry(new Random().Next(20, 100))
                    {
                        Label = labell + " " + i.ToString(),
                        ValueLabel = "",
                        Color = SKColor.Parse(Colorr)
                    });
                }
                return entries;
            }
        }
    }
}