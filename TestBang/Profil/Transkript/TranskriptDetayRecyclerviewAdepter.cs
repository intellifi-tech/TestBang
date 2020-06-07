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
        public TranskriptDetayRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {


            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class TranskriptDetayRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<TranskriptDetayDTO> mData = new List<TranskriptDetayDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        int Genislikk;

        public TranskriptDetayRecyclerViewAdapter(List<TranskriptDetayDTO> GelenData, AppCompatActivity GelenContex,int GelenGenislik)
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
            HolderForAnimation = holder as TranskriptDetayRecyclerViewHolder;
            var item = mData[position];
     
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