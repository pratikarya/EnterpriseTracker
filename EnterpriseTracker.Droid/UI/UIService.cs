using System;
using Android;
using Android.App;
using Android.Content;
using Android.Media;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Droid.Utility;

namespace EnterpriseTracker.Droid.UI
{
    public class UIService : IUIService
    {
        public void ShowLoadingDialog(bool isVisible, string message)
        {
            if(isVisible)
            {
                AndroidHUD.AndHUD.Shared.Show(AndroidHelper.CurrentActivity, message);
            }
            else
            {
                AndroidHUD.AndHUD.Shared.Dismiss(AndroidHelper.CurrentActivity);
            }
        }

        public void ShowSuccessDialog(string message)
        {
            AndroidHUD.AndHUD.Shared.ShowSuccess(AndroidHelper.CurrentActivity, message, timeout: new TimeSpan(2));
        }

        public void ShowErrorDialog(string message)
        {
            AndroidHUD.AndHUD.Shared.ShowError(AndroidHelper.CurrentActivity, message, timeout: TimeSpan.FromSeconds(3));
        }

        public void ShowNotification(string title = "", string message = "", string icon = "")
        {
            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();
            textStyle.BigText(message);
            Notification.Builder builder = new Notification.Builder(AndroidHelper.CurrentActivity)
                               .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                               .SetContentTitle(title)
                               //.SetSmallIcon(Resource.Drawable.notification_bg)
                               .SetStyle(textStyle);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = AndroidHelper.CurrentActivity.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

        public void ShowConfirmationDialog(Action yesAction, Action noAction, string message, string title)
        {
            var alert = new AlertDialog.Builder(AndroidHelper.CurrentActivity);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetCancelable(false);

            alert.Create();

            alert.SetPositiveButton("Yes", (sender, args) =>
            {
                yesAction?.Invoke();
            });
            alert.SetNegativeButton("No", (sender, args) =>
            {
                noAction?.Invoke();
            });
            alert.Show();
        }
    }
}