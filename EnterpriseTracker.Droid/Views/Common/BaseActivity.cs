using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;

using EnterpriseTracker.Core.Speech;
using EnterpriseTracker.Droid.Speech;

using MvvmCross;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

using Plugin.CurrentActivity;

namespace EnterpriseTracker.Droid.Views.Common
{
    [Activity()]
    public class BaseActivity<T> : MvxActivity<T> where T : class, IMvxViewModel
    {
        public bool IsFirstLoad = true;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Activity = this;
        }
        protected override void OnResume()
        {
            base.OnResume();
            CrossCurrentActivity.Current.Activity = this;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == SpeechService.VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    var speechService = (SpeechService) Mvx.IoCProvider.Resolve<ISpeechService>();
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];
                        speechService.SetRecognizedText(textInput);
                    }
                    else
                    {
                        speechService.SetRecognizedText(null);
                    }

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}