using Android.App;

using MvvmCross.Platforms.Android.Views;

namespace EnterpriseTracker.Droid.Views
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", Label = "Enterprise Tracker")]
    public class SplashScreenView : MvxSplashScreenActivity
    {
        public SplashScreenView() : base(Resource.Layout.splash_screen)
        {
        }
    }
}