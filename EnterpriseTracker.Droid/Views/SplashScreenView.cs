using Android.App;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace EnterpriseTracker.Droid.Views
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Splash", Label = "Enterprise Tracker")]
    public class SplashScreenView : MvxSplashScreenAppCompatActivity
    {
        public SplashScreenView() : base(Resource.Layout.splash_screen)
        {
        }
    }
}