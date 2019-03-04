using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Plugin.CurrentActivity;

namespace EnterpriseTracker.Droid.Views.Common
{
    [Activity()]
    public class BaseActivity<T> : MvxAppCompatActivity<T> where T : class, IMvxViewModel
    {
        public bool IsFirstLoad = true;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}