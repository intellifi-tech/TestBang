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

namespace TestBang.Test.TestTamamlandi
{
    [Activity(Label = "TestBang")]
    public class TestTamamlandiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        LinearLayoutManager mLayoutManager;
        TestTamamlandiRecyclerViewAdapter mViewAdapter;
        public List<TestTamamlandiDTO> TestTamamlandiDTO1 = new List<TestTamamlandiDTO>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestTamamlandiBaseActivity);
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