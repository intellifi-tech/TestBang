using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericUI;
using TestBang.Test.TestSinavAlani;
using TestBang.WebServices;
using static TestBang.Profil.DersProgrami.DersProgramiBaseActivity;
using static TestBang.Test.TestOlustur.TestOlusturBaseActivity;

namespace TestBang.Profil.DersProgrami
{
    [Activity(Label = "TestBang")]
    public class DersProgramiEkleBaseActivity : Android.Support.V7.App.AppCompatActivity, AdapterView.IOnItemSelectedListener
    {
        Spinner DersSpinner, KonuSpinner, SoruSayisiSpinner, SureSpinner;
        EditText TestAdiText, TestAciklamasiText;
        TextView TestSaatiText,TarihText;
        List<Lesson> Lesson1 = new List<Lesson>();
        List<Topic> Topic1 = new List<Topic>();
        Button DersProgramiKaydetButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DersProgramiEkleBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Pembe(this);

            DersSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            KonuSpinner = FindViewById<Spinner>(Resource.Id.spinner2);
            SoruSayisiSpinner = FindViewById<Spinner>(Resource.Id.spinner3);
            SureSpinner = FindViewById<Spinner>(Resource.Id.spinner4);
            TarihText = FindViewById<TextView>(Resource.Id.tarihtext);
            TarihText.Text = DersProgramiBaseActivityHelper.SecilenTarih.ToShortDateString();
            SoruSayisiSpinner.OnItemSelectedListener = this;
            SureSpinner.OnItemSelectedListener = this;
            DersSpinner.ItemSelected += DersSpinner_ItemSelected;
            TestAdiText = FindViewById<EditText>(Resource.Id.testadittext);
            TestAciklamasiText = FindViewById<EditText>(Resource.Id.testaciklamasitext);
            TestSaatiText = FindViewById<TextView>(Resource.Id.testsaati);
            DersProgramiKaydetButton = FindViewById<Button>(Resource.Id.button2);
            DersProgramiKaydetButton.Click += DersProgramiKaydetButton_Click;
            TestSaatiText.Click += TestSaatiText_Click;

