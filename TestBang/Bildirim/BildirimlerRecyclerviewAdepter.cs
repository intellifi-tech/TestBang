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
using TestBang.WebServices;

namespace TestBang.Bildirim
{
    class BildirimlerRecyclerViewHolder : RecyclerView.ViewHolder
    {
         public TextView Aciklama,Tarih;
         public AndroidX.CardView.Widget.CardView cardView;

        public BildirimlerRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            Aciklama = itemView.FindViewById<TextView>(Resource.Id.textView2);
            Tarih = itemView.FindViewById<TextView>(Resource.Id.textView3);
            cardView = itemView.FindViewById<AndroidX.CardView.Widget.CardView>(Resource.Id.cardView1);
            Aciklama.Text = "";
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class BildirimlerRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<BILDIRIMLER> mData = new List<BILDIRIMLER>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public BildirimlerRecyclerViewAdapter(List<BILDIRIMLER> GelenData, AppCompatActivity GelenContex)
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
        BildirimlerRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            BildirimlerRecyclerViewHolder viewholder = holder as BildirimlerRecyclerViewHolder;
            HolderForAnimation = holder as BildirimlerRecyclerViewHolder;
            var item = mData[position];
            viewholder.Aciklama.Text = item.text;
            if (item.date!=null)
            {
                viewholder.Tarih.Text = Convert.ToDateTime(item.date).ToShortDateString();
            }
            //app:cardBackgroundColor="#401EB04B"
            if (!item.Okundu)
            {
                viewholder.cardView.SetBackgroundColor(Color.ParseColor("#401EB04B"));
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.BildirimCustomCardView, parent, false);

            return new BildirimlerRecyclerViewHolder(v, OnClick);
        }
     
        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }


        public class TownDTO
        {
            public int cityId { get; set; }
            public string cityName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}