using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;

namespace TestBang.GenericClass
{
   public static class BitmapHelpers
    {
     
        public static Bitmap LoadAndResizeBitmap(int ImageResourceID, int width, int height, Context context)
        {

            Resources r = context.Resources;
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeResource(r, ImageResourceID, options);
            // BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeResource(r, ImageResourceID, options);

            return resizedBitmap;
        }

        public static Bitmap LoadAndResizeBitmap2(Bitmap bitmapp, int width, int height)
        {
            byte[] fileByte;
            using (var stream = new MemoryStream())
            {
                bitmapp.Compress(Bitmap.CompressFormat.Png, 0, stream);
                fileByte = stream.ToArray();
            }

            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeByteArray(fileByte, 0, fileByte.Length, options);

            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }


            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeByteArray(fileByte, 0, fileByte.Length, options);

            using (MemoryStream ms = new MemoryStream())
            {
               resizedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
            }

            return resizedBitmap;
        }

    }
}