            ShowLoading.Show(this, "Lütfen Bekleyin...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                DersleriGetir();
                SoruSayisiSpinnerOlustur();
                SureSpinnerDoldur();
                ShowLoading.Hide();
            })).Start();

        }

        private void DersProgramiKaydetButton_Click(object sender, EventArgs e)
        {
            if (Bosmu())
            {
                SaveTestAndStart();
            }
        }

        private void TestSaatiText_Click(object sender, EventArgs e)
        {
            Saat_Secim frag = Saat_Secim.NewInstance(delegate (string time)
            {
                TestSaatiText.Text = time;
            });
            frag.Show(this.SupportFragmentManager, Tarih_Cek.TAG);
        }

        private void DersSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            KonulariGetir(Lesson1[e.Position].id);
        }

        void SaveTestAndStart()
        {
            if (Bosmu())
            {
                OLUSTURULAN_TESTLER OLUSTURULAN_TESTLER1 = new OLUSTURULAN_TESTLER()
                {
                    name = TestAdiText.Text.Trim(),
                    description = TestAciklamasiText.Text.Trim(),
                    startDate = Convert.ToDateTime(DersProgramiBaseActivityHelper.SecilenTarih.ToShortDateString() + " " + TestSaatiText.Text.ToString()).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    userId = DataBase.MEMBER_DATA_GETIR()[0].email.ToString(),
                    lessonId = Lesson1[DersSpinner.SelectedItemPosition].id.ToString(),
                    topicId = Topic1[KonuSpinner.SelectedItemPosition].id.ToString(),
                    time = SureSpinner.SelectedItemPosition > 0 ? SureSpinner.SelectedItem.ToString().Replace(" dk.", "") : "100000",
                    questionCount = Convert.ToInt32(SoruSayisiSpinner.SelectedItem.ToString()),
                    lessonName = Lesson1[DersSpinner.SelectedItemPosition].name,
                    topicName = Topic1[KonuSpinner.SelectedItemPosition].name
                };
                string jsonString = JsonConvert.SerializeObject(OLUSTURULAN_TESTLER1);
                WebService webService = new WebService();
                var Donus = webService.ServisIslem("user-tests", jsonString, UsePoll: true);
                if (Donus != "Hata")
                {
                    var aa = Donus.ToString();
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<OLUSTURULAN_TESTLER>(Donus.ToString());
                    var Icerik2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TestSoruKaydetGuncelle.TestDTO>(Donus.ToString());
                    if (Icerik.questionCount == null)
                    {
                        Icerik.questionCount = Convert.ToInt32(SoruSayisiSpinner.SelectedItem.ToString());
                        Icerik.lessonName = OLUSTURULAN_TESTLER1.lessonName;
                        Icerik.topicName = OLUSTURULAN_TESTLER1.topicName;
                        Icerik2.questionCount = SoruSayisiSpinner.SelectedItem.ToString();
                    }

                    var Yoll = new TestSoruKaydetGuncelle().KaydetGuncelle(Icerik2);
                    if (!string.IsNullOrEmpty(Yoll[0].ToString()))
                    {
                        Icerik.SorularJsonPath = Yoll[0].ToString();
                        if (DataBase.OLUSTURULAN_TESTLER_EKLE(Icerik))
                        {
                            //SecilenTest.OlusanTest = Icerik2;
                            //this.StartActivity(typeof(TestSinavAlaniBaseActivity));
                            //this.Finish();
                            CreateCalander(Icerik.id);
                        }
                    }
                }
            }
        }

        bool Bosmu()
        {
            if (string.IsNullOrEmpty(TestSaatiText.Text.Trim()))
            {
                AlertHelper.AlertGoster("Lütfen Test Saatini Belirtin.", this);
                return false;
            }
            else if (string.IsNullOrEmpty(TestAdiText.Text.Trim()))
            {
                AlertHelper.AlertGoster("Lütfen Test Adı Belirtin.", this);
                return false;
            }
            else if (DersSpinner.SelectedItemPosition == -1)
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
                if (Lesson1!=null)
                {
                    if (Lesson1.Count > 0)
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            DersSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Lesson1.Select(item => item.name).ToArray());
                        });
                    }
                }
            }
        }
        void KonulariGetir(int LessonID)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("topics/lesson/" + LessonID.ToString());
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
            for (int i = 5; i < 100; i += 5)
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

        void CreateCalander(string TestID)
        {
            //DersProgramiBaseActivityHelper.SecilenTarih = DateTime.Now;
            WebService webService = new WebService();
            var saat = TestSaatiText.Text.Split(":");

            var OlusanTarih = new DateTime(DersProgramiBaseActivityHelper.SecilenTarih.Year,
                                           DersProgramiBaseActivityHelper.SecilenTarih.Month,
                                           DersProgramiBaseActivityHelper.SecilenTarih.Day,
                                           Convert.ToInt32(saat[0]),Convert.ToInt32(saat[1]),0);
            DERS_PROGRAMI dERS_PROGRAMI = new DERS_PROGRAMI()
            {
                date = OlusanTarih.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                description = TestAciklamasiText.Text.Trim(),
                testId= TestID,
                userId = DataBase.MEMBER_DATA_GETIR()[0].id,
            };
            string jsonString = JsonConvert.SerializeObject(dERS_PROGRAMI);
            var Donus = webService.ServisIslem("calendars", jsonString);
            if (Donus != "Hata")
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<DERS_PROGRAMI>(Donus.ToString());
                if (Icerik!=null)
                {
                    DataBase.DERS_PROGRAMI_EKLE(Icerik);
                    AlertHelper.AlertGoster("Ders Programı Oluşturuldu", this);
                    this.Finish();
                    return;
                }
            }
        }

        public class Lesson
        {
            public bool ea { get; set; }
            public string icon { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public bool say { get; set; }
            public bool soz { get; set; }
            public string token { get; set; }
        }
        public class Topic
        {
            public string icon { get; set; }
            public int id { get; set; }
            public int lessonId { get; set; }
            public string name { get; set; }
            public string token { get; set; }
        }
    }
}