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
using TestBang.WebServices;
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
        List<Lesson> Lesson1 = new List<Lesson>();
        public DenemeBaseFragmentRecyclerViewAdapter(List<DenemeDersAnalizDTO> mData2, AppCompatActivity GelenContex)
        {
            mData = mData2;
            BaseActivity = GelenContex;
            DersleriGetir();
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
            //var DerssDetay = Lesson1.Find(item2 => item2.id.ToString() == item.lessonId);
            //if (DerssDetay != null)
            //{
            //    viewholder.DersAdi.Text = "(" + DerssDetay.type + ")" + item.lessonName;
            //}
            //else
            //{
            //    viewholder.DersAdi.Text = item.lessonName;
            //}
            viewholder.DersAdi.Text = item.lessonName;

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


        void DersleriGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("lessons");
            if (Donus != null)
            {
                Lesson1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(Donus.ToString());
              
            }
        }
        public class Lesson
        {
            public int id { get; set; }
            public string name { get; set; }
            public string token { get; set; }
            public string icon { get; set; }
            public bool? say { get; set; }
            public bool? soz { get; set; }
            public bool? ea { get; set; }
            public double? sayKat { get; set; }
            public double? sozKat { get; set; }
            public double? eaKat { get; set; }
            public double? yerKat { get; set; }
            public double? tytKat { get; set; }
            public string type { get; set; }
        }
    }
}
