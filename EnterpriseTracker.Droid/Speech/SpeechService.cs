using System;

using Android.Content;
using Android.Speech;

using EnterpriseTracker.Core.Speech;

using Plugin.CurrentActivity;

namespace EnterpriseTracker.Droid.Speech
{
    public class SpeechService : ISpeechService
    {
        public static readonly int VOICE = 10;

        public SpeechService()
        {
        }

        public void StartRecording()
        {
            var activity = CrossCurrentActivity.Current.Activity;

            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec == "android.hardware.microphone")
            {
                try
                {
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    //voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    activity.StartActivityForResult(voiceIntent, VOICE);
                }
                //catch (ActivityNotFoundException ex)
                //{
                //    String appPackageName = "com.google.android.googlequicksearchbox";
                //    try
                //    {
                //        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("market://details?id=" + appPackageName));
                //        _activity.StartActivityForResult(intent, VOICE);

                //    }
                //    catch (ActivityNotFoundException e)
                //    {
                //        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName));
                //        _activity.StartActivityForResult(intent, VOICE);
                //    }
                //}
                catch(Exception ex)
                {

                }
            }
            else
            {
                //throw new Exception("No mic found");
            }
        }

        public void SetRecognizedText(string input)
        {
            OnSpeechRecognized?.Invoke(input);
        }

        public event SpeechRecognizedDelegate OnSpeechRecognized;
    }
}