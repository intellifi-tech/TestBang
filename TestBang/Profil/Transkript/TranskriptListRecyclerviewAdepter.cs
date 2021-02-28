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
        public TextView Nametxt, Desctxt, Siratxt, Tarihtxt, Turtxt;
        public TranskriptListRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            TranskriptNo1 = itemView.FindViewById<TextView>(Resource.Id.transkriptno1);
            Nametxt = itemView.FindViewById<TextView>(Resource.Id.basliktext);
            Desctxt = itemView.FindViewById<TextView>(Resource.Id.aciklamatext);
            Siratxt = itemView.FindViewById<TextView>(Resource.Id.konutext);
            Tarihtxt = itemView.FindViewById<TextView>(Resource.Id.tarihtext);
            Turtxt = itemView.FindViewById<TextView>(Resource.Id.textView5);
        
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
            //           public TextView TranskriptNo1;
                      // public TextView Nametxt, Desctxt, Siratxt, Tarihtxt, Turtxt;
            TranskriptListRecyclerViewHolder viewholder = holder as TranskriptListRecyclerViewHolder;
            HolderForAnimation = holder as TranskriptListRecyclerViewHolder;
            var item = mData[position];
            viewholder.TranskriptNo1.SetTextColor(Color.White);
            viewholder.TranskriptNo1.Text = item.TanskriptNo.ToString();
            viewholder.Nametxt.Text = item.name;
            if (!string.IsNullOrEmpty(item.description))
            {
                viewholder.Desctxt.Text = item.description;
            }
            else
            {
                viewholder.Desctxt.Text = "Açıklama Yok.";//
            }
            if (item.order!=null)
            {
                viewholder.Siratxt.Text = item.order;
            }
            else
            {
                viewholder.Siratxt.Text = "";
            }
            
            viewholder.Tarihtxt.Text = ((DateTime)item.startDate).ToShortDateString();
            viewholder.Turtxt.Text = item.type;
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