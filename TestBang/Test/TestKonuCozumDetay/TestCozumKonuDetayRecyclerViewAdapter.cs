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
using static TestBang.Test.TestKonuCozumDetay.TestCozumKonuDetayBaseActivity;

namespace TestBang.Test.TestKonuCozumDetay
{
    class TestCozumKonuDetayAdapterHolder : RecyclerView.ViewHolder
    {
        public TextView TopicNamee, BosText, DogruText, YanlisText;
        public TestCozumKonuDetayAdapterHolder(View itemView, Action<int> listener) : base(itemView)
        {
            TopicNamee = itemView.FindViewById<TextView>(Resource.Id.textView2);
            BosText = itemView.FindViewById<TextView>(Resource.Id.textView3);
            DogruText = itemView.FindViewById<TextView>(Resource.Id.textView4);
            YanlisText = itemView.FindViewById<TextView>(Resource.Id.textView5);
            TopicNamee.Text = "";
            BosText.Text = "";
            DogruText.Text = "";
            YanlisText.Text = "";
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
    class TestCozumKonuDetayRecyclerViewAdapter : RecyclerView.Adapter
    {
        AppCompatActivity BaseActivity;
        public event EventHandler<int> ItemClick;
        List<TestCozumKonuDetayDTO> mData;
        public TestCozumKonuDetayRecyclerViewAdapter(List<TestCozumKonuDetayDTO> mData2, AppCompatActivity GelenContex)
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
            TestCozumKonuDetayAdapterHolder viewholder = holder as TestCozumKonuDetayAdapterHolder;
            var item = mData[position];
            if (string.IsNullOrEmpty(viewholder.TopicNamee.Text))
            {
                GetTopicNamebyID(viewholder.TopicNamee, item.topicId);
            }


            viewholder.BosText.Text = item.emptyCount;
            viewholder.DogruText.Text = item.correctCount;
            viewholder.YanlisText.Text = item.wrongCount;
        }
       

        void GetTopicNamebyID(TextView Topicnametext, string TopicID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("topics/" + TopicID);
                if (Donus != null)
                {
                    var CozulenTestlerDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<TopicDto>(Donus.ToString());
                    if (CozulenTestlerDTO1 != null)
                    {
                        BaseActivity.RunOnUiThread(delegate ()
                        {

                            Topicnametext.Text = CozulenTestlerDTO1.name;
                        });
                }
                }
            })).Start();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TestCozumKonuDetayCardView, parent, false);
          
            return new TestCozumKonuDetayAdapterHolder(v, OnClickk);
        }

        void OnClickk(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public class TopicDto
        {
            public bool? ayt { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string lessonId { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}
