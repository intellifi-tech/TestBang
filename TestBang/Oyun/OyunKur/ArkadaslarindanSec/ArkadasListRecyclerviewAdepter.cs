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

namespace TestBang.Oyun.ArkadaslarindanSec
{
    class ArkadasListRecyclerViewHolder : RecyclerView.ViewHolder
    {
         public TextView AdSoyad,IlIce;
        public ArkadasListRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            AdSoyad = itemView.FindViewById<TextView>(Resource.Id.textView1);
            IlIce = itemView.FindViewById<TextView>(Resource.Id.textView2);
            IlIce.Text = "";
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class ArkadasListRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<MEMBER_DATA> mData = new List<MEMBER_DATA>();
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
            viewholder.AdSoyad.Text = item.firstName + " " + item.lastName;
            if (string.IsNullOrEmpty(viewholder.IlIce.Text))
            {
                if (item.townId != null)
                {
                    SetTownNameByID((int)item.townId, viewholder.IlIce);
                }
                else
                {
                    viewholder.IlIce.Text = "";
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.ArkadasListesiCardView, parent, false);

            return new ArkadasListRecyclerViewHolder(v, OnClick);
        }
        void SetTownNameByID(int TownID,TextView IlIlceText)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("towns/" + TownID);
                if (Donus != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<TownDTO>(Donus.ToString());
                    if (Icerik != null)
                    {
                        var Donus2 = webService.OkuGetir("towns/" + TownID);
                        BaseActivity.RunOnUiThread(delegate () {
                            IlIlceText.Text = Icerik.cityName +","+ Icerik.name;
                        });
                    }
                }
            })).Start();
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