using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Test.TestSinavAlani;
using TestBang.WebServices;

namespace TestBang.Test.TestOlustur
{
    [Activity(Label = "TestBang")]
    public class TestOlusturBaseActivity : Android.Support.V7.App.AppCompatActivity,AdapterView.IOnItemSelectedListener
    {
        Button TesteBasla;
        Spinner DersSpinner, KonuSpinner, SoruSayisiSpinner,SureSpinner;

        List<Lesson> Lesson1 = new List<Lesson>();
        List<Topic> Topic1 = new List<Topic>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Yesil(this);
            SetContentView(Resource.Layout.TestOlusturBaseActivity);
            TesteBasla = FindViewById<Button>(Resource.Id.button3);
            TesteBasla.Click += TesteBasla_Click;
            DersSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            KonuSpinner = FindViewById<Spinner>(Resource.Id.spinner2);
            SoruSayisiSpinner = FindViewById<Spinner>(Resource.Id.spinner3);
            SureSpinner = FindViewById<Spinner>(Resource.Id.spinner4);
            SoruSayisiSpinner.OnItemSelectedListener = this;
            SureSpinner.OnItemSelectedListener = this;
            DersSpinner.ItemSelected += DersSpinner_ItemSelected;
           

            ShowLoading.Show(this, "Lütfen Bekleyin...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                DersleriGetir();
                SoruSayisiSpinnerOlustur();
                SureSpinnerDoldur();
                ShowLoading.Hide();
            })).Start();
        }

        private void DersSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            KonulariGetir(Lesson1[e.Position].id);
        }

        private void TesteBasla_Click(object sender, EventArgs e)
        {
            //this.StartActivity(typeof(TestSinavAlaniBaseActivity));
            //this.Finish();
            //return;
            SaveTestAndStart();
            return;
           
        }
        void SaveTestAndStart()
        {
            if (Bosmu())
            {
                OLUSTURULAN_TESTLER OLUSTURULAN_TESTLER1 = new OLUSTURULAN_TESTLER()
                {
                    name = "",
                    description = "",
                    startDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    userId = DataBase.MEMBER_DATA_GETIR()[0].email.ToString(),
                    lessonId = Lesson1[DersSpinner.SelectedItemPosition].id.ToString(),
                    topicId = Topic1[KonuSpinner.SelectedItemPosition].id.ToString(),
                    time = SureSpinner.SelectedItemPosition>0 ? SureSpinner.SelectedItem.ToString().Replace(" dk.",""):"100000",
                    questionCount = Convert.ToInt32(SoruSayisiSpinner.SelectedItem.ToString()),
                    lessonName= Lesson1[DersSpinner.SelectedItemPosition].name,
                    topicName= Topic1[KonuSpinner.SelectedItemPosition].name
                };
                string jsonString = JsonConvert.SerializeObject(OLUSTURULAN_TESTLER1);
                WebService webService = new WebService();
                var Donus = webService.ServisIslem("user-tests", jsonString,UsePoll:true);
                if (Donus != "Hata")
                {
                    var aa = Donus.ToString();
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<OLUSTURULAN_TESTLER>(Donus.ToString());
                    var Icerik2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TestSoruKaydetGuncelle.TestDTO>(Donus.ToString());
                    if (Icerik.questionCount==null)
                    {
                        Icerik.questionCount = Convert.ToInt32(SoruSayisiSpinner.SelectedItem.ToString());
                        Icerik2.questionCount = SoruSayisiSpinner.SelectedItem.ToString();
                        Icerik2.lessonName = OLUSTURULAN_TESTLER1.lessonName;
                        Icerik2.topicName = OLUSTURULAN_TESTLER1.topicName;
                    }

                    var Yoll = new TestSoruKaydetGuncelle().KaydetGuncelle(Icerik2);
                    if (!string.IsNullOrEmpty(Yoll[0].ToString()))
                    {
                        Icerik.SorularJsonPath = Yoll[0].ToString();
                        if (DataBase.OLUSTURULAN_TESTLER_EKLE(Icerik))
                        {
                            SecilenTest.OlusanTest = Icerik2;
                            this.StartActivity(typeof(TestSinavAlaniBaseActivity));
                            this.Finish();
                        }
                    }
                }
            }
        }

        bool Bosmu()
        {
            if (DersSpinner.SelectedItemPosition == -1)
            {
                AlertHelper.AlertGoster("Lütfen Ders Seç.", this);
                return false;
            }
            else if (KonuSpinner.SelectedItemPosition == -1)
            {
                AlertHelper.AlertGoster("Lütfen Konu Seç.", this);
                return false;
            }
            else if (SoruSayisiSpinner.SelectedItemPosition == -1)
            {
                AlertHelper.AlertGoster("Lütfen Soru Sayısı Seç.", this);
                return false;
            }
            else if (SureSpinner.SelectedItemPosition == -1)
            {
                AlertHelper.AlertGoster("Lütfen Test Süresi Belirtin.", this);
                return false;
            }
            else
            {
                return true;
            }
        }
        void DersleriGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("lessons");
            if (Donus != null)
            {
                Lesson1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(Donus.ToString());
                if (Lesson1.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        DersSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Lesson1.Select(item => item.name).ToArray());
                    });
                }
            }
        }
        void KonulariGetir(int LessonID)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("topics/lesson/"+ LessonID.ToString());
            if (Donus != null)
            {
                Topic1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Topic>>(Donus.ToString());
                if (Topic1.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        KonuSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Topic1.Select(item => item.name).ToArray());
                    });
                }
            }
        }
        void SoruSayisiSpinnerOlustur()
        {
            //string[] SoruSayisi = new string[20];
            List<string> SoruSayisi = new List<string>();
            for (int i = 5; i < 100; i+=5)
            {
                SoruSayisi.Add((i).ToString());
            }
            this.RunOnUiThread(delegate ()
            {
                SoruSayisiSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, SoruSayisi.ToArray());
                SoruSayisiSpinner.SetSelection(6);
            });
        }
        void SureSpinnerDoldur()
        {
            List<string> Dakikalar = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                Dakikalar.Add((i + 1).ToString() + " dk.");
            }

            Dakikalar.Insert(0, "Süresiz");
            this.RunOnUiThread(delegate ()
            {
                SureSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Dakikalar.ToArray());
            });
        }
        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            ((TextView)parent.GetChildAt(0)).SetTextColor(Color.White);
        }
        public void OnNothingSelected(AdapterView parent)
        {
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
        public class Topic
        {
            public string icon { get; set; }
            public int id { get; set; }
            public int lessonId { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }

        public static class SecilenTest
        {
            public static TestSoruKaydetGuncelle.TestDTO OlusanTest { get; set; }
        }
    }
}