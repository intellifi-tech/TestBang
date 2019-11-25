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
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Profil.DersProgrami
{
    class DersProgramiListeAdapterHolder : RecyclerView.ViewHolder
    {
        public DersProgramiListeAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class DersProgramiRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<DersProgramiDTO> mData;
        public DersProgramiRecyclerViewAdapter(List<DersProgramiDTO> mData2, AppCompatActivity GelenContex)
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
        DersProgramiListeAdapterHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DersProgramiListeAdapterHolder viewholder = holder as DersProgramiListeAdapterHolder;
            HolderForAnimation = holder as DersProgramiListeAdapterHolder;
            //var item = GelenBase.MapDataModel1[position];
           
        }
       
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.DersProgramiCustomCardView, parent, false);
          
            return new DersProgramiListeAdapterHolder(v, OnClickk);
        }

        void OnClickk(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}
