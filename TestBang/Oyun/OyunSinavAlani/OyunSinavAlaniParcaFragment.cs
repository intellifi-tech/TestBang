using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.GenericClass;
using TestBang.GenericClass.StompHelper;
using static TestBang.GenericClass.OyunSocketHelper;
using static TestBang.Oyun.OyunKur.ArkadaslarindanSec.ArkadasOyunSec_Gelen;

namespace TestBang.Oyun.OyunSinavAlani
{
    public class OyunSinavAlaniParcaFragment : Android.Support.V4.App.Fragment
    {
        int QestionPosition;
        TextView DersAdiText, CurrentSoruNumber, SoruText, Answer_A_Text, Answer_B_Text, Answer_C_Text, Answer_D_Text, Answer_E_Text;
        Button Answer_A_Button, Answer_B_Button, Answer_C_Button, Answer_D_Button, Answer_E_Button;
        ImageView SoruImage, Answer_A_Image, Answer_B_Image, Answer_C_Image, Answer_D_Image, Answer_E_Image;
        RelativeLayout Relative_A, Relative_B, Relative_C, Relative_D, Relative_E;
        MEMBER_DATA MeId = DataBase.MEMBER_DATA_GETIR()[0];
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public OyunSinavAlaniParcaFragment(int QestionPosition2)
        {
            QestionPosition = QestionPosition2;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View Vieww = inflater.Inflate(Resource.Layout.TestSoruParcaFragment, container, false);
            CurrentSoruNumber = Vieww.FindViewById<TextView>(Resource.Id.curentsorunumber);
            DersAdiText = Vieww.FindViewById<TextView>(Resource.Id.derstext);
            SoruText = Vieww.FindViewById<TextView>(Resource.Id.sorutext);
            Answer_A_Text = Vieww.FindViewById<TextView>(Resource.Id.answer_a);
            Answer_B_Text = Vieww.FindViewById<TextView>(Resource.Id.answer_b);
            Answer_C_Text = Vieww.FindViewById<TextView>(Resource.Id.answer_c);
            Answer_D_Text = Vieww.FindViewById<TextView>(Resource.Id.answer_d);
            Answer_E_Text = Vieww.FindViewById<TextView>(Resource.Id.answer_e);

            Answer_A_Button = Vieww.FindViewById<Button>(Resource.Id.button_a);
            Answer_B_Button = Vieww.FindViewById<Button>(Resource.Id.button_b);
            Answer_C_Button = Vieww.FindViewById<Button>(Resource.Id.button_c);
            Answer_D_Button = Vieww.FindViewById<Button>(Resource.Id.button_d);
            Answer_E_Button = Vieww.FindViewById<Button>(Resource.Id.button_e);

            SoruImage = Vieww.FindViewById<ImageView>(Resource.Id.soruimage);
            Answer_A_Image = Vieww.FindViewById<ImageView>(Resource.Id.answer_image_a);
            Answer_B_Image = Vieww.FindViewById<ImageView>(Resource.Id.answer_image_b);
            Answer_C_Image = Vieww.FindViewById<ImageView>(Resource.Id.answer_image_c);
            Answer_D_Image = Vieww.FindViewById<ImageView>(Resource.Id.answer_image_d);
            Answer_E_Image = Vieww.FindViewById<ImageView>(Resource.Id.answer_image_e);


            Relative_A = Vieww.FindViewById<RelativeLayout>(Resource.Id.relative_a);
            Relative_B = Vieww.FindViewById<RelativeLayout>(Resource.Id.relative_b);
            Relative_C = Vieww.FindViewById<RelativeLayout>(Resource.Id.relative_c);
            Relative_D = Vieww.FindViewById<RelativeLayout>(Resource.Id.relative_d);
            Relative_E = Vieww.FindViewById<RelativeLayout>(Resource.Id.relative_e);

         

            DersAdiText.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].lessonName;
            CurrentSoruNumber.Text = "SORU " + (QestionPosition + 1).ToString() + "/" + OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.Count.ToString();
            SoruText.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].text;
            Answer_A_Text.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[0].text;
            Answer_B_Text.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[1].text;
            Answer_C_Text.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[2].text;
            Answer_D_Text.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[3].text;
            Answer_E_Text.Text = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[4].text;


            SoruCevapImageKontrol();

            Answer_A_Button.Click += Answer_A_Button_Click;
            Answer_A_Text.Click += Answer_A_Button_Click;
            Answer_A_Image.Click += Answer_A_Button_Click;
            Relative_A.Click += Answer_A_Button_Click;


            Answer_B_Button.Click += Answer_B_Button_Click;
            Answer_B_Text.Click += Answer_B_Button_Click;
            Answer_B_Image.Click += Answer_B_Button_Click;
            Relative_B.Click += Answer_B_Button_Click;



