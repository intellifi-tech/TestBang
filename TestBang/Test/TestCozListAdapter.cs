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
using static TestBang.Test.TestCozBaseFragment;

namespace TestBang.Test
{
    class TestCozListAdapterHolder : RecyclerView.ViewHolder
    {
        public TextView DersAdiText, BosSayiText, DogruSayiText, YalnisSayiText;
        public TestCozListAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            DersAdiText = itemView.FindViewById<TextView>(Resource.Id.dersaditext);
            BosSayiText = itemView.FindViewById<TextView>(Resource.Id.boscevapsayi);
            DogruSayiText = itemView.FindViewById<TextView>(Resource.Id.dogrucevapsayi);
            YalnisSayiText = itemView.FindViewById<TextView>(Resource.Id.yalniscevapsayi);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class TestCozListRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<UserLessonInfoDTO> mData;
        public TestCozListRecyclerViewAdapter(List<UserLessonInfoDTO> mData2, AppCompatActivity GelenContex)
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
            TestCozListAdapterHolder viewholder = holder as TestCozListAdapterHolder;
            var item = mData[position];
            viewholder.DersAdiText.Text = item.lessonName;
            viewholder.BosSayiText.Text = item.emptyCount.ToString();
            viewholder.DogruSayiText.Text = item.correctCount.ToString();
            viewholder.YalnisSayiText.Text = item.wrongCount.ToString();
        }
       
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TestCozListCardView, parent, false);
          
            return new TestCozListAdapterHolder(v, OnClickk);
        }

        void OnClickk(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}
