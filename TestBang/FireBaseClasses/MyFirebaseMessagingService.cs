using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Newtonsoft.Json;
using TestBang.DataBasee;
using TestBang.Oyun.OyunKur.ArkadaslarindanSec;
using TestBang.Splashh;

namespace TestBang
{
    [Service(Name = "com.testbang.android.MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        static readonly string CHANNEL_ID = "messages_notification";
        NotificationManager notificationManager = null;
        GelenMesajDTO GetMessageDTO1;

        public override void OnMessageReceived(RemoteMessage message)
        {
            CreateNotificationChannel(ApplicationContext);
            GetMessageDTO1 = GetMessageDTO(message.Data);
            
            if (GetMessageDTO1.data.type == "GAME")
            {
                SetNotification_GAME("Test Bang!", "Yeni bir meydan okuma aldın!", JsonConvert.SerializeObject(GetMessageDTO1));

            }

        }


        #region Chat Noti
        GelenMesajDTO GetMessageDTO(IDictionary<string, string> data)
        {
            GelenMesajDTO gelenMesajDTO = new GelenMesajDTO();
            gelenMesajDTO.data = new Data();



            foreach (var key in data.Keys)
            {

                var qqq = data["filters"];

                switch (key)
                {
                    case "type":
                        gelenMesajDTO.data.type = data[key];
                        break;
                    case "userName":
                        gelenMesajDTO.data.userName = data[key];
                        break;
                    case "userToken":
                        gelenMesajDTO.data.userToken = data[key];
                        break;
                    case "category":
                        gelenMesajDTO.data.category = data[key];
                        break;
                    case "userQuestionIndex":
                        gelenMesajDTO.data.userQuestionIndex = data[key];
                        break;
                    case "questionCount":
                        gelenMesajDTO.data.questionCount = data[key];
                        break;
                    case "correctCount":
                        gelenMesajDTO.data.correctCount = data[key];
                        break;
                    case "friendsUser":
                        gelenMesajDTO.data.friendsUser = data[key];
                        break;
                    case "isFriend":
                        gelenMesajDTO.data.friendsUser = data[key];
                        break;
                    case "startTime":
                        gelenMesajDTO.data.startTime = data[key];
                        break;
                    case "filters":
                        gelenMesajDTO.data.filters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(data[key]); ;
                        break;
                }
            }

            return gelenMesajDTO;
        }

        public class Data
        {
            public string type { get; set; }
            public string userName { get; set; }
            public string userToken { get; set; }
            public string category { get; set; }
            public string userQuestionIndex { get; set; }
            public string questionCount { get; set; }
            public string correctCount { get; set; }
            public string friendsUser { get; set; }
            public string startTime { get; set; }
            public string isFriend { get; set; }
            public List<string> filters { get; set; }
         
        }
        public class GelenMesajDTO
        {
            public string to { get; set; }
            public Data data { get; set; }
        }
        #endregion

        #region Notification Init
        void CreateNotificationChannel(Context context)
        {
            if (notificationManager == null)
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    return;
                }

                var name = "Test Bang!";
                var description = "Test Bang!";
                var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
                {
                    Description = description
                };
                var alarmAttributes = new AudioAttributes.Builder()
                                 .SetContentType(AudioContentType.Sonification)
                                 .SetUsage(AudioUsageKind.Notification).Build();
                Android.Net.Uri alarmUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                channel.EnableLights(true);
                channel.EnableVibration(true);
                channel.Importance = NotificationImportance.High;
                channel.SetSound(alarmUri, alarmAttributes);

                notificationManager = (NotificationManager)context.GetSystemService("notification");
                notificationManager.CreateNotificationChannel(channel);
            }

        }
        int NOTIFICATION_ID = 3000;
        void SetNotification_GAME(string MesajTitle, string MesajIcerigi,string GetMessageDTOJSON1)
        {
            int requestID = (int)TimeUtils.CurrentTimeMillis();
            var resultIntent = new Intent(this, typeof(ArkadasOyunSec_Gelen));
            resultIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
            resultIntent.PutExtra("GetMessageDTOJSON", GetMessageDTOJSON1);
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(ArkadasOyunSec_Gelen)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = stackBuilder.GetPendingIntent(requestID, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.OneShot);

            // Build the notification:
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle(MesajTitle) // Set the title
                          .SetNumber(30) // Display the count in the Content Info
                          .SetSmallIcon(Resource.Mipmap.ic_launcher)
                          //.SetVibrate(new long[] { 0, 500, 1000 })
                          .SetPriority(NotificationCompat.PriorityHigh)
                          //.SetSound(uri)
                          .SetContentText(MesajIcerigi); // the message to display.

            //// Instantiate the Big Text style:
            //NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();

            //// Fill it with text:
            //textStyle.BigText(MesajIcerigi);

            //// Set the summary text:
            //textStyle.SetSummaryText(MesajIcerigi);

            //// Plug this style into the builder:
            //builder.SetStyle(textStyle);


            var notificationManager = NotificationManagerCompat.From(this);
            var newid = NOTIFICATION_ID++;
            notificationManager.Notify(requestID, builder.Build());
        }



        public class TimeUtils
        {
            private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public static long CurrentTimeMillis()
            {
                return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            }
        }
        #endregion
    }
}