            Answer_C_Button.Click += Answer_C_Button_Click1; 
            Answer_C_Text.Click += Answer_C_Button_Click1;
            Answer_C_Image.Click += Answer_C_Button_Click1;
            Relative_C.Click += Answer_C_Button_Click1;


            Answer_D_Button.Click += Answer_D_Button_Click;
            Answer_D_Text.Click += Answer_D_Button_Click;
            Answer_D_Image.Click += Answer_D_Button_Click;
            Relative_D.Click += Answer_D_Button_Click;

            Answer_E_Button.Click += Answer_E_Button_Click;
            Answer_E_Text.Click += Answer_E_Button_Click;
            Answer_E_Image.Click += Answer_E_Button_Click;
            Relative_E.Click += Answer_E_Button_Click;

            return Vieww;
        }


        private void Answer_E_Button_Click(object sender, EventArgs e)
        {
            TumSecimleriTemzile();
            Relative_E.SetBackgroundResource(Resource.Drawable.test_secenek_secim_bg);
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].userAnswer = "E";
            GuncellemeGonderSoket();
        }

        private void Answer_D_Button_Click(object sender, EventArgs e)
        {
            TumSecimleriTemzile();
            Relative_D.SetBackgroundResource(Resource.Drawable.test_secenek_secim_bg);
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].userAnswer = "D";
            GuncellemeGonderSoket();
        }

        private void Answer_C_Button_Click1(object sender, EventArgs e)
        {
            
            TumSecimleriTemzile();
            Relative_C.SetBackgroundResource(Resource.Drawable.test_secenek_secim_bg);
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].userAnswer = "C";
            GuncellemeGonderSoket();
        }

        private void Answer_B_Button_Click(object sender, EventArgs e)
        {
            TumSecimleriTemzile();
            Relative_B.SetBackgroundResource(Resource.Drawable.test_secenek_secim_bg);
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].userAnswer = "B";
            GuncellemeGonderSoket();
        }

        private void Answer_A_Button_Click(object sender, EventArgs e)
        {
            TumSecimleriTemzile();
            Relative_A.SetBackgroundResource(Resource.Drawable.test_secenek_secim_bg);
            OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].userAnswer = "A";
            GuncellemeGonderSoket();

        }

        void TumSecimleriTemzile()
        {
            Relative_A.SetBackgroundColor(Color.Transparent);
            Relative_B.SetBackgroundColor(Color.Transparent);
            Relative_C.SetBackgroundColor(Color.Transparent);
            Relative_D.SetBackgroundColor(Color.Transparent);
            Relative_E.SetBackgroundColor(Color.Transparent);
        }


        void GuncellemeGonderSoket()
        {
            var ToplamCozumSayisi = OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList.FindAll(item => !string.IsNullOrEmpty(item.userAnswer)).Count;
            if (ToplamCozumSayisi > 0)
            {
                TestSinavAlaniHelperClass.OyunSinavAlaniBaseActivity1.BenGuncelle(ToplamCozumSayisi);//Lokal UI güncelle

                var content = new SoketSendRegisterDTO()
                {
                    category = OyunSocketHelper_Helper.SecilenAlan,
                    userName = MeId.login,
                    userQuestionIndex = ToplamCozumSayisi.ToString(),
                    userToken = MeId.API_TOKEN,
                    filters = new List<string>(),
                    
                };
             
                var broad = new StompMessage(StompFrame.SEND, JsonConvert.SerializeObject(content));
                broad["content-type"] = "application/json";
                // broad["username"] = MeId.login;
                broad["destination"] = "/app/level";
                var aaa = OyunSocketHelper_Helper.OyunSocketHelper1.serializer.Serialize(broad);
                if (OyunSocketHelper_Helper.OyunSocketHelper1.ws.IsAlive)
                {
                    OyunSocketHelper_Helper.OyunSocketHelper1.ws.Send(aaa);
                }
            }
        }


        void SoruCevapImageKontrol()
        {
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].imagePath))
            {
                ImageYansit(SoruImage, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].imagePath);
            }
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[0].imagePath))
            {
                ImageYansit(Answer_A_Image, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[0].imagePath);
            }
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[1].imagePath))
            {
                ImageYansit(Answer_B_Image, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[1].imagePath);
            }
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[2].imagePath))
            {
                ImageYansit(Answer_C_Image, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[2].imagePath);
            }
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[3].imagePath))
            {
                ImageYansit(Answer_D_Image, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[3].imagePath);
            }
            if (!string.IsNullOrEmpty(OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[4].imagePath))
            {
                ImageYansit(Answer_E_Image, OyunSocketHelper_Helper.RoomQuestionsDTO1.questionList[QestionPosition].answers[4].imagePath);
            }
        }

        void ImageYansit(ImageView GelenView, string Base64String)
        {
            var bitmapp = Base64ToBitmap(Base64String);
            GelenView.SetImageBitmap(bitmapp);
        }
        public Bitmap Base64ToBitmap(string base64String)
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}