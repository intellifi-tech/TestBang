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
using static TestBang.Profil.Transkript.TranskriptListeBaseActivity;

namespace TestBang.Profil.Transkript
{
    class TranskriptListRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView TranskriptNo1;
        public TranskriptListRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            TranskriptNo1 = itemView.FindViewById<TextView>(Resource.Id.transkriptno1);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class TranskriptListRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<TranskriptListDTO> mData = new List<TranskriptListDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public TranskriptListRecyclerViewAdapter(List<TranskriptListDTO> GelenData, AppCompatActivity GelenContex)
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
        TranskriptListRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TranskriptListRecyclerViewHolder viewholder = holder as TranskriptListRecyclerViewHolder;
            HolderForAnimation = holder as TranskriptListRecyclerViewHolder;
            var item = mData[position];
            viewholder.TranskriptNo1.SetTextColor(Color.White);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TranskriptListesiCustomCardView, parent, false);

            return new TranskriptListRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}