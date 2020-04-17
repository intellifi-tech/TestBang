using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace TestBang.GenericClass
{
    public class TestSoruKaydetGuncelle
    {
        public object[] KaydetGuncelle(TestDTO GelenDoluDto)
        {
            string jsonString = JsonConvert.SerializeObject(GelenDoluDto);
            var Dosyaa = documentsFolder() + "//" + GelenDoluDto.id+".txt";
            using (System.IO.StreamWriter write = new System.IO.StreamWriter(Dosyaa, true))
            {
                write.Write(jsonString.ToString());
            }
            object[] returnedobject = new object[1];
            returnedobject[0] = Dosyaa;//Dosya Yolu


            return returnedobject;
        }

        public static string documentsFolder()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)+"//TestBangTestFolder";
            Directory.CreateDirectory(path);
            return path;
        }


        public class Answer
        {
            public string id { get; set; }
            public string index { get; set; }
            public string text { get; set; }
            public string imagePath { get; set; }
            public string questionId { get; set; }
        }

        public class Question
        {
            public string id { get; set; }
            public string text { get; set; }
            public string imagePath { get; set; }
            public string topicId { get; set; }
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public string correctAnswer { get; set; }
            public string description { get; set; }
            public List<Answer> answers { get; set; }
            public string trialId { get; set; }
        }

        public class UserTestQuestion
        {
            public string id { get; set; }
            public string userAnswer { get; set; }
            public bool correct { get; set; }
            public bool empty { get; set; }
            public string questionId { get; set; }
            public string userTestId { get; set; }
            public Question question { get; set; }
        }

        public class TestDTO
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string name { get; set; }
            public int? correctCount { get; set; }
            public int? wrongCount { get; set; }
            public int? emptyCount { get; set; }
            public string topicId { get; set; }
            public string lessonId { get; set; }
            public int? net { get; set; }
            public string description { get; set; }
            public string testTime { get; set; }
            public string time { get; set; }
            public bool finish { get; set; }
            public string startDate { get; set; }
            public string finishDate { get; set; }
            public string questionCount { get; set; }
            public List<UserTestQuestion> userTestQuestions { get; set; }
        }
    }
}