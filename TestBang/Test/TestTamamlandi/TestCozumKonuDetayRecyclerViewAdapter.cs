using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Org.Json;
using static TestBang.Test.TestTamamlandi.TestTamamlandiBaseActivity;

namespace TestBang.Test.TestTamamlandi
{
    class TestTamamlandiAdapterHolder : RecyclerView.ViewHolder
    {
        public TestTamamlandiAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class TestTamamlandiRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<TestTamamlandiDTO> mData;
        public TestTamamlandiRecyclerViewAdapter(List<TestTamamlandiDTO> mData2, AppCompatActivity GelenContex)
        {
            mData = mData2;
            BaseActivity = GelenContex;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }
        public override int ItemCount
        {
            get
            {
                return mData.Count;
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TestTamamlandiAdapterHolder viewholder = holder as TestTamamlandiAdapterHolder;
           
        }
       
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TestTamamlandiCardView, parent, false);
          
            return new TestTamamlandiAdapterHolder(v, OnClickk);
        }

        void OnClickk(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}
