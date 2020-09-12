using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using static TestBang.Profil.Transkript.TranskriptDetayBaseActivity;

namespace TestBang.Profil.Transkript
{
    class TranskriptDetayRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView KonuAdiTxt, BosTxt, DogruTxt, YanlisTxt;
        public TranskriptDetayRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            KonuAdiTxt = itemView.FindViewById<TextView>(Resource.Id.textView1);
            BosTxt = itemView.FindViewById<TextView>(Resource.Id.textView2);
            DogruTxt = itemView.FindViewById<TextView>(Resource.Id.textView3);
            YanlisTxt = itemView.FindViewById<TextView>(Resource.Id.textView4);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class TranskriptDetayRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<DenemeCozumKonuDetayDTO> mData = new List<DenemeCozumKonuDetayDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        int Genislikk;

        public TranskriptDetayRecyclerViewAdapter(List<DenemeCozumKonuDetayDTO> GelenData, AppCompatActivity GelenContex,int GelenGenislik)
        {
            mData = GelenData;
            BaseActivity = GelenContex;
            Genislikk = GelenGenislik;
            
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
        TranskriptDetayRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TranskriptDetayRecyclerViewHolder viewholder = holder as TranskriptDetayRecyclerViewHolder;
            var item = mData[position];
            viewholder.KonuAdiTxt.Text = item.topicName;
            viewholder.BosTxt.Text = item.emptyCount.ToString();
            viewholder.DogruTxt.Text = item.correctCount.ToString();
            viewholder.YanlisTxt.Text = item.wrongCount.ToString();
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TranskriptDetayCustomCardView, parent, false);
            var paramss = v.LayoutParameters;
            paramss.Width = Genislikk;
            v.LayoutParameters = paramss;
            return new TranskriptDetayRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}