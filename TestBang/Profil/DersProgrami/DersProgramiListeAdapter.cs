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
using TestBang.DataBasee;
using TestBang.WebServices;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;

namespace TestBang.Profil.DersProgrami
{
    class DersProgramiListeAdapterHolder : RecyclerView.ViewHolder
    {
        public TextView AyName, GunText, BaslikText, AciklamaText, KonuText, SoruSayisiText;
        public ImageButton KapatButton;
        public DersProgramiListeAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            AyName = itemView.FindViewById<TextView>(Resource.Id.aynametext);
            GunText = itemView.FindViewById<TextView>(Resource.Id.guntext);
            BaslikText = itemView.FindViewById<TextView>(Resource.Id.basliktext);
            AciklamaText = itemView.FindViewById<TextView>(Resource.Id.aciklamatext);
            KonuText = itemView.FindViewById<TextView>(Resource.Id.konutext);
            SoruSayisiText = itemView.FindViewById<TextView>(Resource.Id.textView5);
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
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DersProgramiListeAdapterHolder viewholder = holder as DersProgramiListeAdapterHolder;
            var item = mData[position];

            if (!string.IsNullOrEmpty(item.TestID))
            {
                if (!item.UIUygulandimi)
                {
                    TestDetaylariniGetir(position, item.TestID, viewholder.AyName, viewholder.GunText, viewholder.BaslikText, viewholder.AciklamaText, viewholder.KonuText, viewholder.SoruSayisiText);
                }
            }
            else if (!string.IsNullOrEmpty(item.DenemeID))
            {
                if (!item.UIUygulandimi)
                {
                    DenemeDetaylariniGetir(position, item.DenemeID, viewholder.AyName, viewholder.GunText, viewholder.BaslikText, viewholder.AciklamaText, viewholder.KonuText, viewholder.SoruSayisiText);
                }
            }
        }
        void TestDetaylariniGetir(int Position, string TestID, TextView AyName, TextView GunText, TextView BaslikText, TextView AciklamaText, TextView KonuText, TextView SoruSayisiText)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("user-tests/"+TestID,UsePoll:true);
                if (Donus!=null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<OLUSTURULAN_TESTLER>(Donus.ToString());
                    if (Icerik!=null)
                    {
                        mData[Position].Aciklama = Icerik.description;
                        mData[Position].Baslik = Icerik.name;
                        mData[Position].Tarih = Convert.ToDateTime(Icerik.startDate);
                        mData[Position].TestKonuVeyaSinavAlani = Icerik.lessonName + " - " + Icerik.topicName;
                        mData[Position].TestSoruSayisi = Icerik.questionCount.ToString()+" Soru";
                        mData[Position].UIUygulandimi = true;
                        BaseActivity.RunOnUiThread(delegate () {
                            AyName.Text= Convert.ToDateTime(Icerik.startDate).ToString("MMMM");
                            GunText.Text = Convert.ToDateTime(Icerik.startDate).Day.ToString();
                            BaslikText.Text = mData[Position].Baslik;
                            AciklamaText.Text = mData[Position].Aciklama;
                            KonuText.Text = mData[Position].TestKonuVeyaSinavAlani;
                            SoruSayisiText.Text = mData[Position].TestSoruSayisi;
                        });
                    }
                }
            })).Start();
        }
        void DenemeDetaylariniGetir(int Position, string DenemeID, TextView AyName, TextView GunText, TextView BaslikText, TextView AciklamaText, TextView KonuText, TextView SoruSayisiText)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("trials/" + DenemeID, UsePoll: true);
                if (Donus != null)
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<UzakSunucuDenemeDTO>(Donus.ToString());
                    if (Icerik != null)
                    {
                        mData[Position].Aciklama = Icerik.description;
                        mData[Position].Baslik = Icerik.name;
                        mData[Position].Tarih = Convert.ToDateTime(Icerik.startDate);
                        mData[Position].TestKonuVeyaSinavAlani = Icerik.type;
                        mData[Position].TestSoruSayisi = "";
                        mData[Position].UIUygulandimi = true;
                        BaseActivity.RunOnUiThread(delegate () {
                            AyName.Text = Convert.ToDateTime(Icerik.startDate).ToString("MMMM");
                            GunText.Text = Convert.ToDateTime(Icerik.startDate).Day.ToString();
                            BaslikText.Text = mData[Position].Baslik;
                            AciklamaText.Text = mData[Position].Aciklama;
                            KonuText.Text = mData[Position].TestKonuVeyaSinavAlani;
                            SoruSayisiText.Text = mData[Position].TestSoruSayisi;
                        });
                    }
                }
            })).Start();
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
