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
using static TestBang.Deneme.DenemeBaseFragment;

namespace TestBang.Deneme
{
   public class DenemeBaseFragmentAdapterHolder : RecyclerView.ViewHolder
    {
        public TextView DersAdi, DogruTxt, YalnisTxt, BosTxt;
        public DenemeBaseFragmentAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            DersAdi = itemView.FindViewById<TextView>(Resource.Id.textView2);
            DogruTxt = itemView.FindViewById<TextView>(Resource.Id.textView4);
            YalnisTxt = itemView.FindViewById<TextView>(Resource.Id.textView5);
            BosTxt = itemView.FindViewById<TextView>(Resource.Id.textView3);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class DenemeBaseFragmentRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<DenemeDersAnalizDTO> mData;
        public DenemeBaseFragmentRecyclerViewAdapter(List<DenemeDersAnalizDTO> mData2, AppCompatActivity GelenContex)
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
            DenemeBaseFragmentAdapterHolder viewholder = holder as DenemeBaseFragmentAdapterHolder;
            var item = mData[position];
            if (!string.IsNullOrEmpty(item.trialType))
            {
                viewholder.DersAdi.Text = "(" + item.trialType + ")" + item.lessonName;
            }
            else
            {
                viewholder.DersAdi.Text = item.lessonName;
            }
            
            viewholder.DogruTxt.Text = item.correctCount.ToString();
            viewholder.YalnisTxt.Text = item.wrongCount.ToString();
            viewholder.BosTxt.Text = item.emptyCount.ToString();
        }
       
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.DenemeCozumDersDetayCardView, parent, false);
          
            return new DenemeBaseFragmentAdapterHolder(v, OnClickk);
        }

        void OnClickk(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}
