using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace TestBang.DataBasee
{
    class DataModel
    {
        public DataModel()
        {

        }
    }
    public class MEMBER_DATA
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public bool activated { get; set; }
        //public List<string> authorities { get; set; }
        public string birthday { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public bool? gender { get; set; }
        public int id { get; set; }
        public string imageUrl { get; set; }
        public string langKey { get; set; }
        public string lastModifiedBy { get; set; }
        public string lastModifiedDate { get; set; }
        public string lastName { get; set; }
        public string login { get; set; }

        public int? townId { get; set; }
        //------------------------------------
        public string API_TOKEN { get; set; }
        public string password { get; set; }
    }

    public class OLUSTURULAN_TESTLER
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public int? correctCount { get; set; }
        public string description { get; set; }
        public int? emptyCount { get; set; }
        public bool finish { get; set; }
        public string finishDate { get; set; }
        public string id { get; set; }
        public string lessonId { get; set; }
        public string name { get; set; }
        public int? net { get; set; }
        public int? questionCount { get; set; }
        public string startDate { get; set; }
        public string testTime { get; set; }
        public string time { get; set; }
        public string topicId { get; set; }
        public string userId { get; set; }
        public int? wrongCount { get; set; }
        //Custom
        public string SorularJsonPath { get; set; }
    }

    public class DERS_PROGRAMI
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string testId { get; set; }
        public string trialId { get; set; }
        public int userId { get; set; }
    }
}