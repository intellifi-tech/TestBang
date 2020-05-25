using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using TestBang.DataBasee;

namespace TestBang.Oyun.ArkadaslarindanSec
{
    class ArkadasListRecyclerViewHolder : RecyclerView.ViewHolder
    {
       // public TextView ArkadasNo1;
        public ArkadasListRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            //ArkadasNo1 = itemView.FindViewById<TextView>(Resource.Id.transkriptno1);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class ArkadasListRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<MEMBER_DATA> mData = new List<MEMBER_DATA>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public ArkadasListRecyclerViewAdapter(List<MEMBER_DATA> GelenData, AppCompatActivity GelenContex)
        {
            mData = GelenData;
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
        ArkadasListRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ArkadasListRecyclerViewHolder viewholder = holder as ArkadasListRecyclerViewHolder;
            HolderForAnimation = holder as ArkadasListRecyclerViewHolder;
            var item = mData[position];
            //viewholder.ArkadasNo1.SetTextColor(Color.White);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.ArkadasListesiCardView, parent, false);

            return new ArkadasListRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}