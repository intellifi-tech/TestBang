using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TestBang.DataBasee;
using TestBang.GenericClass;

namespace TestBang.Bildirim
{
    [Activity(Label = "TestBang")]
    public class BildirimlerBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        BildirimlerRecyclerViewAdapter mViewAdapter = null;
        List<BILDIRIMLER> BildirimListesi = DataBase.BILDIRIMLER_GETIR();
        LinearLayout relativeLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.BildirimlerBaseActivity);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            relativeLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout2);
        }

        protected override void OnStart()
        {
            base.OnStart();
            BildirimleriYerlestir();
        }

        void BildirimleriYerlestir()
        {
            BildirimListesi.Reverse();
            if (BildirimListesi.Count>0)
            {
                if (mViewAdapter==null )
                {
                    mViewAdapter = new BildirimlerRecyclerViewAdapter(BildirimListesi, this);
                    mRecyclerView.HasFixedSize = true;
                    mLayoutManager = new LinearLayoutManager(this);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick -= MViewAdapter_ItemClick;
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    relativeLayout.Visibility = ViewStates.Gone;
                }

            }
            else
            {
                relativeLayout.Visibility = ViewStates.Visible;
            }
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var BildirimListesi2 = BildirimListesi.FindAll(item => item.Okundu == false);
                for (int i = 0; i < BildirimListesi2.Count; i++)
                {
                    BildirimListesi2[i].Okundu = true;
                    DataBase.BILDIRIMLER_Guncelle(BildirimListesi2[i]);
                }
            })).Start();
            
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            mViewAdapter.mData[(int)e[0]].Okundu = true;
            BildirimListesi[(int)e[0]].Okundu = true;
            mViewAdapter.NotifyItemChanged((int)e[0]);
        }
    }
}