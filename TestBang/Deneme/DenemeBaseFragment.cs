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
using TestBang.GenericClass;

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
            ButtonsHazne.ClipToOutline = true;
            FnInitTabLayout();
            return Vieww;
        }
        public override void OnStart()
        {
            base.OnStart();
            FillDataModel();
            
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
            tabLayout.SetTabTextColors(Android.Graphics.Color.ParseColor("#11122E"), Android.Graphics.Color.ParseColor("#F05070"));
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