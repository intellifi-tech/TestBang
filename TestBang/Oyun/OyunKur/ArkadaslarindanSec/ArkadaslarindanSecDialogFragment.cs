using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.IO;
using Newtonsoft.Json;
using Refractored.Controls;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.WebServices;

namespace TestBang.Oyun.ArkadaslarindanSec
{
    class ArkadaslarindanSecDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        ImageButton KapatButton;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        ArkadasListRecyclerViewAdapter mViewAdapter;
        List<MEMBER_DATA> favorilerRecyclerViewDataModels = new List<MEMBER_DATA>();
        EditText RakipAraEdittext;
        #endregion
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation3;
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, DPX.dpToPx(this.Activity, 400));
            Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.FillVertical | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
        }
    
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            return dialog;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.RakipAraArkadaslarindanSec, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            KapatButton = view.FindViewById<ImageButton>(Resource.Id.ımageButton1);
            KapatButton.Click += KapatButton_Click;
            RakipAraEdittext = view.FindViewById<EditText>(Resource.Id.editText1);
            RakipAraEdittext.TextChanged += RakipAraEdittext_TextChanged;



            return view;
        }

        private void RakipAraEdittext_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            List<MEMBER_DATA> searchedFriends = (from friend in favorilerRecyclerViewDataModels
                                             where friend.firstName.Contains(RakipAraEdittext.Text, StringComparison.OrdinalIgnoreCase) || friend.lastName.Contains(RakipAraEdittext.Text, StringComparison.OrdinalIgnoreCase)
                                             select friend).ToList<MEMBER_DATA>();

            this.Activity.RunOnUiThread(delegate
            {
                mViewAdapter = new ArkadasListRecyclerViewAdapter(searchedFriends, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this.Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick -= MViewAdapter_ItemClick;
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
            });
        }

        private void KapatButton_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        bool Actinmi = false;
        public override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);
                Actinmi = true;
                GetUsers();
            }
        }
        void GetUsers()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("users");
            if (Donus!=null)
            {
                favorilerRecyclerViewDataModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MEMBER_DATA>>(Donus.ToString());
                if (favorilerRecyclerViewDataModels.Count>0)
                {
                    favorilerRecyclerViewDataModels.Reverse();
                    this.Activity.RunOnUiThread(delegate
                    {
                        mViewAdapter = new ArkadasListRecyclerViewAdapter(favorilerRecyclerViewDataModels, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                        mRecyclerView.HasFixedSize = true;
                        mLayoutManager = new LinearLayoutManager(this.Activity);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        mRecyclerView.SetAdapter(mViewAdapter);
                        mViewAdapter.ItemClick -= MViewAdapter_ItemClick;
                        mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    });
                }
            }
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            var item = mViewAdapter.mData[(int)e[0]];
        }
    }
}