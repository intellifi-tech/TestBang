using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TestBang.GenericClass;

namespace TestBang.Profil.DersProgrami
{
    [Activity(Label = "TestBang")]
    public class DersProgramiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        RecyclerView mRecyclerView;
        Android.Support.V7.Widget.LinearLayoutManager mLayoutManager;
        DersProgramiRecyclerViewAdapter mViewAdapter;
        public List<DersProgramiDTO> DersProgramiList = new List<DersProgramiDTO>();
        Button YeniEkle;
        CalendarView Calenderr;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DersProgramiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            YeniEkle = FindViewById<Button>(Resource.Id.button2);
            YeniEkle.Click += YeniEkle_Click;
            Calenderr = FindViewById<CalendarView>(Resource.Id.calendarView1);
            Calenderr.FocusedMonthDateColor = Color.ParseColor("#1EB04B");
            FillDataModel();
        }

        private void YeniEkle_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DersProgramiEkleBaseActivity));
        }

        void FillDataModel()
        {

            for (int i = 0; i < 20; i++)
            {
                DersProgramiList.Add(new DersProgramiDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new DersProgramiRecyclerViewAdapter(DersProgramiList, this);
            mRecyclerView.SetAdapter(mViewAdapter);
            mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
            mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            try
            {
                SnapHelper snapHelper = new LinearSnapHelper();
                snapHelper.AttachToRecyclerView(mRecyclerView);
            }
            catch
            {
            }
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            
        }

        public class DersProgramiDTO
        {

        }
    }
}