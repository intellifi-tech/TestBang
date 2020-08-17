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
using static TestBang.Deneme.DenemeSinavAlani.OptikParcaFragment;

namespace TestBang.Profil.Optik
{
    class OptikListRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView SoruNumasiText,CA,CB,CC,CD,CE;
        public OptikListRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            SoruNumasiText = itemView.FindViewById<TextView>(Resource.Id.textView1);
            CA = itemView.FindViewById<TextView>(Resource.Id.textView2);
            CB = itemView.FindViewById<TextView>(Resource.Id.textView3);
            CC = itemView.FindViewById<TextView>(Resource.Id.textView4);
            CD = itemView.FindViewById<TextView>(Resource.Id.textView5);
            CE = itemView.FindViewById<TextView>(Resource.Id.textView6);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class OptikListRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<OptikListDTO> mData = new List<OptikListDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public OptikListRecyclerViewAdapter(List<OptikListDTO> GelenData, AppCompatActivity GelenContex)
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
        OptikListRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OptikListRecyclerViewHolder viewholder = holder as OptikListRecyclerViewHolder;
            TumSecimleriTemizle(viewholder);
            var item = mData[position];
            viewholder.SoruNumasiText.Text = (position + 1).ToString();
            switch (item.Cevap)
            {
                case "A":
                    viewholder.CA.SetBackgroundResource(Resource.Drawable.optikcember_select);
                    break;
                case "B":
                    viewholder.CB.SetBackgroundResource(Resource.Drawable.optikcember_select);
                    break;
                case "C":
                    viewholder.CC.SetBackgroundResource(Resource.Drawable.optikcember_select);
                    break;
                case "D":
                    viewholder.CD.SetBackgroundResource(Resource.Drawable.optikcember_select);
                    break;
                case "E":
                    viewholder.CE.SetBackgroundResource(Resource.Drawable.optikcember_select);
                    break;
                default:
                    break;
            }
        }
        void TumSecimleriTemizle(OptikListRecyclerViewHolder hodlerrr)
        {
            hodlerrr.CA.SetBackgroundResource(Resource.Drawable.optikcember_noselect);
            hodlerrr.CB.SetBackgroundResource(Resource.Drawable.optikcember_noselect);
            hodlerrr.CC.SetBackgroundResource(Resource.Drawable.optikcember_noselect);
            hodlerrr.CD.SetBackgroundResource(Resource.Drawable.optikcember_noselect);
            hodlerrr.CE.SetBackgroundResource(Resource.Drawable.optikcember_noselect);
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.OptikCustomCardView, parent, false);

            return new OptikListRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}