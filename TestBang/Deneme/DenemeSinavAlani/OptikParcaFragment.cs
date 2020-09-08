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
using Android.Util;
using Android.Views;
using Android.Widget;
using TestBang.Profil.Optik;

namespace TestBang.Deneme.DenemeSinavAlani
{
    public class OptikParcaFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OptikListRecyclerViewAdapter mViewAdapter;
        List<OptikListDTO> favorilerRecyclerViewDataModels = new List<OptikListDTO>();
        DenemeSinavAlaniBaseActivity DenemeSinavAlaniBaseActivity0;

        public OptikParcaFragment(DenemeSinavAlaniBaseActivity DenemeSinavAlaniBaseActivity1)
        {
            DenemeSinavAlaniBaseActivity0 = DenemeSinavAlaniBaseActivity1;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View vieww= inflater.Inflate(Resource.Layout.OptikParcaFragment, container, false);
            mRecyclerView = vieww.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            GetOptik();
            DenemeSinavAlaniHelperClass.OptikParcaFragment1 = this;
            return vieww;
        }



        void GetOptik()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate {
                    await Task.Delay(100);
                    for (int i = 0; i < DenemeSinavAlaniHelperClass.UzakSunucuDenemeDTO1.questionCount; i++)
                    {
                        favorilerRecyclerViewDataModels.Add(new OptikListDTO());
                    }
                    this.Activity.RunOnUiThread(delegate
                    {
                        mViewAdapter = new OptikListRecyclerViewAdapter(favorilerRecyclerViewDataModels, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                        mRecyclerView.HasFixedSize = true;
                        mLayoutManager = new LinearLayoutManager(this.Activity);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        mRecyclerView.SetAdapter(mViewAdapter);
                        mViewAdapter.ItemClick += MViewAdapter_ItemClick;

                    });
                });
            })).Start();
            
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            DenemeSinavAlaniBaseActivity0.SoruyaGit((int)e[0]);
        }

        public void UpdateOptik(int pos, string Cevap)
        {
            return;
            mViewAdapter.mData[pos].Cevap = Cevap;
            favorilerRecyclerViewDataModels = mViewAdapter.mData;

            mRecyclerView.SmoothScrollToPosition(pos);
            mViewAdapter.NotifyItemChanged(pos);
        }
        public class OptikListDTO
        {
            public string Cevap { get; set; } = "";
        }
    }
}