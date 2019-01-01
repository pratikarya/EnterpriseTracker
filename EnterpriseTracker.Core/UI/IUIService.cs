using System;

namespace EnterpriseTracker.Core.UI
{
    public interface IUIService
    {
        void ShowDialog(bool isVisible, string message = "Please wait!");

        void ShowSuccessDialog(string message = "");

        void ShowErrorDialog(string message = "");

        void ShowNotification(string title = "", string message = "", string icon = "");

        void ShowConfirmationDialog(Action yesAction, Action noAction, string message, string title = "Confirm");
    }
}
