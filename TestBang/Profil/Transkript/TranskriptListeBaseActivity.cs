using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TestBang.DataBasee;
using TestBang.GenericClass;

namespace TestBang.Profil.Transkript
{
    [Activity(Label = "TestBang")]
    public class TranskriptListeBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TranskriptListRecyclerViewAdapter mViewAdapter;
        List<TranskriptListDTO> favorilerRecyclerViewDataModels = new List<TranskriptListDTO>();
        MEMBER_DATA MeUser = DataBase.MEMBER_DATA_GETIR()[0];
        TextView AdSoyadTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TranskriptListesiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            AdSoyadTxt = FindViewById<TextView>(Resource.Id.adsoyadtext);
            AdSoyadTxt.Text = MeUser.firstName + " " + MeUser.lastName;
        }

        protected override void OnStart()
        {
            base.OnStart();
            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate {
                    await Task.Delay(100);
                    GetTranskript();

                });
            })).Start();
        }

        void GetTranskript()
        {
            for (int i = 0; i < 50; i++)
            {
                favorilerRecyclerViewDataModels.Add(new TranskriptListDTO());
            }
            this.RunOnUiThread(delegate
            {
                mViewAdapter = new TranskriptListRecyclerViewAdapter(favorilerRecyclerViewDataModels, (Android.Support.V7.App.AppCompatActivity)this);
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                
            });
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            this.StartActivity(typeof(TranskriptDetayBaseActivity));
        }

        public class TranskriptListDTO
        {
            
        }
    }
}