using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Graphics;
using Android.Util;

using MvvmCross;
using MvvmCross.Platforms.Android;

namespace EnterpriseTracker.Droid.Utility
{
    public static class AndroidHelper
    {
        static AndroidHelper()
        {
            CurrentTheme = new DefaultTheme();
        }

        public static Activity CurrentActivity
        {
            get
            {
                return Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            }
        }

        public static int ConvertDpToPx(int dp)
        {
            return (int)(dp * CurrentActivity.Resources.DisplayMetrics.Density);
        }

        public static int ConvertPxToDp(int px)
        {
            return (int)(px / CurrentActivity.Resources.DisplayMetrics.Density);
        }

        public static ITheme CurrentTheme { get; set; }

        public static int[] GetScreenDimens(Activity activity, bool shouldReturnPixels = false)
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            int height = shouldReturnPixels ? displayMetrics.HeightPixels : ConvertPxToDp(displayMetrics.HeightPixels);
            int width = shouldReturnPixels ? displayMetrics.WidthPixels : ConvertPxToDp(displayMetrics.WidthPixels);
            return new[] { width, height };
        }

        private static Task<Stream> GetStreamFromImageByte(CancellationToken arg)
        {
            //TODO handle bmp. This method is not currently used.
            Bitmap bmp = null;
            //var bmp = BitmapFactory.DecodeFile(ViewModel.Print.Media.Url);
            MemoryStream stream = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            byte[] imageInBytes = stream.ToArray();

            //Since we need to return a Task<Stream> we will use a TaskCompletionSource>
            TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>();

            tcs.TrySetResult(new MemoryStream(imageInBytes));

            return tcs.Task;
        }

        public static Bitmap GetResizedBitmap(Bitmap bmp, float newWidth, float newHeight)
        {
            float width = bmp.Width;
            float height = bmp.Height;

            float aspectRatio = width / height;
            if(newWidth == 0)
            {
                //Cal width, keep height
                newWidth = newHeight * aspectRatio;
            }
            else if(newHeight == 0)
            {
                //Cal height, keep width
                newHeight = newWidth / aspectRatio;
            }
            else
            {
                //By default, scale the image based on width. Thus, only change the height
                newHeight = newWidth / aspectRatio;
            }

            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bmp, (int)newWidth, (int)newHeight, false);
            if (bmp != resizedBitmap)
            {
                bmp.Recycle();
            }
            return resizedBitmap;
        }
    }
}