using Android.OS;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Droid.Fragments
{
    public class BaseDialogFragment<T> : MvxDialogFragment<T> where T : class, IMvxViewModel
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}