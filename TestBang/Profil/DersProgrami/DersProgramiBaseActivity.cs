using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TestBang.GenericClass;

namespace TestBang.Profil.DersProgrami
{
    [Activity(Label = "TestBang")]
    public class DersProgramiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        RecyclerView mRecyclerView;
        Android.Support.V7.Widget.LinearLayoutManager mLayoutManager;
        DersProgramiRecyclerViewAdapter mViewAdapter;
        public List<DersProgramiDTO> DersProgramiList = new List<DersProgramiDTO>();
        Button YeniEkle;

        List<TakvimTarihlerDTO> TakvimTarihlerDTO1 = new List<TakvimTarihlerDTO>();
        TakvimGridAdapter TakvimGridAdapter1;
        GridView TakvimGrid;
        DateTime SonTakvimTarihi;
        ImageButton TakvimIleriButton, TakvimGeriButton;
        TextView TakvimAyAdiText, TakvimYilText;

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DersProgramiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            YeniEkle = FindViewById<Button>(Resource.Id.button2);
            TakvimGrid = FindViewById<GridView>(Resource.Id.gridView1);
            TakvimIleriButton = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            TakvimGeriButton = FindViewById<ImageButton>(Resource.Id.ımageButton2);
            TakvimAyAdiText = FindViewById<TextView>(Resource.Id.takvimayaditext);
            TakvimYilText = FindViewById<TextView>(Resource.Id.takvimyiltext);

