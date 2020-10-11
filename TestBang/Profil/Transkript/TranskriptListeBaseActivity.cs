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
using TestBang.WebServices;
using static TestBang.Profil.Transkript.TranskriptDetayBaseActivity;

namespace TestBang.Profil.Transkript
{
    [Activity(Label = "TestBang")]
    public class TranskriptListeBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TranskriptListRecyclerViewAdapter mViewAdapter;
        List<TranskriptListDTO> TranskriptListDTO1 = new List<TranskriptListDTO>();
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
                    KullaniciDenemeleriniGetir();
                });
            })).Start();
        }
        void KullaniciDenemeleriniGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("trials/user", UsePoll: true);
            if (Donus != null)
            {
                TranskriptListDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TranskriptListDTO>>(Donus.ToString());
                if (TranskriptListDTO1.Count > 0)
                {
                    for (int i = 0; i < TranskriptListDTO1.Count; i++)
                    {
                        TranskriptListDTO1[i].TanskriptNo = i + 1;
                    }
                    TranskriptListDTO1.Reverse();
                    this.RunOnUiThread(delegate
                    {
                        mViewAdapter = new TranskriptListRecyclerViewAdapter(TranskriptListDTO1, this);
                        mRecyclerView.HasFixedSize = true;
                        mLayoutManager = new LinearLayoutManager(this);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        mRecyclerView.SetAdapter(mViewAdapter);
                        mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    });
                }
            }
        }


        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            TranskriptDetayBaseActivity_Helper.SecilenDeneme = TranskriptListDTO1[(int)e[0]];
            this.StartActivity(typeof(TranskriptDetayBaseActivity));
        }

        public class TranskriptListDTO
        {
            public string id { get; set; }
            public string name { get; set; }
            public string schoolId { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? finishDate { get; set; }
            public string description { get; set; }
            public int questionCount { get; set; }
            public string type { get; set; }
            public string userAlan { get; set; }
            //
            public int TanskriptNo { get; set; }
            public double? point { get; set; }
            public double? sayPoint { get; set; }
            public double? sozPoint { get; set; }
            public double? eaPoint { get; set; }
        }
    }
}