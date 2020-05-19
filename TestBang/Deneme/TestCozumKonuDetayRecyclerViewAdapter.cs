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
    class DenemeBaseFragmentAdapterHolder : RecyclerView.ViewHolder
    {
        public DenemeBaseFragmentAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class DenemeBaseFragmentRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<DenemeBaseFragmentDTO> mData;
        public DenemeBaseFragmentRecyclerViewAdapter(List<DenemeBaseFragmentDTO> mData2, AppCompatActivity GelenContex)
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
