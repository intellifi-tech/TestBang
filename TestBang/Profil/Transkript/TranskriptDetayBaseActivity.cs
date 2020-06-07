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
using TestBang.GenericClass;

namespace TestBang.Profil.Transkript
{
    [Activity(Label = "TestBang")]
    public class TranskriptDetayBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        FrameLayout PuanHazne;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TranskriptDetayRecyclerViewAdapter mViewAdapter;
        List<TranskriptDetayDTO> TranskriptDetayDTO1 = new List<TranskriptDetayDTO>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TranskriptDetayBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            PuanHazne = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            PuanLayoutYerlestir();
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            mRecyclerView.HasFixedSize = true;
        }
        void PuanLayoutYerlestir()
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View v = inflater.Inflate(Resource.Layout.AYTPuanTranskriptUI, PuanHazne, false);
            PuanHazne.AddView(v);
        }

        protected override void OnStart()
        {
            base.OnStart();
            ListeyiOlustur();
        }
        void ListeyiOlustur()
        {
            #region Genislik Alır
            int width = 0;
            int height = 0;

            mRecyclerView.Post(() =>
            {
                width = mRecyclerView.Width;
                height = mRecyclerView.Height;
                var Genislikk = width / 2;

                for (int i = 0; i < 16; i++)
                {
                    TranskriptDetayDTO1.Add(new TranskriptDetayDTO() { });
                }

                this.RunOnUiThread(delegate ()
                {
                    mViewAdapter = new TranskriptDetayRecyclerViewAdapter(TranskriptDetayDTO1, (Android.Support.V7.App.AppCompatActivity)this, Genislikk);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));
                    mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                    var layoutManager = new GridLayoutManager(this, 2);
                    mRecyclerView.SetLayoutManager(layoutManager);
                });
            });

            #endregion
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            
        }

        public class TranskriptDetayDTO
        {

        }
    }
}