            YeniEkle.Click += YeniEkle_Click;
            TakvimIleriButton.Click += TakvimIleriButton_Click;
            TakvimGeriButton.Click += TakvimGeriButton_Click;
            FillDataModel();
        }

        private void TakvimGeriButton_Click(object sender, EventArgs e)
        {
            CreateTakvimDayList(SonTakvimTarihi.AddMonths(1));
        }

        private void TakvimIleriButton_Click(object sender, EventArgs e)
        {
            CreateTakvimDayList(SonTakvimTarihi.AddMonths(-1));
        }

        protected override void OnStart()
        {
            base.OnStart();
            CreateTakvimDayList(DateTime.Now);
        }
        #region Takim Icin

        void CreateTakvimDayList(DateTime GelenAy)
        {
            TakvimTarihlerDTO1 = new List<TakvimTarihlerDTO>();
            var BuAykiTotalGunSayisi = DateTime.DaysInMonth(GelenAy.Year, GelenAy.Month);
            var OtelemeSayisii = OtelemeSayisi(GelenAy);
            var TakvimBaslanginc = new DateTime(GelenAy.Year, GelenAy.Month, 1).AddDays(-1 * OtelemeSayisii);
            for (int i = 0; i < 35; i++)
            {
                if (i < OtelemeSayisii)
                {
                    TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                    {
                        GunGoster = false,
                        Tarih = TakvimBaslanginc.AddDays(-1 * i)
                    });
                }
                else
                {
                    if (i < BuAykiTotalGunSayisi + OtelemeSayisii)
                    {
                        TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                        {
                            GunGoster = true,
                            Tarih = TakvimBaslanginc.AddDays(i)
                        });
                    }
                    else
                    {
                        TakvimTarihlerDTO1.Add(new TakvimTarihlerDTO()
                        {
                            GunGoster = false,
                            Tarih = TakvimBaslanginc.AddDays(i)
                        });
                    }
                }
            }
            var aaa = TakvimTarihlerDTO1;
            TakvimTarihlerDTO1[25].DenemeSinaviVami = true;
            TakvimTarihlerDTO1[10].KisiselTestVarmi = true;
            TakvimTarihlerDTO1[22].KisiselTestVarmi = true;
            TakvimTarihlerDTO1[22].DenemeSinaviVami = true;

            this.RunOnUiThread(() => {
                TakvimGridAdapter1 = new TakvimGridAdapter(this, Resource.Layout.TakvimCustomGridCell, TakvimTarihlerDTO1);
                TakvimGrid.Adapter = TakvimGridAdapter1;
            });

            TakvimAyAdiText.Text = GelenAy.ToString("MMMM");
            TakvimYilText.Text = GelenAy.Year.ToString();
            SonTakvimTarihi = GelenAy;
        }



        int OtelemeSayisi(DateTime GelenGun)
        {
            var dtIlkGunu = new DateTime(GelenGun.Year, GelenGun.Month, 1);
            switch (dtIlkGunu.DayOfWeek)
            {
                case DayOfWeek.Monday://Pazartesi
                    return 0;
                case DayOfWeek.Tuesday://Salı
                    return 1;
                case DayOfWeek.Wednesday://Çarşamba
                    return 2;
                case DayOfWeek.Thursday://Perşembe
                    return 3;
                case DayOfWeek.Friday://Cuma
                    return 4;
                case DayOfWeek.Saturday://Cumartesi
                    return 5;
                case DayOfWeek.Sunday://Pazar
                    return 6;
                default:
                    return -1;
            }
        }

        public class TakvimTarihlerDTO
        {
            public DateTime? Tarih { get; set; }
            public bool GunGoster { get; set; }
            public bool DenemeSinaviVami { get; set; }
            public bool KisiselTestVarmi { get; set; }
        }


        class TakvimGridAdapter : BaseAdapter<TakvimTarihlerDTO>, View.IOnClickListener
        {
            private Context mContext;
            private int mRowLayout;
            private List<TakvimTarihlerDTO> mUrunler;
            public TakvimGridAdapter(Context context, int rowLayout, List<TakvimTarihlerDTO> friends)
            {
                mContext = context;
                mRowLayout = rowLayout;
                mUrunler = friends;
            }
            public override int Count
            {
                get { return mUrunler.Count; }
            }

            public override TakvimTarihlerDTO this[int position]
            {
                get { return mUrunler[position]; }
            }

            public override long GetItemId(int position)
            {
                return position;
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                MyViewHolder3 holder3;
                View row = convertView;
                var GelenDTO = mUrunler[position];
                if (row != null)
                {

                    holder3 = row.Tag as MyViewHolder3;
                }
                else //(row == null)
                {
                    holder3 = new MyViewHolder3();
                    row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false); //**
                    holder3.TarihLabel = row.FindViewById<TextView>(Resource.Id.textView1);
                    
                    row.Tag = holder3;
                }


                if (GelenDTO.Tarih != null)
                {
                    holder3.TarihLabel.Text = ((DateTime)GelenDTO.Tarih).Day.ToString();
                    if (!GelenDTO.GunGoster)
                    {
                        holder3.TarihLabel.SetTextColor(Color.Rgb(235, 235, 235));
                        holder3.TarihLabel.Text = "";
                    }
                    else
                    {
                        holder3.TarihLabel.SetTextColor(Color.Rgb(17, 18, 46));
                        if (((DateTime)GelenDTO.Tarih).DayOfWeek == DayOfWeek.Saturday || ((DateTime)GelenDTO.Tarih).DayOfWeek == DayOfWeek.Sunday)
                        {
                            holder3.TarihLabel.SetTextColor(Color.Rgb(240, 81, 113));
                        }
                        if (((DateTime)GelenDTO.Tarih).Date == DateTime.Today)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_gun_gosterge);
                        }
                        if (GelenDTO.DenemeSinaviVami)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_deneme_sinavi_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                        if (GelenDTO.KisiselTestVarmi)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_kisisel_test_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                        if (GelenDTO.KisiselTestVarmi == true && GelenDTO.DenemeSinaviVami == true)
                        {
                            holder3.TarihLabel.SetBackgroundResource(Resource.Drawable.takvim_deneme_ve_test_gosterge);
                            holder3.TarihLabel.SetTextColor(Color.White);
                        }
                    }
                }

                holder3.TarihLabel.Tag = position;
                holder3.TarihLabel.SetOnClickListener(this);
                return row;
            }

            public void OnClick(View v)
            {
                var TagDegerleri = v.Tag.ToString();
                //if (TagDegerleri[0].ToString() == "Detaylar")
                //{
                //    ((UrunSec)mContext).ItemClikEvent(mUrunler[Convert.ToInt32(TagDegerleri[1])], 1);
                //}
                //else if (TagDegerleri[0].ToString() == "Sec")
                //{
                //    ((UrunSec)mContext).ItemClikEvent(mUrunler[Convert.ToInt32(TagDegerleri[1])], 0);
                //}

            }

            private class MyViewHolder3 : Java.Lang.Object
            {
                public TextView TarihLabel { get; set; }
            }
        }



        #endregion




        private void YeniEkle_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DersProgramiEkleBaseActivity));
        }

        void FillDataModel()
        {

            for (int i = 0; i < 20; i++)
            {
                DersProgramiList.Add(new DersProgramiDTO());
            }
            mRecyclerView.HasFixedSize = true;
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mViewAdapter = new DersProgramiRecyclerViewAdapter(DersProgramiList, this);
            mRecyclerView.SetAdapter(mViewAdapter);
            mViewAdapter.ItemClick += MViewAdapter_ItemClick; ;
            mLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            try
            {
                SnapHelper snapHelper = new LinearSnapHelper();
                snapHelper.AttachToRecyclerView(mRecyclerView);
            }
            catch
            {
            }
        }

        private void MViewAdapter_ItemClick(object sender, int e)
        {
            
        }

        public class DersProgramiDTO
        {

        }

        public static class DersProgramiBaseActivityHelper
        {
            public static DersProgramiBaseActivity DersProgramiBaseActivity1 { get; set; }
            public static DateTime SecilenTarih { get; set; }
        }
    }
}