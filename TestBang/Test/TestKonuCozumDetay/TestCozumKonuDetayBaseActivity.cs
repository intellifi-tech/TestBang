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

namespace TestBang.Test.TestKonuCozumDetay
{
    [Activity(Label = "TestBang")]
    public class TestCozumKonuDetayBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        TestCozumKonuDetayRecyclerViewAdapter mViewAdapter;
        public List<TestCozumKonuDetayDTO> CozulenTestlerDTO1 = new List<TestCozumKonuDetayDTO>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestCozumKonuDetayBaseActivity);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
        }
        protected override void OnStart()
        {
            base.OnStart();
            FillDataModel();
        }
        void FillDataModel()
        {

            for (int i = 0; i < 20; i++)
            {
                CozulenTestlerDTO1.Add(new TestCozumKonuDetayDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new TestCozumKonuDetayRecyclerViewAdapter(CozulenTestlerDTO1, this);
            mRecyclerView.SetAdapter(mViewAdapter);
            mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {

        }

        public class TestCozumKonuDetayDTO
        {

        }
    }
}