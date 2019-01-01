using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Droid.Controls;
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

        public static ITheme CurrentTheme { get; set; }
    }
}